using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class RecruitmentService : IRecruitmentService
    {
        #region Fields
        private readonly IRecruitmentRepository _repository;
        #endregion

        #region Constructors
        public RecruitmentService(IRecruitmentRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Public Methods
        public async Task<Result<List<RecruitmentBudgetDTO>>> GetRecruitmentBudgetAsync(string? departmentCode, bool? onHold)
        {
            List<RecruitmentBudgetDTO> budgetList = new List<RecruitmentBudgetDTO>();

            try
            {
                var repoResult = await _repository.GetRecruitmentBudgetAsync(departmentCode, onHold);
                if (!repoResult.Success)
                {
                    return Result<List<RecruitmentBudgetDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                budgetList = repoResult.Value!.Select(e => new RecruitmentBudgetDTO
                {
                    BudgetId = e.BudgetId,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    BudgetHeadCount = e.BudgetHeadCount,
                    BudgetDescription = e.BudgetDescription,
                    ActiveCount = e.ActiveCount.HasValue ? Convert.ToInt32(e.ActiveCount) : 0,
                    ExitCount = e.ExitCount.HasValue ? Convert.ToInt32(e.ExitCount) : 0,
                    RequisitionCount = e.RequisitionCount.HasValue ? Convert.ToInt32(e.RequisitionCount) : 0,
                    NetGapCount = e.NetGapCount.HasValue ? Convert.ToInt32(e.NetGapCount) : 0,
                    NewIndentCount = e.NewIndentCount.HasValue ? Convert.ToInt32(e.NewIndentCount) : 0,
                    OnHold = e.OnHold.HasValue ? Convert.ToBoolean(e.OnHold) : false,
                    Remarks = e.Remarks,
                    CreatedDate = e.CreatedDate,
                    LastUpdateDate = e.LastUpdateDate
                }).ToList();

                return Result<List<RecruitmentBudgetDTO>>.SuccessResult(budgetList);
            }
            catch (Exception ex)
            {
                return Result<List<RecruitmentBudgetDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in GetRecruitmentBudgetAsync() method.");
            }
        }

        /// <summary>
        /// Perform update and add operations in "RecruitmentBudget" table
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<int>> SaveRecruitmentBudgetAsync(RecruitmentBudgetDTO dto, CancellationToken cancellationToken = default)
        {
            int saveResult = 0;

            try
            {
                #region Initialize RecruitmentBudget entity
                RecruitmentBudget budgetEntity = new RecruitmentBudget()
                {
                    BudgetId = dto.BudgetId,
                    DepartmentCode = dto.DepartmentCode,
                    BudgetDescription = dto.BudgetDescription,
                    BudgetHeadCount = dto.BudgetHeadCount,
                    ActiveCount = dto.ActiveCount,
                    ExitCount = dto.ExitCount,
                    RequisitionCount = dto.RequisitionCount,
                    NetGapCount = dto.NetGapCount,
                    NewIndentCount = dto.NewIndentCount,
                    OnHold = dto.OnHold,
                    Remarks = dto.Remarks
                };
                #endregion

                if (budgetEntity.BudgetId == 0)
                {
                    budgetEntity.CreatedDate = DateTime.Now;

                    var addResult = await _repository.AddRecruitmentBudgetAsync(budgetEntity, cancellationToken);
                    if (addResult.Success)
                    {
                        saveResult = addResult.Value;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(addResult.Error))
                            throw new Exception(addResult.Error);
                        else
                            throw new Exception("Unable to add new budget record in the database. Please try saving again.");
                    }
                }
                else
                {
                    budgetEntity.LastUpdateDate = DateTime.Now;

                    var updateResult = await _repository.UpdateRecruitmentBudgetAsync(budgetEntity, cancellationToken);
                    if (updateResult.Success)
                    {
                        saveResult = updateResult.Value;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(updateResult.Error))
                            throw new Exception(updateResult.Error);
                        else
                            throw new Exception("Unable to update the selected budget record. Please try saving again.");
                    }
                }

                return Result<int>.SuccessResult(saveResult);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }
        /// <summary>
        /// Perform delete operation in "RecruitmentBudget" table
        /// </summary>
        /// <param name="autoID"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteRecruitmentBudgetAsync(int budgetID, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.DeleteRecruitmentBudgetAsync(budgetID, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to delete the selected budget due to unknown error. Please refresh the page then try to delete again.");
                }

                return Result<bool>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString());
            }
        }
        #endregion
    }
}
