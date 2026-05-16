using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskDataServices.Configurations
{
    public class EmailSettings
    {
        #region Properties
        public string SmtpServer { get; set; } = string.Empty;
        public int Port { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string SenderEmail { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string AdminEmail { get; set; } = string.Empty;
        #endregion
    }
}
