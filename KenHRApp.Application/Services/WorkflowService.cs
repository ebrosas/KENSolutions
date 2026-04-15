using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities.Workflow;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using KenHRApp.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace KenHRApp.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        #region Fields
        private readonly IWorkflowRepository _repository;
        private readonly IWorkflowEmailService _emailService;
        private readonly AppSettings _settings;
        #endregion

        #region Contructors
        public WorkflowService(
            IWorkflowRepository repository, 
            IWorkflowEmailService emailService,
            IOptions<AppSettings> settings)
        {
            _repository = repository;
            _emailService = emailService;
            _settings = settings.Value;
        }
        #endregion

        #region Public Methods     
        public async Task<Result<List<RequestTypeDTO>>> GetPendingRequestAsync(
            int empNo,
            string? requestType,
            byte? periodType,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<RequestTypeDTO> requestTypeList = new();
            
            try
            {
                var repoResult = await _repository.GetPendingRequestAsync(empNo, requestType, periodType, startDate, endDate);
                if (!repoResult.Success)
                {
                    return Result<List<RequestTypeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    requestTypeList = model.Select(e => new RequestTypeDTO
                    {
                        RequestTypeCode = e.RequestTypeCode,
                        RequestTypeName = e.RequestTypeName,
                        RequestTypeDesc = e.RequestTypeDesc,
                        IconName = e.IconName,
                        AssignedCount = e.AssignedCount
                    }).ToList();
                }

                return Result<List<RequestTypeDTO>>.SuccessResult(requestTypeList);
            }
            catch (Exception ex)
            {
                return Result<List<RequestTypeDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching request types from the database.");
            }
        }

        public async Task<Result<int>> StartWorkflowAsync(string entityName, long entityId)
        {
            int workflowInstanceId = 0;

            try
            {
                var repoResult = await _repository.StartWorkflowAsync(entityName, entityId);
                if (!repoResult.Success)
                {
                    return Result<int>.Failure(repoResult.Error ?? "Unknown repository error");
                }
                else
                {
                    workflowInstanceId = repoResult.Value;
                }
                
                return Result<int>.SuccessResult(workflowInstanceId);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString() ?? "Unknown error while executing StartWorkflowAsync() method.");
            }
        }

        public async Task<Result<bool>> ApproveStepAsync(int stepInstanceId, int approverEmpNo, string? approverUserID, string? comments)
        {
            bool isSuccess = false;

            try
            {
                var repoResult = await _repository.ApproveStepAsync(stepInstanceId, approverEmpNo, approverUserID, comments);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }
                else
                {
                    isSuccess = repoResult.Value;
                }

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error while executing ApproveStepAsync() method.");
            }
        }

        public async Task<Result<bool>> RejectStepAsync(int stepInstanceId, int approverEmpNo, string? approverUserID, string comments)
        {
            bool isSuccess = false;

            try
            {
                var repoResult = await _repository.RejectStepAsync(stepInstanceId, approverEmpNo, approverUserID, comments);
                if (!repoResult.Success)
                {
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                }
                else
                {
                    isSuccess = repoResult.Value;
                }

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error while executing RejectStepAsync() method.");
            }
        }
        #endregion

        #region Private Methods
        public async Task<Result<bool>> SendPendingApprovalAsync(
            string email,
            int empNo,
            DateTime doj,
            CancellationToken cancellationToken = default)
        {
            try
            {
                int approverEmpNo = 0;

                //var token = await GenerateEmailVerificationTokenAsync(
                //    empNo,
                //    doj,
                //    cancellationToken);

                //if (string.IsNullOrEmpty(token))
                //    return Result<bool>.Failure("Unable to generate email verification token!");

                var baseUrl = _settings.BaseUrl.TrimEnd('/');

                //return $"{baseUrl}/UserAccount/VerifyEmail?token={Uri.EscapeDataString(token)}";
                //Navigation.NavigateTo($"/TimeAttendance/leaverequest?ActionType=View&LeaveRequestNo={item.LeaveRequestId}&CallerForm=LeaveInquiry");

                var requestLink = $"{baseUrl}/TimeAttendance/leaverequest?ActionType=View";

                #region Build the email contents
                var subject = "Verify your KenHR account";

                var body = $@"
                    Hello,

                    Thank you for registering with KenHR.

                    Please verify your email by clicking the link below:

                    {requestLink}

                    Note that the verification link is valid for 24 hours only. If you did not initiate this request, please ignore this email.

                    Regards,
                    KenHR Team";
                #endregion

                await _emailService.SendAsync(approverEmpNo, subject, body, cancellationToken);

                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in SendVerificationEmailAsync() method.");
            }
        }
        #endregion
    }
}
