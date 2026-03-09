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
    public class LeaveRequisitionDTO
    {
        #region Properties
        public long LeaveRequestId { get; set; }       // Identity column  
                
        public string? LeaveInstanceID { get; set; } = null;

        [Required(ErrorMessage = "Leave Type is required")]
        [Label("Leave Type")]
        [StringLength(20, ErrorMessage = "Leave Type can't be more than 20 characters.")]
        public string LeaveType { get; set; } = null!;

        public int LeaveEmpNo { get; set; }

        [Label("Employee Name")]
        [StringLength(150, ErrorMessage = "Employee Name can't be more than 150 characters.")]
        public string? LeaveEmpName { get; set; } = null;

        [Label("Email")]
        [StringLength(50, ErrorMessage = "Email can't be more than 50 characters.")]
        public string? LeaveEmpEmail { get; set; } = null;

        [Display(Name = "Leave Start Date")]
        [DataType(DataType.Date)]
        public DateTime? LeaveStartDate { get; set; }

        [Display(Name = "Leave End Date")]
        [DataType(DataType.Date)]
        public DateTime? LeaveEndDate { get; set; }

        [Display(Name = "Leave Resume Date")]
        [DataType(DataType.Date)]
        public DateTime? LeaveResumeDate { get; set; }

        [Label("Cost Center")]
        [StringLength(20, ErrorMessage = "Cost Center can't be more than 20 characters.")]
        public string? LeaveEmpCostCenter { get; set; } = null;

        [Label("Remarks")]
        [StringLength(500, ErrorMessage = "Leave Remarks can't be more than 500 characters.")]
        public string? LeaveRemarks { get; set; } = null;

        [Display(Name = "Leave Constraints")]
        public bool? LeaveConstraints { get; set; } = null;

        [Required(ErrorMessage = "Leave Status Code is required")]
        [Label("Leave Status Code")]
        [StringLength(20, ErrorMessage = "Leave Status Code can't be more than 20 characters.")]
        public string LeaveStatusCode { get; set; } = null!;

        [Label("Approval Flag")]
        public char? LeaveApprovalFlag { get; set; }

        [Display(Name = "Visa Required?")]
        public bool? LeaveVisaRequired { get; set; } = null;

        [Display(Name = "Advance Payment Required?")]
        public bool? LeavePayAdv { get; set; } = null;

        [Display(Name = "Is Fire Team?")]
        public bool? LeaveIsFTMember { get; set; } = null;

        [Display(Name = "Leave Balance")]
        public double? LeaveBalance { get; set; }

        [Display(Name = "Leave Duration")]
        public double? LeaveDuration { get; set; }

        [Display(Name = "No. of Holidays")]
        public int? NoOfHolidays { get; set; }

        [Display(Name = "No. of Weekends")]
        public int? NoOfWeekends { get; set; }

        [Display(Name = "Planned Leave")]
        public char? PlannedLeave { get; set; }

        public int? LeavePlannedNo { get; set; }

        [Display(Name = "Half Day Flag")]
        public char? HalfDayLeaveFlag { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.Date)]
        public DateTime? LeaveCreatedDate { get; set; }

        public int? LeaveCreatedBy { get; set; }

        public string? LeaveCreatedUserID { get; set; } = null;

        public string? LeaveCreatedEmail { get; set; } = null;

        [Display(Name = "Last Updated Date")]
        [DataType(DataType.Date)]
        public DateTime? LeaveUpdatedDate { get; set; }

        public int? LeaveUpdatedBy { get; set; }

        public string? LeaveUpdatedUserID { get; set; } = null;

        public string? LeaveUpdatedEmail { get; set; } = null;

        public Guid WorkflowId { get; private set; } = Guid.NewGuid();

        public byte? StartDayMode { get; set; }
        public byte? EndDayMode { get; set; }

        public List<FileUploadDTO> Files { get; set; } = new();
        #endregion
    }
}
