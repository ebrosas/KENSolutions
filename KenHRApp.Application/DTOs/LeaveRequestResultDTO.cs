using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class LeaveRequestResultDTO
    {
        #region Properties
        public long LeaveRequestId { get; set; }       // Identity column  
        public Guid LeaveAttachmentId { get; set; } = Guid.NewGuid();
        public Guid WorkflowId { get; set; } = Guid.NewGuid();
        public string? LeaveInstanceID { get; set; } = null;
        public string LeaveType { get; set; } = null!;
        public int LeaveEmpNo { get; set; }
        public string? LeaveEmpName { get; set; } = null;
        public string? LeaveEmpEmail { get; set; } = null;
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public DateTime LeaveResumeDate { get; set; }
        public string? LeaveEmpCostCenter { get; set; } = null;
        public string? LeaveRemarks { get; set; } = null;
        public bool? LeaveConstraints { get; set; } = null;
        public string LeaveStatusCode { get; set; } = null!;
        public char? LeaveApprovalFlag { get; set; }
        public bool? LeaveVisaRequired { get; set; } = null;
        public bool? LeavePayAdv { get; set; } = null;
        public bool? LeaveIsFTMember { get; set; } = null;
        public double? LeaveBalance { get; set; }
        public double? LeaveDuration { get; set; }
        public int? NoOfHolidays { get; set; }
        public int? NoOfWeekends { get; set; }
        public char? PlannedLeave { get; set; }
        public int? LeavePlannedNo { get; set; }
        public char? HalfDayLeaveFlag { get; set; }
        public DateTime? LeaveCreatedDate { get; set; }
        public int? LeaveCreatedBy { get; set; }
        public string? LeaveCreatedUserID { get; set; } = null;
        public string? LeaveCreatedEmail { get; set; } = null;
        public DateTime? LeaveUpdatedDate { get; set; }
        public int? LeaveUpdatedBy { get; set; }
        public string? LeaveUpdatedUserID { get; set; } = null;
        public string? LeaveUpdatedEmail { get; set; } = null;
        public int? LeaveStatusID { get; set; }
        public string? StatusHandlingCode { get; set; } = null;
        public string? StartDayMode { get; set; } = null;
        public string? EndDayMode { get; set; } = null;
        public string? StatusDesc { get; set; } = null;
        public string? ApprovalFlagDesc { get; set; } = null;
        public string? CreatedByName { get; set; } = null;
        public List<LeaveAttachment> AttachmentList { get; set; } = new();
        #endregion
    }
}
