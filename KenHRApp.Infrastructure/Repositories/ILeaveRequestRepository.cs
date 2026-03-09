using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public interface ILeaveRequestRepository
    {
        #region Properties
        Task<bool> IsPublicHolidayAsync(DateTime date);
        Task<bool> IsDayOffAsync(int employeeId, DateTime date);
        #endregion

        #region Abstract methods
        Task<Result<int>> AddLeaveRequestAsync(LeaveRequisitionWF entity, CancellationToken cancellationToken);
        #endregion
    }
}
