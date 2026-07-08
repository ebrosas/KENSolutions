using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KenHRApp.Infrastructure.Repositories
{
    public class LeaveRequestRepository : ILeaveRequestRepository
    {
        #region Fields
        private readonly AppDbContext _db;
        #endregion

        #region Constructors                
        public LeaveRequestRepository(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Properties
        public Task<bool> IsPublicHolidayAsync(DateTime date)
        {
            bool result = false;

            try
            {
                var existing = _db.Holidays
                    .FirstOrDefault(e =>
                        e.HolidayDate == date && e.HolidayType == 1);

                if (existing != null)
                    result = true;

                return Task.FromResult(result);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }

        public Task<bool> IsDayOffAsync(int employeeId, DateTime date)
        {
            // Implement logic to check if the given date is a day off for the specified employee
            return Task.FromResult(false); // Placeholder implementation
        }

        public Task<bool> IsLeaveOverlapAsync(int employeeNo, DateTime date)
        {
            bool result = false;

            try
            {
                var existing = _db.LeaveRequisitionWFs
                    .FirstOrDefault(e =>
                        e.LeaveEmpNo == employeeNo &&
                        (date >= e.LeaveStartDate && date <= e.LeaveEndDate) &&
                        (e.StatusHandlingCode != "Cancelled" && e.StatusHandlingCode != "Rejected"));

                if (existing != null)
                    result = true;

                return Task.FromResult(result);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Task.FromResult(false);
            }
        }
        #endregion

        #region Abstract methods        
        /// <summary>
        /// Add new leave request
        /// </summary>
        public async Task<Result<long>> AddLeaveRequestAsync(LeaveRequisitionWF dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                #region Initialize entity
                var leaveRequest = new LeaveRequisitionWF
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

                #region Initialize attachments
                if (dto.AttachmentList != null && dto.AttachmentList.Any())
                {
                    leaveRequest.AttachmentList = dto.AttachmentList.Select(e => new LeaveAttachment
                    {
                        LeaveAttachmentId = e.LeaveAttachmentId,
                        FileName = e.FileName,
                        StoredFileName = e.StoredFileName,
                        ContentType = e.ContentType,
                        FileSize = e.FileSize
                    }).ToList();
                }
                #endregion

                // Save to database
                await _db.LeaveRequisitionWFs.AddAsync(leaveRequest);
                await _db.SaveChangesAsync(cancellationToken);

                // ✅ EF Core automatically populates identity after SaveChanges
                long generatedId = leaveRequest.LeaveRequestId;

                return Result<long>.SuccessResult(generatedId);

            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<long>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<long>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update leave request
        /// </summary>
        public async Task<Result<int>> UpdateLeaveRequestAsync(LeaveRequisitionWF leaveRequest, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (leaveRequest == null)
                    throw new ArgumentNullException(nameof(leaveRequest));

                var existing = await _db.LeaveRequisitionWFs
                    .FirstOrDefaultAsync(e =>
                        e.LeaveRequestId == leaveRequest.LeaveRequestId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find leave request with the specified request no.");

                #region Update leave request information
                existing.LeaveType = leaveRequest.LeaveType;
                existing.LeaveStartDate = leaveRequest.LeaveStartDate;
                existing.StartDayMode = leaveRequest.StartDayMode;
                existing.LeaveResumeDate = leaveRequest.LeaveResumeDate;
                existing.EndDayMode = leaveRequest.EndDayMode;
                existing.LeaveRemarks = leaveRequest.LeaveRemarks;
                existing.LeaveVisaRequired = leaveRequest.LeaveVisaRequired;
                existing.LeavePayAdv = leaveRequest.LeavePayAdv;
                existing.LeaveUpdatedBy = leaveRequest.LeaveUpdatedBy;
                existing.LeaveUpdatedUserID = leaveRequest.LeaveUpdatedUserID;
                existing.LeaveUpdatedEmail = leaveRequest.LeaveUpdatedEmail;
                existing.LeaveUpdatedDate = leaveRequest.LeaveUpdatedDate;
                #endregion

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);
                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancel leave request
        /// </summary>
        public async Task<Result<int>> CancelLeaveRequestAsync(
            LeaveRequisitionWF leaveRequest, 
            CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (leaveRequest == null)
                    throw new ArgumentNullException(nameof(leaveRequest));

                var existing = await _db.LeaveRequisitionWFs
                    .FirstOrDefaultAsync(e =>
                        e.LeaveRequestId == leaveRequest.LeaveRequestId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find leave request with the specified request no.");

                #region Update leave information to cancel the request
                existing.LeaveStatusCode = leaveRequest.LeaveStatusCode;
                existing.LeaveStatusID = leaveRequest.LeaveStatusID;
                existing.StatusHandlingCode = leaveRequest.StatusHandlingCode;
                existing.LeaveApprovalFlag = leaveRequest.LeaveApprovalFlag;
                existing.LeaveUpdatedBy = leaveRequest.LeaveUpdatedBy;
                existing.LeaveUpdatedUserID = leaveRequest.LeaveUpdatedUserID;
                existing.LeaveUpdatedEmail = leaveRequest.LeaveUpdatedEmail;
                existing.LeaveUpdatedDate = leaveRequest.LeaveUpdatedDate;
                #endregion

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                if (rowsUpdated > 0)
                {
                    #region Cancel the associated workflow step instance
                    var wfInstance = await (from wd in _db.WorkflowDefinitions
                                            join wi in _db.WorkflowInstances on wd.WorkflowDefinitionId equals wi.WorkflowDefinitionId
                                            join wsi in _db.WorkflowStepInstances on wi.WorkflowInstanceId equals wsi.WorkflowInstanceId
                                            where wd.EntityName == "RTYPELEAVE"
                                                && wi.EntityId == leaveRequest.LeaveRequestId
                                                && wsi.Status == "Pending"
                                            select wsi).ToListAsync();
                    if (wfInstance != null && wfInstance.Any())
                    {
                        foreach (var item in wfInstance)
                        {
                            item.Status = "Cancelled";
                        }

                        int wfRowsUpdated = await _db.SaveChangesAsync(cancellationToken);
                    }
                    #endregion
                }

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<EmployeeResult>>> GetEmployeeAsync(int? empNo, string? costCenter)
        {
            List<EmployeeResult> employeeList = new();

            try
            {
                var model = await _db.Set<EmployeeResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetEmployeeList @empNo = {0}, @costCenter = {1}",
                    empNo!, costCenter!)
                    .AsNoTracking()
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    employeeList = model.Select(e => new EmployeeResult
                    {
                        EmployeeId = e.EmployeeId,
                        EmployeeNo = e.EmployeeNo,
                        FirstName  = e.FirstName,
                        MiddleName  = e.MiddleName,
                        LastName  = e.LastName,
                        Gender  = e.Gender,
                        HireDate  = e.HireDate,
                        DOB  = e.DOB,
                        ReportingManagerCode  = e.ReportingManagerCode,
                        ReportingManager  = e.ReportingManager,
                        DepartmentCode  = e.DepartmentCode,
                        DepartmentName  = e.DepartmentName,
                        JobTitle = e.JobTitle,
                        EmpEmail = e.EmpEmail,
                        DILBalance = e.DILBalance,
                        LeaveBalance = e.LeaveBalance,
                        SLBalance = e.SLBalance
                    }).ToList();
                }

                return Result<List<EmployeeResult>>.SuccessResult(employeeList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<EmployeeResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> CheckIfLeaveDateIsHolidayAsync(DateTime? leaveDate)
        {
            bool result = false;

            try
            {
                if (leaveDate == null)
                    return Result<bool>.Failure("Leave date cannot be null.");

                var existing = await _db.Holidays
                    .FirstOrDefaultAsync(e => e.HolidayDate == Convert.ToDateTime(leaveDate).Date 
                        && e.HolidayType == 1);

                if (existing != null)
                    result = true;

                //var query = _db.Holidays.AsQueryable()
                //            .Where(h => h.HolidayDate.Date == leaveDate.Value.Date &&
                //            h.HolidayType == 1);

                //var model = await query.FirstOrDefaultAsync();
                //if (model != null)
                //    result = true;

                return Result<bool>.SuccessResult(result);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get leave request details
        /// </summary>
        /// <param name="leaveRequestNo"></param>
        /// <returns></returns>
        public async Task<Result<LeaveRequestResult>> GetLeaveRequestAsync(long leaveRequestNo)
        {
            LeaveRequestResult leaveRequest = new();

            try
            {
                var model = await _db.Set<LeaveRequestResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetLeaveRequestDetail @leaveNo = {0}",
                    leaveRequestNo)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    leaveRequest.LeaveRequestId = model[0].LeaveRequestId;
                    leaveRequest.LeaveAttachmentId = model[0].LeaveAttachmentId;
                    leaveRequest.WorkflowId = model[0].WorkflowId;
                    leaveRequest.LeaveInstanceID = model[0].LeaveInstanceID;
                    leaveRequest.LeaveType = model[0].LeaveType;
                    leaveRequest.LeaveEmpNo = model[0].LeaveEmpNo;
                    leaveRequest.LeaveEmpName = model[0].LeaveEmpName;
                    leaveRequest.LeaveEmpEmail = model[0].LeaveEmpEmail;
                    leaveRequest.LeaveStartDate = model[0].LeaveStartDate;
                    leaveRequest.LeaveEndDate = model[0].LeaveEndDate;
                    leaveRequest.LeaveResumeDate = model[0].LeaveResumeDate;
                    leaveRequest.LeaveEmpCostCenter = model[0].LeaveEmpCostCenter;
                    leaveRequest.LeaveRemarks = model[0].LeaveRemarks;
                    leaveRequest.LeaveConstraints = model[0].LeaveConstraints;
                    leaveRequest.LeaveStatusCode = model[0].LeaveStatusCode;
                    leaveRequest.LeaveApprovalFlag = model[0].LeaveApprovalFlag;
                    leaveRequest.LeaveVisaRequired = model[0].LeaveVisaRequired;
                    leaveRequest.LeavePayAdv = model[0].LeavePayAdv;
                    leaveRequest.LeaveIsFTMember = model[0].LeaveIsFTMember;
                    leaveRequest.LeaveBalance = model[0].LeaveBalance;
                    leaveRequest.LeaveDuration = model[0].LeaveDuration;
                    leaveRequest.NoOfHolidays = model[0].NoOfHolidays;
                    leaveRequest.NoOfWeekends = model[0].NoOfWeekends;
                    leaveRequest.PlannedLeave = model[0].PlannedLeave;
                    leaveRequest.LeavePlannedNo = model[0].LeavePlannedNo;
                    leaveRequest.HalfDayLeaveFlag = model[0].HalfDayLeaveFlag;
                    leaveRequest.LeaveCreatedDate = model[0].LeaveCreatedDate;
                    leaveRequest.LeaveCreatedBy = model[0].LeaveCreatedBy;
                    leaveRequest.LeaveCreatedUserID = model[0].LeaveCreatedUserID;
                    leaveRequest.LeaveCreatedEmail = model[0].LeaveCreatedEmail;
                    leaveRequest.LeaveUpdatedDate = model[0].LeaveUpdatedDate;
                    leaveRequest.LeaveUpdatedBy = model[0].LeaveUpdatedBy;
                    leaveRequest.LeaveUpdatedUserID = model[0].LeaveType;
                    leaveRequest.LeaveUpdatedEmail = model[0].LeaveUpdatedEmail;
                    leaveRequest.LeaveStatusID = model[0].LeaveStatusID;
                    leaveRequest.StatusHandlingCode = model[0].StatusHandlingCode;
                    leaveRequest.StartDayMode = model[0].StartDayMode;
                    leaveRequest.EndDayMode = model[0].EndDayMode;
                    leaveRequest.StatusDesc = model[0].StatusDesc;
                    leaveRequest.ApprovalFlagDesc = model[0].ApprovalFlagDesc;
                    leaveRequest.CreatedByName = model[0].CreatedByName;
                    leaveRequest.ApproverNo = model[0].ApproverNo;
                    leaveRequest.ApproverName = model[0].ApproverName;
                    leaveRequest.SubstituteNo = model[0].ApproverNo;
                    leaveRequest.SubstituteName = model[0].ApproverName;

                    #region Get the file attachments
                    List<LeaveAttachment> attachments = new();

                    var attachModel = await (from attach in _db.LeaveAttachments
                                             where attach.LeaveAttachmentId == leaveRequest.LeaveAttachmentId
                                             select attach).ToListAsync();
                    if (attachModel != null)
                    {
                        leaveRequest.AttachmentList = attachModel.Select(e => new LeaveAttachment
                        {
                            Id = e.Id,
                            LeaveAttachmentId = e.LeaveAttachmentId,
                            FileName = e.FileName,
                            StoredFileName = e.StoredFileName,
                            ContentType = e.ContentType,
                            FileSize = e.FileSize
                        }).ToList();
                    }
                    #endregion
                }

                return Result<LeaveRequestResult>.SuccessResult(leaveRequest);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<LeaveRequestResult>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get leave request details
        /// </summary>
        /// <param name="leaveRequestNo"></param>
        /// <returns></returns>
        public async Task<Result<List<LeaveRequestResult>>> SearchLeaveRequestAsync(
            long? leaveRequestNo,
            int? empNo,
            string? costCenter,
            string? leaveType,
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<LeaveRequestResult> leaveRequestList = new();

            try
            {
                var model = (await _db.Set<LeaveRequestResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetLeaveRequestDetail @leaveNo = {0}, @empNo = {1}, @costCenter = {2}, @leaveType = {3}, @status = {4}, @startDate = {5}, @endDate = {6}",
                    leaveRequestNo!,
                    empNo!,
                    costCenter!,
                    leaveType!,
                    status!,
                    startDate!,
                    endDate!)
                    .ToListAsync()).AsEnumerable().OrderByDescending(a => a.LeaveRequestId);
                if (model != null && model.Any())
                {
                    leaveRequestList = model.Select(e => new LeaveRequestResult
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
                        LeaveDuration = e.LeaveDuration,
                        NoOfHolidays = e.NoOfHolidays,
                        NoOfWeekends = e.NoOfWeekends,
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
                        ApproverNo = e.ApproverNo,
                        ApproverName = e.ApproverName
                    }).ToList();

                    if (leaveRequestList.Any())
                    {
                        foreach (var item in leaveRequestList)
                        {
                            #region Get the file attachments
                            var attachModel = await (from attach in _db.LeaveAttachments
                                                     where attach.LeaveAttachmentId == item.LeaveAttachmentId
                                                     select attach).ToListAsync();
                            if (attachModel != null)
                            {
                                item.AttachmentList = attachModel.Select(e => new LeaveAttachment
                                {
                                    Id = e.Id,
                                    LeaveAttachmentId = e.LeaveAttachmentId,
                                    FileName = e.FileName,
                                    StoredFileName = e.StoredFileName,
                                    ContentType = e.ContentType,
                                    FileSize = e.FileSize
                                }).ToList();
                            }
                            #endregion
                        }
                    }
                }

                return Result<List<LeaveRequestResult>>.SuccessResult(leaveRequestList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<LeaveRequestResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get leave entitlement details
        /// </summary>
        /// <param name="entitlementId"></param>
        /// <param name="empNo"></param>
        /// <param name="costCenter"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<Result<List<LeaveEntitlementResult>>> GetLeaveEntitlementAsync(
            int? entitlementId,
            int? empNo,
            string? costCenter,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<LeaveEntitlementResult> leaveEntitlementList = new();
            try
            {
                var model = await _db.Set<LeaveEntitlementResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetLeaveEntitlement @entitlementId = {0}, @empNo = {1}, @costCenter = {2}, @startDate = {3}, @endDate = {4}",
                    entitlementId!,
                    empNo!,
                    costCenter!,
                    startDate!,
                    endDate!)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    leaveEntitlementList = model.Select(e => new LeaveEntitlementResult
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

                return Result<List<LeaveEntitlementResult>>.SuccessResult(leaveEntitlementList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<LeaveEntitlementResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Add new leave entitlement
        /// </summary>
        /// <param name="entitlement"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<int>> AddLeaveEntitlementAsync(LeaveEntitlement entitlement, CancellationToken cancellationToken = default)
        {
            try
            {
                if (entitlement == null)
                    throw new ArgumentNullException(nameof(entitlement));

                // Save to database
                await _db.LeaveEntitlements.AddAsync(entitlement);
                await _db.SaveChangesAsync(cancellationToken);

                // ✅ EF Core automatically populates identity after SaveChanges
                int generatedId = entitlement.LeaveEntitlementId;

                return Result<int>.SuccessResult(generatedId);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update leave entitlement
        /// </summary>
        /// <param name="entitlement"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Result<int>> UpdateLeaveEntitlementAsync(LeaveEntitlement entitlement, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (entitlement == null)
                    throw new ArgumentNullException(nameof(entitlement));

                var existing = await _db.LeaveEntitlements
                    .FirstOrDefaultAsync(e =>
                        e.LeaveEntitlementId == entitlement.LeaveEntitlementId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        $"Could not find leave entitlement record for employee '{entitlement.Employee}'.");

                #region Update leave request information
                existing.LeaveUOM = entitlement.LeaveUOM;
                existing.DILBalance = entitlement.DILBalance;
                existing.LeaveBalance = entitlement.LeaveBalance;
                existing.ALEntitlementCount = entitlement.ALEntitlementCount;
                existing.ALRenewalType = entitlement.ALRenewalType;
                existing.SLBalance = entitlement.SLBalance;
                existing.SLEntitlementCount = entitlement.SLEntitlementCount;
                existing.SLRenewalType = entitlement.SLRenewalType;
                existing.SickLeaveUOM = entitlement.SickLeaveUOM;
                existing.EffectiveDate = entitlement.EffectiveDate;
                existing.LastUpdatedBy = entitlement.LastUpdatedBy;
                existing.LastUpdatedUserID = entitlement.LastUpdatedUserID;
                existing.LastUpdatedDate = entitlement.LastUpdatedDate;
                #endregion

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);
                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteLeaveEntitlementAsync(int entitlementID, CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;

            try
            {
                var recordToDelete = await _db.LeaveEntitlements.FindAsync(entitlementID);
                if (recordToDelete == null)
                    throw new Exception("Unable to delete because no matching record was found in the database.");

                _db.LeaveEntitlements.Remove(recordToDelete);

                int rowsDeleted = await _db.SaveChangesAsync(cancellationToken);
                if (rowsDeleted > 0)
                    isSuccess = true;

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Add new planned leave request
        /// </summary>
        public async Task<Result<long>> AddPlannedLeaveRequestAsync(
            PlannedLeaveRequest dto, 
            CancellationToken cancellationToken)
        {
            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                #region Initialize entity
                var leaveRequest = new PlannedLeaveRequest
                {
                    EmpNo = dto.EmpNo,
                    EmpName = dto.EmpName,
                    CostCenter = dto.CostCenter,
                    LeaveStartDate = Convert.ToDateTime(dto.LeaveStartDate),
                    LeaveEndDate = Convert.ToDateTime(dto.LeaveEndDate),
                    LeaveResumeDate = Convert.ToDateTime(dto.LeaveResumeDate),
                    StartDayMode = dto.StartDayMode,
                    EndDayMode = dto.EndDayMode,
                    LeaveDuration = dto.LeaveDuration,
                    NoOfHolidays = dto.NoOfHolidays,
                    NoOfWeekends = dto.NoOfWeekends,
                    Remarks = dto.Remarks,
                    CreatedBy = dto.CreatedBy,
                    CreatedByName = dto.CreatedByName,
                    CreatedEmail = dto.CreatedEmail,
                    CreatedUserID = dto.CreatedUserID,
                    CreatedDate = dto.CreatedDate,
                    StatusCode = dto.StatusCode,
                    StatusID = dto.StatusID,
                    StatusHandlingCode = dto.StatusHandlingCode
                };
                #endregion

                // Save to database
                await _db.PlannedLeaveRequest.AddAsync(leaveRequest);
                await _db.SaveChangesAsync(cancellationToken);

                // ✅ EF Core automatically populates identity after SaveChanges
                long generatedId = leaveRequest.PlannedLeaveId;

                return Result<long>.SuccessResult(generatedId);

            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<long>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<long>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update planned leave request
        /// </summary>
        public async Task<Result<int>> UpdatePlannedLeaveRequestAsync(
            PlannedLeaveRequest leaveRequest, 
            CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (leaveRequest == null)
                    throw new ArgumentNullException(nameof(leaveRequest));

                var existing = await _db.PlannedLeaveRequest
                    .FirstOrDefaultAsync(e =>
                        e.PlannedLeaveId == leaveRequest.PlannedLeaveId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find planned leave request with the specified request no.");

                #region Update leave request information
                existing.LeaveStartDate = leaveRequest.LeaveStartDate;
                existing.LeaveEndDate = leaveRequest.LeaveEndDate;
                existing.LeaveResumeDate = leaveRequest.LeaveResumeDate;
                existing.StartDayMode = leaveRequest.StartDayMode;
                existing.EndDayMode = leaveRequest.EndDayMode;
                existing.Remarks = leaveRequest.Remarks;
                existing.LastUpdatedBy = leaveRequest.LastUpdatedBy;
                existing.LastUpdatedName = leaveRequest.LastUpdatedName;
                existing.LastUpdatedUserID = leaveRequest.LastUpdatedUserID;
                existing.LastUpdatedEmail = leaveRequest.LastUpdatedEmail;
                existing.LastUpdatedDate = leaveRequest.LastUpdatedDate;
                #endregion

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);
                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Cancel planned leave request
        /// </summary>
        public async Task<Result<int>> CancelPlannedLeaveRequestAsync(
            PlannedLeaveRequest leaveRequest,
            CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (leaveRequest == null)
                    throw new ArgumentNullException(nameof(leaveRequest));

                var existing = await _db.PlannedLeaveRequest
                    .FirstOrDefaultAsync(e =>
                        e.PlannedLeaveId == leaveRequest.PlannedLeaveId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find planned leave request with the specified request no.");

                #region Update leave information to cancel the request
                existing.StatusCode = leaveRequest.StatusCode;
                existing.StatusID = leaveRequest.StatusID;
                existing.StatusHandlingCode = leaveRequest.StatusHandlingCode;
                existing.LastUpdatedBy = leaveRequest.LastUpdatedBy;
                existing.LastUpdatedName = leaveRequest.LastUpdatedName;
                existing.LastUpdatedUserID = leaveRequest.LastUpdatedUserID;
                existing.LastUpdatedEmail = leaveRequest.LastUpdatedEmail;
                existing.LastUpdatedDate = leaveRequest.LastUpdatedDate;
                #endregion

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);
                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get leave request details
        /// </summary>
        /// <param name="leaveRequestNo"></param>
        /// <returns></returns>
        public async Task<Result<PlannedLeaveResult>> GetPlannedLeaveAsync(long leaveRequestNo)
        {
            PlannedLeaveResult leaveRequest = new();

            try
            {
                var model = await _db.Set<PlannedLeaveResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetPlannedLeaveRequest @leaveNo = {0}",
                    leaveRequestNo)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    leaveRequest.PlannedLeaveId = model[0].PlannedLeaveId;
                    leaveRequest.LeaveNo = model[0].LeaveNo;
                    leaveRequest.EmpNo = model[0].EmpNo;
                    leaveRequest.EmpName = model[0].EmpName;
                    leaveRequest.LeaveStartDate = model[0].LeaveStartDate;
                    leaveRequest.LeaveEndDate = model[0].LeaveEndDate;
                    leaveRequest.LeaveResumeDate = model[0].LeaveResumeDate;
                    leaveRequest.StartDayMode = model[0].StartDayMode;
                    leaveRequest.StartDayModeDesc = model[0].StartDayModeDesc;
                    leaveRequest.EndDayMode = model[0].EndDayMode;
                    leaveRequest.EndDayModeDesc = model[0].EndDayModeDesc;
                    leaveRequest.CostCenter = model[0].CostCenter;
                    leaveRequest.CostCenterName = model[0].CostCenterName;
                    leaveRequest.Remarks = model[0].Remarks;
                    leaveRequest.LeaveDuration = model[0].LeaveDuration;
                    leaveRequest.NoOfHolidays = model[0].NoOfHolidays;
                    leaveRequest.NoOfWeekends = model[0].NoOfWeekends;
                    leaveRequest.HalfDayLeaveFlag = model[0].HalfDayLeaveFlag;
                    leaveRequest.StatusID = model[0].StatusID;
                    leaveRequest.StatusCode = model[0].StatusCode;
                    leaveRequest.StatusDesc = model[0].StatusDesc;
                    leaveRequest.StatusHandlingCode = model[0].StatusHandlingCode;
                    leaveRequest.CreatedDate = model[0].CreatedDate;
                    leaveRequest.CreatedBy = model[0].CreatedBy;
                    leaveRequest.CreatedByName = model[0].CreatedByName;
                    leaveRequest.CreatedUserID = model[0].CreatedUserID;
                    leaveRequest.CreatedEmail = model[0].CreatedEmail;
                    leaveRequest.LastUpdatedDate = model[0].LastUpdatedDate;
                    leaveRequest.LastUpdatedBy = model[0].LastUpdatedBy;
                    leaveRequest.LastUpdatedName = model[0].LastUpdatedName;
                    leaveRequest.LastUpdatedUserID = model[0].LastUpdatedUserID;
                    leaveRequest.LastUpdatedEmail = model[0].LastUpdatedEmail;
                }

                return Result<PlannedLeaveResult>.SuccessResult(leaveRequest);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<PlannedLeaveResult>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Get leave request details
        /// </summary>
        /// <param name="leaveRequestNo"></param>
        /// <param name="empNo"></param>
        /// <param name="costCenter"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="usedLeave"></param>
        /// <returns></returns>
        public async Task<Result<List<PlannedLeaveResult>>> SearchPlannedLeaveAsync(
            long? leaveRequestNo,
            int? empNo,
            string? costCenter,
            string? status,
            DateTime? startDate,
            DateTime? endDate,
            bool usedLeave)
        {
            List<PlannedLeaveResult> leaveRequestList = new();

            try
            {
                var model = (await _db.Set<PlannedLeaveResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetPlannedLeaveRequest @leaveNo = {0}, @empNo = {1}, @costCenter = {2}, @status = {3}, @startDate = {4}, @endDate = {5}, @usedLeave = {6}",
                    leaveRequestNo!,
                    empNo!,
                    costCenter!,
                    status!,
                    startDate!,
                    endDate!,
                    usedLeave!)
                    .ToListAsync()).AsEnumerable().OrderByDescending(a => a.PlannedLeaveId);
                if (model != null && model.Any())
                {
                    leaveRequestList = model.Select(e => new PlannedLeaveResult
                    {
                        PlannedLeaveId = e.PlannedLeaveId,
                        LeaveNo = e.LeaveNo,
                        EmpNo = e.EmpNo,
                        EmpName = e.EmpName,
                        LeaveStartDate = e.LeaveStartDate,
                        LeaveEndDate = e.LeaveEndDate,
                        LeaveResumeDate = e.LeaveResumeDate,
                        StartDayMode = e.StartDayMode,
                        StartDayModeDesc = e.StartDayModeDesc,
                        EndDayMode = e.EndDayMode,
                        EndDayModeDesc = e.EndDayModeDesc,
                        CostCenter = e.CostCenter,
                        CostCenterName = e.CostCenterName,
                        Remarks = e.Remarks,
                        LeaveDuration = e.LeaveDuration,
                        NoOfHolidays = e.NoOfHolidays,
                        NoOfWeekends = e.NoOfWeekends,
                        HalfDayLeaveFlag = e.HalfDayLeaveFlag,
                        StatusID = e.StatusID,
                        StatusCode = e.StatusCode,
                        StatusDesc = e.StatusDesc,
                        StatusHandlingCode = e.StatusHandlingCode,
                        CreatedDate = e.CreatedDate,
                        CreatedBy = e.CreatedBy,
                        CreatedByName = e.CreatedByName,
                        CreatedUserID = e.CreatedUserID,
                        CreatedEmail = e.CreatedEmail,
                        LastUpdatedDate = e.LastUpdatedDate,
                        LastUpdatedBy = e.LastUpdatedBy,
                        LastUpdatedName = e.LastUpdatedName,
                        LastUpdatedUserID = e.LastUpdatedUserID,
                        LastUpdatedEmail = e.LastUpdatedEmail
                    }).ToList();
                }

                return Result<List<PlannedLeaveResult>>.SuccessResult(leaveRequestList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<PlannedLeaveResult>>.Failure($"Database error: {ex.Message}");
            }
        }
        #endregion
    }
}
