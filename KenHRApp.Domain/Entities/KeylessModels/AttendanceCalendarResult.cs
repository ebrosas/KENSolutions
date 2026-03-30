using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Domain.Entities.KeylessModels
{
    public class AttendanceCalendarResult
    {
        #region Properties
        public int EmpNo { get; set; }
        public DateTime AttendanceDate { get; set; }
        public string? LegendCode { get; set; } = null;
        public string? LegendDesc { get; set; } = null;
        #endregion
    }
}
