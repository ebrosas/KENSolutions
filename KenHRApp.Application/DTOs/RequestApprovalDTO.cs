using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class RequestApprovalDTO
    {
        #region Properties
        public long ApprovalId { get; set; }
                
        public string RequestTypeCode { get; set; } = null!;

        [Required(ErrorMessage = "Request Type is required")]
        [Display(Name = "Request Type")]
        public string RequestTypeDesc { get; set; } = null!;

        [Required(ErrorMessage = "Requisition No. is required")]
        [Display(Name = "Requisition No.")]
        public long RequisitionNo { get; set; }

        public int? RoutineSequence { get; set; }

        [Required(ErrorMessage = "Assignee No. is required")]
        [Display(Name = "Assignee No.")]
        public int AssignedEmpNo { get; set; }

        [Display(Name = "Assignee Name")]
        public string? AssignedEmpName { get; set; } = null;

        [Display(Name = "Approval Role")]
        public string? ApprovalRole { get; set; } = null;

        [Display(Name = "Action Role")]
        public int ActionRole { get; set; }

        [Display(Name = "Approved?")]
        public bool? IsApproved { get; set; }

        [Display(Name = "On-hold?")]
        public bool? IsHold { get; set; }

        [Display(Name = "Remarks")]
        [StringLength(150, ErrorMessage = "Remarks can't be more than 500 characters.")]
        public string? Remarks { get; set; } = null;

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? CreatedUserID { get; set; } = null;

        [Display(Name = "Last Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedBy { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? LastUpdatedUserID { get; set; } = null;
        #endregion
    }
}
