using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class AttendanceDetailDTO
    {
        #region Properties
        public int EmployeeNo { get; set; }
        public DateTime AttendanceDate { get; set; }
        public DateTime? FirstTimeIn { get; set; }
        public DateTime? LastTimeOut { get; set; }
        public string? WorkDurationDesc { get; set; } = null;
        public string? DeficitHoursDesc { get; set; } = null;
        public string? RegularizedType { get; set; } = null;
        public string? RegularizedReason { get; set; } = null;
        public string? LeaveStatus { get; set; } = null;
        public string? LeaveDetails { get; set; } = null;
        public string? RawSwipes { get; set; } = null;
        public string? SwipeType { get; set; } = null;
        public List<AttendanceSwipeDTO>? SwipeLogList { get; set; }
        #endregion

        #region Extended Properties
        public string ActualTiming 
        { 
            get
            {
                if (FirstTimeIn.HasValue && LastTimeOut.HasValue)
                    return $"{FirstTimeIn:hh:mm tt} - {LastTimeOut:hh:mm tt}";
                else
                    return "-";
            }
        }
        #endregion
    }
}
