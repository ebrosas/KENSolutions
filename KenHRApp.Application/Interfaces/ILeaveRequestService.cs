using KenHRApp.Application.DTOs;
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
        Task<Result<int>> AddLeaveRequestAsync(LeaveRequisitionDTO dto, CancellationToken cancellationToken = default);
        #endregion
    }
}
