using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class TimecardResult
    {
        #region Properties
        public long AutoId { get; set; }       // Identity column  
        public string CostCenter { get; set; } = null!;
        public string? CostCenterName { get; set; } = null;
        public int EmpNo { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string? Position { get; set; } = null;
        public DateTime? AttendanceDate { get; set; } = null;
        public string? DOW { get; set; } = null;
        public string? ShiftPatCode { get; set; } = null;
        public string? SchedShiftCode { get; set; } = null;
        public string? ShiftDescription { get; set; } = null;
        public string? ShiftTiming { get; set; }
        public DateTime? FirstTimeIn { get; set; } = null;
        public DateTime? LastTimeOut { get; set; } = null;
        public int? DurationRequired { get; set; }
        public int? WorkDuration { get; set; }
        public int? NoPayHours { get; set; }
        public string? RemarkCode { get; set; } = null;
        public string? LeaveType { get; set; } = null;
        public string? AbsenceReasonCode { get; set; } = null;
        public string? ROADesc { get; set; } = null;
        public string? AttendanceStatus { get; set; } = null;
        public string? AttendanceRemarks { get; set; } = null;
        public int? OTDuration { get; set; }
        public string? OTStatus { get; set; } = null;
        #endregion
    }
}
