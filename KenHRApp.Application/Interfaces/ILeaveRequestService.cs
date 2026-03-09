using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KenHRApp.Application.Services.LeaveRequestService;

namespace KenHRApp.Application.Interfaces
{
    public interface ILeaveRequestService
    {
        #region Abstract Methods
        Task<Result<decimal>> CalculateAsync(
            int employeeId,
            DateTime start,
            DateTime end,
            LeaveDayMode startMode,
            LeaveDayMode endMode);
        #endregion
    }
}
