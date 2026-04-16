using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Entities.Workflow;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public interface IWorkflowRepository
    {
        #region Workflow Abstract Methods
        Task<Result<List<int>>> StartWorkflowAsync(string entityName, long entityId);
        Task<Result<bool>> ApproveStepAsync(int stepInstanceId, int approverEmpNo, string? approverUserID, string? comments);
        Task<Result<bool>> RejectStepAsync(int stepInstanceId, int approverEmpNo, string? approverUserID, string comments);
        #endregion

        #region Database Access Abstract Methods
        Task<Result<List<RequestTypeResult>>> GetPendingRequestAsync(
            int empNo,
            string? requestType,
            byte? periodType,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<Employee?>> GetEmployeeInfoAsync(
            int? empNo,
            CancellationToken cancellationToken = default);
        #endregion
    }
}
