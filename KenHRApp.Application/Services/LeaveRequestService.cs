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
        private int _holidayCount = 0;
        private int _weekendCount = 0;
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

        #region Properties
        //public int HolidayCount { get; set; }
        //public int WeekendCount { get; set; }

        public Task<int> HolidayCount()
        {
            return Task.FromResult(_holidayCount);
        }

        public Task<int> WeekendCount()
        {
            return Task.FromResult(_weekendCount);
        }
        #endregion

        #region Public Methods       
        public async Task<Result<long>> AddLeaveRequestAsync(
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
                    StatusHandlingCode = dto.StatusHandlingCode,
                    LeaveApprovalFlag = dto.LeaveApprovalFlag
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

                return Result<long>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<long>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> UpdateLeaveRequestAsync(LeaveRequisitionDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
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

        public async Task<Result<int>> CancelLeaveRequestAsync(LeaveRequisitionDTO dto, CancellationToken cancellationToken = default)
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
                    LeaveUpdatedBy = dto.LeaveCreatedBy,
                    LeaveUpdatedEmail = dto.LeaveUpdatedEmail,
                    LeaveUpdatedUserID = dto.LeaveUpdatedUserID,
                    LeaveUpdatedDate = dto.LeaveUpdatedDate,
                    LeaveStatusCode = dto.LeaveStatusCode,
                    LeaveStatusID = dto.LeaveStatusID,
                    StatusHandlingCode = dto.StatusHandlingCode,
                    LeaveApprovalFlag = dto.LeaveApprovalFlag
                };
                #endregion

                var result = await _repository.CancelLeaveRequestAsync(leaveRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to cancel leave request due to unhandled error. Please check the data entry then try again!");
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

        public async Task<bool> CheckIfLeaveDateIsHolidayAsync(DateTime leaveDate)
        {
            bool isHoliday = false;

            try
            {
                if (await _repository.IsPublicHolidayAsync(leaveDate))
                {
                    isHoliday = true;
                }

                //var repoResult = await _repository.CheckIfLeaveDateIsHolidayAsync(leaveDate);
                //if (!repoResult.Success)
                //{
                //    return false;
                //}

                //bool isHoliday = repoResult.Value;

                return isHoliday;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> CheckIfLeavePeriodExistAsync(int employeeNo, DateTime leaveDate)
        {
            bool isLeaveExist = false;

            try
            {
                if (await _repository.IsLeaveOverlapAsync(employeeNo, leaveDate))
                {
                    isLeaveExist = true;
                }

                return isLeaveExist;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

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

                // Reset holiday and weekend counter
                _holidayCount = 0;
                _weekendCount = 0;

                for (var date = start; date <= end; date = date.AddDays(1))
                {
                    if (await _repository.IsDayOffAsync(empNo, date))
                    {
                        _weekendCount++;
                        continue;
                    }

                    if (await _repository.IsPublicHolidayAsync(date))
                    {
                        // Increment the holiday count
                        _holidayCount++;
                        continue;
                    }

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

        public async Task<Result<LeaveRequisitionDTO?>> GetLeaveRequestAsync(long leaveRequestNo)
        {
            LeaveRequisitionDTO? leaveRequest = null;

            try
            {
                var repoResult = await _repository.GetLeaveRequestAsync(leaveRequestNo);
                if (!repoResult.Success)
                {
                    return Result<LeaveRequisitionDTO?>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    leaveRequest = new LeaveRequisitionDTO
                    {
                        LeaveRequestId = model.LeaveRequestId,
                        LeaveAttachmentId = model.LeaveAttachmentId,
                        WorkflowId = model.WorkflowId,
                        LeaveInstanceID = model.LeaveInstanceID,
                        LeaveType = model.LeaveType,
                        LeaveEmpNo = model.LeaveEmpNo,
                        LeaveEmpName = model.LeaveEmpName,
                        LeaveEmpEmail = model.LeaveEmpEmail,
                        LeaveStartDate = model.LeaveStartDate,
                        LeaveEndDate = model.LeaveEndDate,
                        LeaveResumeDate = model.LeaveResumeDate,
                        LeaveEmpCostCenter = model.LeaveEmpCostCenter,
                        LeaveRemarks = model.LeaveRemarks,
                        LeaveConstraints = model.LeaveConstraints,
                        LeaveStatusCode = model.LeaveStatusCode,
                        LeaveApprovalFlag = model.LeaveApprovalFlag,
                        LeaveVisaRequired = model.LeaveVisaRequired,
                        LeavePayAdv = model.LeavePayAdv,
                        LeaveIsFTMember = model.LeaveIsFTMember,
                        LeaveBalance = model.LeaveBalance,
                        LeaveDuration = model.LeaveDuration,
                        NoOfHolidays = model.NoOfHolidays,
                        NoOfWeekends = model.NoOfWeekends,
                        PlannedLeave = model.PlannedLeave,
                        LeavePlannedNo = model.LeavePlannedNo,
                        HalfDayLeaveFlag = model.HalfDayLeaveFlag,
                        LeaveCreatedDate = model.LeaveCreatedDate,
                        LeaveCreatedBy = model.LeaveCreatedBy,
                        LeaveCreatedUserID = model.LeaveCreatedUserID,
                        LeaveCreatedEmail = model.LeaveCreatedEmail,
                        LeaveUpdatedDate = model.LeaveUpdatedDate,
                        LeaveUpdatedBy = model.LeaveUpdatedBy,
                        LeaveUpdatedUserID = model.LeaveType,
                        LeaveUpdatedEmail = model.LeaveUpdatedEmail,
                        LeaveStatusID = model.LeaveStatusID,
                        StatusHandlingCode = model.StatusHandlingCode,
                        StartDayMode = model.StartDayMode,
                        EndDayMode = model.EndDayMode,
                        StatusDesc = model.StatusDesc,
                        ApprovalFlagDesc = model.ApprovalFlagDesc,

                        Files = model.AttachmentList!.Select(e => new LeaveAttachmentDTO
                        {
                            Id = e.Id,
                            LeaveAttachmentId = e.LeaveAttachmentId,
                            FileName = e.FileName,
                            StoredFileName = e.StoredFileName,
                            ContentType = e.ContentType,
                            FileSize = e.FileSize
                        }).ToList(),
                    };
                }

                return Result<LeaveRequisitionDTO?>.SuccessResult(leaveRequest);
            }
            catch (Exception ex)
            {
                return Result<LeaveRequisitionDTO?>.Failure(ex.Message.ToString() ?? "Unknown error while fetching leave request record from the database.");
            }
        }
        #endregion
    }
}
