using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IWorkflowEmailService
    {
        #region Public Methods
        Task<Result<bool>> SendApprovalNotificationAsync(
            int userId,
            string subject,
            string requestTypeDesc,
            string requestLink,
            long requestID,
            string webRootPath,
            string fileName,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> SendRejectionNotificationAsync(
            int userId,
            string subject,
            string requestTypeDesc,
            string requestLink,
            long requestID,
            string webRootPath,
            string fileName,
            string rejectionReason,
            CancellationToken cancellationToken = default);

        Task<Result<bool>> SendAsync(
            long requestID,
            string requestTypeCode,
            string subject,
            string message,
            CancellationToken cancellationToken = default);
        #endregion
    }
}
