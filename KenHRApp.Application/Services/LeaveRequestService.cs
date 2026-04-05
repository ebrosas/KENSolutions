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
                    LeaveRequestId = dto.LeaveRequestId,
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
                        CreatedByName = model.CreatedByName,

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

        public async Task<Result<List<LeaveRequestResultDTO>>> SearchLeaveRequestAsync(
            long? leaveRequestNo,
            int? empNo,
            string? costCenter,
            string? leaveType,
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<LeaveRequestResultDTO> leaveRequestList = new();

            try
            {
                var repoResult = await _repository.SearchLeaveRequestAsync(leaveRequestNo, empNo, costCenter, leaveType, status, startDate, endDate);
                if (!repoResult.Success)
                {
                    return Result<List<LeaveRequestResultDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    leaveRequestList = model.Select(e => new LeaveRequestResultDTO
                    {
                        LeaveRequestId = e.LeaveRequestId,
                        LeaveAttachmentId = e.LeaveAttachmentId,
                        WorkflowId = e.WorkflowId,
                        LeaveInstanceID = e.LeaveInstanceID,
                        LeaveType = e.LeaveType,
                        LeaveTypeDesc = e.LeaveTypeDesc,
                        LeaveEmpNo = e.LeaveEmpNo,
                        LeaveEmpName = e.LeaveEmpName,
                        LeaveEmpEmail = e.LeaveEmpEmail,
                        LeaveStartDate = e.LeaveStartDate,
                        LeaveEndDate = e.LeaveEndDate,
                        LeaveResumeDate = e.LeaveResumeDate,
                        LeaveEmpCostCenter = e.LeaveEmpCostCenter,
                        LeaveRemarks = e.LeaveRemarks,
                        LeaveConstraints = e.LeaveConstraints,
                        LeaveStatusCode = e.LeaveStatusCode,
                        LeaveApprovalFlag = e.LeaveApprovalFlag,
                        LeaveVisaRequired = e.LeaveVisaRequired,
                        LeavePayAdv = e.LeavePayAdv,
                        LeaveIsFTMember = e.LeaveIsFTMember,
                        LeaveBalance = e.LeaveBalance,
                        LeaveDuration = e.LeaveDuration.HasValue ? Convert.ToDouble(e.LeaveDuration) : 0,
                        NoOfHolidays = e.NoOfHolidays.HasValue ? Convert.ToInt32(e.NoOfHolidays) : 0,
                        NoOfWeekends = e.NoOfWeekends.HasValue ? Convert.ToInt32(e.NoOfWeekends) : 0,
                        PlannedLeave = e.PlannedLeave,
                        LeavePlannedNo = e.LeavePlannedNo,
                        HalfDayLeaveFlag = e.HalfDayLeaveFlag,
                        LeaveCreatedDate = e.LeaveCreatedDate,
                        LeaveCreatedBy = e.LeaveCreatedBy,
                        LeaveCreatedUserID = e.LeaveCreatedUserID,
                        LeaveCreatedEmail = e.LeaveCreatedEmail,
                        LeaveUpdatedDate = e.LeaveUpdatedDate,
                        LeaveUpdatedBy = e.LeaveUpdatedBy,
                        LeaveUpdatedUserID = e.LeaveType,
                        LeaveUpdatedEmail = e.LeaveUpdatedEmail,
                        LeaveStatusID = e.LeaveStatusID,
                        StatusHandlingCode = e.StatusHandlingCode,
                        StartDayMode = e.StartDayMode,
                        StartDayModeDesc = e.StartDayModeDesc,
                        EndDayMode = e.EndDayMode,
                        EndDayModeDesc = e.EndDayModeDesc,
                        StatusDesc = e.StatusDesc,
                        ApprovalFlagDesc = e.ApprovalFlagDesc,
                        CreatedByName = e.CreatedByName,
                        DepartmentCode = e.DepartmentCode,
                        DepartmentName = e.DepartmentName,
                        AttachmentList = e.AttachmentList
                    }).ToList();
                }

                return Result<List<LeaveRequestResultDTO>>.SuccessResult(leaveRequestList);
            }
            catch (Exception ex)
            {
                return Result<List<LeaveRequestResultDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the leave requisition records from the database.");
            }
        }

        public async Task<Result<List<LeaveEntitlementDTO>>> GetLeaveEntitlementAsync(
            int? entitlementId,
            int? empNo,
            string? costCenter,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<LeaveEntitlementDTO> leaveEntitlementList = new();

            try
            {
                var repoResult = await _repository.GetLeaveEntitlementAsync(entitlementId, empNo, costCenter, startDate, endDate);
                if (!repoResult.Success)
                {
                    return Result<List<LeaveEntitlementDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    leaveEntitlementList = model.Select(e => new LeaveEntitlementDTO
                    {
                        LeaveEntitlementId = e.LeaveEntitlementId,
                        EmployeeNo = e.EmployeeNo,
                        EmployeeName = e.EmployeeName,
                        DepartmentCode = e.DepartmentCode,
                        DepartmentName = e.DepartmentName,
                        EffectiveDate = e.EffectiveDate,
                        ALEntitlementCount = e.ALEntitlementCount,
                        SLEntitlementCount = e.SLEntitlementCount,
                        ALRenewalType = e.ALRenewalType,
                        ALRenewalTypeDesc = e.ALRenewalTypeDesc,
                        SLRenewalType = e.SLRenewalType,
                        SLRenewalTypeDesc = e.SLRenewalTypeDesc,
                        LeaveUOM = e.LeaveUOM,
                        LeaveUOMDesc = e.LeaveUOMDesc,
                        SickLeaveUOM = e.SickLeaveUOM,
                        SickLeaveUOMDesc = e.SickLeaveUOMDesc,
                        LeaveBalance = e.LeaveBalance,
                        SLBalance = e.SLBalance,
                        DILBalance = e.DILBalance,
                        CreatedDate = e.CreatedDate,
                        LeaveCreatedBy = e.LeaveCreatedBy,
                        CreatedUserID = e.CreatedUserID,
                        LastUpdatedDate = e.LastUpdatedDate,
                        LastUpdatedBy = e.LastUpdatedBy,
                        LastUpdatedUserID = e.LastUpdatedUserID
                    }).ToList();
                }

                return Result<List<LeaveEntitlementDTO>>.SuccessResult(leaveEntitlementList);
            }
            catch (Exception ex)
            {
                return Result<List<LeaveEntitlementDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the leave entitlement records from the database.");
            }
        }

        public async Task<Result<int>> AddLeaveEntitlementAsync(LeaveEntitlementDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize entity
                LeaveEntitlement entitlement = new LeaveEntitlement()
                {
                    EmployeeNo = dto.EmployeeNo,
                    EffectiveDate = Convert.ToDateTime(dto.EffectiveDate).Date,
                    ALEntitlementCount = Convert.ToDouble(dto.ALEntitlementCount),
                    SLEntitlementCount = dto.SLEntitlementCount,
                    ALRenewalType = dto.ALRenewalType,
                    SLRenewalType = dto.SLRenewalType,
                    LeaveUOM = dto.LeaveUOM,
                    SickLeaveUOM = dto.SickLeaveUOM,
                    LeaveBalance = dto.LeaveBalance,
                    SLBalance = dto.SLBalance,
                    DILBalance = dto.DILBalance,
                    CreatedDate = dto.CreatedDate,
                    LeaveCreatedBy = dto.LeaveCreatedBy,
                    CreatedUserID = dto.CreatedUserID
                };
                #endregion

                var result = await _repository.AddLeaveEntitlementAsync(entitlement, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to commit data changes to the database. Please try saving again.");
                }

                return Result<int>.SuccessResult(result.Value);

            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> UpdateLeaveEntitlementAsync(LeaveEntitlementDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize entity
                LeaveEntitlement entitlement = new LeaveEntitlement()
                {
                    LeaveEntitlementId = dto.LeaveEntitlementId,
                    EmployeeNo = dto.EmployeeNo,
                    EffectiveDate = Convert.ToDateTime(dto.EffectiveDate).Date,
                    ALEntitlementCount = Convert.ToDouble(dto.ALEntitlementCount),
                    SLEntitlementCount = dto.SLEntitlementCount,
                    ALRenewalType = dto.ALRenewalType,
                    SLRenewalType = dto.SLRenewalType,
                    LeaveUOM = dto.LeaveUOM,
                    SickLeaveUOM = dto.SickLeaveUOM,
                    LeaveBalance = dto.LeaveBalance,
                    SLBalance = dto.SLBalance,
                    DILBalance = dto.DILBalance,
                    LastUpdatedDate = dto.LastUpdatedDate,
                    LastUpdatedBy = dto.LastUpdatedBy,
                    LastUpdatedUserID = dto.LastUpdatedUserID
                };
                #endregion

                var result = await _repository.UpdateLeaveEntitlementAsync(entitlement, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to commit data changes to the database. Please try saving again.");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<bool>> DeleteLeaveEntitlementAsync(int entitlementID, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.DeleteLeaveEntitlementAsync(entitlementID, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to delete the selected leave entitlement due to unknown error. Please refresh the page then try to delete again.");
                }

                return Result<bool>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString());
            }
        }
        #endregion
    }
}
