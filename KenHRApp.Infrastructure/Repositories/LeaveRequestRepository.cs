using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
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
        #endregion
    }
}
