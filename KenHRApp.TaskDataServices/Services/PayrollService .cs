using KenHRApp.TaskDataServices.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskDataServices.Services
{
    public class PayrollService : IJobService
    {
        #region Fields
        private readonly ILogger<PayrollService> _logger;
        private readonly PayrollSettings _settings;

        public string ExecutionKey => _settings.ExecutionKey;
        #endregion

        #region Constructors
        public PayrollService(
            ILogger<PayrollService> logger,
            IOptions<PayrollSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }
        #endregion

        #region Public Methods
        public async Task ExecuteAsync()
        {
            _logger.LogInformation(
                "Executing Payroll Service");

            await Task.Delay(1000);

            _logger.LogInformation(
                "Payroll Service completed.");
        }
        #endregion
    }
}
