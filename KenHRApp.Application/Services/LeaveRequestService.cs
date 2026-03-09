using Azure.Core;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        #region Fields
        private readonly ILeaveRequestRepository _repository;
        #endregion

        #region Enums
        public enum LeaveDayMode
        {
            FullDay = 1,
            FirstHalf = 2,
            SecondHalf = 3
        }
        #endregion

        #region Constructors
        public LeaveRequestService(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Public Methods
        public async Task<Result<decimal>> CalculateAsync(
            int empNo,
            DateTime start,
            DateTime end,
            LeaveDayMode startMode,
            LeaveDayMode endMode)
        {
            try
            {
                decimal total = 0;

                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    if (await _repository.IsDayOffAsync(empNo, date))
                        continue;

                    if (await _repository.IsPublicHolidayAsync(date))
                        continue;

                    total += 1;
                }

                if (startMode != LeaveDayMode.FullDay)
                    total -= 0.5m;

                if (endMode != LeaveDayMode.FullDay)
                    total -= 0.5m;

                return Result<decimal>.SuccessResult(total);
            }
            catch (Exception ex)
            {
                return Result<decimal>.Failure(ex.Message.ToString() ?? "Unknown error while fetching shift roster records from the database.");
            }
        }

        public async Task<Result<int>> AddLeaveRequestAsync(LeaveRequisitionDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                //var total = await CalculateAsync(
                //    dto.LeaveEmpNo,
                //    dto.LeaveStartDate!.Value,
                //    dto.LeaveEndDate!.Value,
                //    dto.StartDayMode!.Value,
                //    dto.EndDayMode!.Value,
                //    _shiftService,
                //    _holidayService);

                #region Create "LeaveRequisitionWF" entity from DTO
                LeaveRequisitionWF leaveRequest = new LeaveRequisitionWF()
                {
                    LeaveType = dto.LeaveType,
                    LeaveEmpNo = dto.LeaveEmpNo,
                    LeaveEmpName = dto.LeaveEmpName,
                    LeaveEmpEmail = dto.LeaveEmpEmail,
                    LeaveStartDate = Convert.ToDateTime(dto.LeaveStartDate),
                    LeaveEndDate = Convert.ToDateTime(dto.LeaveEndDate),
                    LeaveResumeDate = Convert.ToDateTime(dto.LeaveResumeDate),
                    LeaveEmpCostCenter = dto.LeaveEmpCostCenter,
                    LeaveCreatedBy = dto.LeaveCreatedBy,
                    LeaveApprovalFlag = 'A',
                    LeaveRemarks = dto.LeaveRemarks,
                    LeaveCreatedUserID = dto.LeaveCreatedUserID,
                    LeaveCreatedDate = DateTime.Now
                };
                #endregion

                var result = await _repository.AddLeaveRequestAsync(leaveRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save shift roster changes due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }
        #endregion
    }
}
