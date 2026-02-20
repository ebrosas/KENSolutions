using KenHRApp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.SymbolStore;

namespace KenHRApp.Domain.Entities
{
    public class Employee : IContactDetail, IEmploymentDetail, IAttributeDetail, IBankDetail, ISocialConnect, IPrimaryLocation, IAuthenticationDetail
    {
        #region Personal Detail         
        [Comment("Primary key for Employee entity")]
        public int EmployeeId { get; set; }
        
        [Column(TypeName = "varchar(50)")]
        public string FirstName { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string? MiddleName { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string LastName { get; set; } = null!;

        [Column(TypeName = "varchar(100)")]
        public string Position { get; set; } = null!;

        [Column(TypeName = "datetime")]
        public DateTime? DOB { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string NationalityCode { get; set; } = null!;

        [NotMapped]
        public string NationalityDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string ReligionCode { get; set; } = null!;

        [NotMapped]
        public string ReligionDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string GenderCode { get; set; } = null!;

        [NotMapped]
        public string GenderDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string MaritalStatusCode { get; set; } = null!;

        [NotMapped]
        public string MaritalStatusDesc { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string? Salutation { get; set; }

        [NotMapped]
        public string? SalutationDesc { get; set; }
        #endregion

        #region Contact Detail Implementation
        [Column(TypeName = "varchar(50)")]
        public string OfficialEmail { get; set; } = null!;

        [Column(TypeName = "varchar(50)")]
        public string? PersonalEmail { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? AlternateEmail { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? OfficeLandlineNo { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? ResidenceLandlineNo { get; set; } = null;

        [Column(TypeName = "varchar(10)")]
        public string? OfficeExtNo { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? MobileNo { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? AlternateMobileNo { get; set; } = null;
        #endregion

        #region Employment Detail Implementation
        [Comment("Alternate key used for reference navigation")]
        public int EmployeeNo { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? EmployeeStatusCode { get; set; } = null;

        [NotMapped]
        public string? EmployeeStatusDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public int? ReportingManagerCode { get; set; } = null;

        [NotMapped]
        public string ReportingManager { get; set; } = null!;

        [Column(TypeName = "varchar(30)")]
        public string? WorkPermitID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? WorkPermitExpiryDate { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime HireDate { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? DateOfConfirmation { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? TerminationDate { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? DateOfSuperannuation { get; set; } = null;

        [Column(TypeName = "bit")]
        public bool? Reemployed { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public int? OldEmployeeNo { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string DepartmentCode { get; set; } = null!;

        [NotMapped]
        public string DepartmentName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string EmploymentTypeCode { get; set; } = null!;

        [NotMapped]
        public string EmploymentType { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string RoleCode { get; set; } = null!;

        [NotMapped]
        public string RoleType { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string FirstAttendanceModeCode { get; set; } = null!;

        [NotMapped]
        public string FirstAttendanceMode { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? SecondAttendanceModeCode { get; set; } = null;

        [NotMapped]
        public string? SecondAttendanceMode { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? ThirdAttendanceModeCode { get; set; } = null;

        [NotMapped]
        public string? ThirdAttendanceMode { get; set; } = null;

        public int? SecondReportingManagerCode { get; set; }

        [NotMapped]
        public string? SecondReportingManager { get; set; } = null;
        #endregion

        #region Attribute Detail Implementation
        [Column(TypeName = "varchar(150)")]
        public string Company { get; set; } = null!;

        [Column(TypeName = "varchar(150)")]
        public string? CompanyBranch { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? EducationCode { get; set; } = null;

        [NotMapped]
        public string? EducationDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? EmployeeClassCode { get; set; } = null;

        [NotMapped]
        public string? EmployeeClassDesc { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string JobTitleCode { get; set; } = null!;

        [NotMapped]
        public string JobTitleDesc { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string PayGrade { get; set; } = null!;

        [NotMapped]
        public string PayGradeDesc { get; set; } = null!;

        public bool IsActive { get; set; }
        #endregion

        #region Bank Detail Implementation  
        [Column(TypeName = "varchar(20)")]
        public string? AccountTypeCode { get; set; } = null;

        [NotMapped]
        public string? AccountTypeDesc { get; set; } = null;

        [Column(TypeName = "varchar(30)")]
        public string? AccountNumber { get; set; } = null;

        [Column(TypeName = "varchar(50)")]
        public string? AccountHolderName { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? BankNameCode { get; set; } = null;

        [NotMapped]
        public string? BankName { get; set; } = null;

        [Column(TypeName = "varchar(150)")]
        public string? BankBranchName { get; set; } = null;

        [Column(TypeName = "varchar(30)")]
        public string? IBANNumber { get; set; } = null;

        [Column(TypeName = "varchar(40)")]
        public string? TaxNumber { get; set; } = null;
        #endregion

        #region Social Connect Implementation
        [Column(TypeName = "varchar(40)")]
        public string? LinkedInAccount { get; set; } = null;

        [Column(TypeName = "varchar(40)")]
        public string? FacebookAccount { get; set; } = null;

        [Column(TypeName = "varchar(40)")]
        public string? TwitterAccount { get; set; } = null;

        [Column(TypeName = "varchar(40)")]
        public string? InstagramAccount { get; set; } = null;
        #endregion

        #region Primary Location Implementation
        [Column(TypeName = "varchar(300)")]
        public string? PresentAddress { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PresentCountryCode { get; set; } = null;

        [NotMapped]
        public string? PresentCountryDesc { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? PresentCity { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PresentAreaCode { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PresentContactNo { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PresentMobileNo { get; set; } = null;

        [Column(TypeName = "varchar(300)")]
        public string? PermanentAddress { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PermanentCountryCode { get; set; } = null;

        [NotMapped]
        public string? PermanentCountryDesc { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? PermanentCity { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PermanentAreaCode { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PermanentContactNo { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? PermanentMobileNo { get; set; } = null;
        #endregion

        #region Authentication Detail Implementation
        [Column(TypeName = "varchar(40)")]
        public string? UserID { get; set; } = null;

        [Column(TypeName = "varchar(200)")]
        public string? PasswordHash { get; set; } = null;

        public int FailedLoginAttempts { get; set; } 

        [Column(TypeName = "bit")]
        public bool IsLocked { get; set; }

        public void IncrementFailedAttempts()
        {
            FailedLoginAttempts++;

            if (FailedLoginAttempts >= 3)
                IsLocked = true;
        }

        public void ResetFailedAttempts()
        {
            FailedLoginAttempts = 0;
        }

        public void UnlockAccount()
        {
            FailedLoginAttempts = 0;
            IsLocked = false;
        }

        public void ChangePassword(string newHash)
        {
            PasswordHash = newHash;
        }
        #endregion

        #region Extended Properties
        [NotMapped]
        public string EmployeeFullName
        {
            get
            {
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }
        #endregion

        #region Navigation Properties
        public ICollection<EmergencyContact> EmergencyContactList { get; set; } = new List<EmergencyContact>();
        public IdentityProof? IdentityProof { get; set; } = null;
        public ICollection<Qualification> Qualifications { get; set; } = new List<Qualification>();
        public ICollection<EmployeeSkill> EmployeeSkills { get; set; } = new List<EmployeeSkill>();
        public ICollection<EmployeeCertification> EmployeeCertifications { get; set; } = new List<EmployeeCertification>();
        public ICollection<LanguageSkill> LanguageSkills { get; set; } = new List<LanguageSkill>();
        public ICollection<FamilyMember> FamilyMembers { get; set; } = new List<FamilyMember>();
        public ICollection<FamilyVisa> FamilyVisas { get; set; } = new List<FamilyVisa>();
        public ICollection<EmploymentHistory> EmploymentHistories { get; set; } = new List<EmploymentHistory>();
        public ICollection<OtherDocument> OtherDocuments { get; set; } = new List<OtherDocument>();
        public ICollection<EmployeeTransaction> EmployeeTransactions { get; set; } = new List<EmployeeTransaction>();
        public LeaveEntitlement? LeaveEntitlement { get; set; } = null;
        #endregion
    }
}
