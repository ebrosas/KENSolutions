using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class PlannedLeaveResult
    {
        #region Properties
        public long PlannedLeaveId { get; set; }       // Identity column  
        public long? LeaveNo { get; set; }
        public int EmpNo { get; set; }
        public string? EmpName { get; set; } = null;
        public DateTime LeaveStartDate { get; set; }
        public DateTime LeaveEndDate { get; set; }
        public DateTime LeaveResumeDate { get; set; }
        public string? StartDayMode { get; set; } = null;
        public string? StartDayModeDesc { get; set; } = null;
        public string? EndDayMode { get; set; } = null;
        public string? EndDayModeDesc { get; set; } = null;
        public string? CostCenter { get; set; } = null;
        public string? CostCenterName { get; set; } = null;
        public string? Remarks { get; set; } = null;
        public double? LeaveDuration { get; set; }
        public int? NoOfHolidays { get; set; }
        public int? NoOfWeekends { get; set; }
        public char? HalfDayLeaveFlag { get; set; }
        public int? StatusID { get; set; }
        public string StatusCode { get; set; } = null!;
        public string? StatusDesc { get; set; } = null;
        public string? StatusHandlingCode { get; set; } = null;
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string? CreatedByName { get; set; } = null;
        public string? CreatedUserID { get; set; } = null;
        public string? CreatedEmail { get; set; } = null;
        public DateTime? LastUpdatedDate { get; set; }
        public int? LastUpdatedBy { get; set; }
        public string? LastUpdatedName { get; set; } = null;
        public string? LastUpdatedUserID { get; set; } = null;
        public string? LastUpdatedEmail { get; set; } = null;
        #endregion
    }
}
