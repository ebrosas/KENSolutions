using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
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
        public async Task<Result<int>> CancelLeaveRequestAsync(LeaveRequisitionWF leaveRequest, CancellationToken cancellationToken = default)
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
        #endregion
    }
}
