using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskDataServices.Configurations
{
    public class AttendanceSettings
    {
        #region Properties
        public int ExecutionDateOffset { get; set; }
        public string? AttendanceDate { get; set; }
        #endregion
    }
}
