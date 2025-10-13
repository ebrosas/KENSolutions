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
        #endregion
    }
}
