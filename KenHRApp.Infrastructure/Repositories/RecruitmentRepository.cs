using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public class RecruitmentRepository : IRecruitmentRepository
    {
        #region Fields
        private readonly AppDbContext _db;
        #endregion

        #region Constructors                
        public RecruitmentRepository(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Public Methods
        public async Task<Result<List<RecruitmentBudget>?>> GetRecruitmentBudgetAsync(string? departmentCode, bool? onHold)
        {
            List<RecruitmentBudget> budgetList = new List<RecruitmentBudget>();

            try
            {
                var model = await (from r in _db.RecruitmentBudgets
                                   join dm in _db.DepartmentMasters on r.DepartmentCode equals dm.DepartmentCode
                                   where (r.DepartmentCode == departmentCode || string.IsNullOrEmpty(departmentCode))
                                    && (r.OnHold == onHold || !onHold.HasValue)
                                   select new
                                   {
                                       RecruitmentBudget = r,
                                       DepartmentName = dm.DepartmentName
                                   }).ToListAsync();

                if (model != null)
                {
                    #region Initialize entity list
                    foreach (var item in model)
                    {
                        budgetList.Add(new RecruitmentBudget()
                        {
                            BudgetId = item.RecruitmentBudget.BudgetId,
                            DepartmentCode = item.RecruitmentBudget.DepartmentCode,
                            DepartmentName = item.DepartmentName,
                            BudgetHeadCount = item.RecruitmentBudget.BudgetHeadCount,
                            BudgetDescription = item.RecruitmentBudget.BudgetDescription,   
                            ActiveCount = item.RecruitmentBudget.ActiveCount,
                            ExitCount = item.RecruitmentBudget.ExitCount,
                            RequisitionCount = item.RecruitmentBudget.RequisitionCount,
                            NetGapCount = item.RecruitmentBudget.NetGapCount,
                            NewIndentCount = item.RecruitmentBudget.NewIndentCount,
                            OnHold = item.RecruitmentBudget.OnHold,
                            Remarks = item.RecruitmentBudget.Remarks,
                            CreatedDate = item.RecruitmentBudget.CreatedDate,
                            LastUpdateDate = item.RecruitmentBudget.LastUpdateDate
                        });
                    }
                    #endregion
                }

                return Result<List<RecruitmentBudget>?>.SuccessResult(budgetList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<RecruitmentBudget>?>.Failure($"Database error: {ex.Message}");
            }
        }
        #endregion
    }
}
