using KenHRApp.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class SmtpEmailService : IEmailService
    {
        #region Fields
        private readonly IConfiguration _configuration;
        private readonly ILogger<SmtpEmailService> _logger;
        #endregion

        #region Constructor 
        public SmtpEmailService(
            IConfiguration configuration,
            ILogger<SmtpEmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        #endregion

        #region Public Methods
        public async Task SendAsync(
        string to,
        string subject,
        string body,
        bool isHtml = false,
        CancellationToken cancellationToken = default)
        {
            try
            {
                var smtpSection = _configuration.GetSection("Smtp");

                using var client = new SmtpClient(smtpSection["Host"])
                {
                    Port = int.Parse(smtpSection["Port"]!),
                    Credentials = new NetworkCredential(
                        smtpSection["Username"],
                        smtpSection["Password"]),
                    EnableSsl = true
                };

                using var message = new MailMessage
                {
                    From = new MailAddress(smtpSection["Username"]!),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = isHtml
                };

                message.To.Add(to);

                await client.SendMailAsync(message, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Email}", to);
                throw;
            }
        }
        #endregion
    }
}
