using KenHRApp.Application.DTOs;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IWorkflowService
    {
        Task<Result<List<RequestTypeDTO>>> GetPendingRequestAsync(
            int empNo,
            string? requestType,
            byte? periodType,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<bool>> StartWorkflowAsync(
            string entityName,
            long entityId,
            string webRootPath,
            int originatorEmpNo,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> ApproveStepAsync(
            int stepInstanceId,
            int approverEmpNo,
            string? approverUserID,
            string? comments,
            string entityName,
            long entityId,
            string webRootPath,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> RejectStepAsync(
            int stepInstanceId,
            int? creatorEmpNo,
            int approverEmpNo,
            string? approverUserID,
            string rejectionReason,
            string entityName,
            long entityId,
            string webRootPath,
            CancellationToken cancellationToken = default);

        Task<Result<List<WorkflowDetailResultDTO>>> GetWorkflowStatusAsync(
            string workflowTypeCode,
            long requestNo);

        Task<Result<List<ApprovalRequestResultDTO>>> GetApprovalRequestAsync(
            byte searchType,
            int? empNo,
            string? requestType);
    }
}
