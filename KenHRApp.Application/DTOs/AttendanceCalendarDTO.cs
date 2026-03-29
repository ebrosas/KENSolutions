using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class AttendanceCalendarDTO
    {
        #region Properties
        public int EmpNo { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string LegendCode { get; set; } = "";
        #endregion
    }
}
