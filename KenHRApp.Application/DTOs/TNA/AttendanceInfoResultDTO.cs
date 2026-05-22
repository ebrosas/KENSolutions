using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs.TNA
{
    public class AttendanceInfoResultDTO
    {
        #region Properties
        public DateTime AttendanceDate { get; set; }
        public int EmployeeNo { get; set; }
        public string EmployeeName { get; set; } = null!;
        public string ShiftRoster { get; set; } = null!;
        public string ShiftRosterDesc { get; set; } = null!;
        public string ShiftTiming { get; set; } = null!;
        public decimal TotalDeficitHour { get; set; }
        public decimal TotalWorkHour { get; set; }
        public int TotalWorkMinute { get; set; }
        public List<AttendanceSwipeDTO>? SwipeLogList { get; set; }
        #endregion

        #region Extended Properties
        public string ShiftDetails 
        { 
            get
            {
                if (!string.IsNullOrWhiteSpace(ShiftRoster) &&
                    !string.IsNullOrWhiteSpace(ShiftTiming))
                {
                    return $"{ShiftRoster} - {ShiftTiming}";
                }
                else
                    return "-Not defined-";
            }
            set { } 
        }
        #endregion
    }
}
