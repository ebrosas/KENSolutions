using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
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
        public string? StreamCode { get; set; } = null;
        public string? Stream{ get; set; } = null;
        public string? SpecializationCode { get; set; } = null;
        public string? Specialization{ get; set; } = null;
        #endregion

        #region Reference Navigation to Recruitment Request
        public int RequisitionId { get; set; }
        public RecruitmentRequestDTO RecruitmentRequest { get; set; } = null!;
        #endregion
    }
}
