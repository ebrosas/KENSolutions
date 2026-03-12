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
        public async Task<Result<int>> AddLeaveRequestAsync(LeaveRequisitionWF entity, CancellationToken cancellationToken)
        {
            int rowsUpdated = 0;

            try
            {
                // Add new leave request entity
                var newShiftRoster = new LeaveRequisitionWF
                {
                    LeaveEmpNo = entity.LeaveEmpNo,
                    LeaveEmpName = entity.LeaveEmpName
                };

                // Save to database
                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);

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
        #endregion
    }
}
