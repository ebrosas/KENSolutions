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
                    //ActiveRecruitmentList = e.ActiveRecruitmentList
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
                    Remarks = dto.Remarks,
                    CreatedDate = dto.CreatedDate,
                    LastUpdateDate = dto.LastUpdateDate
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

        public async Task<Result<int>> AddRecruitmentRequestAsync(RecruitmentRequestDTO recruitment, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize RecruitmentRequisition entity
                RecruitmentRequisition recruitmentEntity = new RecruitmentRequisition();

                #region Position Properties       
                recruitmentEntity.RequisitionId = recruitment.RequisitionId;
                recruitmentEntity.EmploymentTypeCode = recruitment.EmploymentTypeCode;
                recruitmentEntity.QualificationModeCode = recruitment.QualificationModeCode;
                recruitmentEntity.PositionTypeCode = recruitment.PositionTypeCode;
                recruitmentEntity.InterviewProcessCode = recruitment.InterviewProcessCode;
                recruitmentEntity.IsPreAssessment = recruitment.IsPreAssessment;
                recruitmentEntity.CompanyCode = recruitment.CompanyCode;
                recruitmentEntity.DepartmentCode = recruitment.DepartmentCode;
                recruitmentEntity.CountryCode = recruitment.CountryCode;
                recruitmentEntity.EducationCode = recruitment.EducationCode;
                recruitmentEntity.EmployeeClassCode = recruitment.EmployeeClassCode;
                recruitmentEntity.EthnicityCode = recruitment.EthnicityCode;
                recruitmentEntity.JobTitleCode = recruitment.JobTitleCode;
                recruitmentEntity.PayGradeCode = recruitment.PayGradeCode;
                #endregion

                #region Position Description
                recruitmentEntity.PositionDescription = recruitment.PositionDescription;
                recruitmentEntity.TotalWorkExperience = recruitment.TotalWorkExperience;
                recruitmentEntity.MinWorkExperience = recruitment.MinWorkExperience;
                recruitmentEntity.MaxWorkExperience = recruitment.MaxWorkExperience;
                recruitmentEntity.TotalRelevantExperience = recruitment.TotalRelevantExperience;
                recruitmentEntity.MinRelevantExperience = recruitment.MinRelevantExperience;
                recruitmentEntity.MaxRelevantExperience = recruitment.MaxRelevantExperience;
                recruitmentEntity.AgeRange = recruitment.AgeRange;
                recruitmentEntity.MinAge = recruitment.MinAge;
                recruitmentEntity.MaxAge = recruitment.MaxAge;
                recruitmentEntity.RequiredGender = recruitment.RequiredGender;
                recruitmentEntity.RequiredAsset = recruitment.RequiredAsset;
                recruitmentEntity.VideoDescriptionURL = recruitment.VideoDescriptionURL;
                #endregion

                #region Compensation and Benefits
                recruitmentEntity.SalaryRangeType = recruitment.SalaryRangeType;
                recruitmentEntity.YearlySalaryRange = recruitment.YearlySalaryRange;
                recruitmentEntity.YearlySalaryRangeMin = recruitment.YearlySalaryRangeMin;
                recruitmentEntity.YearlySalaryRangeMax = recruitment.YearlySalaryRangeMax;
                recruitmentEntity.YearlySalaryRangeCurrency = recruitment.YearlySalaryRangeCurrency;
                recruitmentEntity.MonthlySalaryRange = recruitment.MonthlySalaryRange;
                recruitmentEntity.MonthlySalaryRangeMin = recruitment.MonthlySalaryRangeMin;
                recruitmentEntity.MonthlySalaryRangeMax = recruitment.MonthlySalaryRangeMax;
                recruitmentEntity.MonthlySalaryRangeCurrency = recruitment.MonthlySalaryRangeCurrency;
                recruitmentEntity.DailySalaryRange = recruitment.DailySalaryRange;
                recruitmentEntity.DailySalaryRangeMin = recruitment.DailySalaryRangeMin;
                recruitmentEntity.DailySalaryRangeMax = recruitment.DailySalaryRangeMax;
                recruitmentEntity.DailySalaryRangeCurrency = recruitment.DailySalaryRangeCurrency;
                recruitmentEntity.HourlySalaryRange = recruitment.HourlySalaryRange;
                recruitmentEntity.HourlySalaryRangeMin = recruitment.HourlySalaryRangeMin;
                recruitmentEntity.HourlySalaryRangeMax = recruitment.HourlySalaryRangeMax;
                recruitmentEntity.HourlySalaryRangeCurrency = recruitment.HourlySalaryRangeCurrency;
                recruitmentEntity.Responsibilities = recruitment.Responsibilities;
                recruitmentEntity.Competencies = recruitment.Competencies;
                recruitmentEntity.GeneralRemarks = recruitment.GeneralRemarks;
                #endregion

                #region General
                recruitmentEntity.CreatedByNo = recruitment.CreatedByNo;
                recruitmentEntity.CreatedByUserID = recruitment.CreatedByUserID;
                recruitmentEntity.CreatedByName = recruitment.CreatedByName;
                recruitmentEntity.CreatedDate = recruitment.CreatedDate;
                #endregion
                #endregion

                #region Initialize Qualifications entity
                List<JobQualification> qualificationList = new List<JobQualification>();

                if (recruitment.QualificationList != null && recruitment.QualificationList.Any())
                {
                    qualificationList = recruitment.QualificationList.Select(e => new JobQualification
                    {
                        //AutoId = e.AutoId,
                        RequisitionId = e.RequisitionId,
                        QualificationCode = e.QualificationCode,
                        StreamCode = e.StreamCode,
                        SpecializationCode = e.SpecializationCode
                    }).ToList();
                }

                recruitmentEntity.QualificationList = qualificationList;
                #endregion

                var result = await _repository.AddRecruitmentRequestAsync(recruitmentEntity, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to commit data changes to the database. Please try saving again.");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> UpdateRecruitmentRequestAsync(RecruitmentRequestDTO recruitment, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize RecruitmentRequisition entity
                RecruitmentRequisition recruitmentEntity = new RecruitmentRequisition();

                #region Position Properties       
                recruitmentEntity.RequisitionId = recruitment.RequisitionId;
                recruitmentEntity.EmploymentTypeCode = recruitment.EmploymentTypeCode;
                recruitmentEntity.QualificationModeCode = recruitment.QualificationModeCode;
                recruitmentEntity.PositionTypeCode = recruitment.PositionTypeCode;
                recruitmentEntity.InterviewProcessCode = recruitment.InterviewProcessCode;
                recruitmentEntity.IsPreAssessment = recruitment.IsPreAssessment;
                recruitmentEntity.CompanyCode = recruitment.CompanyCode;
                recruitmentEntity.DepartmentCode = recruitment.DepartmentCode;
                recruitmentEntity.CountryCode = recruitment.CountryCode;
                recruitmentEntity.EducationCode = recruitment.EducationCode;
                recruitmentEntity.EmployeeClassCode = recruitment.EmployeeClassCode;
                recruitmentEntity.EthnicityCode = recruitment.EthnicityCode;
                recruitmentEntity.JobTitleCode = recruitment.JobTitleCode;
                recruitmentEntity.PayGradeCode = recruitment.PayGradeCode;
                #endregion

                #region Position Description
                recruitmentEntity.PositionDescription = recruitment.PositionDescription;
                recruitmentEntity.TotalWorkExperience = recruitment.TotalWorkExperience;
                recruitmentEntity.MinWorkExperience = recruitment.MinWorkExperience;
                recruitmentEntity.MaxWorkExperience = recruitment.MaxWorkExperience;
                recruitmentEntity.TotalRelevantExperience = recruitment.TotalRelevantExperience;
                recruitmentEntity.MinRelevantExperience = recruitment.MinRelevantExperience;
                recruitmentEntity.MaxRelevantExperience = recruitment.MaxRelevantExperience;
                recruitmentEntity.AgeRange = recruitment.AgeRange;
                recruitmentEntity.MinAge = recruitment.MinAge;
                recruitmentEntity.MaxAge = recruitment.MaxAge;
                recruitmentEntity.RequiredGender = recruitment.RequiredGender;
                recruitmentEntity.RequiredAsset = recruitment.RequiredAsset;
                recruitmentEntity.VideoDescriptionURL = recruitment.VideoDescriptionURL;
                #endregion

                #region Compensation and Benefits
                recruitmentEntity.SalaryRangeType = recruitment.SalaryRangeType;
                recruitmentEntity.YearlySalaryRange = recruitment.YearlySalaryRange;
                recruitmentEntity.YearlySalaryRangeMin = recruitment.YearlySalaryRangeMin;
                recruitmentEntity.YearlySalaryRangeMax = recruitment.YearlySalaryRangeMax;
                recruitmentEntity.YearlySalaryRangeCurrency = recruitment.YearlySalaryRangeCurrency;
                recruitmentEntity.MonthlySalaryRange = recruitment.MonthlySalaryRange;
                recruitmentEntity.MonthlySalaryRangeMin = recruitment.MonthlySalaryRangeMin;
                recruitmentEntity.MonthlySalaryRangeMax = recruitment.MonthlySalaryRangeMax;
                recruitmentEntity.MonthlySalaryRangeCurrency = recruitment.MonthlySalaryRangeCurrency;
                recruitmentEntity.DailySalaryRange = recruitment.DailySalaryRange;
                recruitmentEntity.DailySalaryRangeMin = recruitment.DailySalaryRangeMin;
                recruitmentEntity.DailySalaryRangeMax = recruitment.DailySalaryRangeMax;
                recruitmentEntity.DailySalaryRangeCurrency = recruitment.DailySalaryRangeCurrency;
                recruitmentEntity.HourlySalaryRange = recruitment.HourlySalaryRange;
                recruitmentEntity.HourlySalaryRangeMin = recruitment.HourlySalaryRangeMin;
                recruitmentEntity.HourlySalaryRangeMax = recruitment.HourlySalaryRangeMax;
                recruitmentEntity.HourlySalaryRangeCurrency = recruitment.HourlySalaryRangeCurrency;
                recruitmentEntity.Responsibilities = recruitment.Responsibilities;
                recruitmentEntity.Competencies = recruitment.Competencies;
                recruitmentEntity.GeneralRemarks = recruitment.GeneralRemarks;
                #endregion

                #region General
                recruitmentEntity.CreatedByNo = recruitment.CreatedByNo;
                recruitmentEntity.CreatedByUserID = recruitment.CreatedByUserID;
                recruitmentEntity.CreatedByName = recruitment.CreatedByName;
                recruitmentEntity.CreatedDate = recruitment.CreatedDate;
                #endregion

                #endregion

                #region Initialize Qualifications entity
                List<JobQualification> qualificationList = new List<JobQualification>();

                if (recruitment.QualificationList != null && recruitment.QualificationList.Any())
                {
                    qualificationList = recruitment.QualificationList.Select(e => new JobQualification
                    {
                        AutoId = e.AutoId,
                        RequisitionId = e.RequisitionId,
                        QualificationCode = e.QualificationCode,
                        StreamCode = e.StreamCode,
                        SpecializationCode = e.SpecializationCode
                    }).ToList();
                }

                recruitmentEntity.QualificationList = qualificationList;
                #endregion

                var result = await _repository.UpdateRecruitmentRequestAsync(recruitmentEntity, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to commit data changes to the database. Please try saving again.");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }
        #endregion
    }
}
