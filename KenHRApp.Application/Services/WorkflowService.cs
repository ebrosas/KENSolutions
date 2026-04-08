using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        #region Fields
        private readonly IWorkflowRepository _repository;
        #endregion

        #region Contructors
        public WorkflowService(IWorkflowRepository repository)
        {
            _repository = repository;
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
        #endregion
    }
}
