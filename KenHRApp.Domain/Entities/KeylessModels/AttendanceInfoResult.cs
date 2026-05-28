using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class AttendanceInfoResult
    {
        #region Properties
        public DateTime AttendanceDate { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string ShiftRoster { get; set; } = null!;
        public string ShiftRosterDesc { get; set; } = null!;
        public string ShiftTiming { get; set; } = null!;
        public decimal TotalDeficitHour { get; set; }
        public int TotalDeficitMinute { get; set; }
        public decimal TotalWorkHour { get; set; }
        public int TotalWorkMinute { get; set; }
        public string? RemarkCode { get; set; } = null;
        public List<AttendanceSwipeLog>? SwipeLogList { get; set; }
        #endregion
    }
}
