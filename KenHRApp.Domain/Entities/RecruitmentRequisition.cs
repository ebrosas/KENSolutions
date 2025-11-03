using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class RecruitmentRequisition
    {
        #region Position Properties
        public int RequisitionId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string EmploymentTypeCode { get; set; } = null!;

        [NotMapped]
        public string EmploymentType { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string QualificationModeCode { get; set; } = null!;

        [NotMapped]
        public string QualificationMode { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string PositionTypeCode { get; set; } = null!;

        [NotMapped]
        public string PositionType { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string InterviewProcessCode { get; set; } = null!;

        [NotMapped]
        public string InterviewProcess { get; set; } = null!;

        public bool? IsPreAssessment { get; set; }
        #endregion

        #region Attributes for Planning Properties
        [Column(TypeName = "varchar(20)")]
        public string CompanyCode { get; set; } = null!;

        [NotMapped]
        public string Company { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string DepartmentCode { get; set; } = null!;

        [NotMapped]
        public string DepartmentName { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? CountryCode { get; set; } = null;

        [NotMapped]
        [Display(Name = "Country")]
        public string? Country { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string? EducationCode { get; set; } = null;

        [NotMapped]
        public string? Education { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string EmployeeClassCode { get; set; } = null!;

        [NotMapped]
        public string EmployeeClass { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? EthnicityCode { get; set; } = null;

        [NotMapped]
        public string? Ethnicity { get; set; } = null;

        [Column(TypeName = "varchar(20)")]
        public string JobTitleCode { get; set; } = null!;

        [NotMapped]
        public string JobTitle { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string PayGradeCode { get; set; } = null!;

        [NotMapped]
        public string PayGradeDesc { get; set; } = null!;
        #endregion

        #region Position Description Properties        
        [Column(TypeName = "varchar(1000)")]
        public string PositionDescription { get; set; } = null!;
                
        public int TotalWorkExperience { get; set; }
        public int? MinWorkExperience { get; set; }
        public int? MaxWorkExperience { get; set; }
        public int TotalRelevantExperience { get; set; }
        public int? MinRelevantExperience { get; set; }
        public int? MaxRelevantExperience { get; set; }
        public int AgeRange { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }

        public string? RequiredGender { get; set; } = null;       
        public string? RequiredAsset { get; set; } = null;
        public string? VideoDescriptionURL { get; set; } = null;
        #endregion

        #region Compensation and Benefits
        public string? SalaryRangeType { get; set; } = null;

        public int? YearlySalaryRange { get; set; }
        public int? YearlySalaryRangeMin { get; set; }
        public int? YearlySalaryRangeMax { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? YearlySalaryRangeCurrency { get; set; } = null;

        public int? MonthlySalaryRange { get; set; }
        public int? MonthlySalaryRangeMin { get; set; }
        public int? MonthlySalaryRangeMax { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? MonthlySalaryRangeCurrency { get; set; } = null;
        
        public int? DailySalaryRange { get; set; }
        public int? DailySalaryRangeMin { get; set; }
        public int? DailySalaryRangeMax { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? DailySalaryRangeCurrency { get; set; } = null;

        public int? HourlySalaryRange { get; set; }
        public int? HourlySalaryRangeMin { get; set; }
        public int? HourlySalaryRangeMax { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string? HourlySalaryRangeCurrency { get; set; } = null;

        [Column(TypeName = "varchar(5000)")]
        public string Responsibilities { get; set; } = null!;

        [Column(TypeName = "varchar(5000)")]
        public string Competencies { get; set; } = null!;

        [Column(TypeName = "varchar(5000)")]
        public string? GeneralRemarks { get; set; } = null;
        #endregion

        #region General Properties
        public int? CreatedByNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedByUserID { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? CreatedByName { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; } = null;

        public int? LastUpdatedByNo { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedUserID { get; set; } = null;

        [Column(TypeName = "varchar(100)")]
        public string? LastUpdatedName { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdateDate { get; set; } = null;
        #endregion

        #region Reference Navigations
        public ICollection<JobQualification> QualificationList { get; set; } = new List<JobQualification>();
        #endregion
    }
}
