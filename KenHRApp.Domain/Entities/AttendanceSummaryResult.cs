using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities
{
    public class AttendanceSummaryResult
    {
        #region Properties
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string? ShiftRoster { get; set; } = null;
        public string? ShiftRosterDesc { get; set; } = null;
        public string? ShiftTiming { get; set; } = null;
        public int TotalAbsent { get; set; }
        public int TotalHalfDay { get; set; }
        public decimal TotalLeave { get; set; }
        public int TotalLate { get; set; }
        public int TotalEarlyOut { get; set; }
        public decimal TotalDeficitHour { get; set; }
        public decimal TotalWorkHour { get; set; }
        public int TotalDaysWorked { get; set; }
        public decimal AverageWorkHour { get; set; }
        public decimal TotalLeaveBalance { get; set; }
        public decimal TotalSLBalance { get; set; }
        public decimal TotalDILBalance { get; set; }
        #endregion
    }
}
