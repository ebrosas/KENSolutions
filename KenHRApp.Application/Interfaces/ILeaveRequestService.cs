using KenHRApp.Application.DTOs;
using KenHRApp.Application.DTOs.TNA;
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
        Task<Result<int>> AddLeaveRequestAsync(
            LeaveRequisitionDTO dto,
            List<FileUploadDTO> files,
            string webRootPath,
            CancellationToken cancellationToken = default);

        Task<Result<int>> UpdateLeaveRequestAsync(LeaveRequisitionDTO dto, CancellationToken cancellationToken = default);
        Task<Result<List<EmployeeResultDTO>>> GetEmployeeAsync(int? empNo = 0, string costCenter = "");
        Task<bool> CheckIfLeaveDateIsHolidayAsync(DateTime? leaveDate);
        Task<decimal> CalculateAsync(
            int empNo,
            DateTime start,
            DateTime end,
            byte? startDay,
            byte? endDay);
        #endregion
    }
}
