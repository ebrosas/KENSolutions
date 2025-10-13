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
        #endregion
    }
}
