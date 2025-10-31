using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class RecruitmentRequestDTO
    {
        #region Position Properties
        public int RequisitionId { get; set; }

        public string EmploymentTypeCode { get; set; } = null!;

        [Required(ErrorMessage = "Employment Type is required")]
        [Display(Name = "Employment Type")]
        public string EmploymentType { get; set; } = null!;

        public string QualificationModeCode { get; set; } = null!;

        [Required(ErrorMessage = "Qualification Mode is required")]
        [Display(Name = "Qualification Mode")]
        public string QualificationMode { get; set; } = null!;

        public string PositionTypeCode { get; set; } = null!;

        [Required(ErrorMessage = "Position Type is required")]
        [Display(Name = "Position Type")]
        public string PositionType { get; set; } = null!;

        public string InterviewProcessCode { get; set; } = null!;

        [Required(ErrorMessage = "Interview Process is required")]
        [Display(Name = "Interview Process")]
        public string InterviewProcess { get; set; } = null!;

        public bool? IsPreAssessment { get; set; } = null;
        #endregion

        #region Attributes for Planning Properties
        [Required(ErrorMessage = "Company is required")]
        [Display(Name = "Company")]        
        public string Company { get; set; } = null!;

        public string CompanyCode { get; set; } = null!;

        public string DepartmentCode { get; set; } = null!;

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public string DepartmentName { get; set; } = null!;

        public string? CountryCode { get; set; } = null;

        [Display(Name = "Country")]
        public string? Country { get; set; } = null;

        public string? EducationCode { get; set; } = null;

        [Display(Name = "Education")]
        public string? Education { get; set; } = null;

        public string EmployeeClassCode { get; set; } = null!;

        [Display(Name = "Employee Class")]
        [Required(ErrorMessage = "Employee Class is required")]
        public string EmployeeClass { get; set; } = null!;

        public string? EthnicityCode { get; set; } = null;

        [Display(Name = "Education")]
        public string? Ethnicity { get; set; } = null;

        public string JobTitleCode { get; set; } = null!;

        [Display(Name = "Job Title")]
        [Required(ErrorMessage = "Job Title is required")]
        public string JobTitle { get; set; } = null!;

        public string PayGradeCode { get; set; } = null!;

        [Required(ErrorMessage = "Pay Grade is required")]
        [Display(Name = "Pay Grade")]
        public string PayGradeDesc { get; set; } = null!;
        #endregion

        #region Position Description Properties
        [Required(ErrorMessage = "Position Description is required")]
        [Display(Name = "Position Description")]
        [StringLength(1000, ErrorMessage = "Position Description length can't be more than 1000 characters.")]
        public string PositionDescription { get; set; } = null!;

        [Required(ErrorMessage = "Total Work Experience is required")]
        [Display(Name = "Total Work Experience")]
        public int TotalWorkExperience { get; set; }
        public int MinWorkExperience { get; set; }
        public int MaxWorkExperience { get; set; }

        [Required(ErrorMessage = "Relevant Experience is required")]
        [Display(Name = "Relevant Experience")]
        public int TotalRelevantExperience { get; set; }
        public int MinRelevantExperience { get; set; }
        public int MaxRelevantExperience { get; set; }

        [Required(ErrorMessage = "Age Range is required")]
        [Display(Name = "Age Range (years)")]
        public int AgeRange { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }

        public IReadOnlyCollection<string>? GenderList { get; set; } = null;
        public string? RequiredGender { get; set; } = null;

        public IReadOnlyCollection<string>? AssetList { get; set; } = null;
        public string? RequiredAsset { get; set; } = null;

        public string? VideoDescriptionURL { get; set; } = null;
        #endregion

        #region Compensation and Benefits
        public string? SalaryRangeType { get; set; } = null;

        [Display(Name = "Yearly Salary Range")]
        public int YearlySalaryRange { get; set; }
        public int YearlySalaryRangeMin { get; set; } = 0;
        public int YearlySalaryRangeMax { get; set; } = 100000;
        public string? YearlySalaryRangeCurrency { get; set; } = "BHD";

        [Display(Name = "Monthly Salary Range")]
        public int MonthlySalaryRange { get; set; }
        public int MonthlySalaryRangeMin { get; set; } = 0;
        public int MonthlySalaryRangeMax { get; set; } = 10000;
        public string? MonthlySalaryRangeCurrency { get; set; } = "BHD";

        [Display(Name = "Daily Salary Range")]
        public int DailySalaryRange { get; set; }
        public int DailySalaryRangeMin { get; set; } = 0;
        public int DailySalaryRangeMax { get; set; } = 1000;
        public string? DailySalaryRangeCurrency { get; set; } = "BHD";

        [Display(Name = "Hourly Salary Range")]
        public int HourlySalaryRange { get; set; }
        public int HourlySalaryRangeMin { get; set; } = 0;
        public int HourlySalaryRangeMax { get; set; } = 100;
        public string? HourlySalaryRangeCurrency { get; set; } = "BHD";


        [Required(ErrorMessage = "Duties and Responsibilities is required")]
        [Display(Name = "Duties and Responsibilities")]
        [StringLength(5000, ErrorMessage = "Duties and Responsibilities length can't be more than 5000 characters.")]
        public string Responsibilities { get; set; } = null!;

        [Required(ErrorMessage = "Competencies is required")]
        [Display(Name = "Competencies")]
        [StringLength(5000, ErrorMessage = "Competencies length can't be more than 5000 characters.")]
        public string Competencies { get; set; } = null!;

        [Display(Name = "General Remarks")]
        [StringLength(5000, ErrorMessage = "General Remarks length can't be more than 5000 characters.")]
        public string? GeneralRemarks { get; set; } = null;
        #endregion

        #region Reference Navigations
        public List<RecruitmentLanguageSkillDTO> LanguageSkillList { get; set; } = new List<RecruitmentLanguageSkillDTO>();
        public List<JobQualificationDTO> QualificationList { get; set; } = new List<JobQualificationDTO>();
        #endregion
    }
}
