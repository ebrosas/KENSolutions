using KenHRApp.TaskDataServices.Configurations;
using KenHRApp.TaskDataServices.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskDataServices.Services
{
    public class AttendanceService : IJobService
    {
        #region Fields
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<AttendanceService> _logger;
        private readonly AttendanceSettings _settings;
        private readonly INotificationService _notificationService;

        public string ExecutionKey => _settings.ExecutionKey;
        #endregion

        #region Constructors
        public AttendanceService(
            ApplicationDbContext dbContext,
            ILogger<AttendanceService> logger,
            IOptions<AttendanceSettings> settings,
            INotificationService notificationService)
        {
            _dbContext = dbContext;
            _logger = logger;
            _settings = settings.Value;
            _notificationService = notificationService;
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

        public async Task ExecuteAsync()
        {
            try
            {
                DateTime attendanceDate;

                if (!string.IsNullOrWhiteSpace(_settings.AttendanceDate))
                {
                    if (!DateTime.TryParse(
                            _settings.AttendanceDate,
                            out attendanceDate))
                    {
                        throw new Exception(
                            "Invalid AttendanceDate format.");
                    }

                    attendanceDate = attendanceDate.Date;
                }
                else
                {
                    attendanceDate = DateTime.Today
                        .AddDays(_settings.ExecutionDateOffset)
                        .Date;
                }

                _logger.LogInformation(
                    "Executing Attendance Service for {AttendanceDate}",
                    attendanceDate);

                var parameter = new SqlParameter(
                    "@AttendanceDate",
                    attendanceDate);

                await _dbContext.Database.ExecuteSqlRawAsync(
                    "EXEC kenuser.Pr_GenerateAttendance @AttendanceDate",
                    parameter);

                _logger.LogInformation(
                    "Attendance generation completed successfully.");

                await _notificationService
                    .SendSuccessNotificationAsync(attendanceDate);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Attendance execution failed.");

                await _notificationService
                    .SendFailureNotificationAsync(ex);

                throw;
            }
        }
        #endregion
    }
}
