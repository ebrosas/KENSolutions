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
        [StringLength(150, ErrorMessage = "Company length can't be more than 150 characters.")]
        public string Company { get; set; } = null!;

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

        [Required(ErrorMessage = "Relevant Experience is required")]
        [Display(Name = "Relevant Experience")]
        public int TotalRelevantExperience { get; set; }

        [Required(ErrorMessage = "Min. Age is required")]
        [Display(Name = "Min. Age")]
        public int MinAge { get; set; }

        [Required(ErrorMessage = "Max. Age is required")]
        [Display(Name = "Max. Age")]
        public int MaxAge { get; set; }

        [Required(ErrorMessage = "Age Range is required")]
        [Display(Name = "Age Range (years)")]
        public int AgeRange { get; set; }

        public string GenderCode { get; set; } = null!;

        [Required(ErrorMessage = "Gender is required")]
        [Display(Name = "Gender")]
        public string Gender { get; set; } = null!;

        public string? VideoDescriptionURL { get; set; } = null;
        #endregion

        #region Reference Navigations
        public List<RecruitmentLanguageSkillDTO> LanguageSkillList { get; set; } = new List<RecruitmentLanguageSkillDTO>();
        public List<JobQualificationDTO> QualificationList { get; set; } = new List<JobQualificationDTO>();
        #endregion
    }
}
