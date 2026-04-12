using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities.Workflow;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public async Task<Result<bool>> ApproveStepAsync(int stepInstanceId, int userId, string? comments)
        {
            bool isSuccess = false;

            try
            {
                var repoResult = await _repository.ApproveStepAsync(stepInstanceId, userId, comments);
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

        public async Task<Result<bool>> RejectStepAsync(int stepInstanceId, int userId, string comments)
        {
            bool isSuccess = false;

            try
            {
                var repoResult = await _repository.RejectStepAsync(stepInstanceId, userId, comments);
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
    }
}
