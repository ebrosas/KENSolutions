using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class JobQualification
    {
        #region Properties
        public int AutoId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string QualificationCode { get; set; } = null!;

        [NotMapped]
        public string Qualification { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string StreamCode { get; set; } = null!;

        [NotMapped]
        public string Stream { get; set; } = null!;

        [Column(TypeName = "varchar(20)")]
        public string? SpecializationCode { get; set; } = null;

        [NotMapped]
        public string? Specialization { get; set; } = null;

        [Column(TypeName = "varchar(500)")]
        public string? Remarks { get; set; } = null;
        #endregion

        #region Reference Navigation to Recruitment Request
        [Comment("Foreign key that references primary key: RecruitmentRequest.RequisitionId")]
        public int RequisitionId { get; set; }
        public RecruitmentRequisition RecruitmentRequest { get; set; } = null!;
        #endregion
    }
}
