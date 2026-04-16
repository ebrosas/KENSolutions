using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
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
        #endregion

        #region Constructors
        public WorkflowEmailService(
            IWorkflowRepository wfRepository,
            ILeaveRequestRepository leaveRepository,
            IEmailService emailService)
        {
            _wfRepository = wfRepository;
            _leaveRepository = leaveRepository;
            _emailService = emailService;
        }
        #endregion

        #region Public Methods
        public async Task<Result<bool>> SendAsync(
            int userId, 
            string subject,
            string requestTypeDesc,
            string requestLink,
            long requestID,
            CancellationToken cancellationToken = default)
        {
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

                #region Build the email contents
                string message = $@"
                                Dear {user!.FirstName},

                                {requestTypeDesc} No. {requestID} has been assigned to you for approval. 

                                Please take action on the assigned request by clicking the link below:

                                {requestLink}

                                Note that if you had taken action already on this request, please ignore this email.

                                Regards,
                                KEN-HR Team";
                #endregion

                await _emailService.SendAsync(
                   user!.OfficialEmail,
                   subject,
                   message,
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
