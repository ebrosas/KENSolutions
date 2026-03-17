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
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class LeaveRequestService : ILeaveRequestService
    {
        #region Fields
        private readonly ILeaveRequestRepository _repository;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        private const string CONST_LEAVE_FULL_DAY = "LEAVEFD";
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
                
        #region Contructors
        public LeaveRequestService(ILeaveRequestRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Private Methods
        
        #endregion

        #region Public Methods       
        public async Task<Result<int>> AddLeaveRequestAsync(
            LeaveRequisitionDTO dto, 
            List<FileUploadDTO> files,
            string webRootPath,
            CancellationToken cancellationToken = default)
        {
            try
            {
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
                    StartDayMode = dto.StartDayMode,
                    EndDayMode = dto.EndDayMode,
                    LeaveBalance = dto.LeaveBalance,
                    LeaveDuration = dto.LeaveDuration, 
                    NoOfHolidays = dto.NoOfHolidays,
                    NoOfWeekends = dto.NoOfWeekends,
                    LeavePayAdv = dto.LeavePayAdv,
                    LeaveVisaRequired = dto.LeaveVisaRequired,
                    LeaveRemarks = dto.LeaveRemarks,
                    LeaveCreatedBy = dto.LeaveCreatedBy,
                    LeaveCreatedEmail = dto.LeaveCreatedEmail,
                    LeaveCreatedUserID = dto.LeaveCreatedUserID,
                    LeaveCreatedDate = dto.LeaveCreatedDate,
                    LeaveStatusCode = dto.LeaveStatusCode,
                    LeaveStatusID = dto.LeaveStatusID,
                    StatusHandlingCode = dto.StatusHandlingCode
                };
                #endregion

                #region Initialize the file upload path
                string uploadPath = Path.Combine(
                    webRootPath,
                    "uploads",
                    "attachments");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                #endregion

                #region Initialize file attachments
                if (files is not null && files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file.Size > MaxFileSize)
                            throw new InvalidOperationException(
                                $"File {file.FileName} exceeds 10MB limit.");

                        if (file.Content is null)
                            throw new InvalidOperationException(
                                $"File stream for {file.FileName} is null.");

                        string storedFileName =
                            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                        string fullPath =
                            Path.Combine(uploadPath, storedFileName);

                        try
                        {
                            await using (var fileStream = new FileStream(
                               fullPath,
                               FileMode.Create,
                               FileAccess.Write,
                               FileShare.None,
                               81920,
                               useAsync: true))
                                {
                                    await file.Content.CopyToAsync(fileStream);
                                }
                        }
                        catch (Exception attachErr)
                        {
                        }

                        var attachment = new LeaveAttachment(
                            leaveRequest.LeaveAttachmentId,
                            file.FileName,                            
                            file.ContentType,
                            storedFileName,
                            file.Size);

                        leaveRequest.AddAttachment(attachment);
                    }
                }
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

        public async Task<Result<int>> UpdateLeaveRequestAsync(LeaveRequisitionDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                //LeaveDayMode startMode = LeaveDayMode.NotDefined;
                //if (Enum.IsDefined(typeof(LeaveDayMode), dto.StartDayMode!.Value))
                //    startMode = (LeaveDayMode)dto.StartDayMode!.Value;

                //LeaveDayMode endMode = LeaveDayMode.NotDefined;
                //if (Enum.IsDefined(typeof(LeaveDayMode), dto.EndDayMode!.Value))
                //    endMode = (LeaveDayMode)dto.EndDayMode!.Value;

                //var total = await CalculateAsync(
                //    dto.LeaveEmpNo,
                //    dto.LeaveStartDate!.Value,
                //    dto.LeaveEndDate!.Value,
                //    startMode,
                //    endMode);

                #region Create "LeaveRequisitionWF" entity from DTO
                LeaveRequisitionWF leaveRequest = new LeaveRequisitionWF()
                {
                    LeaveType = dto.LeaveType,
                    //LeaveEmpNo = dto.LeaveEmpNo,
                    //LeaveEmpName = dto.LeaveEmpName,
                    //LeaveEmpEmail = dto.LeaveEmpEmail,
                    //LeaveEmpCostCenter = dto.LeaveEmpCostCenter,
                    LeaveStartDate = Convert.ToDateTime(dto.LeaveStartDate),
                    LeaveEndDate = Convert.ToDateTime(dto.LeaveEndDate),
                    LeaveResumeDate = Convert.ToDateTime(dto.LeaveResumeDate),
                    LeaveBalance = dto.LeaveBalance,
                    LeaveDuration = dto.LeaveDuration, //Convert.ToDouble(total),
                    NoOfHolidays = dto.NoOfHolidays,
                    NoOfWeekends = dto.NoOfWeekends,
                    LeavePayAdv = dto.LeavePayAdv,
                    LeaveVisaRequired = dto.LeaveVisaRequired,
                    LeaveRemarks = dto.LeaveRemarks,
                    LeaveUpdatedBy = dto.LeaveCreatedBy,                    
                    LeaveUpdatedEmail = dto.LeaveUpdatedEmail,
                    LeaveUpdatedUserID = dto.LeaveUpdatedUserID,
                    LeaveUpdatedDate = dto.LeaveUpdatedDate,
                    LeaveStatusCode = dto.LeaveStatusCode,
                    LeaveStatusID = dto.LeaveStatusID,
                    StatusHandlingCode = dto.StatusHandlingCode                    
                };
                #endregion

                var result = await _repository.UpdateLeaveRequestAsync(leaveRequest, cancellationToken);
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
                        JobTitle = e.JobTitle,
                        EmpEmail = e.EmpEmail,
                        DILBalance = e.DILBalance,
                        LeaveBalance = e.LeaveBalance,
                        SLBalance = e.SLBalance
                    }).ToList();
                }

                return Result<List<EmployeeResultDTO>>.SuccessResult(employeeList);
            }
            catch (Exception ex)
            {
                return Result<List<EmployeeResultDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching employee list from the database.");
            }
        }

        public async Task<bool> CheckIfLeaveDateIsHolidayAsync(DateTime? leaveDate)
        {
            try
            {
                var repoResult = await _repository.CheckIfLeaveDateIsHolidayAsync(leaveDate);
                if (!repoResult.Success)
                {
                    return false;
                }

                bool isHoliday = repoResult.Value;

                return isHoliday;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        //public async Task<decimal> CalculateAsync(
        //   int empNo,
        //   DateTime start,
        //   DateTime end,
        //   byte? startDay,
        //   byte? endDay)
        //{
        //    try
        //    {
        //        decimal total = 0;
        //        LeaveDayMode startMode = LeaveDayMode.FullDay;
        //        LeaveDayMode endMode = LeaveDayMode.FullDay;

        //        if (startDay.HasValue && Enum.IsDefined(typeof(LeaveDayMode), startDay.Value))
        //            startMode = (LeaveDayMode)startDay.Value;

        //        if (endDay.HasValue && Enum.IsDefined(typeof(LeaveDayMode), endDay.Value))
        //            endMode = (LeaveDayMode)endDay.Value;

        //        for (var date = start; date <= end; date = date.AddDays(1))
        //        {
        //            if (await _repository.IsDayOffAsync(empNo, date))
        //                continue;

        //            if (await _repository.IsPublicHolidayAsync(date))
        //                continue;

        //            total += 1;
        //        }

        //        if (startMode != LeaveDayMode.FullDay)
        //            total -= 0.5m;

        //        if (endMode != LeaveDayMode.FullDay)
        //            total -= 0.5m;

        //        return total;
        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;
        //    }
        //}

        public async Task<decimal> CalculateAsync(
           int empNo,
           DateTime start,
           DateTime end,
           string? startDay,
           string? endDay)
        {
            try
            {
                decimal total = 0;
                //LeaveDayMode startMode = LeaveDayMode.FullDay;
                //LeaveDayMode endMode = LeaveDayMode.FullDay;

                //if (startDay.HasValue && Enum.IsDefined(typeof(LeaveDayMode), startDay.Value))
                //    startMode = (LeaveDayMode)startDay.Value;

                //if (endDay.HasValue && Enum.IsDefined(typeof(LeaveDayMode), endDay.Value))
                //    endMode = (LeaveDayMode)endDay.Value;

                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    if (await _repository.IsDayOffAsync(empNo, date))
                        continue;

                    if (await _repository.IsPublicHolidayAsync(date))
                        continue;

                    total += 1;
                }

                if (startDay != CONST_LEAVE_FULL_DAY)
                    total -= 0.5m;

                if (endDay != CONST_LEAVE_FULL_DAY)
                    total -= 0.5m;

                return total;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        #endregion
    }
}
