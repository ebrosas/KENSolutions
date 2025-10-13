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
        #endregion
    }
}
