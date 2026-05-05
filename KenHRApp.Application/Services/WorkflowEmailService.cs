using KenHRApp.Application.Common.Helpers;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class WorkflowEmailService : IWorkflowEmailService
    {
        #region Fields
        private readonly IWorkflowRepository _wfRepository;
        private readonly ILeaveRequestRepository _leaveRepository;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        #endregion

        #region Constructors
        public WorkflowEmailService(
            IWorkflowRepository wfRepository,
            ILeaveRequestRepository leaveRepository,
            IEmailService emailService,
            IConfiguration configuration)
        {
            _wfRepository = wfRepository;
            _leaveRepository = leaveRepository;
            _emailService = emailService;
            _configuration = configuration;
        }
        #endregion

        #region Public Methods
        public async Task<Result<bool>> NotifyOriginatorAsync(
            int originatorEmpNo,
            string subject,
            string requestTypeDesc,
            string requestLink,
            long requestID,
            string webRootPath,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            StringBuilder sb = new StringBuilder();
            string adminName = "System Administrator";

            try
            {
                var repoResult = await _wfRepository.GetEmployeeInfoAsync(originatorEmpNo, cancellationToken);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var user = repoResult.Value;
                if (user == null)
                {
                    throw new Exception("Employee not found.");
                }
                else if (user != null && string.IsNullOrWhiteSpace(user.OfficialEmail))
                {
                    throw new Exception("Employee email is not defined.");
                }

                #region Initialize the physical file path of the message templeate
                string msgTemplatePath = Path.Combine(
                    webRootPath,
                    "messages");

                if (!Directory.Exists(msgTemplatePath))
                    Directory.CreateDirectory(msgTemplatePath);
                #endregion

                #region Initialize email sender 
                var smtpSection = _configuration.GetSection("Smtp");
                if (smtpSection != null)
                {
                    adminName = smtpSection["AdminName"]!.ToString();
                }
                #endregion

                #region Build the email contents
                sb.Clear();
                sb.AppendLine($"{requestTypeDesc} No. <b>{requestID}</b> has been submitted successfully.");

                // Build the message body
                string body = String.Empty;
                string htmLBody = string.Empty;
                string fullPath = string.Empty;

                if (!string.IsNullOrEmpty(fileName))
                {
                    fullPath = Path.Combine(msgTemplatePath, fileName);
                    body = String.Format(ServiceHelper.RetrieveXmlMessage(fullPath),
                        user!.EmployeeFullName,
                        sb.ToString(),
                        requestLink,
                        adminName
                        ).Replace("&lt;", "<").Replace("&gt;", ">");
                }
                else
                {
                    body = $@"
                            Dear {user!.FirstName},

                            {requestTypeDesc} No. {requestID} has been submitted successfuly. 

                            You may view the request by clicking the below link:

                            {requestLink}
                            
                            Regards,
                            KEN-HR Team";
                }

                // Format the message contents
                htmLBody = string.Format("<HTML><BODY><p>{0}</p></BODY></HTML>", body);
                #endregion

                await _emailService.SendAsync(
                   user!.OfficialEmail,
                   subject,
                   htmLBody,
                   true,
                   cancellationToken);

                return Result<bool>.SuccessResult(true);

            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in SendAsync() method.");
            }
        }

        public async Task<Result<bool>> SendApprovalNotificationAsync(
            int userId, 
            string subject,
            string requestTypeDesc,
            string requestLink,
            long requestID,
            string webRootPath,
            string fileName,
            CancellationToken cancellationToken = default)
        {
            StringBuilder sb = new StringBuilder();
            string adminName = "System Administrator";

            try
            {
                var repoResult = await _wfRepository.GetEmployeeInfoAsync(userId, cancellationToken);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var user = repoResult.Value;
                if (user == null)
                {
                    throw new Exception("Employee not found.");
                }
                else if (user != null && string.IsNullOrWhiteSpace(user.OfficialEmail))
                {
                    throw new Exception("Employee email is not defined.");
                }                               

                #region Initialize the physical file path of the message templeate
                string msgTemplatePath = Path.Combine(
                    webRootPath,
                    "messages");

                if (!Directory.Exists(msgTemplatePath))
                    Directory.CreateDirectory(msgTemplatePath);
                #endregion

                #region Initialize email sender 
                var smtpSection = _configuration.GetSection("Smtp");
                if (smtpSection != null)
                {
                    adminName = smtpSection["AdminName"]!.ToString();
                }
                #endregion

                #region Build the email contents
                sb.Clear();
                sb.AppendLine($"{requestTypeDesc} No. <b>{requestID}</b> has been assigned to you for approval. ");
                sb.Append("Please take necessary action and update the system.");

                // Build the message body
                string body = String.Empty;
                string htmLBody = string.Empty;
                string fullPath = string.Empty;

                if (!string.IsNullOrEmpty(fileName))
                {
                    fullPath = Path.Combine(msgTemplatePath, fileName);
                    body = String.Format(ServiceHelper.RetrieveXmlMessage(fullPath),
                        user!.EmployeeFullName,
                        sb.ToString(),
                        requestLink,
                        adminName
                        ).Replace("&lt;", "<").Replace("&gt;", ">");
                }
                else
                {
                    body = $@"
                            Dear {user!.FirstName},

                            {requestTypeDesc} No. {requestID} has been assigned to you for approval. 

                            Please take action on the assigned request by clicking the link below:

                            {requestLink}

                            Note that if you had taken action already on this request, please ignore this email.

                            Regards,
                            KEN-HR Team";
                }

                // Format the message contents
                htmLBody = string.Format("<HTML><BODY><p>{0}</p></BODY></HTML>", body);
                #endregion

                await _emailService.SendAsync(
                   user!.OfficialEmail,
                   subject,
                   htmLBody,
                   true,
                   cancellationToken);

                return Result<bool>.SuccessResult(true);

            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in SendAsync() method.");
            }
        }

        public async Task<Result<bool>> SendRejectionNotificationAsync(
            int userId,
            string subject,
            string requestTypeDesc,
            string requestLink,
            long requestID,
            string webRootPath,
            string fileName,
            string rejectionReason,
            CancellationToken cancellationToken = default)
        {
            StringBuilder sb = new StringBuilder();
            string adminName = "System Administrator";

            try
            {
                var repoResult = await _wfRepository.GetEmployeeInfoAsync(userId, cancellationToken);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var user = repoResult.Value;
                if (user == null)
                {
                    throw new Exception("Employee not found.");
                }
                else if (user != null && string.IsNullOrWhiteSpace(user.OfficialEmail))
                {
                    throw new Exception("Employee email is not defined.");
                }

                #region Initialize the physical file path of the message templeate
                string msgTemplatePath = Path.Combine(
                    webRootPath,
                    "messages");

                if (!Directory.Exists(msgTemplatePath))
                    Directory.CreateDirectory(msgTemplatePath);
                #endregion

                #region Initialize email sender 
                var smtpSection = _configuration.GetSection("Smtp");
                if (smtpSection != null)
                {
                    adminName = smtpSection["AdminName"]!.ToString();
                }
                #endregion

                #region Build the email contents
                sb.Clear();
                sb.AppendLine($"{requestTypeDesc} No. <b>{requestID}</b> has been rejected due to the following reasons: ");
                sb.AppendLine();
                sb.Append($"<p style=\"color: red; font-weight: bold;\">{rejectionReason}</p>");

                // Build the message body
                string body = String.Empty;
                string htmLBody = string.Empty;
                string fullPath = string.Empty;

                if (!string.IsNullOrEmpty(fileName))
                {
                    fullPath = Path.Combine(msgTemplatePath, fileName);
                    body = String.Format(ServiceHelper.RetrieveXmlMessage(fullPath),
                        user!.EmployeeFullName,
                        sb.ToString(),
                        requestLink,
                        adminName
                        ).Replace("&lt;", "<").Replace("&gt;", ">");
                }
                else
                {
                    body = $@"
                            Dear {user!.FirstName},

                            {requestTypeDesc} No. {requestID} has been rejected due to the following reasons: 

                            You may view the request by clicking the following link:

                            {requestLink}

                            Regards,
                            KEN-HR Team";
                }

                // Format the message contents
                htmLBody = string.Format("<HTML><BODY><p>{0}</p></BODY></HTML>", body);
                #endregion

                await _emailService.SendAsync(
                   user!.OfficialEmail,
                   subject,
                   htmLBody,
                   true,
                   cancellationToken);

                return Result<bool>.SuccessResult(true);

            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in SendAsync() method.");
            }
        }

        public async Task<Result<bool>> SendAsync(
            long requestID, 
            string requestTypeCode,
            string subject, 
            string message, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                string? emailRecipient = null;

                if (requestTypeCode == "")
                {
                    var repoResult = await _leaveRepository.GetLeaveRequestAsync(requestID);
                    if (!repoResult.Success)
                    {
                        return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                    }

                    var leaveRequest = repoResult.Value;
                    if (leaveRequest == null)
                        throw new Exception("Leave request not found.");
                    else
                        emailRecipient = leaveRequest.LeaveEmpEmail;
                }

                if (!string.IsNullOrWhiteSpace(emailRecipient))
                {
                    await _emailService.SendAsync(
                       emailRecipient,
                       subject,
                       message,
                       true);

                    return Result<bool>.SuccessResult(true);
                }

                return Result<bool>.SuccessResult(false);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in SendAsync() method.");
            }
        }
        #endregion
    }
}
