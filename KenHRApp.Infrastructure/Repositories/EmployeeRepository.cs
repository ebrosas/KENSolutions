using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Microsoft.VisualBasic;

namespace KenHRApp.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        #region Fields
        private readonly AppDbContext _db;
        //private readonly IDbContextFactory<AppDbContext> _contextFactory;
        #endregion

        #region Constructors                
        public EmployeeRepository(AppDbContext db)
        {
            _db = db;
            //_contextFactory = contextFactory;
        }
        #endregion

        #region Private Methods
        
        #endregion

        #region Public Methods                
        public async Task<List<Employee>> GetAllAsync()
        {
            try
            {                
                return await _db.Employees.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task AddAsync(Employee employee)
        {
            try
            {
                _db.Employees.Add(employee);
                await _db.SaveChangesAsync();
            }
            catch (SqlException sqlErr)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<Result<List<UserDefinedCode>?>> GetUserDefinedCodeAsync(string udcKey)
        {
            try
            {
                int groupID = _db.UserDefinedCodeGroups.Where(a => a.UDCGCode == udcKey).FirstOrDefault()!.UDCGroupId;
                if (groupID > 0)
                {
                    var udcList = await _db.UserDefinedCodes
                        .Where(u => u.GroupID == groupID)
                        .OrderBy(e => e.UDCDesc1)
                        .AsNoTracking()
                        .ToListAsync();

                    return Result<List<UserDefinedCode>?>.SuccessResult(udcList);
                }

                return Result<List<UserDefinedCode>?>.Failure("No record found.");
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<UserDefinedCode>?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<RepositoryResult<List<UserDefinedCode>>> GetAllUserDefinedCodeAsync()
        {
            try
            {
                var query = _db.UserDefinedCodes.AsQueryable();

                query = query.OrderBy(e => e.GroupID).ThenBy(e => e.UDCDesc1).AsNoTracking();

                var result = await query.ToListAsync();

                return RepositoryResult<List<UserDefinedCode>>.Ok(result);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return RepositoryResult<List<UserDefinedCode>>.Fail($"Error fetching User Defined Codes data: {ex.Message}");
            }
        }

        public async Task<Result<List<UserDefinedCode>>> GetUserDefinedCodeAllAsync()
        {
            try
            {
                var query = _db.UserDefinedCodes.AsQueryable();

                query = query.OrderBy(e => e.GroupID).ThenBy(e => e.UDCDesc1).AsNoTracking();

                var udc = await query.ToListAsync();

                return Result<List<UserDefinedCode>>.SuccessResult(udc);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<UserDefinedCode>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<UserDefinedCodeGroup>>> GetUserDefinedCodeGroupAsync()
        {
            try
            {
                var query = _db.UserDefinedCodeGroups.AsQueryable();

                query = query.OrderBy(e => e.UDCGDesc1).AsNoTracking();

                var udc = await query.ToListAsync();

                return Result<List<UserDefinedCodeGroup>>.SuccessResult(udc);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<UserDefinedCodeGroup>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<EmployeeMaster>?>> SearchEmployeeAsync(int? empNo, string? firstName, string? lastName, int? managerEmpNo,
            DateTime? joiningDate, string? statusCode, string? employmentType, string? departmentCode)
        {
            List<EmployeeMaster> employeeList = new List<EmployeeMaster>();
            
            try
            {
                var model = await _db.EmployeeMasters
                    .FromSqlRaw("EXEC kenuser.Pr_SearchEmployee @empNo = {0}, @firstName = {1}, @lastName = {2}, @managerEmpNo = {3}, @joiningDate = {4}, @statusCode = {5}, @employmentType = {6}, @departmentCode = {7}",
                    empNo!, firstName!, lastName!, managerEmpNo!, joiningDate!, statusCode!, employmentType!, departmentCode!)
                    .ToListAsync();
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        employeeList.Add(new EmployeeMaster()
                        {
                            EmployeeId = item.EmployeeId,
                            EmployeeNo = item.EmployeeNo,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Gender = item.Gender,
                            HireDate = item.HireDate,
                            EmploymentTypeCode = item.EmploymentTypeCode,
                            EmploymentType = item.EmploymentType,
                            ReportingManagerCode = item.ReportingManagerCode,
                            ReportingManager = item.ReportingManager,
                            DepartmentCode = item.DepartmentCode,
                            DepartmentName = item.DepartmentName,
                            EmployeeStatusCode = item.EmployeeStatusCode,
                            EmployeeStatus = item.EmployeeStatus
                        });
                    }
                }

                return Result<List<EmployeeMaster>?>.SuccessResult(employeeList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<EmployeeMaster>?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<DepartmentMaster>?>> GetDepartmentMasterAsync(string? departmentCode, string? departmentName, string description, string? groupCode, 
            int? superintendentNo, int? managerEmpNo, bool isActive)
        {
            List<DepartmentMaster> departmentList = new List<DepartmentMaster>();

            try
            {
                var query = from d in _db.DepartmentMasters
                            join deptGroup in _db.UserDefinedCodes on d.GroupCode equals deptGroup.UDCCode into gjDeptGroup from subDeptGroup in gjDeptGroup.DefaultIfEmpty()               // LEFT JOIN   
                            join deptHead in _db.Employees on d.SuperintendentEmpNo equals deptHead.EmployeeNo into gjDeptHead from subDeptHead in gjDeptHead.DefaultIfEmpty()              // LEFT JOIN   
                            join deptManager in _db.Employees on d.ManagerEmpNo equals deptManager.EmployeeNo into gjDeptManager from subDeptManager in gjDeptManager.DefaultIfEmpty()      // LEFT JOIN   
                            select new
                            {
                                Department = d,
                                GroupName = subDeptGroup != null ? subDeptGroup.UDCDesc1 : null,
                                SuperintendentName = subDeptHead != null ? subDeptHead.EmployeeFullName : null,
                                ManagerName = subDeptManager != null ? subDeptManager.EmployeeFullName : null
                            };

                if (!string.IsNullOrEmpty(departmentCode))
                    query = query.Where(d => d.Department.DepartmentCode == departmentCode);

                if (!string.IsNullOrEmpty(departmentName))
                    query = query.Where(d => d.Department.DepartmentName != null &&
                                             d.Department.DepartmentName.Trim().ToUpper().Contains(departmentName.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(description))
                    query = query.Where(d => d.Department.Description != null &&
                                             d.Department.Description.Trim().ToUpper().Contains(description.Trim().ToUpper()));

                if (!string.IsNullOrEmpty(groupCode))
                    query = query.Where(d => d.Department.GroupCode == groupCode);

                if (superintendentNo.HasValue && superintendentNo > 0)
                    query = query.Where(d => d.Department.SuperintendentEmpNo == superintendentNo);

                if (managerEmpNo.HasValue && managerEmpNo > 0)
                    query = query.Where(d => d.Department.ManagerEmpNo == managerEmpNo);

                query = query.Where(d => d.Department.IsActive == isActive);

                var model = await query.OrderBy(d => d.Department.DepartmentName).ToListAsync();
                if (model != null)
                {
                    #region Initialize entity list
                    foreach (var item in model)
                    {
                        departmentList.Add(new DepartmentMaster()
                        {
                            DepartmentId = item.Department.DepartmentId,
                            DepartmentCode = item.Department.DepartmentCode,
                            DepartmentName = item.Department.DepartmentName,
                            Description = item.Department.Description,
                            GroupCode = item.Department.GroupCode,
                            GroupName = item.GroupName,
                            ParentDepartmentId = item.Department.ParentDepartmentId,
                            SuperintendentEmpNo = item.Department.SuperintendentEmpNo,
                            SuperintendentName = item.SuperintendentName,   
                            ManagerEmpNo = item.Department.ManagerEmpNo,
                            ManagerName = item.ManagerName,
                            IsActive = item.Department.IsActive,
                            CreatedAt = item.Department.CreatedAt,  
                            UpdatedAt = item.Department.UpdatedAt
                        });
                    }
                    #endregion
                }

                return Result<List<DepartmentMaster>?>.SuccessResult(departmentList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<DepartmentMaster>?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<Employee>?>> GetReportingManagerAsync(string? departmentCode, bool isActive)
        {
            try
            {
                var query = _db.Employees.AsQueryable();

                if (!string.IsNullOrEmpty(departmentCode))
                    query = query.Where(e => e.DepartmentCode == departmentCode);

                query = query.Where(e => e.IsActive == isActive);

                var managerList = await query.ToListAsync();

                return Result<List<Employee>?>.SuccessResult(managerList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<Employee>?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetMaxEmployeeNoAsync()
        {
            try
            {
                int maxEmployeeNo = await _db.Employees.MaxAsync(e => e.EmployeeNo) + 1;

                return Result<int>.SuccessResult(maxEmployeeNo);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<Employee?>> GetEmployeeDetailAsync(int employeeId)
        {
            Employee? employeeDetail = null;

            try
            { 
                #region Get employee details
                var model = await (from e in _db.Employees
                                   join nc in _db.UserDefinedCodes on e.NationalityCode equals nc.UDCCode               // INNER JOIN
                                   join rc in _db.UserDefinedCodes on e.ReligionCode equals rc.UDCCode                  // INNER JOIN
                                   join gd in _db.UserDefinedCodes on e.GenderCode equals gd.UDCCode                    // INNER JOIN
                                   join ms in _db.UserDefinedCodes on e.MaritalStatusCode equals ms.UDCCode             // INNER JOIN
                                   join employ in _db.UserDefinedCodes on e.EmploymentTypeCode equals employ.UDCCode    // INNER JOIN
                                   join role in _db.UserDefinedCodes on e.RoleCode equals role.UDCCode                  // INNER JOIN
                                   join firstAttend in _db.UserDefinedCodes on e.FirstAttendanceModeCode equals firstAttend.UDCCode                  // INNER JOIN
                                   join sl in _db.UserDefinedCodes on e.Salutation equals sl.UDCCode into gjSL from subSL in gjSL.DefaultIfEmpty()             // LEFT JOIN                                    
                                   join es in _db.UserDefinedCodes on e.EmployeeStatusCode equals es.UDCCode into gjES from subES in gjES.DefaultIfEmpty()     // LEFT JOIN
                                   join ec in _db.UserDefinedCodes on e.EmployeeClassCode equals ec.UDCCode into gjEC from subEC in gjEC.DefaultIfEmpty()      // LEFT JOIN
                                   join el in _db.UserDefinedCodes on e.EducationCode equals el.UDCCode into gjEL from subEL in gjEL.DefaultIfEmpty()          // LEFT JOIN
                                   join ac in _db.UserDefinedCodes on e.AccountTypeCode equals ac.UDCCode into gjAC from subAC in gjAC.DefaultIfEmpty()        // LEFT JOIN
                                   join bn in _db.UserDefinedCodes on e.BankNameCode equals bn.UDCCode into gjBN from subBN in gjBN.DefaultIfEmpty()           // LEFT JOIN
                                   join jt in _db.UserDefinedCodes on e.JobTitleCode equals jt.UDCCode into gjJT from subJT in gjJT.DefaultIfEmpty()           // LEFT JOIN
                                   join pg in _db.UserDefinedCodes on e.PayGrade equals pg.UDCCode into gjPG from subPG in gjPG.DefaultIfEmpty()               // LEFT JOIN
                                   join pc in _db.UserDefinedCodes on e.PresentCountryCode equals pc.UDCCode into gjPC from subPC in gjPC.DefaultIfEmpty()      // LEFT JOIN
                                   join pa in _db.UserDefinedCodes on e.PermanentCountryCode equals pa.UDCCode into gjPA from subPA in gjPA.DefaultIfEmpty()      // LEFT JOIN
                                   join dep in _db.DepartmentMasters on e.DepartmentCode equals dep.DepartmentCode into gjDEP from subDEP in gjDEP.DefaultIfEmpty()      // LEFT JOIN
                                   join mngr in _db.Employees on e.ReportingManagerCode equals mngr.EmployeeNo into gjMngr from subMngr in gjMngr.DefaultIfEmpty()      // LEFT JOIN
                                   join secondAttend in _db.UserDefinedCodes on e.SecondAttendanceModeCode equals secondAttend.UDCCode into gjSecondAttend from subSecondAttend in gjSecondAttend.DefaultIfEmpty()      // LEFT JOIN
                                   join thirdAttend in _db.UserDefinedCodes on e.ThirdAttendanceModeCode equals thirdAttend.UDCCode into gjThirdAttend from subThirdAttend in gjThirdAttend.DefaultIfEmpty()      // LEFT JOIN
                                   where e.EmployeeId == employeeId
                                    select new
                                    {
                                        Employee = e,
                                        NationalityDesc = nc.UDCDesc1,
                                        ReligionDesc = rc.UDCDesc1,
                                        GenderDesc = gd.UDCDesc1,
                                        MaritalStatusDesc = ms.UDCDesc1,
                                        EmploymentType = employ.UDCDesc1,
                                        RoleType = role.UDCDesc1,
                                        FirstAttendanceMode = firstAttend.UDCDesc1,
                                        SalutationDesc = subSL != null ? subSL.UDCDesc1 : null,                                        
                                        EmployeeStatusDesc = subES != null ? subES.UDCDesc1 : null,
                                        EmployeeClassDesc = subEC != null ? subEC.UDCDesc1 : null,
                                        EducationDesc = subEL != null ? subEL.UDCDesc1 : null,
                                        AccountTypeDesc = subAC != null ? subAC.UDCDesc1 : null,
                                        BankName = subBN != null ? subBN.UDCDesc1 : null,
                                        JobTitleDesc = subJT != null ? subJT.UDCDesc1 : null,
                                        PayGradeDesc = subPG != null ? subPG.UDCDesc1 : null,
                                        PresentCountryDesc = subPC != null ? subPC.UDCDesc1 : null,
                                        PermanentCountryDesc = subPA != null ? subPA.UDCDesc1 : null,
                                        DepartmentName = subDEP != null ? subDEP.DepartmentName : null,
                                        ReportingManager = subMngr != null ? $"{subMngr.FirstName} {subMngr.MiddleName} {subMngr.LastName}" : null,
                                        SecondAttendanceMode = subSecondAttend != null ? subSecondAttend.UDCDesc1 : null,
                                        ThirdAttendanceMode = subThirdAttend != null ? subThirdAttend.UDCDesc1 : null
                                    }).FirstOrDefaultAsync();
                #endregion

                if (model != null)
                {
                    #region Bind employee data
                    employeeDetail = new Employee()
                    {
                        EmployeeId = model.Employee.EmployeeId,
                        FirstName = model.Employee.FirstName,
                        MiddleName = model.Employee.MiddleName,
                        LastName = model.Employee.LastName,
                        Position = model.Employee.Position,
                        DOB = model.Employee.DOB,
                        NationalityCode = model.Employee.NationalityCode,
                        NationalityDesc = model.NationalityDesc,
                        ReligionCode = model.Employee.ReligionCode,
                        ReligionDesc = model.ReligionDesc,
                        GenderCode = model.Employee.GenderCode,
                        GenderDesc = model.GenderDesc,
                        MaritalStatusCode = model.Employee.MaritalStatusCode,
                        MaritalStatusDesc = model.MaritalStatusDesc,
                        Salutation = model.Employee.Salutation,
                        SalutationDesc = model.SalutationDesc,
                        OfficialEmail = model.Employee.OfficialEmail,
                        PersonalEmail = model.Employee.PersonalEmail,
                        AlternateEmail = model.Employee.AlternateEmail,
                        OfficeLandlineNo = model.Employee.OfficeLandlineNo,
                        ResidenceLandlineNo = model.Employee.ResidenceLandlineNo,
                        OfficeExtNo = model.Employee.OfficeExtNo,
                        MobileNo = model.Employee.MobileNo,
                        AlternateMobileNo = model.Employee.AlternateMobileNo,
                        EmployeeNo = model.Employee.EmployeeNo,
                        ReportingManagerCode = model.Employee.ReportingManagerCode,
                        ReportingManager = model.ReportingManager,  
                        WorkPermitID = model.Employee.WorkPermitID,
                        WorkPermitExpiryDate = model.Employee.WorkPermitExpiryDate,
                        HireDate = model.Employee.HireDate,
                        DateOfConfirmation = model.Employee.DateOfConfirmation,
                        TerminationDate = model.Employee.TerminationDate,
                        DateOfSuperannuation = model.Employee.DateOfSuperannuation,
                        Reemployed = model.Employee.Reemployed,
                        OldEmployeeNo = model.Employee.OldEmployeeNo,
                        DepartmentCode = model.Employee.DepartmentCode,
                        DepartmentName = model.DepartmentName,
                        EmploymentTypeCode = model.Employee.EmploymentTypeCode,
                        EmploymentType = model.EmploymentType,
                        RoleCode = model.Employee.RoleCode,
                        RoleType = model.RoleType,
                        FirstAttendanceModeCode = model.Employee.FirstAttendanceModeCode,
                        FirstAttendanceMode = model.FirstAttendanceMode,
                        SecondAttendanceModeCode = model.Employee.SecondAttendanceModeCode,
                        SecondAttendanceMode = model.SecondAttendanceMode,
                        ThirdAttendanceModeCode = model.Employee.ThirdAttendanceModeCode,
                        ThirdAttendanceMode = model.ThirdAttendanceMode,
                        SecondReportingManagerCode = model.Employee.SecondReportingManagerCode,
                        SecondReportingManager = model.Employee.SecondReportingManager,
                        Company = model.Employee.Company,
                        CompanyBranch= model.Employee.CompanyBranch,
                        EducationCode = model.Employee.EducationCode,
                        EducationDesc = model.EducationDesc,
                        EmployeeClassCode = model.Employee.EmployeeClassCode,
                        EmployeeClassDesc = model.EmployeeClassDesc,
                        JobTitleCode = model.Employee.JobTitleCode,
                        JobTitleDesc = model.JobTitleDesc,
                        PayGrade = model.Employee.PayGrade,
                        PayGradeDesc = model.PayGradeDesc,
                        IsActive = model.Employee.IsActive,
                        AccountTypeCode = model.Employee.AccountTypeCode,
                        AccountTypeDesc = model.AccountTypeDesc,
                        AccountNumber = model.Employee.AccountNumber,
                        AccountHolderName = model.Employee.AccountHolderName,
                        BankNameCode = model.Employee.BankNameCode,
                        BankName= model.BankName,
                        BankBranchName = model.Employee.BankBranchName,
                        IBANNumber = model.Employee.IBANNumber,
                        TaxNumber = model.Employee.TaxNumber,
                        LinkedInAccount = model.Employee.LinkedInAccount,
                        FacebookAccount = model.Employee.FacebookAccount,
                        TwitterAccount = model.Employee.TwitterAccount,
                        InstagramAccount = model.Employee.InstagramAccount,
                        PresentAddress = model.Employee.PresentAddress,
                        PresentCountryCode = model.Employee.PresentCountryCode,
                        PresentCountryDesc = model.PresentCountryDesc,
                        PresentCity = model.Employee.PresentCity,
                        PresentAreaCode = model.Employee.PresentAreaCode,
                        PresentContactNo = model.Employee.PresentContactNo,
                        PresentMobileNo = model.Employee.PresentMobileNo,
                        PermanentAddress = model.Employee.PermanentAddress,
                        PermanentCountryCode = model.Employee.PermanentCountryCode,
                        PermanentCountryDesc = model.PermanentCountryDesc,
                        PermanentCity = model.Employee.PermanentCity,
                        PermanentAreaCode = model.Employee.PermanentAreaCode,
                        PermanentContactNo = model.Employee.PermanentContactNo,
                        PermanentMobileNo = model.Employee.PermanentMobileNo,
                        EmployeeStatusCode = model.Employee.EmployeeStatusCode,
                        EmployeeStatusDesc = model.EmployeeStatusDesc                        
                    };
                    #endregion

                    if (employeeDetail != null)
                    {
                        #region Get Employee Identity Proof
                        var identityModel = await (from ip in _db.IdentityProofs
                                                   join nid in _db.UserDefinedCodes on ip.NationalIDTypeCode equals nid.UDCCode into gjNID from subNID in gjNID.DefaultIfEmpty()   // LEFT JOIN 
                                                   join vc in _db.UserDefinedCodes on ip.VisaCountryCode equals vc.UDCCode into gjVC from subVC in gjVC.DefaultIfEmpty()           // LEFT JOIN 
                                                   join vt in _db.UserDefinedCodes on ip.VisaTypeCode equals vt.UDCCode into gjVT from subVT in gjVT.DefaultIfEmpty()              // LEFT JOIN 
                                                   where ip.EmployeeNo == employeeDetail.EmployeeNo
                                                   select new
                                                   {
                                                       IdentityProof = ip,
                                                       NationalIDTypeDesc = subNID != null ? subNID.UDCDesc1 : null,
                                                       VisaCountryDesc = subVC != null ? subVC.UDCDesc1 : null,
                                                       VisaTypeDesc = subVT != null ? subVT.UDCDesc1 : null
                                                   }).FirstOrDefaultAsync();

                        if (identityModel != null)
                        {
                            employeeDetail.IdentityProof = new IdentityProof()
                            {
                                AutoId = identityModel.IdentityProof.AutoId,
                                PassportNumber = identityModel.IdentityProof.PassportNumber,
                                DateOfIssue = identityModel.IdentityProof.DateOfIssue,
                                DateOfExpiry = identityModel.IdentityProof.DateOfExpiry,
                                PlaceOfIssue = identityModel.IdentityProof.PlaceOfIssue,
                                DrivingLicenseNo = identityModel.IdentityProof.DrivingLicenseNo,
                                DLDateOfIssue = identityModel.IdentityProof.DLDateOfIssue,
                                DLDateOfExpiry = identityModel.IdentityProof.DLDateOfExpiry,
                                DLPlaceOfIssue = identityModel.IdentityProof.DLPlaceOfIssue,
                                TypeOfDocument = identityModel.IdentityProof.TypeOfDocument,
                                OtherDocNumber = identityModel.IdentityProof.OtherDocNumber,
                                OtherDocDateOfIssue = identityModel.IdentityProof.OtherDocDateOfIssue,
                                OtherDocDateOfExpiry = identityModel.IdentityProof.OtherDocDateOfExpiry,
                                NationalIDNumber = identityModel.IdentityProof.NationalIDNumber,
                                NationalIDTypeCode = identityModel.IdentityProof.NationalIDTypeCode,
                                NationalIDTypeDesc = identityModel.NationalIDTypeDesc,
                                NatIDPlaceOfIssue = identityModel.IdentityProof.NatIDPlaceOfIssue,
                                NatIDDateOfIssue = identityModel.IdentityProof.NatIDDateOfIssue,
                                NatIDDateOfExpiry = identityModel.IdentityProof.NatIDDateOfExpiry,
                                ContractNumber = identityModel.IdentityProof.ContractNumber,
                                ContractPlaceOfIssue = identityModel.IdentityProof.ContractPlaceOfIssue,
                                ContractDateOfIssue = identityModel.IdentityProof.ContractDateOfIssue,
                                ContractDateOfExpiry = identityModel.IdentityProof.ContractDateOfExpiry,
                                VisaNumber = identityModel.IdentityProof.VisaNumber,
                                VisaTypeCode = identityModel.IdentityProof.VisaTypeCode,
                                VisaTypeDesc = identityModel.VisaTypeDesc,
                                VisaCountryCode = identityModel.IdentityProof.VisaCountryCode,
                                VisaCountryDesc = identityModel.VisaCountryDesc,
                                Profession = identityModel.IdentityProof.Profession,
                                Sponsor = identityModel.IdentityProof.Sponsor,
                                VisaDateOfIssue = identityModel.IdentityProof.VisaDateOfIssue,
                                VisaDateOfExpiry = identityModel.IdentityProof.VisaDateOfExpiry
                            };
                        }
                        #endregion

                        #region Get Emergency Contacts
                        var emergencyContactModel = await (from ec in _db.EmergencyContacts
                                                           join rel in _db.UserDefinedCodes on ec.RelationCode equals rel.UDCCode 
                                                           join cty in _db.UserDefinedCodes on ec.CountryCode equals cty.UDCCode into gjCTY from subCTY in gjCTY.DefaultIfEmpty()   // LEFT JOIN 
                                                           where ec.EmployeeNo == employeeDetail.EmployeeNo
                                                           select new
                                                           {
                                                               EmergencyContact = ec,
                                                               Relation = rel.UDCDesc1,
                                                               CountryDesc = subCTY != null ? subCTY.UDCDesc1 : null
                                                           }).ToListAsync();
                        if (emergencyContactModel != null)
                        {
                            foreach (var item in emergencyContactModel)
                            {
                                employeeDetail.EmergencyContactList.Add(new EmergencyContact()
                                {
                                    AutoId = item.EmergencyContact.AutoId,
                                    ContactPerson = item.EmergencyContact.ContactPerson,
                                    RelationCode = item.EmergencyContact.RelationCode,
                                    Relation = item.Relation,
                                    MobileNo = item.EmergencyContact.MobileNo,
                                    LandlineNo = item.EmergencyContact.LandlineNo,
                                    Address = item.EmergencyContact.Address,
                                    CountryCode = item.EmergencyContact.CountryCode,
                                    CountryDesc = item.CountryDesc,
                                    City = item.EmergencyContact.City,
                                    EmployeeNo = item.EmergencyContact.EmployeeNo,
                                    TransactionNo = item.EmergencyContact.TransactionNo
                                });
                            }
                        }
                        #endregion

                        #region Get Qualifications
                        var qualificationModel = await (from q in _db.Qualifications
                                                        join qCode in _db.UserDefinedCodes on q.QualificationCode equals qCode.UDCCode     // INNER JOIN
                                                        join qMode in _db.UserDefinedCodes on q.QualificationMode equals qMode.UDCCode     // INNER JOIN
                                                        join fMonth in _db.UserDefinedCodes on q.FromMonthCode equals fMonth.UDCCode       // INNER JOIN
                                                        join tMonth in _db.UserDefinedCodes on q.ToMonthCode equals tMonth.UDCCode         // INNER JOIN
                                                        join pMonth in _db.UserDefinedCodes on q.PassMonthCode equals pMonth.UDCCode       // INNER JOIN
                                                        join stream in _db.UserDefinedCodes on q.StreamCode equals stream.UDCCode into gjStream from subStream in gjStream.DefaultIfEmpty()                // LEFT JOIN 
                                                        join spec in _db.UserDefinedCodes on q.SpecializationCode equals spec.UDCCode into gjSpec from subSpec in gjSpec.DefaultIfEmpty()                  // LEFT JOIN 
                                                        join country in _db.UserDefinedCodes on q.CountryCode equals country.UDCCode into gjCountry from subCountry in gjCountry.DefaultIfEmpty()          // LEFT JOIN 
                                                        join state in _db.UserDefinedCodes on q.StateCode equals state.UDCCode into gjState from subState in gjState.DefaultIfEmpty()          // LEFT JOIN 
                                                        where q.EmployeeNo == employeeDetail.EmployeeNo
                                                        select new
                                                        {
                                                            Qualification = q,
                                                            QualificationDesc = qCode.UDCDesc1,
                                                            QualificationModeDesc = qMode.UDCDesc1,
                                                            FromMonthDesc = fMonth.UDCDesc1,
                                                            ToMonthDesc = tMonth.UDCDesc1,
                                                            PassMonthDesc = pMonth.UDCDesc1,
                                                            StreamDesc = subStream != null ? subStream.UDCDesc1 : null,
                                                            SpecializationDesc = subSpec != null ? subSpec.UDCDesc1 : null,
                                                            CountryDesc = subCountry != null ? subCountry.UDCDesc1 : null,
                                                            StateDesc = subState != null ? subState.UDCDesc1 : null
                                                        }).ToListAsync();
                        if (qualificationModel != null)
                        {
                            foreach (var item in qualificationModel)
                            {
                                employeeDetail.Qualifications.Add(new Qualification()
                                {
                                    AutoId = item.Qualification.AutoId,
                                    QualificationCode  = item.Qualification.QualificationCode,
                                    QualificationDesc  = item.QualificationDesc,
                                    StreamCode  = item.Qualification.StreamCode,
                                    StreamDesc  = item.StreamDesc,
                                    SpecializationCode  = item.Qualification.SpecializationCode,
                                    SpecializationDesc  = item.SpecializationDesc,
                                    UniversityName  = item.Qualification.UniversityName,
                                    Institute  = item.Qualification.Institute,
                                    QualificationMode  = item.Qualification.QualificationMode,
                                    QualificationModeDesc  = item.QualificationModeDesc,
                                    CountryCode  = item.Qualification.CountryCode,
                                    CountryDesc  = item.CountryDesc,
                                    StateCode  = item.Qualification.StateCode,
                                    StateDesc  = item.StateDesc,
                                    CityTownName  = item.Qualification.CityTownName,
                                    FromMonthCode  = item.Qualification.FromMonthCode,
                                    FromMonthDesc  = item.FromMonthDesc,
                                    FromYear  = item.Qualification.FromYear,
                                    ToMonthCode  = item.Qualification.ToMonthCode,
                                    ToMonthDesc  = item.ToMonthDesc,
                                    ToYear  = item.Qualification.ToYear,
                                    PassMonthCode  = item.Qualification.PassMonthCode,
                                    PassMonthDesc  = item.PassMonthDesc,
                                    PassYear  = item.Qualification.PassYear
                                });
                            }
                        }
                        #endregion

                        #region Get Employee Skills
                        var skillModel = await (from s in _db.EmployeeSkills
                                                        join level in _db.UserDefinedCodes on s.LevelCode equals level.UDCCode into gjLevel from subLevel in gjLevel.DefaultIfEmpty()      // LEFT JOIN 
                                                        join lastUsedMonth in _db.UserDefinedCodes on s.LastUsedMonthCode equals lastUsedMonth.UDCCode into gjLastUsedMonth from subLastUsedMonth in gjLastUsedMonth.DefaultIfEmpty()      // LEFT JOIN 
                                                        join fromMonth in _db.UserDefinedCodes on s.FromMonthCode equals fromMonth.UDCCode into gjFromMonth from subFromMonth in gjFromMonth.DefaultIfEmpty()      // LEFT JOIN 
                                                        join toMonth in _db.UserDefinedCodes on s.ToMonthCode equals toMonth.UDCCode into gjToMonth from subToMonth in gjToMonth.DefaultIfEmpty()      // LEFT JOIN                                                         
                                                        where s.EmployeeNo == employeeDetail.EmployeeNo
                                                        select new
                                                        {
                                                            EmployeeSkill = s,
                                                            LevelDesc = subLevel != null ? subLevel.UDCDesc1 : null,
                                                            LastUsedMonthDesc = subLastUsedMonth != null ? subLastUsedMonth.UDCDesc1 : null,
                                                            FromMonthDesc = subFromMonth != null ? subFromMonth.UDCDesc1 : null,
                                                            ToMonthDesc = subToMonth != null ? subToMonth.UDCDesc1 : null,
                                                        }).ToListAsync();
                        if (skillModel != null)
                        {
                            foreach (var item in skillModel)
                            {
                                employeeDetail.EmployeeSkills.Add(new EmployeeSkill()
                                {
                                    AutoId = item.EmployeeSkill.AutoId,
                                    SkillName = item.EmployeeSkill.SkillName,
                                    LevelCode = item.EmployeeSkill.LevelCode,
                                    LevelDesc = item.LevelDesc,
                                    LastUsedMonthCode = item.EmployeeSkill.LastUsedMonthCode,
                                    LastUsedMonthDesc = item.LastUsedMonthDesc,
                                    LastUsedYear = item.EmployeeSkill.LastUsedYear,
                                    FromMonthCode = item.EmployeeSkill.FromMonthCode,
                                    FromMonthDesc = item.FromMonthDesc,
                                    FromYear = item.EmployeeSkill.FromYear,
                                    ToMonthCode = item.EmployeeSkill.ToMonthCode,
                                    ToMonthDesc = item.ToMonthDesc,
                                    ToYear = item.EmployeeSkill.ToYear
                                });
                            }
                        }
                        #endregion

                        #region Get Certifications & Trainings
                        var certificationModel = await (from e in _db.EmployeeCertifications
                                                join qualification in _db.UserDefinedCodes on e.QualificationCode equals qualification.UDCCode     // INNER JOIN 
                                                join fromMonth in _db.UserDefinedCodes on e.FromMonthCode equals fromMonth.UDCCode                 // INNER JOIN 
                                                join toMonth in _db.UserDefinedCodes on e.ToMonthCode equals toMonth.UDCCode                       // INNER JOIN                                                         
                                                join passMonth in _db.UserDefinedCodes on e.PassMonthCode equals passMonth.UDCCode                 // INNER JOIN
                                                join country in _db.UserDefinedCodes on e.CountryCode equals country.UDCCode into gjCountry from subCountry in gjCountry.DefaultIfEmpty()      // LEFT JOIN                                                                                                        
                                                join stream in _db.UserDefinedCodes on e.StreamCode equals stream.UDCCode into gjStream from subStream in gjStream.DefaultIfEmpty()      // LEFT JOIN 
                                                where e.EmployeeNo == employeeDetail.EmployeeNo
                                                select new
                                                {
                                                    EmployeeCertification = e,
                                                    QualificationDesc = qualification.UDCDesc1,
                                                    FromMonth = fromMonth.UDCDesc1,
                                                    ToMonth = toMonth.UDCDesc1,
                                                    PassMonth = passMonth.UDCDesc1,
                                                    Country = subCountry != null ? subCountry.UDCDesc1 : null,
                                                    StreamDesc = subStream != null ? subStream.UDCDesc1 : null
                                                }).ToListAsync();
                        if (certificationModel != null)
                        {
                            foreach (var item in certificationModel)
                            {
                                employeeDetail.EmployeeCertifications.Add(new EmployeeCertification()
                                {
                                    AutoId = item.EmployeeCertification.AutoId,
                                    QualificationCode = item.EmployeeCertification.QualificationCode,
                                    QualificationDesc = item.QualificationDesc,
                                    StreamCode = item.EmployeeCertification.StreamCode,
                                    StreamDesc = item.StreamDesc,
                                    Specialization = item.EmployeeCertification.Specialization,
                                    University = item.EmployeeCertification.University,
                                    Institute = item.EmployeeCertification.Institute,
                                    CountryCode = item.EmployeeCertification.CountryCode,
                                    Country = item.Country,
                                    State = item.EmployeeCertification.State,
                                    CityTownName = item.EmployeeCertification.CityTownName,
                                    FromMonthCode = item.EmployeeCertification.FromMonthCode,
                                    FromMonth = item.FromMonth,
                                    FromYear = item.EmployeeCertification.FromYear,
                                    ToMonthCode = item.EmployeeCertification.ToMonthCode,
                                    ToMonth = item.ToMonth,
                                    ToYear = item.EmployeeCertification.ToYear,
                                    PassMonthCode = item.EmployeeCertification.PassMonthCode,
                                    PassMonth = item.PassMonth,
                                    PassYear = item.EmployeeCertification.PassYear
                                });
                            }
                        }
                        #endregion

                        #region Get Language Skills
                        var languageModel = await (from l in _db.LanguageSkills
                                                        join langCode in _db.UserDefinedCodes on l.LanguageCode equals langCode.UDCCode     // INNER JOIN 
                                                        where l.EmployeeNo == employeeDetail.EmployeeNo
                                                        select new
                                                        {
                                                            LanguageSkill = l,
                                                            LanguageDesc = langCode.UDCDesc1
                                                        }).ToListAsync();
                        if (languageModel != null)
                        {
                            foreach (var item in languageModel)
                            {
                                employeeDetail.LanguageSkills.Add(new LanguageSkill()
                                {
                                    AutoId = item.LanguageSkill.AutoId,
                                    LanguageCode = item.LanguageSkill.LanguageCode,
                                    LanguageDesc = item.LanguageDesc,
                                    CanWrite = item.LanguageSkill.CanWrite,
                                    CanSpeak = item.LanguageSkill.CanSpeak,
                                    CanRead = item.LanguageSkill.CanRead,
                                    MotherTongue = item.LanguageSkill.MotherTongue
                                });
                            }
                        }
                        #endregion

                        #region Get Family Visas
                        var familyVisaModel = await (from f in _db.FamilyVisas
                                                    join fm in _db.FamilyMembers on f.FamilyId equals fm.AutoId    // INNER JOIN 
                                                    join visaType in _db.UserDefinedCodes on f.VisaTypeCode equals visaType.UDCCode    // INNER JOIN 
                                                    join country in _db.UserDefinedCodes on f.CountryCode equals country.UDCCode       // INNER JOIN 
                                                    where f.EmployeeNo == employeeDetail.EmployeeNo
                                                    select new
                                                    {
                                                        FamilyVisa = f,
                                                        FamilyMemberName = $"{fm.FirstName} {fm.MiddleName} {fm.LastName}",
                                                        VisaType = visaType.UDCDesc1,
                                                        Country = country.UDCDesc1
                                                    }).ToListAsync();
                        if (familyVisaModel != null)
                        {
                            foreach (var item in familyVisaModel)
                            {
                                employeeDetail.FamilyVisas.Add(new FamilyVisa()
                                {
                                    AutoId = item.FamilyVisa.AutoId,
                                    FamilyId = item.FamilyVisa.FamilyId,    
                                    FamilyMemberName = item.FamilyMemberName,
                                    CountryCode = item.FamilyVisa.CountryCode,
                                    Country = item.Country,
                                    VisaTypeCode = item.FamilyVisa.VisaTypeCode,
                                    VisaType = item.VisaType,
                                    Profession = item.FamilyVisa.Profession,
                                    IssueDate = item.FamilyVisa.IssueDate,
                                    ExpiryDate = item.FamilyVisa.ExpiryDate
                                });
                            }
                        }
                        #endregion

                        #region Get Family Members
                        var familyMemberModel = await (from f in _db.FamilyMembers
                                                        join relation in _db.UserDefinedCodes on f.RelationCode equals relation.UDCCode    // INNER JOIN 
                                                        join qualification in _db.UserDefinedCodes on f.QualificationCode equals qualification.UDCCode into gjQualification from subQualification in gjQualification.DefaultIfEmpty()      // LEFT JOIN    
                                                        join stream in _db.UserDefinedCodes on f.StreamCode equals stream.UDCCode into gjStream from subStream in gjStream.DefaultIfEmpty()      // LEFT JOIN 
                                                        join spec in _db.UserDefinedCodes on f.SpecializationCode equals spec.UDCCode into gjSpec from subSpec in gjSpec.DefaultIfEmpty()                  // LEFT JOIN 
                                                        join country in _db.UserDefinedCodes on f.CountryCode equals country.UDCCode into gjCountry from subCountry in gjCountry.DefaultIfEmpty()          // LEFT JOIN 
                                                        join state in _db.UserDefinedCodes on f.StateCode equals state.UDCCode into gjState from subState in gjState.DefaultIfEmpty()          // LEFT JOIN 
                                                        where f.EmployeeNo == employeeDetail.EmployeeNo
                                                         select new
                                                         {
                                                             FamilyMember = f,
                                                             Relation = relation.UDCDesc1,
                                                             Qualification = subQualification != null ? subQualification.UDCDesc1 : null,
                                                             StreamDesc = subStream != null ? subStream.UDCDesc1 : null,
                                                             Specialization = subSpec != null ? subSpec.UDCDesc1 : null,
                                                             Country = subCountry != null ? subCountry.UDCDesc1 : null,
                                                             State = subState != null ? subState.UDCDesc1 : null
                                                         }).ToListAsync();
                        if (familyMemberModel != null)
                        {
                            foreach (var item in familyMemberModel)
                            {
                                employeeDetail.FamilyMembers.Add(new FamilyMember()
                                {
                                    AutoId = item.FamilyMember.AutoId,
                                    FirstName = item.FamilyMember.FirstName,
                                    MiddleName = item.FamilyMember.MiddleName,
                                    LastName = item.FamilyMember.LastName,
                                    RelationCode = item.FamilyMember.RelationCode,
                                    Relation = item.Relation,
                                    DOB = item.FamilyMember.DOB,
                                    QualificationCode = item.FamilyMember.QualificationCode,
                                    Qualification = item.Qualification,
                                    StreamCode = item.FamilyMember.StreamCode,
                                    StreamDesc = item.StreamDesc,
                                    SpecializationCode = item.FamilyMember.SpecializationCode,
                                    Specialization = item.Specialization,
                                    Occupation = item.FamilyMember.Occupation,
                                    ContactNo = item.FamilyMember.ContactNo,
                                    CountryCode = item.FamilyMember.CountryCode,
                                    Country = item.Country,
                                    StateCode = item.FamilyMember.StateCode,
                                    State = item.State,
                                    CityTownName = item.FamilyMember.CityTownName,
                                    District = item.FamilyMember.District,
                                    IsDependent = item.FamilyMember.IsDependent
                                });
                            }
                        }
                        #endregion

                        #region Get Other Documents
                        var otherDocumentModel = await (from d in _db.OtherDocuments
                                                       join docType in _db.UserDefinedCodes on d.DocumentTypeCode equals docType.UDCCode    // INNER JOIN 
                                                       join contentType in _db.UserDefinedCodes on d.ContentTypeCode equals contentType.UDCCode into gjContentType from subContentType in gjContentType.DefaultIfEmpty()      // LEFT JOIN    
                                                       where d.EmployeeNo == employeeDetail.EmployeeNo
                                                       select new
                                                       {
                                                           OtherDocument = d,
                                                           DocumentTypeDesc = docType.UDCDesc1,
                                                           ContentTypeDesc = subContentType != null ? subContentType.UDCDesc1 : null
                                                       }).ToListAsync();
                        if (otherDocumentModel != null)
                        {
                            foreach (var item in otherDocumentModel)
                            {
                                employeeDetail.OtherDocuments.Add(new OtherDocument()
                                {
                                    AutoId = item.OtherDocument.AutoId,
                                    DocumentName = item.OtherDocument.DocumentName,
                                    DocumentTypeCode = item.OtherDocument.DocumentTypeCode,
                                    DocumentTypeDesc = item.DocumentTypeDesc,
                                    Description = item.OtherDocument.Description,
                                    FileData = item.OtherDocument.FileData,
                                    FileExtension = item.OtherDocument.FileExtension,
                                    ContentTypeCode = item.OtherDocument.ContentTypeCode,
                                    ContentTypeDesc = item.ContentTypeDesc,
                                    UploadDate = item.OtherDocument.UploadDate
                                });
                            }
                        }
                        #endregion

                        #region Get Employment History
                        var employmentHistoryModel = await (from e in _db.EmploymentHistories
                                                            join salType in _db.UserDefinedCodes on e.SalaryTypeCode equals salType.UDCCode into gjSalType from subSalType in gjSalType.DefaultIfEmpty()      // LEFT JOIN    
                                                            join salCurrency in _db.UserDefinedCodes on e.SalaryCurrencyCode equals salCurrency.UDCCode into gjSalCurrency from subSalCurrency in gjSalCurrency.DefaultIfEmpty()      // LEFT JOIN    
                                                            where e.EmployeeNo == employeeDetail.EmployeeNo
                                                            select new
                                                            {
                                                                EmploymentHistory = e,
                                                                SalaryType = subSalType != null ? subSalType.UDCDesc1 : null,
                                                                SalaryCurrency = subSalCurrency != null ? subSalCurrency.UDCDesc1 : null
                                                            }).ToListAsync();
                        if (employmentHistoryModel != null)
                        {
                            foreach (var item in employmentHistoryModel)
                            {
                                employeeDetail.EmploymentHistories.Add(new EmploymentHistory()
                                {
                                    AutoId = item.EmploymentHistory.AutoId,
                                    CompanyName = item.EmploymentHistory.CompanyName,
                                    CompanyAddress = item.EmploymentHistory.CompanyAddress,
                                    Designation = item.EmploymentHistory.Designation,
                                    Role = item.EmploymentHistory.Role,
                                    FromDate = item.EmploymentHistory.FromDate,
                                    ToDate = item.EmploymentHistory.ToDate,
                                    LastDrawnSalary = item.EmploymentHistory.LastDrawnSalary,
                                    SalaryTypeCode = item.EmploymentHistory.SalaryTypeCode,
                                    SalaryType = item.SalaryType,
                                    SalaryCurrencyCode = item.EmploymentHistory.SalaryCurrencyCode,
                                    SalaryCurrency = item.SalaryCurrency,
                                    ReasonOfChange = item.EmploymentHistory.ReasonOfChange,
                                    ReportingManager = item.EmploymentHistory.ReportingManager,
                                    CompanyWebsite = item.EmploymentHistory.CompanyWebsite
                                });
                            }
                        }
                        #endregion
                    }
                }

                return Result<Employee?>.SuccessResult(employeeDetail);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<Employee?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveEmployeeAsync(Employee dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                var employee = await _db.Employees.FirstOrDefaultAsync(x => x.EmployeeNo == dto.EmployeeNo, cancellationToken);
                if (employee == null) 
                    throw new InvalidOperationException("Employee not found");

                #region Update Employee entity

                #region Personal Detail         
                employee.FirstName = dto.FirstName;
                employee.MiddleName = dto.MiddleName;
                employee.LastName = dto.LastName;
                employee.Position = dto.Position;
                employee.DOB = dto.DOB;
                employee.NationalityCode = dto.NationalityCode;
                employee.ReligionCode = dto.ReligionCode;
                employee.GenderCode = dto.GenderCode;
                employee.MaritalStatusCode = dto.MaritalStatusCode;
                employee.Salutation = dto.Salutation;
                #endregion

                #region Contact Detail Implementation
                employee.OfficialEmail = dto.OfficialEmail;
                employee.PersonalEmail = dto.PersonalEmail;
                employee.AlternateEmail = dto.AlternateEmail;
                employee.OfficeLandlineNo = dto.OfficeLandlineNo;
                employee.ResidenceLandlineNo = dto.ResidenceLandlineNo;
                employee.OfficeExtNo = dto.OfficeExtNo;
                employee.MobileNo = dto.MobileNo;
                employee.AlternateMobileNo = dto.AlternateMobileNo;
                #endregion

                #region Employment Detail Implementation
                employee.EmployeeNo = dto.EmployeeNo;
                employee.EmployeeStatusCode = dto.EmployeeStatusCode;
                employee.ReportingManagerCode = dto.ReportingManagerCode;
                employee.WorkPermitID = dto.WorkPermitID;
                employee.WorkPermitExpiryDate = dto.WorkPermitExpiryDate;
                employee.HireDate = dto.HireDate;
                employee.DateOfConfirmation = dto.DateOfConfirmation;
                employee.TerminationDate = dto.TerminationDate;
                employee.DateOfSuperannuation = dto.DateOfSuperannuation;
                employee.Reemployed = dto.Reemployed;
                employee.OldEmployeeNo = dto.OldEmployeeNo;
                employee.DepartmentCode = dto.DepartmentCode;
                employee.EmploymentTypeCode = dto.EmploymentTypeCode;
                employee.RoleCode = dto.RoleCode;
                employee.FirstAttendanceModeCode = dto.FirstAttendanceModeCode;
                employee.SecondAttendanceModeCode = dto.SecondAttendanceModeCode;
                employee.ThirdAttendanceModeCode = dto.ThirdAttendanceModeCode;
                employee.SecondReportingManagerCode = dto.SecondReportingManagerCode;
                #endregion

                #region Attribute Detail Implementation
                employee.Company = dto.Company;
                employee.CompanyBranch = dto.CompanyBranch;
                employee.EducationCode = dto.EducationCode;
                employee.EmployeeClassCode = dto.EmployeeClassCode;
                employee.JobTitleCode = dto.JobTitleCode;
                employee.PayGrade = dto.PayGrade;
                employee.IsActive = dto.IsActive;
                #endregion

                #region Bank Detail Implementation  
                employee.AccountTypeCode = dto.AccountTypeCode;
                employee.AccountNumber = dto.AccountNumber;
                employee.AccountHolderName = dto.AccountHolderName;
                employee.BankNameCode = dto.BankNameCode;
                employee.BankBranchName = dto.BankBranchName;
                employee.IBANNumber = dto.IBANNumber;
                employee.TaxNumber = dto.TaxNumber;
                #endregion

                #region Social Connect Implementation
                employee.LinkedInAccount = dto.LinkedInAccount;
                employee.FacebookAccount = dto.FacebookAccount;
                employee.TwitterAccount = dto.TwitterAccount;
                employee.InstagramAccount = dto.InstagramAccount;
                #endregion

                #region Primary Location Implementation
                employee.PresentAddress = dto.PresentAddress;
                employee.PresentCountryCode = dto.PresentCountryCode;
                employee.PresentCity = dto.PresentCity;
                employee.PresentAreaCode = dto.PresentAreaCode;
                employee.PresentContactNo = dto.PresentContactNo;
                employee.PresentMobileNo = dto.PresentMobileNo;
                employee.PermanentAddress = dto.PermanentAddress;
                employee.PermanentCountryCode = dto.PermanentCountryCode;
                employee.PermanentCity = dto.PermanentCity;
                employee.PermanentAreaCode = dto.PermanentAreaCode;
                employee.PermanentContactNo = dto.PermanentContactNo;
                employee.PermanentMobileNo = dto.PermanentMobileNo;
                #endregion

                #endregion

                #region Update IdentityProof entity
                if (dto.IdentityProof != null)
                {
                    var existingIdentityProof = await _db.IdentityProofs
                           .FirstOrDefaultAsync(e => e.EmployeeNo == dto.EmployeeNo && e.AutoId == dto.IdentityProof.AutoId, cancellationToken);

                    if (existingIdentityProof != null)
                    {
                        // Update existing Identity Proof record
                        existingIdentityProof.AutoId = dto.IdentityProof!.AutoId;
                        existingIdentityProof.PassportNumber = dto.IdentityProof!.PassportNumber;
                        existingIdentityProof.DateOfIssue = dto.IdentityProof!.DateOfIssue;
                        existingIdentityProof.DateOfExpiry = dto.IdentityProof!.DateOfExpiry;
                        existingIdentityProof.PlaceOfIssue = dto.IdentityProof!.PlaceOfIssue;
                        existingIdentityProof.DrivingLicenseNo = dto.IdentityProof!.DrivingLicenseNo;
                        existingIdentityProof.DLDateOfIssue = dto.IdentityProof!.DLDateOfIssue;
                        existingIdentityProof.DLDateOfExpiry = dto.IdentityProof!.DLDateOfExpiry;
                        existingIdentityProof.DLPlaceOfIssue = dto.IdentityProof!.DLPlaceOfIssue;
                        existingIdentityProof.TypeOfDocument = dto.IdentityProof!.TypeOfDocument;
                        existingIdentityProof.OtherDocNumber = dto.IdentityProof!.OtherDocNumber;
                        existingIdentityProof.OtherDocDateOfIssue = dto.IdentityProof!.OtherDocDateOfIssue;
                        existingIdentityProof.OtherDocDateOfExpiry = dto.IdentityProof!.OtherDocDateOfExpiry;
                        existingIdentityProof.NationalIDNumber = dto.IdentityProof!.NationalIDNumber;
                        existingIdentityProof.NationalIDTypeCode = dto.IdentityProof!.NationalIDTypeCode;
                        existingIdentityProof.NatIDPlaceOfIssue = dto.IdentityProof!.NatIDPlaceOfIssue;
                        existingIdentityProof.NatIDDateOfIssue = dto.IdentityProof!.NatIDDateOfIssue;
                        existingIdentityProof.NatIDDateOfExpiry = dto.IdentityProof!.NatIDDateOfExpiry;
                        existingIdentityProof.ContractNumber = dto.IdentityProof!.ContractNumber;
                        existingIdentityProof.ContractPlaceOfIssue = dto.IdentityProof!.ContractPlaceOfIssue;
                        existingIdentityProof.ContractDateOfIssue = dto.IdentityProof!.ContractDateOfIssue;
                        existingIdentityProof.ContractDateOfExpiry = dto.IdentityProof!.ContractDateOfExpiry;
                        existingIdentityProof.VisaNumber = dto.IdentityProof!.VisaNumber;
                        existingIdentityProof.VisaTypeCode = dto.IdentityProof!.VisaTypeCode;
                        existingIdentityProof.VisaCountryCode = dto.IdentityProof!.VisaCountryCode;
                        existingIdentityProof.Profession = dto.IdentityProof!.Profession;
                        existingIdentityProof.Sponsor = dto.IdentityProof!.Sponsor;
                        existingIdentityProof.VisaDateOfIssue = dto.IdentityProof!.VisaDateOfIssue;
                        existingIdentityProof.VisaDateOfExpiry = dto.IdentityProof!.VisaDateOfExpiry;
                    }
                }
                #endregion

                #region Update EmergencyContact entity
                if (dto.EmergencyContactList != null && dto.EmergencyContactList.Count > 0  )
                {
                    #region Delete entity items that don't exist in the DTO
                    var emergencyContactNotInDTO = _db.EmergencyContacts.AsEnumerable()
                                    .ExceptBy(dto.EmergencyContactList.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (emergencyContactNotInDTO.Any())
                    {
                        _db.EmergencyContacts.RemoveRange(emergencyContactNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var contact in dto.EmergencyContactList)
                    {
                        var existingContact = await _db.EmergencyContacts
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == contact.AutoId, cancellationToken);

                        if (existingContact != null)
                        {
                            // Update existing contact
                            existingContact.ContactPerson = contact.ContactPerson;
                            existingContact.RelationCode = contact.RelationCode;
                            existingContact.MobileNo = contact.MobileNo;
                            existingContact.LandlineNo = contact.LandlineNo;
                            existingContact.Address = contact.Address;
                            existingContact.CountryCode = contact.CountryCode;
                            existingContact.City = contact.City;
                        }
                        else
                        {
                            // Add new contact
                            var newContact = new EmergencyContact
                            {
                                EmployeeNo = dto.EmployeeNo,
                                ContactPerson = contact.ContactPerson,
                                RelationCode = contact.RelationCode,
                                MobileNo = contact.MobileNo,
                                LandlineNo = contact.LandlineNo,
                                Address = contact.Address,
                                CountryCode = contact.CountryCode,
                                City = contact.City
                            };
                            await _db.EmergencyContacts.AddAsync(newContact, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update Qualification entity
                if (dto.Qualifications != null && dto.Qualifications.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var qualificationsNotInDTO = _db.Qualifications.AsEnumerable()
                                    .ExceptBy(dto.Qualifications.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (qualificationsNotInDTO.Any())
                    {
                        _db.Qualifications.RemoveRange(qualificationsNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var qualification in dto.Qualifications)
                    {
                        var existingQualification = await _db.Qualifications
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == qualification.AutoId, cancellationToken);

                        if (existingQualification != null)
                        {
                            // Update existing contact
                            existingQualification.QualificationCode = qualification.QualificationCode; 
                            existingQualification.StreamCode = qualification.StreamCode;
                            existingQualification.SpecializationCode = qualification.SpecializationCode;
                            existingQualification.UniversityName = qualification.UniversityName;
                            existingQualification.Institute = qualification.Institute;
                            existingQualification.QualificationMode = qualification.QualificationMode;
                            existingQualification.CountryCode = qualification.CountryCode;
                            existingQualification.StateCode = qualification.StateCode;
                            existingQualification.CityTownName = qualification.CityTownName;
                            existingQualification.FromMonthCode = qualification.FromMonthCode;
                            existingQualification.FromYear = qualification.FromYear;
                            existingQualification.ToMonthCode = qualification.ToMonthCode;
                            existingQualification.ToYear = qualification.ToYear;
                            existingQualification.PassMonthCode = qualification.PassMonthCode;
                            existingQualification.PassYear = qualification.PassYear;
                        }
                        else
                        {
                            // Add new Qualification
                            var newQualification = new Qualification
                            {
                                EmployeeNo = dto.EmployeeNo,
                                QualificationCode = qualification.QualificationCode,
                                StreamCode = qualification.StreamCode,
                                SpecializationCode = qualification.SpecializationCode,
                                UniversityName = qualification.UniversityName,
                                Institute = qualification.Institute,
                                QualificationMode = qualification.QualificationMode,
                                CountryCode = qualification.CountryCode,
                                StateCode = qualification.StateCode,
                                CityTownName = qualification.CityTownName,
                                FromMonthCode = qualification.FromMonthCode,
                                FromYear = qualification.FromYear,
                                ToMonthCode = qualification.ToMonthCode,
                                ToYear = qualification.ToYear,
                                PassMonthCode = qualification.PassMonthCode,
                                PassYear = qualification.PassYear
                        };
                            await _db.Qualifications.AddAsync(newQualification, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update EmployeeSkill entity
                if (dto.EmployeeSkills != null && dto.EmployeeSkills.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var skillsNotInDTO = _db.EmployeeSkills.AsEnumerable()
                                    .ExceptBy(dto.EmployeeSkills.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (skillsNotInDTO.Any())
                    {
                        _db.EmployeeSkills.RemoveRange(skillsNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var skill in dto.EmployeeSkills)
                    {
                        var existingSkill = await _db.EmployeeSkills
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == skill.AutoId, cancellationToken);

                        if (existingSkill != null)
                        {
                            // Update existing skill
                            existingSkill.SkillName = skill.SkillName;
                            existingSkill.LevelCode = skill.LevelCode;
                            existingSkill.LastUsedMonthCode = skill.LastUsedMonthCode;
                            existingSkill.LastUsedYear = skill.LastUsedYear;
                            existingSkill.FromMonthCode = skill.FromMonthCode;
                            existingSkill.FromYear = skill.FromYear;
                            existingSkill.ToMonthCode = skill.ToMonthCode;
                            existingSkill.ToYear = skill.ToYear;
                        }
                        else
                        {
                            // Add new Skill
                            var newSkill = new EmployeeSkill
                            {
                                EmployeeNo = dto.EmployeeNo,
                                SkillName = skill.SkillName,
                                LevelCode = skill.LevelCode,
                                LastUsedMonthCode = skill.LastUsedMonthCode,
                                LastUsedYear = skill.LastUsedYear,
                                FromMonthCode = skill.FromMonthCode,
                                FromYear = skill.FromYear,
                                ToMonthCode = skill.ToMonthCode,
                                ToYear = skill.ToYear
                            };
                            await _db.EmployeeSkills.AddAsync(newSkill, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update EmployeeCertification entity
                if (dto.EmployeeCertifications != null && dto.EmployeeCertifications.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var certificationsNotInDTO = _db.EmployeeCertifications.AsEnumerable()
                                    .ExceptBy(dto.EmployeeCertifications.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (certificationsNotInDTO.Any())
                    {
                        _db.EmployeeCertifications.RemoveRange(certificationsNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var certificate in dto.EmployeeCertifications)
                    {
                        var existingCertificate = await _db.EmployeeCertifications
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == certificate.AutoId, cancellationToken);

                        if (existingCertificate != null)
                        {
                            // Update existing skill
                            existingCertificate.QualificationCode = certificate.QualificationCode;
                            existingCertificate.StreamCode = certificate.StreamCode;
                            existingCertificate.Specialization = certificate.Specialization;
                            existingCertificate.University = certificate.University;
                            existingCertificate.Institute = certificate.Institute;
                            existingCertificate.CountryCode = certificate.CountryCode;
                            existingCertificate.State = certificate.State;
                            existingCertificate.CityTownName = certificate.CityTownName;
                            existingCertificate.FromMonthCode = certificate.FromMonthCode;
                            existingCertificate.FromYear = certificate.FromYear;
                            existingCertificate.ToMonthCode = certificate.ToMonthCode;
                            existingCertificate.ToMonth = certificate.ToMonth;
                            existingCertificate.ToYear = certificate.ToYear;
                            existingCertificate.PassMonthCode = certificate.PassMonthCode;
                            existingCertificate.PassYear = certificate.PassYear;
                        }
                        else
                        {
                            // Add new Certification
                            var newCertificate = new EmployeeCertification
                            {
                                EmployeeNo = dto.EmployeeNo,
                                QualificationCode = certificate.QualificationCode,
                                StreamCode = certificate.StreamCode,
                                Specialization = certificate.Specialization,
                                University = certificate.University,
                                Institute = certificate.Institute,
                                CountryCode = certificate.CountryCode,
                                State = certificate.State,
                                CityTownName = certificate.CityTownName,
                                FromMonthCode = certificate.FromMonthCode,
                                FromYear = certificate.FromYear,
                                ToMonthCode = certificate.ToMonthCode,
                                ToMonth = certificate.ToMonth,
                                ToYear = certificate.ToYear,
                                PassMonthCode = certificate.PassMonthCode,
                                PassYear = certificate.PassYear
                            };
                            await _db.EmployeeCertifications.AddAsync(newCertificate, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update LanguageSkill entity
                if (dto.LanguageSkills != null && dto.LanguageSkills.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var languageNotInDTO = _db.LanguageSkills.AsEnumerable()
                                    .ExceptBy(dto.LanguageSkills.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (languageNotInDTO.Any())
                    {
                        _db.LanguageSkills.RemoveRange(languageNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var language in dto.LanguageSkills)
                    {
                        var existingLanguage = await _db.LanguageSkills
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == language.AutoId, cancellationToken);

                        if (existingLanguage != null)
                        {
                            // Update existing skill
                            existingLanguage.LanguageCode = language.LanguageCode;
                            existingLanguage.CanWrite = language.CanWrite;
                            existingLanguage.CanSpeak = language.CanSpeak;
                            existingLanguage.CanRead = language.CanRead;
                            existingLanguage.MotherTongue = language.MotherTongue;
                        }
                        else
                        {
                            // Add new Certification
                            var newLanguage = new LanguageSkill
                            {
                                EmployeeNo = dto.EmployeeNo,
                                LanguageCode = language.LanguageCode,
                                CanWrite = language.CanWrite,
                                CanSpeak = language.CanSpeak,
                                CanRead = language.CanRead,
                                MotherTongue = language.MotherTongue
                            };
                            await _db.LanguageSkills.AddAsync(newLanguage, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update FamilyMember entity
                if (dto.FamilyMembers != null && dto.FamilyMembers.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var familyNotInDTO = _db.FamilyMembers.AsEnumerable()
                                    .ExceptBy(dto.FamilyMembers.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (familyNotInDTO.Any())
                    {
                        _db.FamilyMembers.RemoveRange(familyNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var family in dto.FamilyMembers)
                    {
                        var existingFamily = await _db.FamilyMembers
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == family.AutoId, cancellationToken);

                        if (existingFamily != null)
                        {
                            // Update existing family
                            existingFamily.FirstName = family.FirstName;
                            existingFamily.MiddleName = family.MiddleName;
                            existingFamily.LastName = family.LastName;
                            existingFamily.RelationCode = family.RelationCode;
                            existingFamily.DOB = family.DOB;
                            existingFamily.QualificationCode = family.QualificationCode;
                            existingFamily.StreamCode = family.StreamCode;
                            existingFamily.SpecializationCode = family.SpecializationCode;
                            existingFamily.Occupation = family.Occupation;
                            existingFamily.ContactNo = family.ContactNo;
                            existingFamily.CountryCode = family.CountryCode;
                            existingFamily.StateCode = family.StateCode;
                            existingFamily.CityTownName = family.CityTownName;
                            existingFamily.District = family.District;
                            existingFamily.IsDependent = family.IsDependent;
                        }
                        else
                        {
                            // Add new family member
                            var newFamily = new FamilyMember
                            {
                                EmployeeNo = dto.EmployeeNo,
                                FirstName = family.FirstName,
                                MiddleName = family.MiddleName,
                                LastName = family.LastName,
                                RelationCode = family.RelationCode,
                                DOB = family.DOB,
                                QualificationCode = family.QualificationCode,
                                StreamCode = family.StreamCode,
                                SpecializationCode = family.SpecializationCode,
                                Occupation = family.Occupation,
                                ContactNo = family.ContactNo,
                                CountryCode = family.CountryCode,
                                StateCode = family.StateCode,
                                CityTownName = family.CityTownName,
                                District = family.District,
                                IsDependent = family.IsDependent
                            };
                            await _db.FamilyMembers.AddAsync(newFamily, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update FamilyVisa entity
                if (dto.FamilyVisas != null && dto.FamilyVisas.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var visaNotInDTO = _db.FamilyVisas.AsEnumerable()
                                    .ExceptBy(dto.FamilyVisas.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (visaNotInDTO.Any())
                    {
                        _db.FamilyVisas.RemoveRange(visaNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var visa in dto.FamilyVisas)
                    {
                        var existingFamilyVisa = await _db.FamilyVisas
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == visa.AutoId, cancellationToken);

                        if (existingFamilyVisa != null)
                        {
                            // Update existing family visa
                            existingFamilyVisa.CountryCode = visa.CountryCode;
                            existingFamilyVisa.VisaTypeCode = visa.VisaTypeCode;
                            existingFamilyVisa.Profession = visa.Profession;
                            existingFamilyVisa.IssueDate = visa.IssueDate;
                            existingFamilyVisa.ExpiryDate = visa.ExpiryDate;
                            existingFamilyVisa.FamilyId = visa.FamilyId;
                            existingFamilyVisa.TransactionNo = visa.TransactionNo;
                        }
                        else
                        {
                            // Add new family visa
                            var newFamilyVisa = new FamilyVisa
                            {
                                EmployeeNo = dto.EmployeeNo,
                                CountryCode = visa.CountryCode,
                                VisaTypeCode = visa.VisaTypeCode,
                                Profession = visa.Profession,
                                IssueDate = visa.IssueDate,
                                ExpiryDate = visa.ExpiryDate,
                                FamilyId = visa.FamilyId,
                                TransactionNo = visa.TransactionNo
                            };
                            await _db.FamilyVisas.AddAsync(newFamilyVisa, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update EmploymentHistory entity
                if (dto.EmploymentHistories != null && dto.EmploymentHistories.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var historyNotInDTO = _db.EmploymentHistories.AsEnumerable()
                                    .ExceptBy(dto.EmploymentHistories.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (historyNotInDTO.Any())
                    {
                        _db.EmploymentHistories.RemoveRange(historyNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var history in dto.EmploymentHistories)
                    {
                        var existingHistory = await _db.EmploymentHistories
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == history.AutoId, cancellationToken);

                        if (existingHistory != null)
                        {
                            // Update existing employment history
                            existingHistory.CompanyName = history.CompanyName;
                            existingHistory.CompanyAddress = history.CompanyAddress;
                            existingHistory.Designation = history.Designation;
                            existingHistory.Role = history.Role;
                            existingHistory.FromDate = history.FromDate;
                            existingHistory.ToDate = history.ToDate;
                            existingHistory.LastDrawnSalary = history.LastDrawnSalary;
                            existingHistory.SalaryTypeCode = history.SalaryTypeCode;
                            existingHistory.SalaryCurrencyCode = history.SalaryCurrencyCode;
                            existingHistory.ReasonOfChange = history.ReasonOfChange;
                            existingHistory.ReportingManager = history.ReportingManager;
                            existingHistory.CompanyWebsite = history.CompanyWebsite;
                            existingHistory.TransactionNo = history.TransactionNo;
                        }
                        else
                        {
                            // Add new employment history
                            var newHistory = new EmploymentHistory
                            {
                                EmployeeNo = dto.EmployeeNo,
                                CompanyName = history.CompanyName,
                                CompanyAddress = history.CompanyAddress,
                                Designation = history.Designation,
                                Role = history.Role,
                                FromDate = history.FromDate,
                                ToDate = history.ToDate,
                                LastDrawnSalary = history.LastDrawnSalary,
                                SalaryTypeCode = history.SalaryTypeCode,
                                SalaryCurrencyCode = history.SalaryCurrencyCode,
                                ReasonOfChange = history.ReasonOfChange,
                                ReportingManager = history.ReportingManager,
                                CompanyWebsite = history.CompanyWebsite,
                                TransactionNo = history.TransactionNo
                            };
                            await _db.EmploymentHistories.AddAsync(newHistory, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update OtherDocument entity
                if (dto.OtherDocuments != null && dto.OtherDocuments.Count > 0)
                {
                    #region Delete entity items that don't exist in the DTO
                    var documentNotInDTO = _db.OtherDocuments.AsEnumerable()
                                    .ExceptBy(dto.OtherDocuments.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (documentNotInDTO.Any())
                    {
                        _db.OtherDocuments.RemoveRange(documentNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var docs in dto.OtherDocuments)
                    {
                        var existingDocs = await _db.OtherDocuments
                            .FirstOrDefaultAsync(ec => ec.EmployeeNo == dto.EmployeeNo && ec.AutoId == docs.AutoId, cancellationToken);

                        if (existingDocs != null)
                        {
                            // Update existing employment history
                            existingDocs.DocumentName = docs.DocumentName;
                            existingDocs.DocumentTypeCode = docs.DocumentTypeCode;
                            existingDocs.Description = docs.Description;
                            existingDocs.FileData = docs.FileData;
                            existingDocs.FileExtension = docs.FileExtension;
                            existingDocs.ContentTypeCode = docs.ContentTypeCode;
                            existingDocs.UploadDate = docs.UploadDate;
                            existingDocs.TransactionNo = docs.TransactionNo;
                        }
                        else
                        {
                            // Add new employment history
                            var newDocs = new OtherDocument
                            {
                                EmployeeNo = dto.EmployeeNo,
                                DocumentName = docs.DocumentName,
                                DocumentTypeCode = docs.DocumentTypeCode,
                                Description = docs.Description,
                                FileData = docs.FileData,
                                FileExtension = docs.FileExtension,
                                ContentTypeCode = docs.ContentTypeCode,
                                UploadDate = docs.UploadDate,
                                TransactionNo = docs.TransactionNo
                            };
                            await _db.OtherDocuments.AddAsync(newDocs, cancellationToken);
                        }
                    }
                }
                #endregion

                // Save to database
                _db.Employees.Update(employee);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                // Save to database
                _db.Employees.Add(employee);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;

            try
            {
                var employee = await _db.Employees.FindAsync(employeeId);
                if (employee == null)
                    throw new Exception("Could not perform deletion because employee record is not found in the database.");

                _db.Employees.Remove(employee);
                
                int rowsDeleted = await _db.SaveChangesAsync(cancellationToken);
                if (rowsDeleted > 0)
                    isSuccess = true;

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> SaveDepartmentAsync(DepartmentMaster dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                var department = await _db.DepartmentMasters.FirstOrDefaultAsync(x => x.DepartmentId == dto.DepartmentId, cancellationToken);
                if (department == null)
                    throw new InvalidOperationException("Department not found");

                #region Update Employee entity
                department.DepartmentCode = dto.DepartmentCode;
                department.DepartmentName = dto.DepartmentName;
                department.GroupCode = dto.GroupCode;
                department.Description = dto.Description;
                department.ParentDepartmentId = dto.ParentDepartmentId;
                department.SuperintendentEmpNo = dto.SuperintendentEmpNo;
                department.ManagerEmpNo = dto.ManagerEmpNo;
                department.IsActive = dto.IsActive;
                department.UpdatedAt = dto.UpdatedAt;
                #endregion

                // Save to database
                _db.DepartmentMasters.Update(department);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteDepartmentAsync(int departmentID, CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;

            try
            {
                var department = await _db.DepartmentMasters.FindAsync(departmentID);
                if (department == null)
                    throw new Exception("Could not perform deletion because department employee record is not found in the database.");

                _db.DepartmentMasters.Remove(department);

                int rowsDeleted = await _db.SaveChangesAsync(cancellationToken);
                if (rowsDeleted > 0)
                    isSuccess = true;

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> AddDepartmentAsync(DepartmentMaster department, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {                
                // Save to database
                _db.DepartmentMasters.Add(department);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteEmergencyContactAsync(int autoID, CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;

            try
            {
                var contactPerson = await _db.EmergencyContacts.FindAsync(autoID);
                if (contactPerson == null)
                    throw new Exception("Could not perform deletion because the selectec contact person is not found in the database.");

                _db.EmergencyContacts.Remove(contactPerson);

                int rowsDeleted = await _db.SaveChangesAsync(cancellationToken);
                if (rowsDeleted > 0)
                    isSuccess = true;

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateEmergencyContactAsync(EmergencyContact dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                var contact = await _db.EmergencyContacts.FirstOrDefaultAsync(x => x.AutoId == dto.AutoId, cancellationToken);
                if (contact == null)
                    throw new InvalidOperationException("Emergency contact person not found");

                #region Update Employee entity
                contact.ContactPerson = dto.ContactPerson;
                contact.RelationCode = dto.RelationCode;
                contact.MobileNo = dto.MobileNo;
                contact.LandlineNo = dto.LandlineNo;
                contact.Address = dto.Address;
                contact.CountryCode = dto.CountryCode;
                contact.City = dto.City;
                #endregion

                // Save to database
                _db.EmergencyContacts.Update(contact);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> AddEmergencyContactAsync(EmergencyContact contact, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                // Save to database
                _db.EmergencyContacts.Add(contact);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an employee by EmployeeCode OR Email.
        /// Case-insensitive comparison.
        /// </summary>
        public async Task<Result<Employee?>> GetByUserIDOrEmailAsync(
            string userIdOrEmail,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userIdOrEmail))
                    throw new ArgumentException(
                        "User Id or Email must be provided.",
                        nameof(userIdOrEmail));

                var normalizedInput = userIdOrEmail.Trim().ToLower();

                Employee? employee = await _db.Employees
                    .FirstOrDefaultAsync(e =>
                        e.UserID!.ToLower() == normalizedInput ||
                        e.OfficialEmail.ToLower() == normalizedInput,
                        cancellationToken);

                return Result<Employee?>.SuccessResult(employee);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<Employee?>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<Employee?>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Unlock user account
        /// </summary>
        public async Task<Result<int>> UnlockUserAccountAsync(string userIdOrEmail, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (string.IsNullOrWhiteSpace(userIdOrEmail))
                    throw new ArgumentException(
                        "User Id or Email must be provided.",
                        nameof(userIdOrEmail));

                var normalizedInput = userIdOrEmail.Trim().ToLower();

                var user = await _db.Employees
                    .FirstOrDefaultAsync(e =>
                        e.UserID!.ToLower() == normalizedInput ||
                        e.OfficialEmail.ToLower() == normalizedInput,
                        cancellationToken);

                if (user == null)
                    return Result<int>.Failure("User not found.");

                if (!user.IsLocked)
                    return Result<int>.Failure("Account is not locked.");

                user.UnlockAccount();

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);
                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates employee entity safely.
        /// </summary>
        public async Task<Result<int>> UpdateAsync(Employee employee, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (employee == null)
                    throw new ArgumentNullException(nameof(employee));

                var existing = await _db.Employees
                    .FirstOrDefaultAsync(e => e.EmployeeNo == employee.EmployeeNo,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        $"Employee with User ID {employee.UserID} not found.");

                // Apply changes manually to avoid unintended overwrites
                _db.Entry(existing).CurrentValues.SetValues(employee);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves an employee by EmployeeCode and Joining Date
        /// Case-insensitive comparison.
        /// </summary>
        public async Task<Result<Employee?>> GetByEmployeeCodeAndHireDateAsync(
            string employeeCode, DateTime joiningDate,
            CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(employeeCode))
                    throw new ArgumentException(
                        "User Id or Email must be provided.",
                        nameof(employeeCode));

                var normalizedInput = employeeCode.Trim().ToLower();

                Employee? employee = await _db.Employees
                    .FirstOrDefaultAsync(e =>
                        (e.UserID!.ToLower() == normalizedInput ||
                        e.OfficialEmail.ToLower() == normalizedInput) &&
                        e.HireDate == joiningDate,
                        cancellationToken);

                return Result<Employee?>.SuccessResult(employee);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<Employee?>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<Employee?>.Failure($"Database error: {ex.Message}");
            }
        }
        #endregion
    }
}
