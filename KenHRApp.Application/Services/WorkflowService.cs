using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities.Workflow;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using KenHRApp.Infrastructure.Settings;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace KenHRApp.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        #region Fields
        private readonly IWorkflowRepository _repository;
        private readonly IWorkflowEmailService _emailService;
        private readonly AppSettings _settings;

        #region Constants
        private readonly string CONST_GENERIC_MESSAGE = "GenericMessage.xml";
        #endregion

        #endregion

        #region Enums
        private enum WorkflowRequestType
        {
            NotDefined,
            LeaveRequisition,
            Overtime,
            Regularization,
            TravelRequest,
            ExpenseClaim,
            RecruitmentOffer
        }
        #endregion

        #region Properties
        private WorkflowRequestType RequestType { get; set; } = WorkflowRequestType.NotDefined;
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

        public async Task<Result<bool>> StartWorkflowAsync(
            string entityName, 
            long entityId,
            string webRootPath,
            CancellationToken cancellationToken = default)
        {
            try
            {
                string baseUrl = _settings.BaseUrl.TrimEnd('/');
                string subject = "Pending Approval Request";
                string requestTypeDesc = string.Empty;
                string requestLink = string.Empty;

                if (!string.IsNullOrWhiteSpace(entityName))
                {
                    #region Identity the type of request
                    switch (entityName)
                    {
                        case "RTYPELEAVE":
                            this.RequestType = WorkflowRequestType.LeaveRequisition;
                            break;
                    }
                    #endregion
                }

                var repoResult = await _repository.StartWorkflowAsync(entityName, entityId);
                if (!repoResult.Success)
                {
                    throw new Exception(repoResult.Error ?? "Unknown repository error");
                }
                else
                {
                    List<int> approverList = repoResult.Value!;
                    if (approverList != null && approverList.Any())
                    {
                        if (this.RequestType == WorkflowRequestType.LeaveRequisition)
                        {
                            #region Build the email parameters
                            subject = "Leave Request for Approval";
                            requestTypeDesc = "Leave Requisition";
                            requestLink = $"{baseUrl}/TimeAttendance/leaverequest?ActionType=View&LeaveRequestNo={entityId}&CallerForm=LeaveInquiry";
                            #endregion

                            foreach (int approver in approverList)
                            {
                                await SendPendingApprovalAsync(approver, subject, requestTypeDesc, requestLink, entityId, webRootPath, cancellationToken);
                            }
                        }
                    }
                }
                
                return Result<bool>.SuccessResult(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error while executing StartWorkflowAsync() method.");
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

        public async Task<Result<List<WorkflowDetailResultDTO>>> GetWorkflowStatusAsync(
            string workflowTypeCode,
            long requestNo)
        {
            List<WorkflowDetailResultDTO> requestTypeList = new();

            try
            {
                var repoResult = await _repository.GetWorkflowStatusAsync(workflowTypeCode, requestNo);
                if (!repoResult.Success)
                {
                    return Result<List<WorkflowDetailResultDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    requestTypeList = model.Select(e => new WorkflowDetailResultDTO
                    {
                        RequestNo = e.RequestNo,
                        WorkflowType = e.WorkflowType,
                        WorkflowStatus = e.WorkflowStatus,
                        ActivityID = e.ActivityID,
                        ActivityName = e.ActivityName,
                        ActivityOrder = e.ActivityOrder,
                        ActivityStatus = e.ActivityStatus,
                        ApproverNo = e.ApproverNo,
                        ApproverName = e.ApproverName
                    }).ToList();
                }

                return Result<List<WorkflowDetailResultDTO>>.SuccessResult(requestTypeList);
            }
            catch (Exception ex)
            {
                return Result<List<WorkflowDetailResultDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching request types from the database.");
            }
        }

        public async Task<Result<List<ApprovalRequestResultDTO>>> GetApprovalRequestAsync(
            int empNo,
            string requestType)
        {
            List<ApprovalRequestResultDTO> requestTypeList = new();

            try
            {
                var repoResult = await _repository.GetApprovalRequestAsync(empNo, requestType);
                if (!repoResult.Success)
                {
                    return Result<List<ApprovalRequestResultDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    requestTypeList = model.Select(e => new ApprovalRequestResultDTO
                    {
                        RequestNo = e.RequestNo,
                        RequestTypeCode = e.RequestTypeCode,
                        RequestTypeDesc = e.RequestTypeDesc,
                        AppliedDate = e.AppliedDate,
                        RequestedByNo = e.RequestedByNo,
                        RequestedByName = e.RequestedByName,
                        Detail = e.Detail,
                        ApprovalRole = e.ApprovalRole,
                        CurrentStatus = e.CurrentStatus,
                        ApproverNo = e.ApproverNo,
                        ApproverName = e.ApproverName,
                        PendingDays = e.PendingDays
                    }).ToList();
                }

                return Result<List<ApprovalRequestResultDTO>>.SuccessResult(requestTypeList);
            }
            catch (Exception ex)
            {
                return Result<List<ApprovalRequestResultDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching request types from the database.");
            }
        }
        #endregion

        #region Private Methods
        private async Task<Result<bool>> SendPendingApprovalAsync(
            int approverEmpNo,
            string subject,
            string requestTypeDesc,
            string requestLink,
            long requestID,
            string webRootPath,
            CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;
            try
            {
                var repoResult = await _emailService.SendAsync(approverEmpNo, subject, requestTypeDesc, 
                    requestLink, requestID, webRootPath, CONST_GENERIC_MESSAGE, cancellationToken);
                if (!repoResult.Success)
                    return Result<bool>.Failure(repoResult.Error ?? "Unknown repository error");
                else
                    isSuccess = repoResult.Value;

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString() ?? "Unknown error in SendPendingApprovalAsync() method.");
            }
        }
        #endregion
    }
}
