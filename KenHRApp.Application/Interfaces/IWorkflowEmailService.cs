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
        #region Abstract Methods
        Task<Result<bool>> SendAsync(
            int userId,
            string subject,
            string message,
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
