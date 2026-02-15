using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class EmployeeDTO
    {
        #region Properties

        #region Personal Details
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [Label("First Name")]
        [StringLength(50, ErrorMessage = "First Name length can't be more than 50 characters.")]
        public string FirstName { get; set; } = null!;

        [Display(Name = "Middle Name")]
        [StringLength(50, ErrorMessage = "Middle Name length can't be more than 50 characters.")]
        public string? MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last Name length can't be more than 50 characters.")]
        public string LastName { get; set; } = null!;

        //[Required(ErrorMessage = "Position is required")]
        [Display(Name = "Position")]
        [StringLength(100, ErrorMessage = "Position length can't be more than 100 characters.")]
        public string Position { get; set; } = null!;

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DOB { get; set; }

        public string NationalityCode { get; set; } = null!;

        [Required(ErrorMessage = "Nationality is required")]
        [Display(Name = "Nationality")]
        public string NationalityDesc { get; set; } = null!;

        public string ReligionCode { get; set; } = null!;

        [Required(ErrorMessage = "Religion is required")]
        [Display(Name = "Religion")]
        public string ReligionDesc { get; set; } = null!;

        public string GenderCode { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string GenderDesc { get; set; } = null!;

        public string MaritalStatusCode { get; set; } = null!;

        [Required(ErrorMessage = "Marital Status is required")]
        [Display(Name = "Marital Status")]
        public string MaritalStatusDesc { get; set; } = null!;

        public string? Salutation { get; set; }

        [Display(Name = "Salutation")]
        public string? SalutationDesc { get; set; }
        #endregion

        #region Contact Details
        [Required(ErrorMessage = "Office Email is required")]        
        [EmailAddress]
        [Display(Name = "Office Email")]
        [StringLength(50, ErrorMessage = "Office Email length can't be more than 50 characters.")]
        public string OfficialEmail { get; set; } = null!;

        [OptionalEmail]
        [Display(Name = "Personal Email")]
        [StringLength(50, ErrorMessage = "Personal Email length can't be more than 50 characters.")]
        public string? PersonalEmail { get; set; } = null;

        [OptionalEmail]
        [Display(Name = "Alternate Email")]
        [StringLength(50, ErrorMessage = "Alternate Email length can't be more than 50 characters.")]
        public string? AlternateEmail { get; set; } = null;

        [Display(Name = "Office Landline No.")]
        [StringLength(20, ErrorMessage = "Office Landline No. length can't be more than 20 characters.")]
        public string? OfficeLandlineNo { get; set; } = null;

        [Display(Name = "Residential Landline No.")]
        [StringLength(20, ErrorMessage = "Residential Landline No. length can't be more than 20 characters.")]
        public string? ResidenceLandlineNo { get; set; } = null;

        [Display(Name = "Office Extension")]
        [StringLength(10, ErrorMessage = "Office Extension length can't be more than 10 characters.")]
        public string? OfficeExtNo { get; set; } = null;

        [Display(Name = "Mobile No.")]
        [StringLength(20, ErrorMessage = "Mobile No. length can't be more than 20 characters.")]
        public string? MobileNo { get; set; } = null;

        [Display(Name = "Alternate Mobile No.")]
        [StringLength(20, ErrorMessage = "Alternate Mobile No. length can't be more than 20 characters.")]
        public string? AlternateMobileNo { get; set; } = null;
        #endregion

        #region Employment Details
        [Required(ErrorMessage = "Employee No. is required")]
        [Display(Name = "Employee No.")]
        public int EmployeeNo { get; set; }

        public string? EmployeeStatusCode { get; set; } = null;

        [Display(Name = "Employee Status")]
        public string? EmployeeStatusDesc { get; set; } = null;

        public int? ReportingManagerCode { get; set; } = null;

        [Required(ErrorMessage = "Reporting Manager is required")]
        [Display(Name = "Reporting Manager")]
        public string ReportingManager { get; set; } = null!;

        [Display(Name = "Work Permit ID")]
        public string? WorkPermitID { get; set; } = null;

        [Display(Name = "Work Permit Expiry Date")]
        [DataType(DataType.Date)]
        public DateTime? WorkPermitExpiryDate { get; set; } = null;

        [Display(Name = "Date of Joining")]
        [DataType(DataType.Date)]
        public DateTime? HireDate { get; set; } = null;

        [Display(Name = "Date of Confirmation")]
        [DataType(DataType.Date)]
        public DateTime? DateOfConfirmation { get; set; } = null;

        [Display(Name = "Relieving Date")]
        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; } = null;

        [Display(Name = "Date of Superannuation")]
        [DataType(DataType.Date)]
        public DateTime? DateOfSuperannuation { get; set; } = null;

        public bool? Reemployed { get; set; } = null;
        public int? OldEmployeeNo { get; set; } = null;
        public string DepartmentCode { get; set; } = null!;

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; } = null!;

        public string EmploymentTypeCode { get; set; } = null!;

        [Required(ErrorMessage = "Employment Type is required")]
        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; } = null!;

        public string RoleCode { get; set; } = null!;

        [Required(ErrorMessage = "Role Type is required")]
        [Display(Name = "Role Type")]
        public string RoleType { get; set; } = null!;

        public string FirstAttendanceModeCode { get; set; } = null!;

        [Required(ErrorMessage = "First Attendance Mode is required")]
        [Display(Name = "First Attendance Mode")]
        public string FirstAttendanceMode { get; set; } = null!;

        public string? SecondAttendanceModeCode { get; set; } = null;

        [Display(Name = "Second Attendance Mode")]
        public string? SecondAttendanceMode { get; set; } = null;

        public string? ThirdAttendanceModeCode { get; set; } = null;

        [Display(Name = "Third Attendance Mode")]
        public string? ThirdAttendanceMode { get; set; } = null!;

        public int? SecondReportingManagerCode { get; set; }

        [Display(Name = "Second Reporting Manager")]
        public string? SecondReportingManager { get; set; } = null;
        #endregion

        #region Attribute Details     
        [Required(ErrorMessage = "Company is required")]
        [Display(Name = "Company")]
        [StringLength(150, ErrorMessage = "Company length can't be more than 150 characters.")]
        public string Company { get; set; } = null!;

        [Display(Name = "Company Branch")]
        [StringLength(150, ErrorMessage = "Company Branch length can't be more than 150 characters.")]
        public string? CompanyBranch { get; set; } = null;

        public string? CompanyBranchDesc { get; set; } = null;

        public string? EducationCode { get; set; } = null;

        [Display(Name = "Education")]
        public string? EducationDesc { get; set; } = null;

        public string? EmployeeClassCode { get; set; } = null;

        [Display(Name = "Employee Class")]
        public string? EmployeeClassDesc { get; set; } = null;

        public string JobTitleCode { get; set; } = null!;

        [Required(ErrorMessage = "Job Title is required")]
        [Display(Name = "Job Title")]        
        public string JobTitleDesc { get; set; } = null!;

        public string PayGrade { get; set; } = null!;

        [Required(ErrorMessage = "Pay Grade is required")]
        [Display(Name = "Pay Grade")]
        public string PayGradeDesc { get; set; } = null!;

        [Required(ErrorMessage = "Is Active is required")]
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        #endregion

        #region Bank Details
        public string? AccountTypeCode { get; set; } = null;

        [Display(Name = "Account Type")]
        public string? AccountTypeDesc { get; set; } = null;
        
        [Display(Name = "Account Number")]
        [StringLength(30, ErrorMessage = "Account Number length can't be more than 30 characters.")]
        public string? AccountNumber { get; set; } = null;

        [Display(Name = "Account Holder Name")]
        public string? AccountHolderName { get; set; } = null;

        public string? BankNameCode { get; set; } = null;

        [Display(Name = "Bank Name")]
        public string? BankName { get; set; } = null;

        [Display(Name = "Bank Branch")]
        public string? BankBranchName { get; set; } = null;

        [Display(Name = "IBAN Number")]
        [StringLength(30, ErrorMessage = "IBAN Number length can't be more than 30 characters.")]
        public string? IBANNumber { get; set; } = null;

        [Display(Name = "Tax Number")]
        [StringLength(40, ErrorMessage = "Tax Number length can't be more than 40 characters.")]
        public string? TaxNumber { get; set; } = null;
        #endregion

        #region Social Connect 
        [Display(Name = "LinkedIn Account")]
        [StringLength(40, ErrorMessage = "LinkedIn Account length can't be more than 40 characters.")]
        public string? LinkedInAccount { get; set; } = null;

        [Display(Name = "Facebook Account")]
        [StringLength(40, ErrorMessage = "Facebook Account length can't be more than 40 characters.")]
        public string? FacebookAccount { get; set; } = null;

        [Display(Name = "Twitter Account")]
        [StringLength(40, ErrorMessage = "Twitter Account length can't be more than 40 characters.")]
        public string? TwitterAccount { get; set; } = null;

        [Display(Name = "Instagram Account")]
        [StringLength(40, ErrorMessage = "Instagram Account length can't be more than 40 characters.")]
        public string? InstagramAccount { get; set; } = null;
        #endregion

        #region Primary Location Implementation
        [Display(Name = "Present Address")]
        [StringLength(300, ErrorMessage = "Present Address length can't be more than 300 characters.")]
        public string? PresentAddress { get; set; } = null;

        public string? PresentCountryCode { get; set; } = null;
        public string? PresentCountryDesc { get; set; } = null;

        [Display(Name = "Present City")]
        [StringLength(100, ErrorMessage = "Present City length can't be more than 100 characters.")]
        public string? PresentCity { get; set; } = null;

        [Display(Name = "Present Area Code")]
        [StringLength(20, ErrorMessage = "Present Area Code length can't be more than 20 characters.")]
        public string? PresentAreaCode { get; set; } = null;

        [Display(Name = "Present Contact No.")]
        [StringLength(20, ErrorMessage = "Present Contact No. length can't be more than 20 characters.")]
        public string? PresentContactNo { get; set; } = null;

        [Display(Name = "Present Mobile No.")]
        [StringLength(20, ErrorMessage = "Present Mobile No. length can't be more than 20 characters.")]
        public string? PresentMobileNo { get; set; } = null;

        [Display(Name = "Permanent Address")]
        [StringLength(200, ErrorMessage = "Permanent Address length can't be more than 300 characters.")]
        public string? PermanentAddress { get; set; } = null;

        public string? PermanentCountryCode { get; set; } = null;
        public string? PermanentCountryDesc { get; set; } = null;

        [Display(Name = "Permanent City")]
        [StringLength(100, ErrorMessage = "Permanent City length can't be more than 100 characters.")]
        public string? PermanentCity { get; set; } = null;

        [Display(Name = "Permanent Area Code")]
        [StringLength(20, ErrorMessage = "Permanent Area Code length can't be more than 20 characters.")]
        public string? PermanentAreaCode { get; set; } = null;

        [Display(Name = "Present Contact No.")]
        [StringLength(20, ErrorMessage = "Present Contact No. length can't be more than 20 characters.")]
        public string? PermanentContactNo { get; set; } = null;

        [Display(Name = "Permanent Mobile No.")]
        [StringLength(20, ErrorMessage = "Permanent Mobile No. length can't be more than 20 characters.")]
        public string? PermanentMobileNo { get; set; } = null;
        #endregion

        #region Authentication Detail
        [Display(Name = "User ID")]
        [StringLength(40, ErrorMessage = "User ID can't be more than 40 characters.")]
        public string? UserID { get; set; } = null;

        [Display(Name = "Password Hash")]
        [StringLength(200, ErrorMessage = "Password Hash can't be more than 200 characters.")]
        public string? PasswordHash { get; set; } = null;

        public int FailedLoginAttempts { get; set; } 
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

        #endregion

        #region Reference Navigations
        public List<EmergencyContactDTO> EmergencyContactList { get; set; } = new List<EmergencyContactDTO>();
        public IdentityProofDTO EmpIdentityProof { get; set; } = new IdentityProofDTO();   
        public List<QualificationDTO> QualificationList { get; set; } = new List<QualificationDTO>();
        public List<EmployeeSkillDTO> EmployeeSkillList { get; set; } = new List<EmployeeSkillDTO>();
        public List<EmployeeCertificationDTO> EmployeeCertificationList { get; set; } = new List<EmployeeCertificationDTO>();
        public List<LanguageSkillDTO> LanguageSkillList { get; set; } = new List<LanguageSkillDTO>();
        public List<FamilyMemberDTO> FamilyMemberList { get; set; } = new List<FamilyMemberDTO>();
        public List<FamilyVisaDTO> FamilyVisaList { get; set; } = new List<FamilyVisaDTO>();
        public List<EmploymentHistoryDTO> EmploymentHistoryList { get; set; } = new List<EmploymentHistoryDTO>();
        public List<OtherDocumentDTO> OtherDocumentList { get; set; } = new List<OtherDocumentDTO>();
        public List<EmployeeTransactionDTO> EmployeeTransactionList { get; set; } = new List<EmployeeTransactionDTO>();
        #endregion

        #region Extended Properties
        public string EmployeeFullName 
        { 
            get
            {
                return $"{FirstName} {MiddleName} {LastName}";
            }
        }

        public string Tenure
        {
            get
            {
                string tenure = string.Empty;

                if (this.HireDate.HasValue)
                    tenure = CalculateTenure(this.HireDate!.Value, DateTime.Today);

                return tenure;
            }
            set { }
        }
        #endregion

        #region Private Methods
        private string CalculateTenure(DateTime fromDate, DateTime toDate)
        {
            try
            {
                if (toDate < fromDate)
                    throw new ArgumentException("End date cannot be earlier than start date.");

                int years = toDate.Year - fromDate.Year;
                int months = toDate.Month - fromDate.Month;
                int days = toDate.Day - fromDate.Day;

                if (days < 0)
                {
                    months--;
                    days += DateTime.DaysInMonth(toDate.Year, (toDate.Month == 1 ? 12 : toDate.Month - 1));
                }

                if (months < 0)
                {
                    years--;
                    months += 12;
                }

                return $"{years} years, {months} months, {days} days";
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
            
        }
        #endregion
    }
}
