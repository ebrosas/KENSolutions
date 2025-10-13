using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public interface IRecruitmentRepository
    {
        #region Public Methods
        Task<Result<List<RecruitmentBudget>?>> GetRecruitmentBudgetAsync(string? departmentCode, bool? onHold);
        Task<Result<int>> UpdateRecruitmentBudgetAsync(RecruitmentBudget dto, CancellationToken cancellationToken = default);
        Task<Result<int>> AddRecruitmentBudgetAsync(RecruitmentBudget dto, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteRecruitmentBudgetAsync(int budgetID, CancellationToken cancellationToken = default);
        #endregion
    }
}
