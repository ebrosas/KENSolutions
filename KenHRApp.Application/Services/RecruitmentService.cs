using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        #region Fields
        private readonly IRecruitmentRepository _repository;
        #endregion

        #region Constructors
        public RecruitmentService(IRecruitmentRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Public Methods
        public async Task<Result<List<RecruitmentBudgetDTO>>> GetRecruitmentBudgetAsync(string? departmentCode, bool? onHold)
        {
            List<RecruitmentBudgetDTO> budgetList = new List<RecruitmentBudgetDTO>();

            try
            {
                var repoResult = await _repository.GetRecruitmentBudgetAsync(departmentCode, onHold);
                if (!repoResult.Success)
                {
                    return Result<List<RecruitmentBudgetDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                budgetList = repoResult.Value!.Select(e => new RecruitmentBudgetDTO
                {
                    BudgetId = e.BudgetId,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    BudgetHeadCount = e.BudgetHeadCount,
                    BudgetDescription = e.BudgetDescription,
                    ActiveCount = e.ActiveCount.HasValue ? Convert.ToInt32(e.ActiveCount) : 0,
                    ExitCount = e.ExitCount.HasValue ? Convert.ToInt32(e.ExitCount) : 0,
                    RequisitionCount = e.RequisitionCount.HasValue ? Convert.ToInt32(e.RequisitionCount) : 0,
                    NetGapCount = e.NetGapCount.HasValue ? Convert.ToInt32(e.NetGapCount) : 0,
                    NewIndentCount = e.NewIndentCount.HasValue ? Convert.ToInt32(e.NewIndentCount) : 0,
                    OnHold = e.OnHold.HasValue ? Convert.ToBoolean(e.OnHold) : false,
                    Remarks = e.Remarks,
                    CreatedDate = e.CreatedDate,
                    LastUpdateDate = e.LastUpdateDate
                }).ToList();

                return Result<List<RecruitmentBudgetDTO>>.SuccessResult(budgetList);
            }
            catch (Exception ex)
            {
                return Result<List<RecruitmentBudgetDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in GetRecruitmentBudgetAsync() method.");
            }
        }

        /// <summary>
        /// Perform update and add operations in "RecruitmentBudget" table
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> SaveRecruitmentBudgetAsync(RecruitmentBudgetDTO dto, CancellationToken cancellationToken = default)
        {
            int saveResult = 0;

            try
            {
                #region Initialize RecruitmentBudget entity
                RecruitmentBudget budgetEntity = new RecruitmentBudget()
                {
                    BudgetId = dto.BudgetId,
                    DepartmentCode = dto.DepartmentCode,
                    BudgetDescription = dto.BudgetDescription,
                    BudgetHeadCount = dto.BudgetHeadCount,
                    ActiveCount = dto.ActiveCount,
                    ExitCount = dto.ExitCount,
                    RequisitionCount = dto.RequisitionCount,
                    NetGapCount = dto.NetGapCount,
                    NewIndentCount = dto.NewIndentCount,
                    OnHold = dto.OnHold,
                    Remarks = dto.Remarks,
                    CreatedDate = dto.CreatedDate,
                    LastUpdateDate = dto.LastUpdateDate
                };
                #endregion

                if (budgetEntity.BudgetId == 0)
                {
                    budgetEntity.CreatedDate = DateTime.Now;

                    var addResult = await _repository.AddRecruitmentBudgetAsync(budgetEntity, cancellationToken);
                    if (addResult.Success)
                    {
                        saveResult = addResult.Value;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(addResult.Error))
                            throw new Exception(addResult.Error);
                        else
                            throw new Exception("Unable to add new budget record in the database. Please try saving again.");
                    }
                }
                else
                {
                    budgetEntity.LastUpdateDate = DateTime.Now;

                    var updateResult = await _repository.UpdateRecruitmentBudgetAsync(budgetEntity, cancellationToken);
                    if (updateResult.Success)
                    {
                        saveResult = updateResult.Value;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(updateResult.Error))
                            throw new Exception(updateResult.Error);
                        else
                            throw new Exception("Unable to update the selected budget record. Please try saving again.");
                    }
                }

                return Result<int>.SuccessResult(saveResult);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Perform delete operation in "RecruitmentBudget" table
        /// </summary>
        /// <param name="autoID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteRecruitmentBudgetAsync(int budgetID, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.DeleteRecruitmentBudgetAsync(budgetID, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to delete the selected budget due to unknown error. Please refresh the page then try to delete again.");
                }

                return Result<bool>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> AddRecruitmentRequestAsync(RecruitmentRequestDTO recruitment, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize Employee entity
                RecruitmentRequisition recruitmentEntity = new RecruitmentRequisition();

                #region Position Properties       
                recruitmentEntity.RequisitionId = recruitment.RequisitionId;
                recruitmentEntity.EmploymentTypeCode = recruitment.EmploymentTypeCode;
                recruitmentEntity.QualificationModeCode = recruitment.QualificationModeCode;
                recruitmentEntity.PositionTypeCode = recruitment.PositionTypeCode;
                recruitmentEntity.InterviewProcessCode = recruitment.InterviewProcessCode;
                recruitmentEntity.IsPreAssessment = recruitment.IsPreAssessment;
                recruitmentEntity.CompanyCode = recruitment.CompanyCode;
                recruitmentEntity.DepartmentCode = recruitment.DepartmentCode;
                recruitmentEntity.CountryCode = recruitment.CountryCode;
                recruitmentEntity.EducationCode = recruitment.EducationCode;
                recruitmentEntity.EmployeeClassCode = recruitment.EmployeeClassCode;
                recruitmentEntity.EthnicityCode = recruitment.EthnicityCode;
                recruitmentEntity.JobTitleCode = recruitment.JobTitleCode;
                recruitmentEntity.PayGradeCode = recruitment.PayGradeCode;
                #endregion

                #region Position Description
                recruitmentEntity.PositionDescription = recruitment.PositionDescription;
                recruitmentEntity.TotalWorkExperience = recruitment.TotalWorkExperience;
                recruitmentEntity.MinWorkExperience = recruitment.MinWorkExperience;
                recruitmentEntity.MaxWorkExperience = recruitment.MaxWorkExperience;
                recruitmentEntity.TotalRelevantExperience = recruitment.TotalRelevantExperience;
                recruitmentEntity.MinRelevantExperience = recruitment.MinRelevantExperience;
                recruitmentEntity.MaxRelevantExperience = recruitment.MaxRelevantExperience;
                recruitmentEntity.AgeRange = recruitment.AgeRange;
                recruitmentEntity.MinAge = recruitment.MinAge;
                recruitmentEntity.MaxAge = recruitment.MaxAge;
                recruitmentEntity.RequiredGender = recruitment.RequiredGender;
                recruitmentEntity.RequiredAsset = recruitment.RequiredAsset;
                recruitmentEntity.VideoDescriptionURL = recruitment.VideoDescriptionURL;
                #endregion

                #region Compensation and Benefits
                recruitmentEntity.SalaryRangeType = recruitment.SalaryRangeType;
                recruitmentEntity.YearlySalaryRange = recruitment.YearlySalaryRange;
                recruitmentEntity.YearlySalaryRangeMin = recruitment.YearlySalaryRangeMin;
                recruitmentEntity.YearlySalaryRangeMax = recruitment.YearlySalaryRangeMax;
                recruitmentEntity.YearlySalaryRangeCurrency = recruitment.YearlySalaryRangeCurrency;
                recruitmentEntity.MonthlySalaryRange = recruitment.MonthlySalaryRange;
                recruitmentEntity.MonthlySalaryRangeMin = recruitment.MonthlySalaryRangeMin;
                recruitmentEntity.MonthlySalaryRangeMax = recruitment.MonthlySalaryRangeMax;
                recruitmentEntity.MonthlySalaryRangeCurrency = recruitment.MonthlySalaryRangeCurrency;
                recruitmentEntity.DailySalaryRange = recruitment.DailySalaryRange;
                recruitmentEntity.DailySalaryRangeMin = recruitment.DailySalaryRangeMin;
                recruitmentEntity.DailySalaryRangeMax = recruitment.DailySalaryRangeMax;
                recruitmentEntity.DailySalaryRangeCurrency = recruitment.DailySalaryRangeCurrency;
                recruitmentEntity.HourlySalaryRange = recruitment.HourlySalaryRange;
                recruitmentEntity.HourlySalaryRangeMin = recruitment.HourlySalaryRangeMin;
                recruitmentEntity.HourlySalaryRangeMax = recruitment.HourlySalaryRangeMax;
                recruitmentEntity.HourlySalaryRangeCurrency = recruitment.HourlySalaryRangeCurrency;
                recruitmentEntity.Responsibilities = recruitment.Responsibilities;
                recruitmentEntity.Competencies = recruitment.Competencies;
                recruitmentEntity.GeneralRemarks = recruitment.GeneralRemarks;
                #endregion

                #region General
                recruitmentEntity.CreatedByNo = recruitment.CreatedByNo;
                recruitmentEntity.CreatedByUserID = recruitment.CreatedByUserID;
                recruitmentEntity.CreatedByName = recruitment.CreatedByName;
                recruitmentEntity.CreatedDate = recruitment.CreatedDate;
                #endregion
                #endregion

                #region Initialize Qualifications entity
                List<JobQualification> qualificationList = new List<JobQualification>();

                if (recruitment.QualificationList != null && recruitment.QualificationList.Any())
                {
                    qualificationList = recruitment.QualificationList.Select(e => new JobQualification
                    {
                        AutoId = e.AutoId,
                        RequisitionId = e.RequisitionId,
                        QualificationCode = e.QualificationCode,
                        StreamCode = e.StreamCode,
                        SpecializationCode = e.SpecializationCode
                    }).ToList();
                }

                recruitmentEntity.QualificationList = qualificationList;
                #endregion

                var result = await _repository.AddRecruitmentRequestAsync(recruitmentEntity, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to commit data changes to the database. Please try saving again.");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> SaveEmployeeAsync(RecruitmentRequestDTO recruitment, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize RecruitmentRequisition entity
                RecruitmentRequisition recruitmentEntity = new RecruitmentRequisition();

                #region Personal Detail         
                recruitmentEntity.EmployeeId = recruitment.EmployeeId;
                recruitmentEntity.FirstName = recruitment.FirstName;
                recruitmentEntity.MiddleName = recruitment.MiddleName;
                recruitmentEntity.LastName = recruitment.LastName;
                recruitmentEntity.Position = recruitment.Position;
                recruitmentEntity.DOB = recruitment.DOB;
                recruitmentEntity.NationalityCode = recruitment.NationalityCode;
                recruitmentEntity.ReligionCode = recruitment.ReligionCode;
                recruitmentEntity.GenderCode = recruitment.GenderCode;
                recruitmentEntity.MaritalStatusCode = recruitment.MaritalStatusCode;
                recruitmentEntity.Salutation = recruitment.Salutation;
                #endregion

                #region Contact Detail Implementation
                recruitmentEntity.OfficialEmail = recruitment.OfficialEmail;
                recruitmentEntity.PersonalEmail = recruitment.PersonalEmail;
                recruitmentEntity.AlternateEmail = recruitment.AlternateEmail;
                recruitmentEntity.OfficeLandlineNo = recruitment.OfficeLandlineNo;
                recruitmentEntity.ResidenceLandlineNo = recruitment.ResidenceLandlineNo;
                recruitmentEntity.OfficeExtNo = recruitment.OfficeExtNo;
                recruitmentEntity.MobileNo = recruitment.MobileNo;
                recruitmentEntity.AlternateMobileNo = recruitment.AlternateMobileNo;
                #endregion

                #region Employment Detail Implementation
                recruitmentEntity.EmployeeNo = recruitment.EmployeeNo;
                recruitmentEntity.EmployeeStatusCode = recruitment.EmployeeStatusCode;
                recruitmentEntity.ReportingManagerCode = recruitment.ReportingManagerCode;
                recruitmentEntity.WorkPermitID = recruitment.WorkPermitID;
                recruitmentEntity.WorkPermitExpiryDate = recruitment.WorkPermitExpiryDate;
                recruitmentEntity.HireDate = recruitment.HireDate!.Value;
                recruitmentEntity.DateOfConfirmation = recruitment.DateOfConfirmation;
                recruitmentEntity.TerminationDate = recruitment.TerminationDate;
                recruitmentEntity.DateOfSuperannuation = recruitment.DateOfSuperannuation;
                recruitmentEntity.Reemployed = recruitment.Reemployed;
                recruitmentEntity.OldEmployeeNo = recruitment.OldEmployeeNo;
                recruitmentEntity.DepartmentCode = recruitment.DepartmentCode;
                recruitmentEntity.EmploymentTypeCode = recruitment.EmploymentTypeCode;
                recruitmentEntity.RoleCode = recruitment.RoleCode;
                recruitmentEntity.FirstAttendanceModeCode = recruitment.FirstAttendanceModeCode;
                recruitmentEntity.SecondAttendanceModeCode = recruitment.SecondAttendanceModeCode;
                recruitmentEntity.ThirdAttendanceModeCode = recruitment.ThirdAttendanceModeCode;
                recruitmentEntity.SecondReportingManagerCode = recruitment.SecondReportingManagerCode;
                #endregion

                #region Attribute Detail Implementation
                recruitmentEntity.Company = recruitment.Company;
                recruitmentEntity.CompanyBranch = recruitment.CompanyBranch;
                recruitmentEntity.EducationCode = recruitment.EducationCode;
                recruitmentEntity.EmployeeClassCode = recruitment.EmployeeClassCode;
                recruitmentEntity.JobTitleCode = recruitment.JobTitleCode;
                recruitmentEntity.PayGrade = recruitment.PayGrade;
                recruitmentEntity.IsActive = recruitment.IsActive;
                #endregion

                #region Bank Detail Implementation  
                recruitmentEntity.AccountTypeCode = recruitment.AccountTypeCode;
                recruitmentEntity.AccountNumber = recruitment.AccountNumber;
                recruitmentEntity.AccountHolderName = recruitment.AccountHolderName;
                recruitmentEntity.BankNameCode = recruitment.BankNameCode;
                recruitmentEntity.BankBranchName = recruitment.BankBranchName;
                recruitmentEntity.IBANNumber = recruitment.IBANNumber;
                recruitmentEntity.TaxNumber = recruitment.TaxNumber;
                #endregion

                #region Social Connect Implementation
                recruitmentEntity.LinkedInAccount = recruitment.LinkedInAccount;
                recruitmentEntity.FacebookAccount = recruitment.FacebookAccount;
                recruitmentEntity.TwitterAccount = recruitment.TwitterAccount;
                recruitmentEntity.InstagramAccount = recruitment.InstagramAccount;
                #endregion

                #region Primary Location Implementation
                recruitmentEntity.PresentAddress = recruitment.PresentAddress;
                recruitmentEntity.PresentCountryCode = recruitment.PresentCountryCode;
                recruitmentEntity.PresentCity = recruitment.PresentCity;
                recruitmentEntity.PresentAreaCode = recruitment.PresentAreaCode;
                recruitmentEntity.PresentContactNo = recruitment.PresentContactNo;
                recruitmentEntity.PresentMobileNo = recruitment.PresentMobileNo;
                recruitmentEntity.PermanentAddress = recruitment.PermanentAddress;
                recruitmentEntity.PermanentCountryCode = recruitment.PermanentCountryCode;
                recruitmentEntity.PermanentCity = recruitment.PermanentCity;
                recruitmentEntity.PermanentAreaCode = recruitment.PermanentAreaCode;
                recruitmentEntity.PermanentContactNo = recruitment.PermanentContactNo;
                recruitmentEntity.PermanentMobileNo = recruitment.PermanentMobileNo;
                #endregion

                #endregion

                #region Initialize IdentityProof entity
                IdentityProof? identityProof = null;

                if (recruitment.EmpIdentityProof != null)
                {
                    identityProof = new IdentityProof()
                    {
                        AutoId = recruitment.EmpIdentityProof!.AutoId,
                        PassportNumber = recruitment.EmpIdentityProof!.PassportNumber,
                        DateOfIssue = recruitment.EmpIdentityProof!.DateOfIssue,
                        DateOfExpiry = recruitment.EmpIdentityProof!.DateOfExpiry,
                        PlaceOfIssue = recruitment.EmpIdentityProof!.PlaceOfIssue,
                        DrivingLicenseNo = recruitment.EmpIdentityProof!.DrivingLicenseNo,
                        DLDateOfIssue = recruitment.EmpIdentityProof!.DLDateOfIssue,
                        DLDateOfExpiry = recruitment.EmpIdentityProof!.DLDateOfExpiry,
                        DLPlaceOfIssue = recruitment.EmpIdentityProof!.DLPlaceOfIssue,
                        TypeOfDocument = recruitment.EmpIdentityProof!.TypeOfDocument,
                        OtherDocNumber = recruitment.EmpIdentityProof!.OtherDocNumber,
                        OtherDocDateOfIssue = recruitment.EmpIdentityProof!.OtherDocDateOfIssue,
                        OtherDocDateOfExpiry = recruitment.EmpIdentityProof!.OtherDocDateOfExpiry,
                        NationalIDNumber = recruitment.EmpIdentityProof!.NationalIDNumber,
                        NationalIDTypeCode = recruitment.EmpIdentityProof!.NationalIDTypeCode,
                        NatIDPlaceOfIssue = recruitment.EmpIdentityProof!.NatIDPlaceOfIssue,
                        NatIDDateOfIssue = recruitment.EmpIdentityProof!.NatIDDateOfIssue,
                        NatIDDateOfExpiry = recruitment.EmpIdentityProof!.NatIDDateOfExpiry,
                        ContractNumber = recruitment.EmpIdentityProof!.ContractNumber,
                        ContractPlaceOfIssue = recruitment.EmpIdentityProof!.ContractPlaceOfIssue,
                        ContractDateOfIssue = recruitment.EmpIdentityProof!.ContractDateOfIssue,
                        ContractDateOfExpiry = recruitment.EmpIdentityProof!.ContractDateOfExpiry,
                        VisaNumber = recruitment.EmpIdentityProof!.VisaNumber,
                        VisaTypeCode = recruitment.EmpIdentityProof!.VisaTypeCode,
                        VisaCountryCode = recruitment.EmpIdentityProof!.VisaCountryCode,
                        Profession = recruitment.EmpIdentityProof!.Profession,
                        Sponsor = recruitment.EmpIdentityProof!.Sponsor,
                        VisaDateOfIssue = recruitment.EmpIdentityProof!.VisaDateOfIssue,
                        VisaDateOfExpiry = recruitment.EmpIdentityProof!.VisaDateOfExpiry
                    };
                }

                recruitmentEntity.IdentityProof = identityProof;
                #endregion

                #region Initialize Emergency Contacts entity
                List<EmergencyContact> emergencyContactList = new List<EmergencyContact>();

                if (recruitment.EmergencyContactList != null && recruitment.EmergencyContactList.Any())
                {
                    emergencyContactList = recruitment.EmergencyContactList.Select(e => new EmergencyContact
                    {
                        AutoId = e.AutoId,
                        ContactPerson = e.ContactPerson,
                        RelationCode = e.RelationCode,
                        MobileNo = e.MobileNo,
                        LandlineNo = e.LandlineNo,
                        Address = e.Address,
                        CountryCode = e.CountryCode,
                        City = e.City
                    }).ToList();
                }

                recruitmentEntity.EmergencyContactList = emergencyContactList;
                #endregion

                #region Initialize Qualifications entity
                List<Qualification> qualificationList = new List<Qualification>();

                if (recruitment.QualificationList != null && recruitment.QualificationList.Any())
                {
                    qualificationList = recruitment.QualificationList.Select(e => new Qualification
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        QualificationCode = e.QualificationCode,
                        StreamCode = e.StreamCode,
                        SpecializationCode = e.SpecializationCode,
                        UniversityName = e.UniversityName,
                        Institute = e.Institute,
                        QualificationMode = e.QualificationMode,
                        CountryCode = e.CountryCode,
                        StateCode = e.StateCode,
                        CityTownName = e.CityTownName,
                        FromMonthCode = e.FromMonthCode,
                        FromYear = e.FromYear,
                        ToMonthCode = e.ToMonthCode,
                        ToYear = e.ToYear,
                        PassMonthCode = e.PassMonthCode,
                        PassYear = e.PassYear
                    }).ToList();
                }

                recruitmentEntity.Qualifications = qualificationList;
                #endregion

                #region Initialize EmployeeSkill entity
                List<EmployeeSkill> skillList = new List<EmployeeSkill>();

                if (recruitment.EmployeeSkillList != null && recruitment.EmployeeSkillList.Any())
                {
                    skillList = recruitment.EmployeeSkillList.Select(e => new EmployeeSkill
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        SkillName = e.SkillName,
                        LevelCode = e.LevelCode,
                        LastUsedMonthCode = e.LastUsedMonthCode,
                        LastUsedYear = e.LastUsedYear,
                        FromMonthCode = e.FromMonthCode,
                        FromYear = e.FromYear,
                        ToMonthCode = e.ToMonthCode,
                        ToYear = e.ToYear
                    }).ToList();
                }

                recruitmentEntity.EmployeeSkills = skillList;
                #endregion

                #region Initialize EmployeeCertification entity
                List<EmployeeCertification> certfList = new List<EmployeeCertification>();

                if (recruitment.EmployeeCertificationList != null && recruitment.EmployeeCertificationList.Any())
                {
                    certfList = recruitment.EmployeeCertificationList.Select(e => new EmployeeCertification
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        QualificationCode = e.QualificationCode,
                        StreamCode = e.StreamCode,
                        Specialization = e.Specialization,
                        University = e.University,
                        Institute = e.Institute,
                        CountryCode = e.CountryCode,
                        State = e.State,
                        CityTownName = e.CityTownName,
                        FromMonthCode = e.FromMonthCode,
                        FromYear = e.FromYear,
                        ToMonthCode = e.ToMonthCode,
                        ToMonth = e.ToMonth,
                        ToYear = e.ToYear,
                        PassMonthCode = e.PassMonthCode,
                        PassYear = e.PassYear
                    }).ToList();
                }

                recruitmentEntity.EmployeeCertifications = certfList;
                #endregion

                #region Initialize LanguageSkill entity
                List<LanguageSkill> languageList = new List<LanguageSkill>();
                if (recruitment.LanguageSkillList != null && recruitment.LanguageSkillList.Any())
                {
                    languageList = recruitment.LanguageSkillList.Select(e => new LanguageSkill
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        LanguageCode = e.LanguageCode,
                        CanWrite = e.CanWrite,
                        CanSpeak = e.CanSpeak,
                        CanRead = e.CanRead,
                        MotherTongue = e.MotherTongue
                    }).ToList();
                }

                recruitmentEntity.LanguageSkills = languageList;
                #endregion

                #region Initialize FamilyMember entity
                List<FamilyMember> familyList = new List<FamilyMember>();
                if (recruitment.FamilyMemberList != null && recruitment.FamilyMemberList.Any())
                {
                    familyList = recruitment.FamilyMemberList.Select(e => new FamilyMember
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        FirstName = e.FirstName,
                        MiddleName = e.MiddleName,
                        LastName = e.LastName,
                        RelationCode = e.RelationCode,
                        DOB = e.DOB,
                        QualificationCode = e.QualificationCode,
                        StreamCode = e.StreamCode,
                        SpecializationCode = e.SpecializationCode,
                        Occupation = e.Occupation,
                        ContactNo = e.ContactNo,
                        CountryCode = e.CountryCode,
                        StateCode = e.StateCode,
                        CityTownName = e.CityTownName,
                        District = e.District,
                        IsDependent = e.IsDependent
                    }).ToList();
                }

                recruitmentEntity.FamilyMembers = familyList;
                #endregion

                #region Initialize FamilyVisa entity
                List<FamilyVisa> familyVisaList = new List<FamilyVisa>();
                if (recruitment.FamilyVisaList != null && recruitment.FamilyVisaList.Any())
                {
                    familyVisaList = recruitment.FamilyVisaList.Select(e => new FamilyVisa
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        CountryCode = e.CountryCode,
                        VisaTypeCode = e.VisaTypeCode,
                        Profession = e.Profession,
                        IssueDate = e.IssueDate,
                        ExpiryDate = e.ExpiryDate,
                        FamilyId = e.FamilyId,
                        TransactionNo = e.TransactionNo
                    }).ToList();
                }

                recruitmentEntity.FamilyVisas = familyVisaList;
                #endregion

                #region Initialize EmploymentHistory entity
                List<EmploymentHistory> historyList = new List<EmploymentHistory>();
                if (recruitment.EmploymentHistoryList != null && recruitment.EmploymentHistoryList.Any())
                {
                    historyList = recruitment.EmploymentHistoryList.Select(e => new EmploymentHistory
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        CompanyName = e.CompanyName,
                        CompanyAddress = e.CompanyAddress,
                        Designation = e.Designation,
                        Role = e.Role,
                        FromDate = e.FromDate,
                        ToDate = e.ToDate,
                        LastDrawnSalary = e.LastDrawnSalary,
                        SalaryTypeCode = e.SalaryTypeCode,
                        SalaryCurrencyCode = e.SalaryCurrencyCode,
                        ReasonOfChange = e.ReasonOfChange,
                        ReportingManager = e.ReportingManager,
                        CompanyWebsite = e.CompanyWebsite,
                        TransactionNo = e.TransactionNo
                    }).ToList();
                }

                recruitmentEntity.EmploymentHistories = historyList;
                #endregion

                #region Initialize OtherDocument entity
                List<OtherDocument> docsList = new List<OtherDocument>();
                if (recruitment.OtherDocumentList != null && recruitment.OtherDocumentList.Any())
                {
                    docsList = recruitment.OtherDocumentList.Select(e => new OtherDocument
                    {
                        AutoId = e.AutoId,
                        EmployeeNo = e.EmployeeNo,
                        DocumentName = e.DocumentName,
                        DocumentTypeCode = e.DocumentTypeCode,
                        Description = e.Description,
                        FileData = e.FileData,
                        FileExtension = e.FileExtension,
                        ContentTypeCode = e.ContentTypeCode,
                        UploadDate = e.UploadDate,
                        TransactionNo = e.TransactionNo
                    }).ToList();
                }

                recruitmentEntity.OtherDocuments = docsList;
                #endregion

                var result = await _repository.UpdateRecruitmentRequestAsync(recruitmentEntity, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to commit data changes to the database. Please try saving again.");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }
        #endregion
    }
}
