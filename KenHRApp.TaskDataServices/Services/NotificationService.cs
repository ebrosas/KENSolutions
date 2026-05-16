using KenHRApp.TaskDataServices.Configurations;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.TaskDataServices.Services
{
    public class NotificationService : INotificationService
    {
        #region MyRegion
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<AttendanceService> _logger;
        #endregion

        #region Constructor
        public NotificationService(
            IOptions<EmailSettings> options,
            ILogger<AttendanceService> logger)
        {
            _emailSettings = options.Value;
            _logger = logger;
        }
        #endregion


        public async Task SendSuccessNotificationAsync(DateTime attendanceDate)
        {
            string subject = "Attendance Generation Successful";

            string body = $@"
                    Attendance generation completed successfully.

                    Attendance Date: {attendanceDate:yyyy-MM-dd}
                    Execution Time : {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                    Machine Name   : {Environment.MachineName}
                    ";

            await SendEmailAsync(subject, body);
        }

        public async Task SendFailureNotificationAsync(Exception ex)
        {
            string subject = "Attendance Generation Failed";

            string body = $@"
                Attendance generation failed.

                Execution Time : {DateTime.Now:yyyy-MM-dd HH:mm:ss}
                Machine Name   : {Environment.MachineName}

                Error Details:
                {ex}
                ";

            await SendEmailAsync(subject, body);
        }

        private async Task SendEmailAsync(string subject, string body)
        {
            try
            {
                var email = new MimeMessage();

                if (!MailboxAddress.TryParse(_emailSettings.SenderEmail, out _))
                {
                    throw new Exception("Invalid SenderEmail format.");
                }

                email.From.Add(
                    new MailboxAddress(
                        _emailSettings.SenderName,
                        _emailSettings.SenderEmail));

                if (!MailboxAddress.TryParse(_emailSettings.AdminEmail, out _))
                {
                    throw new Exception("Invalid AdminEmail format.");
                }

                email.To.Add(
                    new MailboxAddress(
                        "System Administrator",
                        _emailSettings.AdminEmail));

                email.Subject = subject;

                email.Body = new TextPart("plain")
                {
                    Text = body
                };

                using var smtp = new SmtpClient();

                await smtp.ConnectAsync(
                    _emailSettings.SmtpServer,
                    _emailSettings.Port,
                    false);

                await smtp.AuthenticateAsync(
                    _emailSettings.Username,
                    _emailSettings.Password);

                await smtp.SendAsync(email);

                await smtp.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error executing SendEmailAsync() method.");

                throw;
            }
        }
    }
}
