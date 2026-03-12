using Azure.Core;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.DTOs.TNA;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
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
        public enum LeaveDayMode : byte
        {
            NotDefined,
            FullDay,
            FirstHalf,
            SecondHalf
        }
        #endregion

        #region Constants
        private readonly string CONST_APPROVED_PAID = "A";
        private readonly string CONST_WAITING_FOR_APPROVAL = "W";
        private readonly string CONST_APPROVED_NOT_PAID = "N";
        private readonly string CONST_CANCELLED = "C";
        private readonly string CONST_DRAFT = "D";
        private readonly string CONST_REJECTED = "R";
        #endregion

        #region Contructors
        public LeaveRequestService(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Private Methods
        private async Task<decimal> CalculateAsync(
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

                return total;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion

        #region Public Methods       
        public async Task<Result<int>> AddLeaveRequestAsync(LeaveRequisitionDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {

                LeaveDayMode startMode = LeaveDayMode.NotDefined;
                if (Enum.IsDefined(typeof(LeaveDayMode), dto.StartDayMode!.Value))
                    startMode = (LeaveDayMode)dto.StartDayMode!.Value;

                LeaveDayMode endMode = LeaveDayMode.NotDefined;
                if (Enum.IsDefined(typeof(LeaveDayMode), dto.EndDayMode!.Value))
                    endMode = (LeaveDayMode)dto.EndDayMode!.Value;

                var total = await CalculateAsync(
                    dto.LeaveEmpNo,
                    dto.LeaveStartDate!.Value,
                    dto.LeaveEndDate!.Value,
                    startMode,
                    endMode);

                #region Create "LeaveRequisitionWF" entity from DTO
                LeaveRequisitionWF leaveRequest = new LeaveRequisitionWF()
                {
                    LeaveType = dto.LeaveType,
                    LeaveEmpNo = dto.LeaveEmpNo,
                    LeaveEmpName = dto.LeaveEmpName,
                    LeaveEmpEmail = dto.LeaveEmpEmail,
                    LeaveEmpCostCenter = dto.LeaveEmpCostCenter,
                    LeaveStartDate = Convert.ToDateTime(dto.LeaveStartDate),
                    LeaveEndDate = Convert.ToDateTime(dto.LeaveEndDate),
                    LeaveResumeDate = Convert.ToDateTime(dto.LeaveResumeDate),
                    LeaveBalance = dto.LeaveBalance,
                    LeaveDuration = Convert.ToDouble(total),            
                    NoOfHolidays = dto.NoOfHolidays,
                    NoOfWeekends = dto.NoOfWeekends,
                    LeaveCreatedBy = dto.LeaveCreatedBy,
                    LeaveApprovalFlag = char.Parse(CONST_WAITING_FOR_APPROVAL),
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
                        throw new Exception("Unable to save leave request due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<List<EmployeeResultDTO>>> GetEmployeeAsync(int? empNo = 0, string costCenter = "")
        {
            List<EmployeeResultDTO> employeeList = new();

            try
            {
                var repoResult = await _repository.GetEmployeeAsync(empNo, costCenter);
                if (!repoResult.Success)
                {
                    return Result<List<EmployeeResultDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    employeeList = model.Select(e => new EmployeeResultDTO
                    {
                        EmployeeId = e.EmployeeId,
                        EmployeeNo = e.EmployeeNo,
                        FirstName = e.FirstName,
                        MiddleName = e.MiddleName,
                        LastName = e.LastName,
                        Gender = e.Gender,
                        HireDate = e.HireDate,
                        DOB = e.DOB,
                        ReportingManagerCode = e.ReportingManagerCode,
                        ReportingManager = e.ReportingManager,
                        DepartmentCode = e.DepartmentCode,
                        DepartmentName = e.DepartmentName,
                        JobTitle = e.JobTitle
                    }).ToList();
                }

                return Result<List<EmployeeResultDTO>>.SuccessResult(employeeList);
            }
            catch (Exception ex)
            {
                return Result<List<EmployeeResultDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching employee list from the database.");
            }
        }

        #endregion
    }
}
