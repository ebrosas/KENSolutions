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
                    RecruitmentBudget budget;
                    List<RecruitmentRequisition> requisitionList = new List<RecruitmentRequisition>();

                    foreach (var item in model)
                    {
                        budget = new RecruitmentBudget()
                        {
                            BudgetId = item.RecruitmentBudget.BudgetId,
                            DepartmentCode = item.RecruitmentBudget.DepartmentCode,
                            DepartmentName = item.DepartmentName,
                            BudgetHeadCount = item.RecruitmentBudget.BudgetHeadCount,
                            BudgetDescription = item.RecruitmentBudget.BudgetDescription,
                            ActiveCount = item.RecruitmentBudget.ActiveCount,
                            ExitCount = item.RecruitmentBudget.ExitCount,
                            RequisitionCount = item.RecruitmentBudget.RequisitionCount,
                            NewIndentCount = item.RecruitmentBudget.NewIndentCount,
                            OnHold = item.RecruitmentBudget.OnHold,
                            Remarks = item.RecruitmentBudget.Remarks,
                            CreatedDate = item.RecruitmentBudget.CreatedDate,
                            LastUpdateDate = item.RecruitmentBudget.LastUpdateDate
                        };

                        // Calculate the Net Gap Count
                        budget.NetGapCount = budget.BudgetHeadCount - (budget.ActiveCount ?? 0  + budget.RequisitionCount ?? 0) + budget.ExitCount;

                        #region Find the number of active Recruitment Requisitions
                        var recruitmentList = await _db.RecruitmentRequests.Where(a => a.DepartmentCode.Trim() == budget.DepartmentCode.Trim()).ToListAsync();
                        if (recruitmentList != null && recruitmentList.Any())
                        {
                            requisitionList = recruitmentList!.Select(a => new RecruitmentRequisition
                            {
                                RequisitionId = a.RequisitionId,
                                EmploymentTypeCode = a.EmploymentTypeCode,
                                EmploymentType = a.EmploymentType,
                                QualificationModeCode = a.QualificationModeCode,
                                QualificationMode = a.QualificationMode,
                                PositionTypeCode = a.PositionTypeCode,
                                PositionType = a.PositionType,
                                InterviewProcessCode = a.InterviewProcessCode,
                                InterviewProcess = a.InterviewProcess,
                                IsPreAssessment = a.IsPreAssessment,
                                CompanyCode = a.CompanyCode,
                                Company = a.Company,
                                DepartmentCode = a.DepartmentCode,
                                DepartmentName = a.DepartmentName,
                                CountryCode = a.CountryCode,
                                Country = a.Country,
                                EducationCode = a.EducationCode,
                                Education = a.Education,
                                EmployeeClassCode = a.EmployeeClassCode,
                                EmployeeClass = a.EmployeeClass,
                                JobTitle = a.JobTitle,
                                PayGradeDesc = a.PayGradeDesc,
                                Ethnicity = a.Ethnicity
                            }).ToList();

                            budget.RequisitionCount = requisitionList.Count;
                            budget.ActiveRecruitmentList = requisitionList;
                        }
                        else
                            budget.RequisitionCount = 0;
                        #endregion

                        // Add to the list
                        budgetList.Add(budget);
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

        public async Task<Result<int>> AddRecruitmentRequestAsync(RecruitmentRequisition recruitment, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                // Save to database
                _db.RecruitmentRequests.Add(recruitment);

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

        public async Task<Result<int>> UpdateRecruitmentRequestAsync(RecruitmentRequisition dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                var recruitment = await _db.RecruitmentRequests.FirstOrDefaultAsync(x => x.RequisitionId == dto.RequisitionId, cancellationToken);
                if (recruitment == null)
                    throw new InvalidOperationException("Recruitment requisition record not found");

                #region Update Recruitment Requisition entity

                #region Position Properties       
                recruitment.EmploymentTypeCode = dto.EmploymentTypeCode;
                recruitment.QualificationModeCode = dto.QualificationModeCode;
                recruitment.PositionTypeCode = dto.PositionTypeCode;
                recruitment.InterviewProcessCode = dto.InterviewProcessCode;
                recruitment.IsPreAssessment = dto.IsPreAssessment;
                recruitment.CompanyCode = dto.CompanyCode;
                recruitment.DepartmentCode = dto.DepartmentCode;
                recruitment.CountryCode = dto.CountryCode;
                recruitment.EducationCode = dto.EducationCode;
                recruitment.EmployeeClassCode = dto.EmployeeClassCode;
                recruitment.EthnicityCode = dto.EthnicityCode;
                recruitment.JobTitleCode = dto.JobTitleCode;
                recruitment.PayGradeCode = dto.PayGradeCode;
                #endregion

                #region Position Description
                recruitment.PositionDescription = dto.PositionDescription;
                recruitment.TotalWorkExperience = dto.TotalWorkExperience;
                recruitment.MinWorkExperience = dto.MinWorkExperience;
                recruitment.MaxWorkExperience = dto.MaxWorkExperience;
                recruitment.TotalRelevantExperience = dto.TotalRelevantExperience;
                recruitment.MinRelevantExperience = dto.MinRelevantExperience;
                recruitment.MaxRelevantExperience = dto.MaxRelevantExperience;
                recruitment.AgeRange = dto.AgeRange;
                recruitment.MinAge = dto.MinAge;
                recruitment.MaxAge = dto.MaxAge;
                recruitment.RequiredGender = dto.RequiredGender;
                recruitment.RequiredAsset = dto.RequiredAsset;
                recruitment.VideoDescriptionURL = dto.VideoDescriptionURL;
                #endregion

                #region Compensation and Benefits
                recruitment.SalaryRangeType = dto.SalaryRangeType;
                recruitment.YearlySalaryRange = dto.YearlySalaryRange;
                recruitment.YearlySalaryRangeMin = dto.YearlySalaryRangeMin;
                recruitment.YearlySalaryRangeMax = dto.YearlySalaryRangeMax;
                recruitment.YearlySalaryRangeCurrency = dto.YearlySalaryRangeCurrency;
                recruitment.MonthlySalaryRange = dto.MonthlySalaryRange;
                recruitment.MonthlySalaryRangeMin = dto.MonthlySalaryRangeMin;
                recruitment.MonthlySalaryRangeMax = dto.MonthlySalaryRangeMax;
                recruitment.MonthlySalaryRangeCurrency = dto.MonthlySalaryRangeCurrency;
                recruitment.DailySalaryRange = dto.DailySalaryRange;
                recruitment.DailySalaryRangeMin = dto.DailySalaryRangeMin;
                recruitment.DailySalaryRangeMax = dto.DailySalaryRangeMax;
                recruitment.DailySalaryRangeCurrency = dto.DailySalaryRangeCurrency;
                recruitment.HourlySalaryRange = dto.HourlySalaryRange;
                recruitment.HourlySalaryRangeMin = dto.HourlySalaryRangeMin;
                recruitment.HourlySalaryRangeMax = dto.HourlySalaryRangeMax;
                recruitment.HourlySalaryRangeCurrency = dto.HourlySalaryRangeCurrency;
                recruitment.Responsibilities = dto.Responsibilities;
                recruitment.Competencies = dto.Competencies;
                recruitment.GeneralRemarks = dto.GeneralRemarks;
                #endregion

                #region General
                recruitment.LastUpdatedByNo = dto.LastUpdatedByNo;
                recruitment.LastUpdatedUserID = dto.LastUpdatedUserID;
                recruitment.LastUpdatedName = dto.LastUpdatedName;
                recruitment.LastUpdateDate = dto.LastUpdateDate;
                #endregion

                #endregion

                #region Update Job Qualifications entity
                if (dto.QualificationList != null && dto.QualificationList.Any())
                {
                    #region Delete entity items that don't exist in the DTO
                    var qualificationsNotInDTO = _db.JobQualifications.AsEnumerable()
                                    .ExceptBy(dto.QualificationList.Select(d => d.AutoId), e => e.AutoId)
                                    .ToList();
                    if (qualificationsNotInDTO.Any())
                    {
                        _db.JobQualifications.RemoveRange(qualificationsNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var qualification in dto.QualificationList)
                    {
                        var existingQualification = await _db.JobQualifications
                            .FirstOrDefaultAsync(jc => jc.RequisitionId == dto.RequisitionId && jc.AutoId == qualification.AutoId, cancellationToken);

                        if (existingQualification != null)
                        {
                            // Update existing qualification
                            existingQualification.QualificationCode = qualification.QualificationCode;
                            existingQualification.StreamCode = qualification.StreamCode;
                            existingQualification.SpecializationCode = qualification.SpecializationCode;
                            existingQualification.Remarks = qualification.Remarks;
                        }
                        else
                        {
                            // Add new qualification
                            var newQualification = new JobQualification
                            {
                                RequisitionId = dto.RequisitionId,
                                QualificationCode = qualification.QualificationCode,
                                StreamCode = qualification.StreamCode,
                                Remarks = qualification.Remarks,
                            };
                            await _db.JobQualifications.AddAsync(newQualification, cancellationToken);
                        }
                    }
                }
                #endregion

                // Save to database
                _db.RecruitmentRequests.Update(recruitment);

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
        #endregion
    }
}
