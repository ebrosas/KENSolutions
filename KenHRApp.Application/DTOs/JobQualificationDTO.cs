using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class JobQualificationDTO
    {
        #region Properties
        public int AutoId { get; set; }
        public string QualificationCode { get; set; } = null!;
        public string Qualification{ get; set; } = null!;
        public string StreamCode { get; set; } = null!;
        public string Stream{ get; set; } = null!;
        public string? SpecializationCode { get; set; } = null;
        public string? Specialization{ get; set; } = null;

        [Display(Name = "Remarks")]
        [StringLength(500, ErrorMessage = "Remarks length can't be more than 500 characters.")]
        public string? Remarks { get; set; } = null;
        #endregion

        #region Reference Navigation to Recruitment Request
        public int RequisitionId { get; set; }
        public RecruitmentRequestDTO RecruitmentRequest { get; set; } = null!;
        #endregion
    }
}
