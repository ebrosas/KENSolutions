using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskDataServices.Services
{
    public interface IAttendanceService
    {
        Task ExecuteAttendanceGenerationAsync(DateTime attendanceDate);
    }
}
