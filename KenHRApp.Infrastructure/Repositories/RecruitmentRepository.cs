using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.IdentityModel.Tokens;
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

                        #region Get the active recruitment requisitions for each department code
                        if (!string.IsNullOrEmpty(budget.DepartmentCode))
                        {
                            var repoResult = await GetRecruitmentListAsync(budget.DepartmentCode);
                            if (repoResult.Success)
                            {
                                requisitionList = repoResult.Value!.Select(e => new RecruitmentRequisition
                                {
                                    RequisitionId = e.RequisitionId,
                                    EmploymentTypeCode = e.EmploymentTypeCode,
                                    EmploymentType = e.EmploymentType,
                                    QualificationModeCode = e.QualificationModeCode,
                                    QualificationMode = e.QualificationMode,
                                    PositionTypeCode = e.PositionTypeCode,
                                    PositionType = e.PositionType,
                                    InterviewProcessCode = e.InterviewProcessCode,
                                    InterviewProcess = e.InterviewProcess,
                                    IsPreAssessment = e.IsPreAssessment,
                                    CompanyCode = e.CompanyCode,
                                    Company = e.Company,
                                    DepartmentCode = e.DepartmentCode,
                                    DepartmentName = e.DepartmentName,
                                    CountryCode = e.CountryCode,
                                    Country = e.Country,
                                    EducationCode = e.EducationCode,
                                    Education = e.Education,
                                    EmployeeClassCode = e.EmployeeClassCode,
                                    EmployeeClass = e.EmployeeClass,
                                    EthnicityCode = e.EthnicityCode,
                                    Ethnicity = e.Ethnicity,
                                    JobTitleCode = e.JobTitleCode,
                                    JobTitle = e.JobTitle,
                                    PayGradeCode = e.PayGradeCode,
                                    PayGradeDesc = e.PayGradeDesc,
                                    PositionDescription = e.PositionDescription,
                                    TotalWorkExperience = e.TotalWorkExperience,
                                    MinWorkExperience = e.MinWorkExperience,
                                    MaxWorkExperience = e.MaxWorkExperience,
                                    TotalRelevantExperience = e.TotalRelevantExperience,
                                    MinRelevantExperience = e.MinRelevantExperience,
                                    MaxRelevantExperience = e.MaxRelevantExperience,
                                    AgeRange = e.AgeRange,
                                    MinAge = e.MinAge,
                                    MaxAge = e.MaxAge,
                                    RequiredGender = e.RequiredGender,
                                    RequiredAsset = e.RequiredAsset,
                                    VideoDescriptionURL = e.VideoDescriptionURL,
                                    SalaryRangeType = e.SalaryRangeType,
                                    YearlySalaryRange = e.YearlySalaryRange,
                                    YearlySalaryRangeMin = e.YearlySalaryRangeMin,
                                    YearlySalaryRangeMax = e.YearlySalaryRangeMax,
                                    YearlySalaryRangeCurrency = e.YearlySalaryRangeCurrency,
                                    MonthlySalaryRange = e.MonthlySalaryRange,
                                    MonthlySalaryRangeMin = e.MonthlySalaryRangeMin,
                                    MonthlySalaryRangeMax = e.MonthlySalaryRangeMax,
                                    MonthlySalaryRangeCurrency = e.MonthlySalaryRangeCurrency,
                                    DailySalaryRange = e.DailySalaryRange,
                                    DailySalaryRangeMin = e.DailySalaryRangeMin,
                                    DailySalaryRangeMax = e.DailySalaryRangeMax,
                                    DailySalaryRangeCurrency = e.DailySalaryRangeCurrency,
                                    HourlySalaryRange = e.HourlySalaryRange,
                                    HourlySalaryRangeMin = e.HourlySalaryRangeMin,
                                    HourlySalaryRangeMax = e.HourlySalaryRangeMax,
                                    HourlySalaryRangeCurrency = e.HourlySalaryRangeCurrency,
                                    Responsibilities = e.Responsibilities,
                                    Competencies = e.Competencies,
                                    GeneralRemarks = e.GeneralRemarks,
                                    CreatedByNo = e.CreatedByNo,
                                    CreatedByUserID = e.CreatedByUserID,
                                    CreatedByName = e.CreatedByName,
                                    CreatedDate = e.CreatedDate,
                                    LastUpdatedByNo = e.LastUpdatedByNo,
                                    LastUpdatedUserID = e.LastUpdatedUserID,
                                    LastUpdatedName = e.LastUpdatedName,
                                    LastUpdateDate = e.LastUpdateDate
                                }).ToList();
                            }
                        }

                        if (requisitionList != null && requisitionList.Any())
                        {
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

        public async Task<Result<List<RecruitmentRequisition>?>> GetRecruitmentListAsync(string? departmentCode)
        {
            List<RecruitmentRequisition> recruitmentList = new List<RecruitmentRequisition>();

            try
            {
                var model = await (from r in _db.RecruitmentRequests
                                   join dm in _db.DepartmentMasters on r.DepartmentCode equals dm.DepartmentCode
                                   join et in _db.UserDefinedCodes on r.EmploymentTypeCode equals et.UDCCode
                                   join pt in _db.UserDefinedCodes on r.PositionTypeCode equals pt.UDCCode
                                   join com in _db.UserDefinedCodes on r.CompanyCode equals com.UDCCode
                                   join ec in _db.UserDefinedCodes on r.EmployeeClassCode equals ec.UDCCode
                                   join jt in _db.UserDefinedCodes on r.JobTitleCode equals jt.UDCCode
                                   join pg in _db.UserDefinedCodes on r.PayGradeCode equals pg.UDCCode
                                   join qm in _db.UserDefinedCodes on r.QualificationModeCode equals qm.UDCCode
                                   join ip in _db.UserDefinedCodes on r.InterviewProcessCode equals ip.UDCCode
                                   join country in _db.UserDefinedCodes on r.CountryCode equals country.UDCCode into gjCountry from subCountry in gjCountry.DefaultIfEmpty()
                                   join education in _db.UserDefinedCodes on r.EducationCode equals education.UDCCode into gjEducation from subEducation in gjEducation.DefaultIfEmpty()
                                   join ethnicity in _db.UserDefinedCodes on r.EthnicityCode equals ethnicity.UDCCode into gjEthnicity from subEthnicity in gjEthnicity.DefaultIfEmpty()
                                   where (r.DepartmentCode == departmentCode || string.IsNullOrEmpty(departmentCode)) 
                                   //&& r.EmploymentTypeCode == et.UDCCode
                                   //&& r.PositionTypeCode == pt.UDCCode && r.CompanyCode == com.UDCCode
                                   //&& r.EmployeeClassCode == ec.UDCCode && r.JobTitleCode == jt.UDCCode
                                   //&& r.PayGradeCode == pg.UDCCode)
                                   select new
                                   {
                                       RecruitmentRequest = r,
                                       DepartmentName = dm.DepartmentName,
                                       EmploymentType = et.UDCDesc1,
                                       QualificationMode = qm.UDCDesc1,
                                       PositionType = pt.UDCDesc1,
                                       InterviewProcess = ip.UDCDesc1,
                                       Company = com.UDCDesc1,
                                       Country = subCountry != null ? subCountry.UDCDesc1 : null,
                                       Education = subEducation != null ? subEducation.UDCDesc1 : null,
                                       EmployeeClass = ec.UDCDesc1,
                                       Ethnicity = subEthnicity != null ? subEthnicity.UDCDesc1 : null,
                                       JobTitle = jt.UDCDesc1,
                                       PayGradeDesc = pg.UDCDesc1
                                   }).ToListAsync();

                if (model != null)
                {
                    #region Initialize entity list                    
                    RecruitmentRequisition recruitment;

                    foreach (var item in model)
                    {
                        recruitment = new RecruitmentRequisition()
                        {
                            RequisitionId = item.RecruitmentRequest.RequisitionId,
                            EmploymentTypeCode = item.RecruitmentRequest.EmploymentTypeCode,
                            EmploymentType = item.EmploymentType,
                            QualificationModeCode = item.RecruitmentRequest.QualificationModeCode,
                            QualificationMode = item.QualificationMode,
                            PositionTypeCode = item.RecruitmentRequest.PositionTypeCode,
                            PositionType = item.PositionType,
                            InterviewProcessCode = item.RecruitmentRequest.InterviewProcessCode,
                            InterviewProcess = item.InterviewProcess,
                            IsPreAssessment = item.RecruitmentRequest.IsPreAssessment,
                            CompanyCode = item.RecruitmentRequest.CompanyCode,
                            Company = item.Company,
                            DepartmentCode = item.RecruitmentRequest.DepartmentCode,
                            DepartmentName = item.DepartmentName,
                            CountryCode = item.RecruitmentRequest.CountryCode,
                            Country = item.Country,
                            EducationCode = item.RecruitmentRequest.EducationCode,
                            Education = item.Education,
                            EmployeeClassCode = item.RecruitmentRequest.EmployeeClassCode,
                            EmployeeClass = item.EmployeeClass,
                            EthnicityCode = item.RecruitmentRequest.EthnicityCode,
                            Ethnicity = item.Ethnicity,
                            JobTitleCode = item.RecruitmentRequest.JobTitleCode,
                            JobTitle = item.JobTitle,
                            PayGradeCode = item.RecruitmentRequest.PayGradeCode,
                            PayGradeDesc = item.PayGradeDesc,
                            PositionDescription = item.RecruitmentRequest.PositionDescription,
                            TotalWorkExperience = item.RecruitmentRequest.TotalWorkExperience,
                            MinWorkExperience = item.RecruitmentRequest.MinWorkExperience,
                            MaxWorkExperience = item.RecruitmentRequest.MaxWorkExperience,
                            TotalRelevantExperience = item.RecruitmentRequest.TotalRelevantExperience,
                            MinRelevantExperience = item.RecruitmentRequest.MinRelevantExperience,
                            MaxRelevantExperience = item.RecruitmentRequest.MaxRelevantExperience,
                            AgeRange = item.RecruitmentRequest.AgeRange,
                            MinAge = item.RecruitmentRequest.MinAge,
                            MaxAge = item.RecruitmentRequest.MaxAge,
                            RequiredGender = item.RecruitmentRequest.RequiredGender,
                            RequiredAsset = item.RecruitmentRequest.RequiredAsset,
                            VideoDescriptionURL = item.RecruitmentRequest.VideoDescriptionURL,
                            SalaryRangeType = item.RecruitmentRequest.SalaryRangeType,
                            YearlySalaryRange = item.RecruitmentRequest.YearlySalaryRange,
                            YearlySalaryRangeMin = item.RecruitmentRequest.YearlySalaryRangeMin,
                            YearlySalaryRangeMax = item.RecruitmentRequest.YearlySalaryRangeMax,
                            YearlySalaryRangeCurrency = item.RecruitmentRequest.YearlySalaryRangeCurrency,
                            MonthlySalaryRange = item.RecruitmentRequest.MonthlySalaryRange,
                            MonthlySalaryRangeMin = item.RecruitmentRequest.MonthlySalaryRangeMin,
                            MonthlySalaryRangeMax = item.RecruitmentRequest.MonthlySalaryRangeMax,
                            MonthlySalaryRangeCurrency = item.RecruitmentRequest.MonthlySalaryRangeCurrency,
                            DailySalaryRange = item.RecruitmentRequest.DailySalaryRange,
                            DailySalaryRangeMin = item.RecruitmentRequest.DailySalaryRangeMin,
                            DailySalaryRangeMax = item.RecruitmentRequest.DailySalaryRangeMax,
                            DailySalaryRangeCurrency = item.RecruitmentRequest.DailySalaryRangeCurrency,
                            HourlySalaryRange = item.RecruitmentRequest.HourlySalaryRange,
                            HourlySalaryRangeMin = item.RecruitmentRequest.HourlySalaryRangeMin,
                            HourlySalaryRangeMax = item.RecruitmentRequest.HourlySalaryRangeMax,
                            HourlySalaryRangeCurrency = item.RecruitmentRequest.HourlySalaryRangeCurrency,
                            Responsibilities = item.RecruitmentRequest.Responsibilities,
                            Competencies = item.RecruitmentRequest.Competencies,
                            GeneralRemarks = item.RecruitmentRequest.GeneralRemarks,
                            CreatedByNo = item.RecruitmentRequest.CreatedByNo,
                            CreatedByUserID = item.RecruitmentRequest.CreatedByUserID,
                            CreatedByName = item.RecruitmentRequest.CreatedByName,
                            CreatedDate = item.RecruitmentRequest.CreatedDate,
                            LastUpdatedByNo = item.RecruitmentRequest.LastUpdatedByNo,
                            LastUpdatedUserID = item.RecruitmentRequest.LastUpdatedUserID,
                            LastUpdatedName = item.RecruitmentRequest.LastUpdatedName,
                            LastUpdateDate = item.RecruitmentRequest.LastUpdateDate
                        };

                        // Add to the list
                        recruitmentList.Add(recruitment);
                    }
                    #endregion
                }

                return Result<List<RecruitmentRequisition>?>.SuccessResult(recruitmentList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<RecruitmentRequisition>?>.Failure($"Database error: {ex.Message}");
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
