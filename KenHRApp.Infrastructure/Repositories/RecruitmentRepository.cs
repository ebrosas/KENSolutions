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

        public async Task<Result<int>> UpdateRecruitmentBudgetAsync(RecruitmentBudget dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                var budget = await _db.RecruitmentBudgets.FirstOrDefaultAsync(x => x.BudgetId == dto.BudgetId, cancellationToken);
                if (budget == null)
                    throw new InvalidOperationException("Recruitment budget not found");

                #region Update RecruitmentBudget entity
                budget.DepartmentCode = dto.DepartmentCode;
                budget.BudgetDescription = dto.BudgetDescription;
                budget.BudgetHeadCount = dto.BudgetHeadCount;
                budget.ActiveCount = dto.ActiveCount;
                budget.ExitCount = dto.ExitCount;
                budget.RequisitionCount = dto.RequisitionCount;
                budget.NetGapCount = dto.NetGapCount;
                budget.NewIndentCount = dto.NewIndentCount;
                budget.OnHold = dto.OnHold;
                budget.Remarks = dto.Remarks;
                budget.LastUpdateDate = dto.LastUpdateDate;
                #endregion

                // Save to database
                _db.RecruitmentBudgets.Update(budget);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> AddRecruitmentBudgetAsync(RecruitmentBudget dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                // Save to database
                _db.RecruitmentBudgets.Add(dto);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteRecruitmentBudgetAsync(int budgetID, CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;

            try
            {
                var budget = await _db.RecruitmentBudgets.FindAsync(budgetID);
                if (budget == null)
                    throw new Exception("Could not delete budget because record not found in the database.");

                _db.RecruitmentBudgets.Remove(budget);

                int rowsDeleted = await _db.SaveChangesAsync(cancellationToken);
                if (rowsDeleted > 0)
                    isSuccess = true;

                return Result<bool>.SuccessResult(isSuccess);
            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure($"Database error: {ex.Message}");
            }
        }
        #endregion
    }
}
