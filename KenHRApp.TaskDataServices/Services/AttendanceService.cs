using KenHRApp.TaskDataServices.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskDataServices.Services
{
    public class AttendanceService : IAttendanceService
    {
        #region Fields
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AttendanceService> _logger;
        #endregion

        #region Constructors
        public AttendanceService(
            ApplicationDbContext dbContext,
            ILogger<AttendanceService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public async Task ExecuteAttendanceGenerationAsync(DateTime attendanceDate)
        {
            try
            {
                _logger.LogInformation(
                    "Starting attendance generation for {AttendanceDate}",
                    attendanceDate);

                var parameter = new SqlParameter(
                    "@AttendanceDate",
                    attendanceDate);

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC kenuser.Pr_GenerateAttendance @AttendanceDate",
                    parameter);

                _logger.LogInformation(
                    "Attendance generation completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error executing attendance generation.");

                throw;
            }
        }
        #endregion
    }
}
