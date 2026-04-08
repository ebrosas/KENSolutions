using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public class WorkflowRepository : IWorkflowRepository
    {
        #region Fields
        private readonly AppDbContext _db;
        #endregion

        #region Constructors                
        public WorkflowRepository(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Public methods     
        public async Task<Result<List<RequestTypeResult>>> GetPendingRequestAsync(
            int empNo,
            string? requestType,
            byte? periodType,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<RequestTypeResult> requestTypeList = new();

            try
            {
                var model = await _db.Set<RequestTypeResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetPendingRequest @empNo = {0}, @requestType = {1}, @periodType = {2}, @startDate = {3}, @endDate = {4}",
                    empNo,
                    requestType!,
                    periodType!,
                    startDate!,
                    endDate!)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    requestTypeList = model.Select(e => new RequestTypeResult
                    {
                        RequestTypeCode = e.RequestTypeCode,
                        RequestTypeName = e.RequestTypeName,
                        RequestTypeDesc = e.RequestTypeDesc,
                        IconName = e.IconName,
                        AssignedCount = e.AssignedCount
                    }).ToList();
                }

                return Result<List<RequestTypeResult>>.SuccessResult(requestTypeList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<RequestTypeResult>>.Failure($"Database error: {ex.Message}");
            }
        }
        #endregion
    }
}
