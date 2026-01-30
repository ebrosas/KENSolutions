using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class AttendanceDurationDTO
    {
        #region Properties
        public int EmpNo { get; set; }
        public DateTime SwipeDate { get; set; }
        public string? TotalWorkDuration { get; set; } = null;
        #endregion
    }
}
