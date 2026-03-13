using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            // Implement logic to check if the given date is a public holiday
            return Task.FromResult(false); // Placeholder implementation
        }

        public Task<bool> IsDayOffAsync(int employeeId, DateTime date)
        {
            // Implement logic to check if the given date is a day off for the specified employee
            return Task.FromResult(false); // Placeholder implementation
        }
        #endregion

        #region Abstract methods
        /// <summary>
        /// Add new leave request
        /// </summary>
        public async Task<Result<int>> AddLeaveRequestAsync(LeaveRequisitionWF leaveRequest, CancellationToken cancellationToken)
        {
            int rowsInserted = 0;

            try
            {
                // Save to database
                _db.LeaveRequisitionWFs.Add(leaveRequest);
                rowsInserted = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsInserted);

            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Database error: {ex.Message}");
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

                #region Update leave request information to cancel the request
                existing.LeaveStatusCode = leaveRequest.LeaveStatusCode;
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
                        JobTitle = e.JobTitle
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

                var query = _db.Holidays.AsQueryable()
                            .Where(h => h.HolidayDate.Date == leaveDate.Value.Date &&
                            h.HolidayType == 1);

                var model = await query.FirstOrDefaultAsync();
                if (model != null)
                    result = true;

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
