using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class RequestApproval
    {
        #region Properties
        public long ApprovalId { get; set; }

        [Column(TypeName = "varchar(20)")]
        public string RequestTypeCode { get; set; } = null!;

        public long RequisitionNo { get; set; }

        public int? RoutineSequence { get; set; }

        [Column(TypeName = "bit")]
        public bool? IsApproved{ get; set; }

        [Column(TypeName = "bit")]
        public bool? IsHold{ get; set; }

        [Column(TypeName = "varchar(500)")]
        public string? Remarks { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUserID { get; set; } = null;

        [Column(TypeName = "datetime")]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedUserID { get; set; } = null;
        #endregion
    }
}
