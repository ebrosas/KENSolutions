using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
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
        Task<Result<int>> AddLeaveRequestAsync(LeaveRequisitionWF leaveRequest, CancellationToken cancellationToken);
        Task<Result<int>> UpdateLeaveRequestAsync(LeaveRequisitionWF leaveRequest, CancellationToken cancellationToken = default);
        Task<Result<int>> CancelLeaveRequestAsync(LeaveRequisitionWF leaveRequest, CancellationToken cancellationToken = default);
        Task<Result<List<EmployeeResult>>> GetEmployeeAsync(int? empNo, string? costCenter);
        Task<Result<bool>> CheckIfLeaveDateIsHolidayAsync(DateTime? leaveDate);
        #endregion
    }
}
