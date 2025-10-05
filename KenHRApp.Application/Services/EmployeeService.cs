using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Abstractions;

namespace KenHRApp.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        #region Fields
        private readonly IEmployeeRepository _repository;
        private string? _errorMessage;
        #endregion

        #region Constructors
        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Data Initialization
        public async Task<List<EmergencyContactDTO>> GetEmergencyContactAsync()
        {
            try
            {
                List<EmergencyContactDTO> contactList = new List<EmergencyContactDTO>();
                contactList.Add(new EmergencyContactDTO()
                {
                    ContactPerson = "Ervin Olinas Brosas",
                    RelationCode = "Father",
                    MobileNo = "+9733229611",
                    LandlineNo = "38403062",
                    Address = "Flat 82 Bldg. 999 Block 327 Juffair",
                    CountryCode = "349",
                    City = "69"
                });
                contactList.Add(new EmergencyContactDTO()
                {
                    ContactPerson = "Antonina Ramirez Brosas",
                    RelationCode = "Mother",
                    MobileNo = "+97338403062",
                    LandlineNo = "5291928",
                    Address = "Flat 82 Bldg. 999 Block 327 Juffair",
                    CountryCode = "Bahrain",
                    City = "Manama"
                });

                return contactList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<QualificationDTO>> GetQualicationsAsync()
        {
            try
            {
                List<QualificationDTO> qualificationList = new List<QualificationDTO>();

                qualificationList.Add(new QualificationDTO()
                {
                    AutoId = 1,
                    QualificationCode = "BSc",
                    QualificationDesc = "Bachelor Degree",
                    StreamCode = "",
                    StreamDesc = "",
                    SpecializationCode = "",
                    SpecializationDesc = "",
                    UniversityName = "Polytechnic University of the Philippines",
                    Institute = "PUP",
                    QualificationMode = "FT",
                    QualificationModeDesc = "Full Time",
                    CountryCode = "PHL",
                    CountryDesc = "Philippines",
                    StateCode = "MNL",
                    StateDesc = "Manila",
                    CityTownName = "Manila",
                    FromMonthCode = "AUG",
                    FromMonthDesc = "August",
                    FromYear = 1995,
                    ToMonthCode = "JUN",
                    ToMonthDesc = "June",
                    ToYear = 2000,
                    PassMonthCode = "MAY",
                    PassMonthDesc = "May",
                    PassYear = 2000,
                });

                qualificationList.Add(new QualificationDTO()
                {
                    AutoId = 1,
                    QualificationCode = "CERF",
                    QualificationDesc = "Certificate in Cyber Security",
                    StreamCode = "",
                    StreamDesc = "",
                    SpecializationCode = "NETWORK",
                    SpecializationDesc = "Networking & Infrastructure",
                    UniversityName = "BIBF",
                    Institute = "BIBF",
                    QualificationMode = "FT",
                    QualificationModeDesc = "Full Time",
                    CountryCode = "BH",
                    CountryDesc = "Bahrain",
                    StateCode = "MNA",
                    StateDesc = "Manama",
                    CityTownName = "Manama",
                    FromMonthCode = "JAN",
                    FromMonthDesc = "January",
                    FromYear = 2020,
                    ToMonthCode = "DEC",
                    ToMonthDesc = "December",
                    ToYear = 2024,
                    PassMonthCode = "AUG",
                    PassMonthDesc = "August",
                    PassYear = 2024,
                });

                return qualificationList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeSkillDTO>> GetEmployeeSkillAsync()
        {
            try
            {
                List<EmployeeSkillDTO> skillList = new List<EmployeeSkillDTO>();

                skillList.Add(new EmployeeSkillDTO()
                {
                    AutoId = 1,
                    SkillName = "C#.Net",
                    LevelCode = "EXP",
                    LevelDesc = "Expert",
                    LastUsedMonthCode = "PRESENT",
                    LastUsedMonthDesc = "Present",
                    LastUsedYear = 2025,
                    FromMonthCode = "JAN",
                    FromMonthDesc = "January",
                    FromYear = 2000,
                    ToMonthCode = "PRESENT",
                    ToMonthDesc = "Present",
                    ToYear = 2025
                });

                skillList.Add(new EmployeeSkillDTO()
                {
                    AutoId = 2,
                    SkillName = "Entity Framework Core",
                    LevelCode = "EXP",
                    LevelDesc = "Expert",
                    LastUsedMonthCode = "JUL",
                    LastUsedMonthDesc = "July",
                    LastUsedYear = 2025,
                    FromMonthCode = "DEC",
                    FromMonthDesc = "December",
                    FromYear = 2023,
                    ToMonthCode = "PRESENT",
                    ToMonthDesc = "Present",
                    ToYear = 2025
                });

                skillList.Add(new EmployeeSkillDTO()
                {
                    AutoId = 3,
                    SkillName = "Blazor WebApp",
                    LevelCode = "INT",
                    LevelDesc = "Intermediate",
                    LastUsedMonthCode = "AUG",
                    LastUsedMonthDesc = "August",
                    LastUsedYear = 2025,
                    FromMonthCode = "JAN",
                    FromMonthDesc = "January",
                    FromYear = 2024,
                    ToMonthCode = "PRESENT",
                    ToMonthDesc = "Present",
                    ToYear = 2025
                });

                return skillList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeCertificationDTO>> GetCertificationAsync()
        {
            try
            {
                List<EmployeeCertificationDTO> skillList = new List<EmployeeCertificationDTO>();

                skillList.Add(new EmployeeCertificationDTO()
                {
                    AutoId = 1,
                    QualificationCode = "BsC",
                    QualificationDesc = "Bachelor of Science",
                    StreamCode = "Engineering",
                    Specialization = "Computer Engineering",
                    University = "Polytechnic University of the Philippines",
                    Institute = "PUP",
                    CountryCode = "PHL",
                    Country = "Philippines",
                    State = "National Capital Region",
                    CityTownName = "Manila",
                    FromMonthCode = "JAN",
                    FromMonth = "January",
                    FromYear = 2000,
                    ToMonthCode = "PRESENT",
                    ToMonth = "Present",
                    ToYear = 2025,
                    PassMonth = "September",
                    PassYear = 2025
                });

                return skillList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<LanguageSkillDTO>> GetLanguageSkillsAsync()
        {
            try
            {
                List<LanguageSkillDTO> languageList = new List<LanguageSkillDTO>();

                languageList.Add(new LanguageSkillDTO()
                {
                    AutoId = 1,
                    LanguageCode = "ENG",
                    LanguageDesc = "English",
                    CanWrite = true,
                    CanSpeak = true,
                    CanRead = true,
                    MotherTongue = false
                });

                languageList.Add(new LanguageSkillDTO()
                {
                    AutoId = 2,
                    LanguageCode = "TAG",
                    LanguageDesc = "Tagalog",
                    CanWrite = true,
                    CanSpeak = true,
                    CanRead = true,
                    MotherTongue = true
                });

                languageList.Add(new LanguageSkillDTO()
                {
                    AutoId = 3,
                    LanguageCode = "ARA",
                    LanguageDesc = "Arabic",
                    CanWrite = false,
                    CanSpeak = true,
                    CanRead = false,
                    MotherTongue = true
                });

                return languageList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FamilyMemberDTO>> GetFamilyMembersAsync()
        {
            try
            {
                List<FamilyMemberDTO> familyList = new List<FamilyMemberDTO>();

                familyList.Add(new FamilyMemberDTO()
                {
                    AutoId = 1,
                    FirstName = "Antonina",
                    MiddleName = "Ramirez",
                    LastName = "Brosas",
                    RelationCode = "WIFE",
                    Relation = "Wife",
                    DOB = new DateTime(1978, 8, 22),
                    QualificationCode = "BSC",
                    Qualification = "Bachelor of Science",
                    StreamCode = "BSC",
                    StreamDesc = "Bachelor of Science",
                    SpecializationCode = "MGMT",
                    Specialization = "Management",
                    Occupation = "Office Secretary",
                    ContactNo = "38403062",
                    CountryCode = "PHL",
                    Country = "Philippines",
                    StateCode = "BAT",
                    State = "Batangas",
                    CityTownName = "Lipa City",
                    District = "69",
                    IsDependent = true
                });

                familyList.Add(new FamilyMemberDTO()
                {
                    AutoId = 2,
                    FirstName = "Anne Kirsten",
                    MiddleName = "Ramirez",
                    LastName = "Brosas",
                    RelationCode = "DAUGH",
                    Relation = "Daughter",
                    DOB = new DateTime(2007, 12, 21),
                    QualificationCode = "STUD",
                    Qualification = "Student",
                    StreamCode = "",
                    StreamDesc = "",
                    SpecializationCode = "NA",
                    Specialization = "Not applicable",
                    Occupation = "Office Secretary",
                    ContactNo = "39456899",
                    CountryCode = "PHL",
                    Country = "Philippines",
                    StateCode = "LAG",
                    State = "Laguna",
                    CityTownName = "San Pablo City",
                    District = "4004",
                    IsDependent = true
                });

                return familyList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FamilyVisaDTO>> GetFamilyVisasAsync()
        {
            try
            {
                List<FamilyVisaDTO> visaList = new List<FamilyVisaDTO>();

                visaList.Add(new FamilyVisaDTO()
                {
                    AutoId = 1,
                    FamilyId = 1,
                    FamilyMemberName = "Ervin Olinas Brosas",
                    CountryCode = "PHL",
                    Country = "Philippines",
                    VisaTypeCode = "WP",
                    VisaType = "Working Permit",
                    Profession = "Software Engineer",
                    IssueDate = new DateTime(2025, 10, 17),
                    ExpiryDate = new DateTime(2027, 10, 16)
                });

                visaList.Add(new FamilyVisaDTO()
                {
                    AutoId = 2,
                    FamilyId = 2,
                    FamilyMemberName = "Antonina Magtibay Ramirez Brosas",
                    CountryCode = "PHL",
                    Country = "Philippines",
                    VisaTypeCode = "RP",
                    VisaType = "Resident Permit",
                    Profession = "Housewife",
                    IssueDate = new DateTime(2025, 08, 22),
                    ExpiryDate = new DateTime(2027, 08, 21)
                });

                return visaList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmploymentHistoryDTO>> GetEmploymentHistoryAsync()
        {
            try
            {
                List<EmploymentHistoryDTO> employmentList = new List<EmploymentHistoryDTO>();

                employmentList.Add(new EmploymentHistoryDTO()
                {
                    AutoId = 1,
                    CompanyName = "Comprehensive Systems Company",
                    CompanyAddress = "Riyadh, Saudi Arabia",
                    Designation = ".Net Developer - Team Lead",
                    Role = "Senior Level",
                    FromDate = new DateTime(2004, 3, 1),
                    ToDate = new DateTime(2010, 5, 31),
                    LastDrawnSalary = 2000,
                    SalaryTypeCode = "MONTHLY",
                    SalaryType = "Monthly",
                    SalaryCurrencyCode = "SAR",
                    SalaryCurrency = "Saudi Riyal",
                    ReasonOfChange = "Finished contract",
                    ReportingManager = "Surayie Al Dousary",
                    CompanyWebsite = "www.csc.sa"
                });

                employmentList.Add(new EmploymentHistoryDTO()
                {
                    AutoId = 2,
                    CompanyName = "Rendition Digital Inc.",
                    CompanyAddress = "Ayala Ave. Makati City, Philippines",
                    Designation = "Senior .Net Developer",
                    Role = "Senior Level",
                    FromDate = new DateTime(2010, 6, 1),
                    ToDate = new DateTime(2021, 10, 8),
                    LastDrawnSalary = 65000,
                    SalaryTypeCode = "MONTHLY",
                    SalaryType = "Monthly",
                    SalaryCurrencyCode = "PHP",
                    SalaryCurrency = "Philippine Peso",
                    ReasonOfChange = "Seek opportunity to work abroad",
                    ReportingManager = "Sonny Carvajal",
                    CompanyWebsite = "www.renditiondigital.com"
                });

                return employmentList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<OtherDocumentDTO>> GetOtherDocumentAsync()
        {
            try
            {
                List<OtherDocumentDTO> documentList = new List<OtherDocumentDTO>();

                documentList.Add(new OtherDocumentDTO()
                {
                    AutoId = 1,
                    DocumentName = "Project Porfolio",
                    DocumentTypeCode = "PROFDOC",
                    DocumentTypeDesc = "Professional Document",
                    Description = "List of all software application projects",
                    FileExtension = "PDF",
                    ContentTypeCode = "PDF",
                    UploadDate = new DateTime(2025, 8, 12)
                });

                documentList.Add(new OtherDocumentDTO()
                {
                    AutoId = 2,
                    DocumentName = "Project Plan",
                    DocumentTypeCode = "PERDOC",
                    DocumentTypeDesc = "Personal Document",
                    Description = "Timeline of all project deliverables",
                    FileExtension = "DOC",
                    ContentTypeCode = "Microsoft Word",
                    UploadDate = new DateTime(2024, 12, 20)
                });

                return documentList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeTransactionDTO>> GetEmployeeTransactionAsync()
        {
            try
            {
                List<EmployeeTransactionDTO> documentList = new List<EmployeeTransactionDTO>();

                documentList.Add(new EmployeeTransactionDTO()
                {
                    AutoId = 1,
                    ActionCode = "ADD",
                    ActionDesc = "Added skills and qualications",
                    StatusCode = "APPROVED",
                    Status = "Approved",
                    SectionCode = "SKILLQUAL",
                    Section = "Skills & Qualifications",
                    LastUpdateOn = DateTime.Now,
                    EmployeeNo = 10003632,
                    TransactionNo = 10001,
                    CurrentlyAssignedEmpNo = null,
                    CurrentlyAssignedEmpName = null
                });

                documentList.Add(new EmployeeTransactionDTO()
                {
                    AutoId = 2,
                    ActionCode = "UPDATE",
                    ActionDesc = "Updated CPR ID information",
                    StatusCode = "OPEN",
                    Status = "Pending for approval",
                    SectionCode = "IDENTITY",
                    Section = "Identity Proof",
                    LastUpdateOn = DateTime.Now,
                    EmployeeNo = 10003656,
                    TransactionNo = 10002,
                    CurrentlyAssignedEmpNo = 10003632,
                    CurrentlyAssignedEmpName = "Ervin Olinas Brosas"
                });

                return documentList;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<EmployeeMasterDTO>> GetEmployeeMasterAsync()
        {
            try
            {
                List<EmployeeMasterDTO> empList = new List<EmployeeMasterDTO>();

                empList.Add(new EmployeeMasterDTO()
                {
                    EmployeeNo = 10003632,
                    FirstName = "Ervin",
                    LastName = "Brosas",
                    Gender = "Male",
                    HireDate = new DateTime(2011, 10, 17),
                    EmploymentTypeCode = "EMPTYPEFT",
                    EmploymentType = "Full Time",
                    ReportingManagerCode = "10001662",
                    ReportingManager = "Khalid Jalal",
                    EmployeeStatusCode = "STATACTIVE",
                    EmployeeStatus = "Active"
                });

                empList.Add(new EmployeeMasterDTO()
                {
                    EmployeeNo = 10003589,
                    FirstName = "Nagendra",
                    LastName = "Seetharam",
                    Gender = "Male",
                    HireDate = new DateTime(2009, 1, 31),
                    EmploymentTypeCode = "EMPTYPEFT",
                    EmploymentType = "Full Time",
                    ReportingManagerCode = "10001662",
                    ReportingManager = "Khalid Jalal",
                    EmployeeStatusCode = "STATACTIVE",
                    EmployeeStatus = "Active"
                });

                return empList;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Database Methods         
        public async Task<Result<Employee>> CreateEmployeeAsync(Employee employee)
        {
            var errors = new Dictionary<string, string[]>();

            if (string.IsNullOrWhiteSpace(employee.FirstName))
                errors["FirstName"] = new[] { "First Name is required." };

            if (string.IsNullOrWhiteSpace(employee.LastName))
                errors["LastName"] = new[] { "Last Name is required." };

            if (errors.Any())
                return Result<Employee>.ValidationFailure(errors);

            try
            {
                await _repository.AddAsync(employee);
                return Result<Employee>.SuccessResult(employee);
            }
            catch (Exception ex)
            {
                return Result<Employee>.Failure($"Database error: {ex.Message}");
            }
        }
        
        public async Task<List<EmployeeDTO>> GetAllAsync()
        {
            var employees = await _repository.GetAllAsync();
            return employees.Select(e => new EmployeeDTO
            {
                EmployeeNo = e.EmployeeNo,
                FirstName = e.FirstName,
                MiddleName = e.MiddleName,
                LastName = e.LastName,
                Position = e.Position
            }).ToList();
        }

        public async Task AddAsync(EmployeeDTO dto)
        {
            var employee = new Employee
            {
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                LastName = dto.LastName,
                Position = dto.Position,
                NationalityCode = dto.NationalityCode,
                ReligionCode = dto.ReligionCode,
                GenderCode = dto.GenderCode,
                MaritalStatusCode = dto.MaritalStatusCode,
                OfficialEmail = dto.OfficialEmail,
                EmployeeNo = dto.EmployeeNo,
                HireDate = DateTime.Now
            };
            await _repository.AddAsync(employee);
        }

        public async Task<Result<List<UserDefinedCodeDTO>>> GetUserDefinedCodeAsync(string udcKey)
        {
            List<UserDefinedCodeDTO> udcList = new List<UserDefinedCodeDTO>();

            try
            {
                var repoResult = await _repository.GetUserDefinedCodeAsync(udcKey);
                if (!repoResult.Success)
                {
                    return Result<List<UserDefinedCodeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                udcList = repoResult.Value!.Select(e => new UserDefinedCodeDTO
                {
                    GroupID = e.GroupID,
                    UDCId = e.UDCId,
                    UDCCode = e.UDCCode,
                    UDCDesc1 = e.UDCDesc1,
                    UDCDesc2 = e.UDCDesc2,
                    UDCSpecialHandlingCode = e.UDCSpecialHandlingCode,
                    SequenceNo = e.SequenceNo,
                    IsActive = e.IsActive,
                    Amount = e.Amount
                }).ToList();

                return Result<List<UserDefinedCodeDTO>>.SuccessResult(udcList);
            }
            catch (Exception ex)
            {
                return Result<List<UserDefinedCodeDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in GetUserDefinedCodeAsync() method.");
            }
        }

        public async Task<Result<List<UserDefinedCodeDTO>>> GetUserDefinedCodeAllAsync()
        {
            List<UserDefinedCodeDTO> udcList = new List<UserDefinedCodeDTO>();

            try
            {
                var repoResult = await _repository.GetUserDefinedCodeAllAsync();

                if (!repoResult.Success)
                {
                    return Result<List<UserDefinedCodeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                udcList = repoResult.Value!.Select(e => new UserDefinedCodeDTO
                {
                    GroupID = e.GroupID,
                    UDCId = e.UDCId,
                    UDCCode = e.UDCCode,
                    UDCDesc1 = e.UDCDesc1,
                    UDCDesc2 = e.UDCDesc2,
                    UDCSpecialHandlingCode = e.UDCSpecialHandlingCode,
                    SequenceNo = e.SequenceNo,
                    IsActive = e.IsActive,
                    Amount = e.Amount
                }).ToList();

                return Result<List<UserDefinedCodeDTO>>.SuccessResult(udcList); 

            }
            catch (Exception ex)
            {
                return Result<List<UserDefinedCodeDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in GetUserDefinedCodeAllAsync() method.");
            }
        }

        public async Task<Result<List<UserDefinedCodeGroupDTO>>> GetUserDefinedCodeGroupAsync()
        {
            List<UserDefinedCodeGroupDTO> udcList = new List<UserDefinedCodeGroupDTO>();

            try
            {
                var repoResult = await _repository.GetUserDefinedCodeGroupAsync();
                if (!repoResult.Success)
                {
                    return Result<List<UserDefinedCodeGroupDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                udcList = repoResult.Value!.Select(e => new UserDefinedCodeGroupDTO
                {
                    UDCGroupId = e.UDCGroupId,
                    UDCGCode = e.UDCGCode,
                    UDCGDesc1 = e.UDCGDesc1,
                    UDCGDesc2 = e.UDCGDesc2,
                    UDCGSpecialHandlingCode = e.UDCGSpecialHandlingCode
                }).ToList();

                return Result<List<UserDefinedCodeGroupDTO>>.SuccessResult(udcList);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result<List<EmployeeMasterDTO>>> SearchEmployeeAsync(int? empNo, string? firstName, string? lastName, int? managerEmpNo,
            DateTime? joiningDate, string? statusCode, string? employmentType, string? departmentCode)
        {
            List<EmployeeMasterDTO> employeeList = new List<EmployeeMasterDTO>();
            try
            {
                var repoResult = await _repository.SearchEmployeeAsync(empNo, firstName, lastName, managerEmpNo,
                    joiningDate, statusCode, employmentType, departmentCode);

                if (!repoResult.Success)
                {
                    return Result<List<EmployeeMasterDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                employeeList = repoResult.Value!.Select(e => new EmployeeMasterDTO
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeNo = e.EmployeeNo,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Gender = e.Gender,
                    HireDate = e.HireDate,
                    EmploymentTypeCode = e.EmploymentTypeCode,
                    EmploymentType = e.EmploymentType,
                    ReportingManagerCode = e.ReportingManagerCode,
                    ReportingManager = e.ReportingManager,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    EmployeeStatusCode = e.EmployeeStatusCode,
                    EmployeeStatus = e.EmployeeStatus
                }).ToList();

                return Result<List<EmployeeMasterDTO>>.SuccessResult(employeeList);
            }
            catch (Exception ex)
            {
                return Result<List<EmployeeMasterDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching employee records from the database.");
            }
        }
        
        public async Task<Result<List<DepartmentDTO>>> GetDepartmentMasterAsync()
        {
            List<DepartmentDTO> departmentList = new List<DepartmentDTO>();  

            try
            {
                string departmentCode = string.Empty;
                string departmentName = string.Empty;
                string description = string.Empty;
                string groupCode = string.Empty;
                int? superintendentNo = null;
                int? managerEmpNo = null; 
                bool isActive = true;

                var repoResult = await _repository.GetDepartmentMasterAsync(departmentCode, departmentName, description, groupCode, superintendentNo, managerEmpNo, isActive);
                if (!repoResult.Success)
                {
                    return Result<List<DepartmentDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                departmentList = repoResult.Value!.Select(e => new DepartmentDTO
                {
                    DepartmentId = e.DepartmentId,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    GroupCode = e.GroupCode,
                    Description = e.Description,
                    ParentDepartmentId = e.ParentDepartmentId,
                    SuperintendentEmpNo = e.SuperintendentEmpNo,
                    ManagerEmpNo = e.ManagerEmpNo,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                }).ToList();

                return Result<List<DepartmentDTO>>.SuccessResult(departmentList);
            }
            catch (Exception ex)
            {
                return Result<List<DepartmentDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in GetDepartmentMasterAsync() method.");
            }
        }

        public async Task<Result<List<DepartmentDTO>>> SearchDepartmentAsync(string departmentName, string description, int? superintendentNo, int? managerEmpNo, bool isActive, 
            string departmentCode = "", string groupCode = "")
        {
            List<DepartmentDTO> departmentList = new List<DepartmentDTO>();

            try
            {
                var repoResult = await _repository.GetDepartmentMasterAsync(departmentCode, departmentName, description, groupCode, superintendentNo, managerEmpNo, isActive);
                if (!repoResult.Success)
                {
                    return Result<List<DepartmentDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                departmentList = repoResult.Value!.Select(e => new DepartmentDTO
                {
                    DepartmentId = e.DepartmentId,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    GroupCode = e.GroupCode,
                    GroupName = e.GroupName,
                    Description = e.Description,
                    ParentDepartmentId = e.ParentDepartmentId,
                    SuperintendentEmpNo = e.SuperintendentEmpNo,
                    SuperintendentName = e.SuperintendentName,
                    ManagerEmpNo = e.ManagerEmpNo,
                    ManagerName = e.ManagerName,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt,
                    UpdatedAt = e.UpdatedAt
                }).ToList();

                return Result<List<DepartmentDTO>>.SuccessResult(departmentList);
            }
            catch (Exception ex)
            {
                return Result<List<DepartmentDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in SearchDepartmentAsync() method.");
            }
        }

        public async Task<Result<List<EmployeeDTO>>> GetReportingManagerAsync()
        {
            List<EmployeeDTO> managerList = new List<EmployeeDTO>();

            try
            {
                string departmentCode = string.Empty;
                bool isActive = true;

                var repoResult = await _repository.GetReportingManagerAsync(departmentCode, isActive);
                if (!repoResult.Success)
                {
                    return Result<List<EmployeeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                managerList = repoResult.Value!.Select(e => new EmployeeDTO
                {
                    EmployeeNo = e.EmployeeNo,
                    FirstName = e.FirstName,
                    MiddleName = e.MiddleName,
                    LastName = e.LastName,
                    Position = e.Position
                }).ToList();

                return Result<List<EmployeeDTO>>.SuccessResult(managerList);
            }
            catch (Exception ex)
            {
                return Result<List<EmployeeDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in GetReportingManagerAsync() method.");
            }
        }

        public async Task<Result<EmployeeDTO>> GetEmployeeDetailAsync(int employeeId)
        {
            try
            {
                var repoResult = await _repository.GetEmployeeDetailAsync(employeeId);
                if (!repoResult.Success)
                {
                    return Result<EmployeeDTO>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    #region Fetch data for "About Me" and "Address" tabs                                        
                    EmployeeDTO employeeDetail = new EmployeeDTO
                    {
                        EmployeeId = model.EmployeeId,
                        FirstName = model.FirstName,
                        MiddleName = model.MiddleName,
                        LastName = model.LastName,
                        Position = model.Position,
                        DOB = model.DOB,
                        NationalityCode = model.NationalityCode,
                        NationalityDesc = model.NationalityDesc,
                        ReligionCode = model.ReligionCode,
                        ReligionDesc = model.ReligionDesc,
                        GenderCode = model.GenderCode,
                        GenderDesc = model.GenderDesc,
                        MaritalStatusCode = model.MaritalStatusCode,
                        MaritalStatusDesc = model.MaritalStatusDesc,
                        Salutation = model.Salutation,
                        SalutationDesc = model.SalutationDesc,
                        OfficialEmail = model.OfficialEmail,
                        PersonalEmail = model.PersonalEmail,
                        AlternateEmail = model.AlternateEmail,
                        OfficeLandlineNo = model.OfficeLandlineNo,
                        ResidenceLandlineNo = model.ResidenceLandlineNo,
                        OfficeExtNo = model.OfficeExtNo,
                        MobileNo = model.MobileNo,
                        AlternateMobileNo = model.AlternateMobileNo,
                        EmployeeNo = model.EmployeeNo,
                        ReportingManagerCode = model.ReportingManagerCode,
                        ReportingManager = model.ReportingManager,
                        WorkPermitID = model.WorkPermitID,
                        WorkPermitExpiryDate = model.WorkPermitExpiryDate,
                        HireDate = model.HireDate,
                        DateOfConfirmation = model.DateOfConfirmation,
                        TerminationDate = model.TerminationDate,
                        DateOfSuperannuation = model.DateOfSuperannuation,
                        Reemployed = model.Reemployed,
                        OldEmployeeNo = model.OldEmployeeNo,
                        DepartmentCode = model.DepartmentCode,
                        DepartmentName = model.DepartmentName,
                        EmploymentTypeCode = model.EmploymentTypeCode,
                        EmploymentType = model.EmploymentType,
                        RoleCode = model.RoleCode,
                        RoleType = model.RoleType,
                        FirstAttendanceModeCode = model.FirstAttendanceModeCode,
                        FirstAttendanceMode = model.FirstAttendanceMode,
                        SecondAttendanceModeCode = model.SecondAttendanceModeCode,
                        SecondAttendanceMode = model.SecondAttendanceMode,
                        ThirdAttendanceModeCode = model.ThirdAttendanceModeCode,
                        ThirdAttendanceMode = model.ThirdAttendanceMode,
                        SecondReportingManagerCode = model.SecondReportingManagerCode,
                        SecondReportingManager = model.SecondReportingManager,
                        Company = model.Company,
                        CompanyBranch= model.CompanyBranch,
                        EducationCode = model.EducationCode,
                        EducationDesc = model.EducationDesc,
                        EmployeeClassCode = model.EmployeeClassCode,
                        EmployeeClassDesc = model.EmployeeClassDesc,
                        JobTitleCode = model.JobTitleCode,
                        JobTitleDesc = model.JobTitleDesc,
                        PayGrade = model.PayGrade,
                        PayGradeDesc = model.PayGradeDesc,
                        IsActive = model.IsActive,
                        AccountTypeCode = model.AccountTypeCode,
                        AccountTypeDesc = model.AccountTypeDesc,
                        AccountNumber = model.AccountNumber,
                        AccountHolderName = model.AccountHolderName,
                        BankNameCode = model.BankNameCode,
                        BankName = model.BankName,
                        BankBranchName = model.BankBranchName,
                        IBANNumber = model.IBANNumber,
                        TaxNumber = model.TaxNumber,
                        LinkedInAccount = model.LinkedInAccount,
                        FacebookAccount = model.FacebookAccount,
                        TwitterAccount = model.TwitterAccount,
                        InstagramAccount = model.InstagramAccount,
                        PresentAddress = model.PresentAddress,
                        PresentCountryCode = model.PresentCountryCode,
                        PresentCountryDesc = model.PresentCountryDesc,
                        PresentCity = model.PresentCity,
                        PresentAreaCode = model.PresentAreaCode,
                        PresentContactNo = model.PresentContactNo,
                        PresentMobileNo = model.PresentMobileNo,
                        PermanentAddress = model.PermanentAddress,                        
                        PermanentCountryCode = model.PermanentCountryCode,
                        PermanentCountryDesc = model.PermanentCountryDesc,
                        PermanentCity = model.PermanentCity,
                        PermanentAreaCode = model.PermanentAreaCode,
                        PermanentContactNo = model.PermanentContactNo,
                        PermanentMobileNo = model.PermanentMobileNo,
                        EmployeeStatusCode = model.EmployeeStatusCode,
                        EmployeeStatusDesc = model.EmployeeStatusDesc
                    };
                    #endregion

                    #region Fetch data for "Identity Proof" tab
                    if (model.IdentityProof != null)
                        employeeDetail.EmpIdentityProof = new IdentityProofDTO()
                        {
                            AutoId = model.IdentityProof.AutoId,
                            PassportNumber = model.IdentityProof.PassportNumber,
                            DateOfIssue = model.IdentityProof.DateOfIssue,
                            DateOfExpiry = model.IdentityProof.DateOfExpiry,
                            PlaceOfIssue = model.IdentityProof.PlaceOfIssue,
                            DrivingLicenseNo = model.IdentityProof.DrivingLicenseNo,
                            DLDateOfIssue = model.IdentityProof.DLDateOfIssue,
                            DLDateOfExpiry = model.IdentityProof.DLDateOfExpiry,
                            DLPlaceOfIssue = model.IdentityProof.DLPlaceOfIssue,
                            TypeOfDocument = model.IdentityProof.TypeOfDocument,
                            OtherDocNumber = model.IdentityProof.OtherDocNumber,
                            OtherDocDateOfIssue = model.IdentityProof.OtherDocDateOfIssue,
                            OtherDocDateOfExpiry = model.IdentityProof.OtherDocDateOfExpiry,
                            NationalIDNumber = model.IdentityProof.NationalIDNumber,
                            NationalIDTypeCode = model.IdentityProof.NationalIDTypeCode,
                            NationalIDTypeDesc = model.IdentityProof.NationalIDTypeDesc,
                            NatIDPlaceOfIssue = model.IdentityProof.NatIDPlaceOfIssue,
                            NatIDDateOfIssue = model.IdentityProof.NatIDDateOfIssue,
                            NatIDDateOfExpiry = model.IdentityProof.NatIDDateOfExpiry,
                            ContractNumber = model.IdentityProof.ContractNumber,
                            ContractPlaceOfIssue = model.IdentityProof.ContractPlaceOfIssue,
                            ContractDateOfIssue = model.IdentityProof.ContractDateOfIssue,
                            ContractDateOfExpiry = model.IdentityProof.ContractDateOfExpiry,
                            VisaNumber = model.IdentityProof.VisaNumber,
                            VisaTypeCode = model.IdentityProof.VisaTypeCode,
                            VisaTypeDesc = model.IdentityProof.VisaTypeDesc,
                            VisaCountryCode = model.IdentityProof.VisaCountryCode,
                            VisaCountryDesc = model.IdentityProof.VisaCountryDesc,
                            Profession = model.IdentityProof.Profession,
                            Sponsor = model.IdentityProof.Sponsor,
                            VisaDateOfIssue = model.IdentityProof.VisaDateOfIssue,
                            VisaDateOfExpiry = model.IdentityProof.VisaDateOfExpiry
                    };
                    #endregion

                    #region Fetch data for "Emergency Contacts" grid
                    if (model.EmergencyContactList != null)
                    {
                        employeeDetail.EmergencyContactList = model.EmergencyContactList!.Select(e => new EmergencyContactDTO
                        {
                            AutoId = e.AutoId,
                            ContactPerson = e.ContactPerson,
                            RelationCode = e.RelationCode,
                            Relation = e.Relation,
                            MobileNo = e.MobileNo,
                            LandlineNo = e.LandlineNo,
                            Address = e.Address,
                            CountryCode = e.CountryCode,
                            CountryDesc = e.CountryDesc,
                            City = e.City
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Qualifications" grid
                    if (model.Qualifications != null)
                    {
                        employeeDetail.QualificationList = model.Qualifications!.Select(e => new QualificationDTO
                        {
                            AutoId = e.AutoId,
                            QualificationCode = e.QualificationCode,
                            QualificationDesc = e.QualificationDesc,
                            StreamCode = e.StreamCode,
                            StreamDesc = e.StreamDesc,
                            SpecializationCode = e.SpecializationCode,
                            SpecializationDesc = e.SpecializationDesc,
                            UniversityName = e.UniversityName,
                            Institute = e.Institute,
                            QualificationMode = e.QualificationMode,
                            QualificationModeDesc = e.QualificationModeDesc,
                            CountryCode = e.CountryCode,
                            CountryDesc = e.CountryDesc,
                            StateCode = e.StateCode,
                            StateDesc = e.StateDesc,
                            CityTownName = e.CityTownName,
                            FromMonthCode = e.FromMonthCode,
                            FromMonthDesc = e.FromMonthDesc,
                            FromYear = e.FromYear,
                            ToMonthCode = e.ToMonthCode,
                            ToMonthDesc = e.ToMonthDesc,
                            ToYear = e.ToYear,
                            PassMonthCode = e.PassMonthCode,
                            PassMonthDesc = e.PassMonthDesc,
                            PassYear = e.PassYear
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Skills" grid
                    if (model.EmployeeSkills != null)
                    {
                        employeeDetail.EmployeeSkillList = model.EmployeeSkills!.Select(e => new EmployeeSkillDTO
                        {
                            AutoId = e.AutoId,
                            SkillName = e.SkillName,
                            LevelCode = e.LevelCode,
                            LevelDesc = e.LevelDesc,
                            LastUsedMonthCode = e.LastUsedMonthCode,
                            LastUsedMonthDesc = e.LastUsedMonthDesc,
                            LastUsedYear = e.LastUsedYear,
                            FromMonthCode = e.FromMonthCode,
                            FromMonthDesc = e.FromMonthDesc,
                            FromYear = e.FromYear,
                            ToMonthCode = e.ToMonthCode,
                            ToMonthDesc = e.ToMonthDesc,
                            ToYear = e.ToYear
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Certifications & Trainings" grid
                    if (model.EmployeeCertifications != null)
                    {
                        employeeDetail.EmployeeCertificationList = model.EmployeeCertifications!.Select(e => new EmployeeCertificationDTO
                        {
                            AutoId = e.AutoId,
                            QualificationCode = e.QualificationCode,
                            QualificationDesc = e.QualificationDesc,
                            StreamCode = e.StreamCode,
                            StreamDesc = e.StreamDesc,
                            Specialization = e.Specialization,
                            University = e.University,
                            Institute = e.Institute,
                            CountryCode = e.CountryCode,
                            Country = e.Country,
                            State = e.State,
                            CityTownName = e.CityTownName,
                            FromMonthCode = e.FromMonthCode,
                            FromMonth = e.FromMonth,
                            FromYear = e.FromYear,
                            ToMonthCode = e.ToMonthCode,
                            ToMonth = e.ToMonth,
                            ToYear = e.ToYear,
                            PassMonthCode = e.PassMonthCode,
                            PassMonth = e.PassMonth,
                            PassYear = e.PassYear
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Languages" grid
                    if (model.LanguageSkills != null)
                    {
                        employeeDetail.LanguageSkillList = model.LanguageSkills!.Select(e => new LanguageSkillDTO
                        {
                            AutoId = e.AutoId,
                            LanguageCode = e.LanguageCode,
                            LanguageDesc = e.LanguageDesc,
                            CanWrite = e.CanWrite,
                            CanSpeak = e.CanSpeak,
                            CanRead = e.CanRead,
                            MotherTongue = e.MotherTongue
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Family Visa Details" grid
                    if (model.FamilyVisas != null)
                    {
                        employeeDetail.FamilyVisaList = model.FamilyVisas!.Select(e => new FamilyVisaDTO
                        {
                            AutoId = e.AutoId,
                            FamilyId = e.FamilyId,
                            FamilyMemberName = e.FamilyMemberName,
                            CountryCode = e.CountryCode,
                            Country = e.Country,
                            VisaTypeCode = e.VisaTypeCode,
                            VisaType = e.VisaType,
                            Profession = e.Profession,
                            IssueDate = e.IssueDate,
                            ExpiryDate = e.ExpiryDate
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Family Members" grid
                    if (model.FamilyMembers != null)
                    {
                        employeeDetail.FamilyMemberList = model.FamilyMembers!.Select(e => new FamilyMemberDTO
                        {
                            AutoId = e.AutoId,
                            FirstName = e.FirstName,
                            MiddleName = e.MiddleName,
                            LastName = e.LastName,
                            RelationCode = e.RelationCode,
                            Relation = e.Relation,
                            DOB = e.DOB,
                            QualificationCode = e.QualificationCode,
                            Qualification = e.Qualification,
                            StreamCode = e.StreamCode,
                            StreamDesc = e.StreamDesc,
                            SpecializationCode = e.SpecializationCode,
                            Specialization = e.Specialization,
                            Occupation = e.Occupation,
                            ContactNo = e.ContactNo,
                            CountryCode = e.CountryCode,
                            Country = e.Country,
                            StateCode = e.StateCode,
                            State = e.State,
                            CityTownName = e.CityTownName,
                            District = e.District,
                            IsDependent = e.IsDependent
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Employment History" grid
                    if (model.EmploymentHistories != null)
                    {
                        employeeDetail.EmploymentHistoryList = model.EmploymentHistories!.Select(e => new EmploymentHistoryDTO
                        {
                            AutoId = e.AutoId,
                            CompanyName = e.CompanyName,
                            CompanyAddress = e.CompanyAddress,
                            Designation = e.Designation,
                            Role = e.Role,
                            FromDate = e.FromDate,
                            ToDate = e.ToDate,
                            LastDrawnSalary = e.LastDrawnSalary,
                            SalaryTypeCode = e.SalaryTypeCode,
                            SalaryType = e.SalaryType,
                            SalaryCurrencyCode = e.SalaryCurrencyCode,
                            SalaryCurrency = e.SalaryCurrency,
                            ReasonOfChange = e.ReasonOfChange,
                            ReportingManager = e.ReportingManager,
                            CompanyWebsite = e.CompanyWebsite
                        }).ToList();
                    }
                    #endregion

                    #region Fetch data for "Other Documents" grid
                    if (model.OtherDocuments != null)
                    {
                        employeeDetail.OtherDocumentList = model.OtherDocuments!.Select(e => new OtherDocumentDTO
                        {
                            AutoId = e.AutoId,
                            DocumentName = e.DocumentName,
                            DocumentTypeCode = e.DocumentTypeCode,
                            DocumentTypeDesc = e.DocumentTypeDesc,
                            Description = e.Description,
                            FileData = e.FileData,
                            FileExtension = e.FileExtension,
                            ContentTypeCode = e.ContentTypeCode,
                            ContentTypeDesc = e.ContentTypeDesc,
                            UploadDate = e.UploadDate
                        }).ToList();
                    }
                    #endregion

                    return Result<EmployeeDTO>.SuccessResult(employeeDetail);
                }
                else
                {
                    throw new Exception("Employee not found!");
                }
            }
            catch (Exception ex)
            {
                return Result<EmployeeDTO>.Failure(ex.Message.ToString() ?? "Unknown error in GetEmployeeDetailAsync() method.");
            }
        }

        public async Task<Result<int>> GetMaxEmployeeNoAsync()
        {
            int maxEmployeeNo = 0;

            try
            {
                var repoResult = await _repository.GetMaxEmployeeNoAsync();
                if (!repoResult.Success)
                {
                    return Result<int>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                maxEmployeeNo = repoResult.Value;

                return Result<int>.SuccessResult(maxEmployeeNo);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString() ?? "Unknown error in GetMaxEmployeeNoAsync() method.");
            }
        }

        public async Task<Result<int>> SaveEmployeeAsync(EmployeeDTO employee, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize Employee entity
                Employee employeeEntity = new Employee();

                #region Personal Detail         
                employeeEntity.EmployeeId = employee.EmployeeId;
                employeeEntity.FirstName = employee.FirstName;
                employeeEntity.MiddleName = employee.MiddleName;
                employeeEntity.LastName = employee.LastName;
                employeeEntity.Position = employee.Position;
                employeeEntity.DOB = employee.DOB;
                employeeEntity.NationalityCode = employee.NationalityCode;
                employeeEntity.ReligionCode = employee.ReligionCode;
                employeeEntity.GenderCode = employee.GenderCode;
                employeeEntity.MaritalStatusCode = employee.MaritalStatusCode;
                employeeEntity.Salutation = employee.Salutation;
                #endregion

                #region Contact Detail Implementation
                employeeEntity.OfficialEmail = employee.OfficialEmail;
                employeeEntity.PersonalEmail = employee.PersonalEmail;
                employeeEntity.AlternateEmail = employee.AlternateEmail;
                employeeEntity.OfficeLandlineNo = employee.OfficeLandlineNo;
                employeeEntity.ResidenceLandlineNo = employee.ResidenceLandlineNo;
                employeeEntity.OfficeExtNo = employee.OfficeExtNo;
                employeeEntity.MobileNo = employee.MobileNo;
                employeeEntity.AlternateMobileNo = employee.AlternateMobileNo;
                #endregion

                #region Employment Detail Implementation
                employeeEntity.EmployeeNo = employee.EmployeeNo;
                employeeEntity.EmployeeStatusCode = employee.EmployeeStatusCode;
                employeeEntity.ReportingManagerCode = employee.ReportingManagerCode;
                employeeEntity.WorkPermitID = employee.WorkPermitID;
                employeeEntity.WorkPermitExpiryDate = employee.WorkPermitExpiryDate;
                employeeEntity.HireDate = employee.HireDate!.Value;
                employeeEntity.DateOfConfirmation = employee.DateOfConfirmation;
                employeeEntity.TerminationDate = employee.TerminationDate;
                employeeEntity.DateOfSuperannuation = employee.DateOfSuperannuation;
                employeeEntity.Reemployed = employee.Reemployed;
                employeeEntity.OldEmployeeNo = employee.OldEmployeeNo;
                employeeEntity.DepartmentCode = employee.DepartmentCode;
                employeeEntity.EmploymentTypeCode = employee.EmploymentTypeCode;
                employeeEntity.RoleCode = employee.RoleCode;
                employeeEntity.FirstAttendanceModeCode = employee.FirstAttendanceModeCode;
                employeeEntity.SecondAttendanceModeCode = employee.SecondAttendanceModeCode;
                employeeEntity.ThirdAttendanceModeCode = employee.ThirdAttendanceModeCode;
                employeeEntity.SecondReportingManagerCode = employee.SecondReportingManagerCode;
                #endregion

                #region Attribute Detail Implementation
                employeeEntity.Company = employee.Company;
                employeeEntity.CompanyBranch = employee.CompanyBranch;
                employeeEntity.EducationCode = employee.EducationCode;
                employeeEntity.EmployeeClassCode = employee.EmployeeClassCode;
                employeeEntity.JobTitleCode = employee.JobTitleCode;
                employeeEntity.PayGrade = employee.PayGrade;
                employeeEntity.IsActive = employee.IsActive;
                #endregion

                #region Bank Detail Implementation  
                employeeEntity.AccountTypeCode = employee.AccountTypeCode;
                employeeEntity.AccountNumber = employee.AccountNumber;
                employeeEntity.AccountHolderName = employee.AccountHolderName;
                employeeEntity.BankNameCode = employee.BankNameCode;
                employeeEntity.BankBranchName = employee.BankBranchName;
                employeeEntity.IBANNumber = employee.IBANNumber;
                employeeEntity.TaxNumber = employee.TaxNumber;
                #endregion

                #region Social Connect Implementation
                employeeEntity.LinkedInAccount = employee.LinkedInAccount;
                employeeEntity.FacebookAccount = employee.FacebookAccount;
                employeeEntity.TwitterAccount = employee.TwitterAccount;
                employeeEntity.InstagramAccount = employee.InstagramAccount;
                #endregion

                #region Primary Location Implementation
                employeeEntity.PresentAddress = employee.PresentAddress;
                employeeEntity.PresentCountryCode = employee.PresentCountryCode;
                employeeEntity.PresentCity = employee.PresentCity;
                employeeEntity.PresentAreaCode = employee.PresentAreaCode;
                employeeEntity.PresentContactNo = employee.PresentContactNo;
                employeeEntity.PresentMobileNo = employee.PresentMobileNo;
                employeeEntity.PermanentAddress = employee.PermanentAddress;
                employeeEntity.PermanentCountryCode = employee.PermanentCountryCode;
                employeeEntity.PermanentCity = employee.PermanentCity;
                employeeEntity.PermanentAreaCode = employee.PermanentAreaCode;
                employeeEntity.PermanentContactNo = employee.PermanentContactNo;
                employeeEntity.PermanentMobileNo = employee.PermanentMobileNo;
                #endregion

                #endregion

                #region Initialize IdentityProof entity
                IdentityProof? identityProof = null;

                if (employee.EmpIdentityProof != null)
                {
                    identityProof = new IdentityProof()
                    {
                        AutoId = employee.EmpIdentityProof!.AutoId,
                        PassportNumber = employee.EmpIdentityProof!.PassportNumber,
                        DateOfIssue = employee.EmpIdentityProof!.DateOfIssue,
                        DateOfExpiry = employee.EmpIdentityProof!.DateOfExpiry,
                        PlaceOfIssue = employee.EmpIdentityProof!.PlaceOfIssue,
                        DrivingLicenseNo = employee.EmpIdentityProof!.DrivingLicenseNo,
                        DLDateOfIssue = employee.EmpIdentityProof!.DLDateOfIssue,
                        DLDateOfExpiry = employee.EmpIdentityProof!.DLDateOfExpiry,
                        DLPlaceOfIssue = employee.EmpIdentityProof!.DLPlaceOfIssue,
                        TypeOfDocument = employee.EmpIdentityProof!.TypeOfDocument,
                        OtherDocNumber = employee.EmpIdentityProof!.OtherDocNumber,
                        OtherDocDateOfIssue = employee.EmpIdentityProof!.OtherDocDateOfIssue,
                        OtherDocDateOfExpiry = employee.EmpIdentityProof!.OtherDocDateOfExpiry,
                        NationalIDNumber = employee.EmpIdentityProof!.NationalIDNumber,
                        NationalIDTypeCode = employee.EmpIdentityProof!.NationalIDTypeCode,
                        NatIDPlaceOfIssue = employee.EmpIdentityProof!.NatIDPlaceOfIssue,
                        NatIDDateOfIssue = employee.EmpIdentityProof!.NatIDDateOfIssue,
                        NatIDDateOfExpiry = employee.EmpIdentityProof!.NatIDDateOfExpiry,
                        ContractNumber = employee.EmpIdentityProof!.ContractNumber,
                        ContractPlaceOfIssue = employee.EmpIdentityProof!.ContractPlaceOfIssue,
                        ContractDateOfIssue = employee.EmpIdentityProof!.ContractDateOfIssue,
                        ContractDateOfExpiry = employee.EmpIdentityProof!.ContractDateOfExpiry,
                        VisaNumber = employee.EmpIdentityProof!.VisaNumber,
                        VisaTypeCode = employee.EmpIdentityProof!.VisaTypeCode,
                        VisaCountryCode = employee.EmpIdentityProof!.VisaCountryCode,
                        Profession = employee.EmpIdentityProof!.Profession,
                        Sponsor = employee.EmpIdentityProof!.Sponsor,
                        VisaDateOfIssue = employee.EmpIdentityProof!.VisaDateOfIssue,
                        VisaDateOfExpiry = employee.EmpIdentityProof!.VisaDateOfExpiry
                    };
                }

                employeeEntity.IdentityProof = identityProof;
                #endregion

                #region Initialize Emergency Contacts entity
                List<EmergencyContact> emergencyContactList = new List<EmergencyContact>();

                if (employee.EmergencyContactList != null && employee.EmergencyContactList.Any())
                {
                    emergencyContactList = employee.EmergencyContactList.Select(e => new EmergencyContact
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

                employeeEntity.EmergencyContactList = emergencyContactList;
                #endregion

                #region Initialize Qualifications entity
                List<Qualification> qualificationList = new List<Qualification>();

                if (employee.QualificationList != null && employee.QualificationList.Any())
                {
                    qualificationList = employee.QualificationList.Select(e => new Qualification
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

                employeeEntity.Qualifications = qualificationList;
                #endregion

                #region Initialize EmployeeSkill entity
                List<EmployeeSkill> skillList = new List<EmployeeSkill>();

                if (employee.EmployeeSkillList != null && employee.EmployeeSkillList.Any())
                {
                    skillList = employee.EmployeeSkillList.Select(e => new EmployeeSkill
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

                employeeEntity.EmployeeSkills = skillList;
                #endregion

                #region Initialize EmployeeCertification entity
                List<EmployeeCertification> certfList = new List<EmployeeCertification>();

                if (employee.EmployeeCertificationList != null && employee.EmployeeCertificationList.Any())
                {
                    certfList = employee.EmployeeCertificationList.Select(e => new EmployeeCertification
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

                employeeEntity.EmployeeCertifications = certfList;
                #endregion

                #region Initialize LanguageSkill entity
                List<LanguageSkill> languageList = new List<LanguageSkill>();
                if (employee.LanguageSkillList != null && employee.LanguageSkillList.Any())
                {
                    languageList = employee.LanguageSkillList.Select(e => new LanguageSkill
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

                employeeEntity.LanguageSkills = languageList;
                #endregion

                #region Initialize FamilyMember entity
                List<FamilyMember> familyList = new List<FamilyMember>();
                if (employee.FamilyMemberList != null && employee.FamilyMemberList.Any())
                {
                    familyList = employee.FamilyMemberList.Select(e => new FamilyMember
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

                employeeEntity.FamilyMembers = familyList;
                #endregion

                #region Initialize FamilyVisa entity
                List<FamilyVisa> familyVisaList = new List<FamilyVisa>();
                if (employee.FamilyVisaList != null && employee.FamilyVisaList.Any())
                {
                    familyVisaList = employee.FamilyVisaList.Select(e => new FamilyVisa
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

                employeeEntity.FamilyVisas = familyVisaList;
                #endregion

                #region Initialize EmploymentHistory entity
                List<EmploymentHistory> historyList = new List<EmploymentHistory>();
                if (employee.EmploymentHistoryList != null && employee.EmploymentHistoryList.Any())
                {
                    historyList = employee.EmploymentHistoryList.Select(e => new EmploymentHistory
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

                employeeEntity.EmploymentHistories = historyList;
                #endregion

                #region Initialize OtherDocument entity
                List<OtherDocument> docsList = new List<OtherDocument>();
                if (employee.OtherDocumentList != null && employee.OtherDocumentList.Any())
                {
                    docsList = employee.OtherDocumentList.Select(e => new OtherDocument
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

                employeeEntity.OtherDocuments = docsList;
                #endregion

                var result = await _repository.SaveEmployeeAsync(employeeEntity, cancellationToken);
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

        public async Task<Result<int>> AddEmployeeAsync(EmployeeDTO employee, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize Employee entity
                Employee employeeEntity = new Employee();

                #region Personal Detail         
                employeeEntity.EmployeeId = employee.EmployeeId;
                employeeEntity.FirstName = employee.FirstName;
                employeeEntity.MiddleName = employee.MiddleName;
                employeeEntity.LastName = employee.LastName;
                employeeEntity.Position = employee.Position;
                employeeEntity.DOB = employee.DOB;
                employeeEntity.NationalityCode = employee.NationalityCode;
                employeeEntity.ReligionCode = employee.ReligionCode;
                employeeEntity.GenderCode = employee.GenderCode;
                employeeEntity.MaritalStatusCode = employee.MaritalStatusCode;
                employeeEntity.Salutation = employee.Salutation;
                #endregion

                #region Contact Detail Implementation
                employeeEntity.OfficialEmail = employee.OfficialEmail;
                employeeEntity.PersonalEmail = employee.PersonalEmail;
                employeeEntity.AlternateEmail = employee.AlternateEmail;
                employeeEntity.OfficeLandlineNo = employee.OfficeLandlineNo;
                employeeEntity.ResidenceLandlineNo = employee.ResidenceLandlineNo;
                employeeEntity.OfficeExtNo = employee.OfficeExtNo;
                employeeEntity.MobileNo = employee.MobileNo;
                employeeEntity.AlternateMobileNo = employee.AlternateMobileNo;
                #endregion

                #region Employment Detail Implementation
                employeeEntity.EmployeeNo = employee.EmployeeNo;
                employeeEntity.EmployeeStatusCode = employee.EmployeeStatusCode;
                employeeEntity.ReportingManagerCode = employee.ReportingManagerCode;
                employeeEntity.WorkPermitID = employee.WorkPermitID;
                employeeEntity.WorkPermitExpiryDate = employee.WorkPermitExpiryDate;
                employeeEntity.HireDate = employee.HireDate!.Value;
                employeeEntity.DateOfConfirmation = employee.DateOfConfirmation;
                employeeEntity.TerminationDate = employee.TerminationDate;
                employeeEntity.DateOfSuperannuation = employee.DateOfSuperannuation;
                employeeEntity.Reemployed = employee.Reemployed;
                employeeEntity.OldEmployeeNo = employee.OldEmployeeNo;
                employeeEntity.DepartmentCode = employee.DepartmentCode;
                employeeEntity.EmploymentTypeCode = employee.EmploymentTypeCode;
                employeeEntity.RoleCode = employee.RoleCode;
                employeeEntity.FirstAttendanceModeCode = employee.FirstAttendanceModeCode;
                employeeEntity.SecondAttendanceModeCode = employee.SecondAttendanceModeCode;
                employeeEntity.ThirdAttendanceModeCode = employee.ThirdAttendanceModeCode;
                employeeEntity.SecondReportingManagerCode = employee.SecondReportingManagerCode;
                #endregion

                #region Attribute Detail Implementation
                employeeEntity.Company = employee.Company;
                employeeEntity.CompanyBranch = employee.CompanyBranch;
                employeeEntity.EducationCode = employee.EducationCode;
                employeeEntity.EmployeeClassCode = employee.EmployeeClassCode;
                employeeEntity.JobTitleCode = employee.JobTitleCode;
                employeeEntity.Position = employee.JobTitleDesc;
                employeeEntity.PayGrade = employee.PayGrade;
                employeeEntity.IsActive = employee.IsActive;
                #endregion

                #region Bank Detail Implementation  
                employeeEntity.AccountTypeCode = employee.AccountTypeCode;
                employeeEntity.AccountNumber = employee.AccountNumber;
                employeeEntity.AccountHolderName = employee.AccountHolderName;
                employeeEntity.BankNameCode = employee.BankNameCode;
                employeeEntity.BankBranchName = employee.BankBranchName;
                employeeEntity.IBANNumber = employee.IBANNumber;
                employeeEntity.TaxNumber = employee.TaxNumber;
                #endregion

                #region Social Connect Implementation
                employeeEntity.LinkedInAccount = employee.LinkedInAccount;
                employeeEntity.FacebookAccount = employee.FacebookAccount;
                employeeEntity.TwitterAccount = employee.TwitterAccount;
                employeeEntity.InstagramAccount = employee.InstagramAccount;
                #endregion

                #region Primary Location Implementation
                employeeEntity.PresentAddress = employee.PresentAddress;
                employeeEntity.PresentCountryCode = employee.PresentCountryCode;
                employeeEntity.PresentCity = employee.PresentCity;
                employeeEntity.PresentAreaCode = employee.PresentAreaCode;
                employeeEntity.PresentContactNo = employee.PresentContactNo;
                employeeEntity.PresentMobileNo = employee.PresentMobileNo;
                employeeEntity.PermanentAddress = employee.PermanentAddress;
                employeeEntity.PermanentCountryCode = employee.PermanentCountryCode;
                employeeEntity.PermanentCity = employee.PermanentCity;
                employeeEntity.PermanentAreaCode = employee.PermanentAreaCode;
                employeeEntity.PermanentContactNo = employee.PermanentContactNo;
                employeeEntity.PermanentMobileNo = employee.PermanentMobileNo;
                #endregion

                #endregion

                #region Initialize IdentityProof entity
                IdentityProof? identityProof = null;

                if (employee.EmpIdentityProof != null)
                {
                    identityProof = new IdentityProof()
                    {
                        AutoId = employee.EmpIdentityProof!.AutoId,
                        PassportNumber = employee.EmpIdentityProof!.PassportNumber,
                        DateOfIssue = employee.EmpIdentityProof!.DateOfIssue,
                        DateOfExpiry = employee.EmpIdentityProof!.DateOfExpiry,
                        PlaceOfIssue = employee.EmpIdentityProof!.PlaceOfIssue,
                        DrivingLicenseNo = employee.EmpIdentityProof!.DrivingLicenseNo,
                        DLDateOfIssue = employee.EmpIdentityProof!.DLDateOfIssue,
                        DLDateOfExpiry = employee.EmpIdentityProof!.DLDateOfExpiry,
                        DLPlaceOfIssue = employee.EmpIdentityProof!.DLPlaceOfIssue,
                        TypeOfDocument = employee.EmpIdentityProof!.TypeOfDocument,
                        OtherDocNumber = employee.EmpIdentityProof!.OtherDocNumber,
                        OtherDocDateOfIssue = employee.EmpIdentityProof!.OtherDocDateOfIssue,
                        OtherDocDateOfExpiry = employee.EmpIdentityProof!.OtherDocDateOfExpiry,
                        NationalIDNumber = employee.EmpIdentityProof!.NationalIDNumber,
                        NationalIDTypeCode = employee.EmpIdentityProof!.NationalIDTypeCode,
                        NatIDPlaceOfIssue = employee.EmpIdentityProof!.NatIDPlaceOfIssue,
                        NatIDDateOfIssue = employee.EmpIdentityProof!.NatIDDateOfIssue,
                        NatIDDateOfExpiry = employee.EmpIdentityProof!.NatIDDateOfExpiry,
                        ContractNumber = employee.EmpIdentityProof!.ContractNumber,
                        ContractPlaceOfIssue = employee.EmpIdentityProof!.ContractPlaceOfIssue,
                        ContractDateOfIssue = employee.EmpIdentityProof!.ContractDateOfIssue,
                        ContractDateOfExpiry = employee.EmpIdentityProof!.ContractDateOfExpiry,
                        VisaNumber = employee.EmpIdentityProof!.VisaNumber,
                        VisaTypeCode = employee.EmpIdentityProof!.VisaTypeCode,
                        VisaCountryCode = employee.EmpIdentityProof!.VisaCountryCode,
                        Profession = employee.EmpIdentityProof!.Profession,
                        Sponsor = employee.EmpIdentityProof!.Sponsor,
                        VisaDateOfIssue = employee.EmpIdentityProof!.VisaDateOfIssue,
                        VisaDateOfExpiry = employee.EmpIdentityProof!.VisaDateOfExpiry
                    };
                }

                employeeEntity.IdentityProof = identityProof;
                #endregion

                #region Initialize Emergency Contacts entity
                List<EmergencyContact> emergencyContactList = new List<EmergencyContact>();

                if (employee.EmergencyContactList != null && employee.EmergencyContactList.Any())
                {
                    emergencyContactList = employee.EmergencyContactList.Select(e => new EmergencyContact
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

                employeeEntity.EmergencyContactList = emergencyContactList;
                #endregion

                #region Initialize Qualifications entity
                List<Qualification> qualificationList = new List<Qualification>();

                if (employee.QualificationList != null && employee.QualificationList.Any())
                {
                    qualificationList = employee.QualificationList.Select(e => new Qualification
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

                employeeEntity.Qualifications = qualificationList;
                #endregion

                #region Initialize EmployeeSkill entity
                List<EmployeeSkill> skillList = new List<EmployeeSkill>();

                if (employee.EmployeeSkillList != null && employee.EmployeeSkillList.Any())
                {
                    skillList = employee.EmployeeSkillList.Select(e => new EmployeeSkill
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

                employeeEntity.EmployeeSkills = skillList;
                #endregion

                #region Initialize EmployeeCertification entity
                List<EmployeeCertification> certfList = new List<EmployeeCertification>();

                if (employee.EmployeeCertificationList != null && employee.EmployeeCertificationList.Any())
                {
                    certfList = employee.EmployeeCertificationList.Select(e => new EmployeeCertification
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

                employeeEntity.EmployeeCertifications = certfList;
                #endregion

                #region Initialize LanguageSkill entity
                List<LanguageSkill> languageList = new List<LanguageSkill>();
                if (employee.LanguageSkillList != null && employee.LanguageSkillList.Any())
                {
                    languageList = employee.LanguageSkillList.Select(e => new LanguageSkill
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

                employeeEntity.LanguageSkills = languageList;
                #endregion

                #region Initialize FamilyMember entity
                List<FamilyMember> familyList = new List<FamilyMember>();
                if (employee.FamilyMemberList != null && employee.FamilyMemberList.Any())
                {
                    familyList = employee.FamilyMemberList.Select(e => new FamilyMember
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

                employeeEntity.FamilyMembers = familyList;
                #endregion

                #region Initialize FamilyVisa entity
                List<FamilyVisa> familyVisaList = new List<FamilyVisa>();
                if (employee.FamilyVisaList != null && employee.FamilyVisaList.Any())
                {
                    familyVisaList = employee.FamilyVisaList.Select(e => new FamilyVisa
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

                employeeEntity.FamilyVisas = familyVisaList;
                #endregion

                #region Initialize EmploymentHistory entity
                List<EmploymentHistory> historyList = new List<EmploymentHistory>();
                if (employee.EmploymentHistoryList != null && employee.EmploymentHistoryList.Any())
                {
                    historyList = employee.EmploymentHistoryList.Select(e => new EmploymentHistory
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

                employeeEntity.EmploymentHistories = historyList;
                #endregion

                #region Initialize OtherDocument entity
                List<OtherDocument> docsList = new List<OtherDocument>();
                if (employee.OtherDocumentList != null && employee.OtherDocumentList.Any())
                {
                    docsList = employee.OtherDocumentList.Select(e => new OtherDocument
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

                employeeEntity.OtherDocuments = docsList;
                #endregion

                var result = await _repository.AddEmployeeAsync(employeeEntity, cancellationToken);
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

        public async Task<Result<bool>> DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.DeleteEmployeeAsync(employeeId, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to delete the selected employee record due to unknown error. Please refresh the page then try to delete again.");
                }

                return Result<bool>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> SaveDepartmentAsync(DepartmentDTO department, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize DepartmentMaster entity
                DepartmentMaster departmentEntity = new DepartmentMaster()
                {
                    DepartmentId = department.DepartmentId,
                    DepartmentCode = department.DepartmentCode,
                    DepartmentName = department.DepartmentName,
                    GroupCode = department.GroupCode,
                    Description = department.Description,
                    ParentDepartmentId = department.ParentDepartmentId,
                    SuperintendentEmpNo = department.SuperintendentEmpNo,
                    ManagerEmpNo = department.ManagerEmpNo,
                    IsActive = department.IsActive,
                    UpdatedAt = DateTime.Today
                };
                #endregion

                var result = await _repository.SaveDepartmentAsync(departmentEntity, cancellationToken);
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

        public async Task<Result<bool>> DeleteDepartmentAsync(int departmentID, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.DeleteDepartmentAsync(departmentID, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to delete the selected department due to unknown error. Please refresh the page then try to delete again.");
                }

                return Result<bool>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString());
            }
        }
        #endregion
    }
}
