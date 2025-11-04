using KenHRApp.Application.DTOs;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IRecruitmentService
    {
        #region Public Methods
        Task<Result<List<RecruitmentBudgetDTO>>> GetRecruitmentBudgetAsync(string? departmentCode, bool? onHold);
        Task<Result<int>> SaveRecruitmentBudgetAsync(RecruitmentBudgetDTO dto, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteRecruitmentBudgetAsync(int budgetID, CancellationToken cancellationToken = default);
        Task<Result<int>> AddRecruitmentRequestAsync(RecruitmentRequestDTO recruitment, CancellationToken cancellationToken = default);
        Task<Result<int>> UpdateRecruitmentRequestAsync(RecruitmentRequestDTO recruitment, CancellationToken cancellationToken = default);
        #endregion
    }
}
