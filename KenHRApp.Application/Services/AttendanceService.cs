using KenHRApp.Application.Common.Helpers;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.DTOs.TNA;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class AttendanceService : IAttendanceService
    {
        #region Fields
        private readonly IAttendanceRepository _repository;
        private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
        #endregion

        #region Constructors
        public AttendanceService(IAttendanceRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Public Methods
        public async Task<Result<List<ShiftPatternMasterDTO>>> SearchShiftRosterMasterAsync(byte loadType, int? shiftPatternId, string? shiftPatternCode, string? shiftCode, byte? activeFlag)
        {
            List<ShiftPatternMasterDTO> shiftRosterList = new List<ShiftPatternMasterDTO>();
            try
            {
                var repoResult = await _repository.SearchShiftRosterMasterAsync(loadType, shiftPatternId, shiftPatternCode, shiftCode, activeFlag);

                if (!repoResult.Success)
                {
                    return Result<List<ShiftPatternMasterDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                shiftRosterList = repoResult.Value!.Select(e => new ShiftPatternMasterDTO
                {
                    ShiftPatternId = e.ShiftPatternId,
                    ShiftPatternCode = e.ShiftPatternCode,
                    ShiftPatternDescription = e.ShiftPatternDescription,
                    IsActive = e.IsActive,
                    IsDayShift = e.IsDayShift,
                    IsFlexiTime = e.IsFlexiTime,
                    CreatedByEmpNo = e.CreatedByEmpNo,
                    CreatedByName = e.CreatedByName,
                    CreatedByUserID = e.CreatedByUserID,
                    CreatedDate = e.CreatedDate,
                    LastUpdateDate = e.LastUpdateDate,
                    LastUpdateEmpNo = e.LastUpdateEmpNo,
                    LastUpdateUserID = e.LastUpdateUserID,
                    LastUpdatedByName = e.LastUpdatedByName,
                    ShiftPointerList = e.ShiftPointerList!.Select(e => new ShiftPointerDTO
                    {
                        ShiftPointerId = e.ShiftPointerId,
                        ShiftPatternCode = e.ShiftPatternCode,
                        ShiftCode = e.ShiftCode,
                        ShiftDescription = e.ShiftDescription,
                        ShiftPointer = e.ShiftPointer
                    }).ToList()
                }).ToList();

                return Result<List<ShiftPatternMasterDTO>>.SuccessResult(shiftRosterList);
            }
            catch (Exception ex)
            {
                return Result<List<ShiftPatternMasterDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching shift roster records from the database.");
            }
        }

        public async Task<Result<ShiftPatternMasterDTO?>> GetShiftRosterDetailAsync(int shiftPatternId)
        {
            ShiftPatternMasterDTO? shiftRoster = null;

            try
            {
                var repoResult = await _repository.GetShiftRosterDetailAsync(shiftPatternId);
                if (!repoResult.Success)
                {
                    return Result<ShiftPatternMasterDTO?>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    shiftRoster = new ShiftPatternMasterDTO
                    {
                        ShiftPatternId = model.ShiftPatternId,
                        ShiftPatternCode = model.ShiftPatternCode,
                        ShiftPatternDescription = model.ShiftPatternDescription,
                        IsActive = model.IsActive,
                        IsDayShift = model.IsDayShift,
                        IsFlexiTime = model.IsFlexiTime,
                        CreatedByEmpNo = model.CreatedByEmpNo,
                        CreatedByName = model.CreatedByName,
                        CreatedByUserID = model.CreatedByUserID,
                        CreatedDate = model.CreatedDate,
                        LastUpdateDate = model.LastUpdateDate,
                        LastUpdateEmpNo = model.LastUpdateEmpNo,
                        LastUpdateUserID = model.LastUpdateUserID,
                        LastUpdatedByName = model.LastUpdatedByName,
                        ShiftTimingList = model.ShiftTimingList!.Select(e => new ShiftTimingDTO
                        {
                            ShiftTimingId = e.ShiftTimingId,
                            ShiftCode = e.ShiftCode,
                            ShiftDescription = e.ShiftDescription,
                            ArrivalFrom = e.ArrivalFrom,
                            ArrivalTo = e.ArrivalTo,
                            DepartFrom = e.DepartFrom,
                            DepartTo = e.DepartTo,
                            RArrivalFrom = e.RArrivalFrom,
                            RArrivalTo = e.RArrivalTo,
                            RDepartFrom = e.RDepartFrom,
                            RDepartTo = e.RDepartTo,
                            CreatedByEmpNo = e.CreatedByEmpNo,
                            CreatedByName = e.CreatedByName,
                            CreatedByUserID = e.CreatedByUserID,
                            CreatedDate = e.CreatedDate,
                            LastUpdateDate = e.LastUpdateDate,
                            LastUpdateEmpNo = e.LastUpdateEmpNo,
                            LastUpdateUserID = e.LastUpdateUserID,
                            LastUpdatedByName = e.LastUpdatedByName
                        }).ToList(),
                        ShiftPointerList = model.ShiftPointerList!.Select(e => new ShiftPointerDTO
                        {
                            ShiftPointerId = e.ShiftPointerId,
                            ShiftPatternCode = e.ShiftPatternCode,
                            ShiftCode = e.ShiftCode,
                            ShiftDescription = e.ShiftDescription,
                            ShiftPointer = e.ShiftPointer
                        }).ToList()
                    };
                }

                return Result<ShiftPatternMasterDTO?>.SuccessResult(shiftRoster);
            }
            catch (Exception ex)
            {
                return Result<ShiftPatternMasterDTO?>.Failure(ex.Message.ToString() ?? "Unknown error while fetching shift roster records from the database.");
            }
        }

        public async Task<Result<int>> AddShiftRosterMasterAsync(ShiftPatternMasterDTO shiftRoster, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize Shift Roster Master 
                MasterShiftPatternTitle shiftRosterMaster = new MasterShiftPatternTitle();

                shiftRosterMaster.ShiftPatternId = shiftRoster.ShiftPatternId;
                shiftRosterMaster.ShiftPatternCode = shiftRoster.ShiftPatternCode;
                shiftRosterMaster.ShiftPatternDescription = shiftRoster.ShiftPatternDescription;
                shiftRosterMaster.IsActive = shiftRoster.IsActive;
                shiftRosterMaster.IsDayShift = shiftRoster.IsDayShift;
                shiftRosterMaster.IsFlexiTime = shiftRoster.IsFlexiTime;
                shiftRosterMaster.CreatedByEmpNo = shiftRoster.CreatedByEmpNo;
                shiftRosterMaster.CreatedByName = shiftRoster.CreatedByName;
                shiftRosterMaster.CreatedByUserID = shiftRoster.CreatedByUserID;
                shiftRosterMaster.CreatedDate = shiftRoster.CreatedDate;
                #endregion

                #region Initialize Shift Timing 
                List<MasterShiftTime> shiftTimingList = new List<MasterShiftTime>();

                if (shiftRoster.ShiftTimingList != null && shiftRoster.ShiftTimingList.Any())
                {
                    shiftTimingList = shiftRoster.ShiftTimingList.Select(e => new MasterShiftTime
                    {
                        ShiftTimingId = e.ShiftTimingId,
                        ShiftCode = e.ShiftCode,
                        ShiftDescription = e.ShiftDescription,
                        ArrivalFrom = e.ArrivalFrom,
                        ArrivalTo = e.ArrivalTo!.Value,
                        DepartFrom = e.DepartFrom!.Value,
                        DepartTo = e.DepartTo,
                        DurationNormal = Convert.ToInt32((e.ArrivalTo!.Value - e.DepartFrom!.Value).Duration().TotalMinutes),
                        RArrivalFrom = e.RArrivalFrom,
                        RArrivalTo = e.RArrivalTo,
                        RDepartFrom = e.RDepartFrom,
                        RDepartTo = e.RDepartTo,
                        DurationRamadan = Convert.ToInt32((e.RArrivalTo!.Value - e.RDepartFrom!.Value).Duration().TotalMinutes),
                        CreatedByEmpNo = e.CreatedByEmpNo,
                        CreatedByName = e.CreatedByName,
                        CreatedByUserID = e.CreatedByUserID,
                        CreatedDate = e.CreatedDate
                    }).ToList();
                }

                shiftRosterMaster.ShiftTimingList = shiftTimingList;
                #endregion

                #region Initialize Shift Pointer 
                List<MasterShiftPattern> shiftPointerList = new List<MasterShiftPattern>();

                if (shiftRoster.ShiftPointerList != null && shiftRoster.ShiftPointerList.Any())
                {
                    shiftPointerList = shiftRoster.ShiftPointerList.Select(e => new MasterShiftPattern
                    {
                        ShiftPointerId = e.ShiftPointerId,
                        ShiftPatternCode = e.ShiftPatternCode,
                        ShiftCode = e.ShiftCode,
                        ShiftDescription = e.ShiftDescription,
                        ShiftPointer = e.ShiftPointer
                    }).ToList();
                }

                shiftRosterMaster.ShiftPointerList = shiftPointerList;
                #endregion

                var result = await _repository.AddShiftRosterMasterAsync(shiftRosterMaster, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to add new shift roster due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> UpdateShiftRosterMasterAsync(ShiftPatternMasterDTO shiftRoster, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize Shift Roster Master 
                MasterShiftPatternTitle shiftRosterMaster = new MasterShiftPatternTitle();

                shiftRosterMaster.ShiftPatternId = shiftRoster.ShiftPatternId;
                shiftRosterMaster.ShiftPatternCode = shiftRoster.ShiftPatternCode;
                shiftRosterMaster.ShiftPatternDescription = shiftRoster.ShiftPatternDescription;
                shiftRosterMaster.IsActive = shiftRoster.IsActive;
                shiftRosterMaster.IsDayShift = shiftRoster.IsDayShift;
                shiftRosterMaster.IsFlexiTime = shiftRoster.IsFlexiTime;
                shiftRosterMaster.LastUpdateEmpNo = shiftRoster.LastUpdateEmpNo;
                shiftRosterMaster.LastUpdatedByName = shiftRoster.LastUpdatedByName;
                shiftRosterMaster.LastUpdateUserID = shiftRoster.LastUpdateUserID;
                shiftRosterMaster.LastUpdateDate = shiftRoster.LastUpdateDate;
                #endregion

                #region Initialize Qualifications entity
                List<MasterShiftTime> shiftTimingList = new List<MasterShiftTime>();

                if (shiftRoster.ShiftTimingList != null && shiftRoster.ShiftTimingList.Any())
                {
                    shiftTimingList = shiftRoster.ShiftTimingList.Select(e => new MasterShiftTime
                    {
                        ShiftTimingId = e.ShiftTimingId,
                        ShiftCode = e.ShiftCode,
                        ShiftDescription = e.ShiftDescription,
                        ArrivalFrom = e.ArrivalFrom,
                        ArrivalTo = e.ArrivalTo!.Value,
                        DepartFrom = e.DepartFrom!.Value,
                        DepartTo = e.DepartTo,
                        DurationNormal = Convert.ToInt32((e.ArrivalTo!.Value - e.DepartFrom!.Value).Duration().TotalMinutes),
                        RArrivalFrom = e.RArrivalFrom,
                        RArrivalTo = e.RArrivalTo,
                        RDepartFrom = e.RDepartFrom,
                        RDepartTo = e.RDepartTo,
                        DurationRamadan = Convert.ToInt32((e.RArrivalTo!.Value - e.RDepartFrom!.Value).Duration().TotalMinutes),
                        LastUpdateEmpNo = e.LastUpdateEmpNo,
                        LastUpdatedByName = e.LastUpdatedByName,
                        LastUpdateUserID = e.LastUpdateUserID,
                        LastUpdateDate = e.LastUpdateDate
                    }).ToList();
                }

                shiftRosterMaster.ShiftTimingList = shiftTimingList;
                #endregion

                #region Initialize Shift Pointer 
                List<MasterShiftPattern> shiftPointerList = new List<MasterShiftPattern>();

                if (shiftRoster.ShiftPointerList != null && shiftRoster.ShiftPointerList.Any())
                {
                    shiftPointerList = shiftRoster.ShiftPointerList.Select(e => new MasterShiftPattern
                    {
                        ShiftPointerId = e.ShiftPointerId,
                        ShiftPatternCode = e.ShiftPatternCode,
                        ShiftCode = e.ShiftCode,
                        ShiftDescription = e.ShiftDescription,
                        ShiftPointer = e.ShiftPointer
                    }).ToList();
                }

                shiftRosterMaster.ShiftPointerList = shiftPointerList;
                #endregion

                var result = await _repository.UpdateShiftRosterMasterAsync(shiftRosterMaster, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to update shift roster due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<bool>> DeleteShiftRosterMasterAsync(int shiftPatternId, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.DeleteShiftRosterMasterAsync(shiftPatternId, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to delete shift roster due to unknown error. Please refresh the page then try to delete again.");
                }

                return Result<bool>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> AddShiftPatternChangeAsync(List<EmployeeRosterDTO> employeeList, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize "ShiftPatternChange" collection
                List<ShiftPatternChange> shiftPatternChangeList = new List<ShiftPatternChange>();

                shiftPatternChangeList = employeeList.Select(e => new ShiftPatternChange
                {
                    EmpNo = e.EmployeeNo,
                    ShiftPatternCode = e.ShiftPatternCode,
                    ShiftPointer = e.ShiftPointer,
                    ChangeType = e.ChangeTypeCode,
                    EffectiveDate = e.EffectiveDate!.Value,
                    EndingDate = e.EndingDate,
                    CreatedByEmpNo = e.CreatedByEmpNo,
                    CreatedByName = e.CreatedByName,
                    CreatedByUserID = e.CreatedByUserID,
                    CreatedDate = DateTime.Now
                }).ToList();
                #endregion

                var result = await _repository.AddShiftPatternChangeAsync(shiftPatternChangeList, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save shift roster changes due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> UpdateShiftPatternChangeAsync(EmployeeRosterDTO dto, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize entity
                ShiftPatternChange rosterChange = new ShiftPatternChange()
                {
                    AutoId = dto.AutoId,
                    EmpNo = dto.EmployeeNo,
                    ShiftPatternCode = dto.ShiftPatternCode,
                    ShiftPointer = dto.ShiftPointer,
                    ChangeType = dto.ChangeTypeCode,
                    EffectiveDate = dto.EffectiveDate!.Value,
                    EndingDate = dto.EndingDate,
                    LastUpdateEmpNo = dto.CreatedByEmpNo,
                    LastUpdatedByName = dto.CreatedByName,
                    LastUpdateUserID = dto.CreatedByUserID,
                    LastUpdateDate = dto.LastUpdateDate
                };
                #endregion

                var result = await _repository.UpdateShiftPatternChangeAsync(rosterChange, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save shift roster changes due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<List<ShiftPatternChangeDTO>>> SearchShiftPatternChangeAsync(byte loadType, int? autoID, int? empNo, string? changeType, 
            string? shiftPatternCode, DateTime? startDate, DateTime? endDate)
        {
            List<ShiftPatternChangeDTO> rosterChangeList = new List<ShiftPatternChangeDTO>();
            
            try
            {
                var repoResult = await _repository.GetShiftRosterChangeLogAsync(autoID, empNo, changeType, shiftPatternCode, startDate, endDate);
                if (!repoResult.Success)
                {
                    return Result<List<ShiftPatternChangeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                rosterChangeList = repoResult.Value!.Select(e => new ShiftPatternChangeDTO
                {
                    ShiftPatternChangeId = e.AutoId,
                    EmpNo = e.EmpNo,
                    EmpName = e.EmpName,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    ShiftPatternCode = e.ShiftPatternCode,
                    ShiftPointer = e.ShiftPointer,
                    ChangeTypeCode = e.ChangeType,
                    ChangeTypeDesc = e.ChangeTypeDesc,
                    EffectiveDate = e.EffectiveDate,
                    EndingDate = e.EndingDate,
                    CreatedByEmpNo = e.CreatedByEmpNo,
                    CreatedByName = e.CreatedByName,
                    CreatedByUserID = e.CreatedByUserID,
                    CreatedDate = e.CreatedDate,
                    LastUpdateDate = e.LastUpdateDate,
                    LastUpdateEmpNo = e.LastUpdateEmpNo,
                    LastUpdateUserID = e.LastUpdateUserID,
                    LastUpdatedByName = e.LastUpdatedByName
                }).ToList();

                return Result<List<ShiftPatternChangeDTO>>.SuccessResult(rosterChangeList);
            }
            catch (Exception ex)
            {
                return Result<List<ShiftPatternChangeDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching shift roster records from the database.");
            }
        }

        public async Task<Result<ShiftPatternChangeDTO?>> GetShiftPatternChangeAsync(int autoID)
        {
            ShiftPatternChangeDTO? rosterChange = null;

            try
            {
                var repoResult = await _repository.GetShiftPatternChangeAsync(autoID);
                if (!repoResult.Success)
                {
                    return Result<ShiftPatternChangeDTO?>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    rosterChange = new ShiftPatternChangeDTO
                    {
                        ShiftPatternChangeId = model.AutoId,
                        EmpNo = model.EmpNo,
                        EmpName = model.EmpName,
                        DepartmentCode = model.DepartmentCode,
                        DepartmentName = model.DepartmentName,
                        ShiftPatternCode = model.ShiftPatternCode,
                        ShiftPointer = model.ShiftPointer,
                        ChangeTypeCode = model.ChangeType,
                        ChangeTypeDesc = model.ChangeTypeDesc,
                        EffectiveDate = model.EffectiveDate,
                        EndingDate = model.EndingDate,
                        CreatedByEmpNo = model.CreatedByEmpNo,
                        CreatedByName = model.CreatedByName,
                        CreatedByUserID = model.CreatedByUserID,
                        CreatedDate = model.CreatedDate,
                        LastUpdateDate = model.LastUpdateDate,
                        LastUpdateEmpNo = model.LastUpdateEmpNo,
                        LastUpdateUserID = model.LastUpdateUserID,
                        LastUpdatedByName = model.LastUpdatedByName
                    };
                }

                return Result<ShiftPatternChangeDTO?>.SuccessResult(rosterChange);
            }
            catch (Exception ex)
            {
                return Result<ShiftPatternChangeDTO?>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the shift roster change records in the database.");
            }
        }

        public async Task<Result<bool>> DeleteShiftPatternChangeAsync(int autoID, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _repository.DeleteShiftPatternChangeAsync(autoID, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to delete shift roster due to unknown error. Please refresh the page then try to delete again.");
                }

                return Result<bool>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<bool>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<List<HolidayDTO>>> GetPublicHolidaysAsync(int? year, byte? holidayType)
        {
            List<HolidayDTO> holidayList = new();

            try
            {
                var repoResult = await _repository.GetPublicHolidaysAsync(year, holidayType);
                if (!repoResult.Success)
                {
                    return Result<List<HolidayDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                holidayList = repoResult.Value!.Select(e => new HolidayDTO
                {
                    HolidayId = e.HolidayId,
                    HolidayDesc = e.HolidayDesc,
                    HolidayDate = e.HolidayDate,
                    HolidayType = e.HolidayType,
                    CreatedByEmpNo = e.CreatedByEmpNo,
                    CreatedByName = e.CreatedByName,
                    CreatedByUserID = e.CreatedByUserID,
                    CreatedDate = e.CreatedDate,
                    LastUpdateDate = e.LastUpdateDate,
                    LastUpdateEmpNo = e.LastUpdateEmpNo,
                    LastUpdateUserID = e.LastUpdateUserID,
                    LastUpdatedByName = e.LastUpdatedByName
                }).ToList();

                return Result<List<HolidayDTO>>.SuccessResult(holidayList);
            }
            catch (Exception ex)
            {
                return Result<List<HolidayDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the public holidays from the database.");
            }
        }

        public async Task<Result<List<UserDefinedCodeDTO>>> GetUserDefinedCodeAsync(string? udcCode)
        {
            List<UserDefinedCodeDTO> udcList = new List<UserDefinedCodeDTO>();

            try
            {
                var repoResult = await _repository.GetUserDefinedCodeAsync(udcCode);

                if (!repoResult.Success)
                {
                    return Result<List<UserDefinedCodeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                udcList = repoResult.Value!.Select(e => new UserDefinedCodeDTO
                {
                    GroupID = e.GroupID,
                    UDCId = e.UDCId,
                    UDCCode = e.UDCCode,
                    UDCDesc1 = e.UDCDesc1,
                    UDCDesc2 = e.UDCDesc2,
                    UDCSpecialHandlingCode = e.UDCSpecialHandlingCode,
                    SequenceNo = e.SequenceNo,
                    IsActive = e.IsActive,
                    Amount = e.Amount
                }).ToList();

                return Result<List<UserDefinedCodeDTO>>.SuccessResult(udcList);

            }
            catch (Exception ex)
            {
                return Result<List<UserDefinedCodeDTO>>.Failure(ex.Message.ToString() ?? "Unknown error in GetUserDefinedCodeAllAsync() method.");
            }
        }

        public async Task<Result<AttendanceSummaryDTO>> GetAttendanceSummaryAsync(int empNo, DateTime? startDate, DateTime? endDate)
        {
            AttendanceSummaryDTO attendanceSummary = new AttendanceSummaryDTO();

            try
            {
                var repoResult = await _repository.GetAttendanceSummaryAsync(empNo, startDate, endDate);
                if (!repoResult.Success)
                {
                    return Result<AttendanceSummaryDTO>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    attendanceSummary.EmployeeNo = model.EmployeeNo;
                    attendanceSummary.EmployeeName = model.EmployeeName;
                    attendanceSummary.ShiftRoster = model.ShiftRoster;
                    attendanceSummary.ShiftRosterDesc = model.ShiftRosterDesc;
                    attendanceSummary.ShiftTiming = model.ShiftTiming;
                    attendanceSummary.TotalAbsent = model.TotalAbsent;
                    attendanceSummary.TotalHalfDay = model.TotalHalfDay;
                    attendanceSummary.TotalLeave = model.TotalLeave;
                    attendanceSummary.TotalLate = model.TotalLate;
                    attendanceSummary.TotalEarlyOut = model.TotalEarlyOut;
                    attendanceSummary.TotalDeficitHour = model.TotalDeficitHour;
                    attendanceSummary.TotalWorkHour = model.TotalWorkHour;
                    attendanceSummary.TotalDaysWorked = model.TotalDaysWorked;
                    attendanceSummary.AverageWorkHour = model.AverageWorkHour;
                    attendanceSummary.TotalLeaveBalance = model.TotalLeaveBalance;
                    attendanceSummary.TotalSLBalance = model.TotalSLBalance;
                    attendanceSummary.TotalDILBalance = model.TotalDILBalance;
                }

                return Result<AttendanceSummaryDTO>.SuccessResult(attendanceSummary);
            }
            catch (Exception ex)
            {
                return Result<AttendanceSummaryDTO>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the attendance summary information from the database.");
            }
        }

        public async Task<Result<AttendanceDetailDTO>> GetAttendanceDetailAsync(int empNo, DateTime attendanceDate)
        {
            AttendanceDetailDTO attendanceDetail = new AttendanceDetailDTO();

            try
            {
                var repoResult = await _repository.GetAttendanceDetailAsync(empNo, attendanceDate);
                if (!repoResult.Success)
                {
                    return Result<AttendanceDetailDTO>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    attendanceDetail.EmployeeNo = model.EmployeeNo;
                    attendanceDetail.AttendanceDate = model.AttendanceDate;
                    attendanceDetail.FirstTimeIn = model.FirstTimeIn;
                    attendanceDetail.LastTimeOut = model.LastTimeOut;
                    attendanceDetail.WorkDurationDesc = model.WorkDurationDesc;
                    attendanceDetail.DeficitHoursDesc = model.DeficitHoursDesc;
                    attendanceDetail.RegularizedType = model.RegularizedType;
                    attendanceDetail.RegularizedReason = model.RegularizedReason;
                    attendanceDetail.LeaveStatus = model.LeaveStatus;
                    attendanceDetail.LeaveDetails = model.LeaveDetails;
                    attendanceDetail.RawSwipes = model.RawSwipes;
                    attendanceDetail.SwipeType = model.SwipeType;

                    if (model.SwipeLogList != null && model.SwipeLogList.Any())
                    {
                        attendanceDetail.SwipeLogList = model.SwipeLogList!.Select(e => new AttendanceSwipeDTO
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                }

                return Result<AttendanceDetailDTO>.SuccessResult(attendanceDetail);
            }
            catch (Exception ex)
            {
                return Result<AttendanceDetailDTO>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the attendance records from the database.");
            }
        }

        public async Task<Result<int>> SaveSwipeDataAsync(AttendanceSwipeDTO swipeData, CancellationToken cancellationToken = default)
        {
            try
            {
                #region Initialize "AttendanceSwipeLog" entity
                AttendanceSwipeLog swipeLog = new AttendanceSwipeLog()
                {
                    EmpNo = swipeData.EmpNo,
                    SwipeDate = swipeData.SwipeDate,
                    SwipeTime = swipeData.SwipeTime,
                    SwipeType = swipeData.SwipeType,
                    LocationCode = swipeData.LocationCode,
                    ReaderCode = swipeData.ReaderCode,
                    StatusCode = swipeData.StatusCode,
                    SwipeLogDate = DateTime.Now
                };
                #endregion

                var result = await _repository.AddAttendanceSwipeLogAsync(swipeLog, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save swipe data into the database due to an unknown error. Please refresh the page then try again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<AttendanceDurationDTO>> GetAttendanceDurationAsync(int empNo, DateTime attendanceDate)
        {
            AttendanceDurationDTO attendanceDetail = null;

            try
            {
                var repoResult = await _repository.GetAttendanceDurationAsync(empNo, attendanceDate);
                if (!repoResult.Success)
                {
                    return Result<AttendanceDurationDTO>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    attendanceDetail = new AttendanceDurationDTO()
                    {
                        EmpNo = model.EmpNo,
                        SwipeDate = model.SwipeDate,
                        TotalWorkDuration = model.TotalWorkDuration
                    };
                }

                return Result<AttendanceDurationDTO>.SuccessResult(attendanceDetail);
            }
            catch (Exception ex)
            {
                return Result<AttendanceDurationDTO>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the attendance duration from the database.");
            }
        }

        public async Task<Result<List<PayrollPeriodResultDTO>>> GetPayrollPeriodAsync(int fiscalYear = 0)
        {
            List<PayrollPeriodResultDTO> payrollPeriodList = new();

            try
            {
                var repoResult = await _repository.GetPayrollPeriodAsync(fiscalYear);
                if (!repoResult.Success)
                {
                    return Result<List<PayrollPeriodResultDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    payrollPeriodList = model.Select(e => new PayrollPeriodResultDTO
                    {
                        PayrollPeriodId = e.PayrollPeriodId,
                        FiscalYear = e.FiscalYear,
                        FiscalMonth = e.FiscalMonth,
                        PayrollStartDate = e.PayrollStartDate,
                        PayrollEndDate = e.PayrollEndDate,
                        IsActive = e.IsActive
                    }).ToList();
                }

                return Result<List<PayrollPeriodResultDTO>>.SuccessResult(payrollPeriodList);
            }
            catch (Exception ex)
            {
                return Result<List<PayrollPeriodResultDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the attendance duration from the database.");
            }
        }

        public async Task<Result<List<AttendanceCalendarDTO>>> GetAttendanceCalendarAsync(
            int empNo, 
            int year, 
            int month, 
            CancellationToken cancellationToken = default)
        {
            List<AttendanceCalendarDTO> attendanceList = new();

            try
            {
                var repoResult = await _repository.GetAttendanceCalendarAsync(empNo, year, month, cancellationToken);
                if (!repoResult.Success)
                {
                    return Result<List<AttendanceCalendarDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null && model.Any())
                {
                    attendanceList = model.Select(e => new AttendanceCalendarDTO
                    {
                        EmpNo = e.EmpNo,
                        AttendanceDate = e.AttendanceDate,
                        LegendCode = e.LegendCode,
                        LegendDesc = e.LegendDesc
                    }).ToList();
                }

                return Result<List<AttendanceCalendarDTO>>.SuccessResult(attendanceList);
            }
            catch (Exception ex)
            {
                return Result<List<AttendanceCalendarDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching attendance calendar data from the database.");
            }
        }

        public async Task<Result<AttendanceInfoResultDTO>> GetAttendanceInfoAsync(
            int empNo,
            DateTime attendanceDate)
        {
            AttendanceInfoResultDTO attendanceInfo = new AttendanceInfoResultDTO();

            try
            {
                var repoResult = await _repository.GetAttendanceInfoAsync(empNo, attendanceDate);
                if (!repoResult.Success)
                {
                    return Result<AttendanceInfoResultDTO>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    attendanceInfo.AttendanceDate = model.AttendanceDate;
                    attendanceInfo.EmployeeNo = model.EmployeeNo;
                    attendanceInfo.EmployeeName = model.EmployeeName;
                    attendanceInfo.ShiftRoster = model.ShiftRoster;
                    attendanceInfo.ShiftRosterDesc = model.ShiftRosterDesc;
                    attendanceInfo.ShiftTiming = model.ShiftTiming;
                    attendanceInfo.TotalDeficitHour = model.TotalDeficitHour;
                    attendanceInfo.TotalWorkHour = model.TotalWorkHour;
                    attendanceInfo.TotalWorkMinute = model.TotalWorkMinute;

                    if (model.SwipeLogList != null && model.SwipeLogList.Any())
                    {
                        attendanceInfo.SwipeLogList = model.SwipeLogList!.Select(e => new AttendanceSwipeDTO
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                }

                return Result<AttendanceInfoResultDTO>.SuccessResult(attendanceInfo);
            }
            catch (Exception ex)
            {
                return Result<AttendanceInfoResultDTO>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the attendance information from the database.");
            }
        }

        public async Task<Result<long>> AddRegularRequestAsync(
            RegularRequestDTO dto,
            List<FileUploadDTO> files,
            string webRootPath,
            CancellationToken cancellationToken = default)
        {
            try
            {
                #region Create "RegularRequestWF" entity from DTO
                RegularRequestWF regularRequest = new RegularRequestWF
                {
                    EmployeeNo = dto.EmployeeNo,
                    EmployeeName = dto.EmployeeName,
                    CostCenter = dto.CostCenter,
                    AttendanceDate = Convert.ToDateTime(dto.AttendanceDate),
                    ROACode = dto!.ROACode,
                    ActionCode = dto!.ActionCode,
                    RegularizedTimeIn = dto.RegularizedTimeIn!.Value,
                    RegularizedTimeOut = dto.RegularizedTimeOut!.Value,
                    ShiftPattern = dto.ShiftPattern,
                    RegularizedDescription = dto.RegularizedDescription,
                    StatusCode = dto.StatusCode,
                    StatusID = dto.StatusID,
                    StatusHandlingCode = dto.StatusHandlingCode,
                    CreatedDate = dto.CreatedDate,
                    CreatedBy = dto.CreatedBy,
                    CreatedUserID = dto.CreatedUserID,
                    CreatedEmail = dto.CreatedEmail,
                    LastUpdatedDate = dto.LastUpdatedDate,
                    LastUpdatedBy = dto.LastUpdatedBy,
                    LastUpdatedUserID = dto.LastUpdatedUserID,
                    LastUpdatedEmail = dto.LastUpdatedEmail
                };
                #endregion

                #region Initialize the file upload path
                string uploadPath = Path.Combine(
                    webRootPath,
                    "uploads",
                    "attachments");

                if (!Directory.Exists(uploadPath))
                    Directory.CreateDirectory(uploadPath);
                #endregion

                #region Initialize file attachments
                if (files is not null && files.Any())
                {
                    foreach (var file in files)
                    {
                        if (file.Size > MaxFileSize)
                            throw new InvalidOperationException(
                                $"File {file.FileName} exceeds 10MB limit.");

                        if (file.Content is null)
                            throw new InvalidOperationException(
                                $"File stream for {file.FileName} is null.");

                        string storedFileName =
                            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

                        string fullPath =
                            Path.Combine(uploadPath, storedFileName);

                        try
                        {
                            await using (var fileStream = new FileStream(
                               fullPath,
                               FileMode.Create,
                               FileAccess.Write,
                               FileShare.None,
                               81920,
                               useAsync: true))
                            {
                                await file.Content.CopyToAsync(fileStream);
                            }
                        }
                        catch (Exception attachErr)
                        {
                        }

                        var attachment = new FileAttachment(
                            regularRequest.AttachmentId,
                            ServiceHelper.CONST_REGULARIZATION,
                            file.FileName,
                            file.ContentType,
                            storedFileName,
                            file.Size);

                        regularRequest.AddAttachment(attachment);
                    }
                }
                #endregion

                var result = await _repository.AddRegularRequestAsync(regularRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save leave request due to error. Please check the data entry then try to save again!");
                }

                return Result<long>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<long>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> UpdateRegularizRequestAsync(
            RegularRequestDTO dto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                #region Create "RegularRequestWF" entity from DTO
                RegularRequestWF regularRequest = new RegularRequestWF
                {
                    RegularizationId = dto.RegularizationId,
                    EmployeeNo = dto.EmployeeNo,
                    AttendanceDate = Convert.ToDateTime(dto.AttendanceDate),
                    ROACode = dto!.ROACode,
                    RegularizedTimeIn = dto.RegularizedTimeIn!.Value,
                    RegularizedTimeOut = dto.RegularizedTimeOut!.Value,
                    RegularizedDescription = dto.RegularizedDescription,
                    StatusCode = dto.StatusCode,
                    StatusID = dto.StatusID,
                    StatusHandlingCode = dto.StatusHandlingCode,
                    LastUpdatedDate = dto.LastUpdatedDate,
                    LastUpdatedBy = dto.LastUpdatedBy,
                    LastUpdatedUserID = dto.LastUpdatedUserID,
                    LastUpdatedEmail = dto.LastUpdatedEmail
                };
                #endregion

                var result = await _repository.UpdateRegularRequestAsync(regularRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save shift roster changes due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> CancelRegularRequestAsync(
            RegularRequestDTO dto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                #region Create "RegularRequestWF" entity from DTO
                RegularRequestWF regularRequest = new RegularRequestWF
                {
                    RegularizationId = dto.RegularizationId,
                    StatusCode = dto.StatusCode,
                    StatusID = dto.StatusID,
                    StatusHandlingCode = dto.StatusHandlingCode,
                    LastUpdatedDate = dto.LastUpdatedDate,
                    LastUpdatedBy = dto.LastUpdatedBy,
                    LastUpdatedUserID = dto.LastUpdatedUserID,
                    LastUpdatedEmail = dto.LastUpdatedEmail
                };
                #endregion

                var result = await _repository.CancelRegularRequestAsync(regularRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to cancel leave request due to unhandled error. Please check the data entry then try again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<RegularRequestDTO?>> GetRegularRequestAsync(long requestNo)
        {
            RegularRequestDTO? regularRequest = null;

            try
            {
                var repoResult = await _repository.GetRegularRequestAsync(requestNo);
                if (!repoResult.Success)
                {
                    return Result<RegularRequestDTO?>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    regularRequest = new RegularRequestDTO
                    {
                        RegularizationId = model.RegularizationId,
                        AttachmentId = model.AttachmentId,
                        WorkflowId = model.WorkflowId,
                        EmployeeNo = model.EmployeeNo,
                        EmployeeName = model.EmployeeName,
                        CostCenter = model.CostCenter,
                        CostCenterName = model.CostCenter,
                        AttendanceDate = model.AttendanceDate,
                        ROACode = model.ROACode,
                        ROADescription = model.ROADesc,
                        ActionCode = model.ActionCode,
                        ActionDescription = model.ActionDesc,
                        RegularizedTimeIn = model.RegularizedTimeIn,
                        RegularizedTimeOut = model.RegularizedTimeOut,
                        ShiftPattern = model.ShiftPattern,
                        ShiftTiming = model.ShiftTiming,
                        WorkDuration = model.WorkDuration,
                        NoPayHours = model.NoPayHours,
                        RegularizedDescription = model.RegularizedDescription,
                        StatusID = model.StatusID,
                        StatusCode = model.StatusCode,
                        StatusDesc = model.StatusDesc,
                        StatusHandlingCode = model.StatusHandlingCode,
                        CreatedDate = model.CreatedDate,
                        CreatedBy = model.CreatedBy,
                        CreatedUserID = model.CreatedUserID,
                        CreatedEmail = model.CreatedEmail,
                        CreatedByName = model.CreatedByName,
                        LastUpdatedDate = model.LastUpdatedDate,
                        LastUpdatedBy = model.LastUpdatedBy,
                        LastUpdatedUserID = model.LastUpdatedUserID,
                        LastUpdatedEmail = model.LastUpdatedEmail,

                        Files = model.AttachmentList!.Select(e => new FileAttachmentDTO
                        {
                            Id = e.Id,
                            RequestType = e.RequestType,
                            AttachmentId = e.AttachmentId,
                            FileName = e.FileName,
                            StoredFileName = e.StoredFileName,
                            ContentType = e.ContentType,
                            FileSize = e.FileSize
                        }).ToList(),
                    };

                    if (model.SwipeLogList != null && model.SwipeLogList.Any())
                    {
                        regularRequest.SwipeLogList = model.SwipeLogList!.Select(e => new AttendanceSwipeDTO
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                }

                return Result<RegularRequestDTO?>.SuccessResult(regularRequest);
            }
            catch (Exception ex)
            {
                return Result<RegularRequestDTO?>.Failure(ex.Message.ToString() ?? "Unknown error while fetching leave request record from the database.");
            }
        }

        public async Task<Result<List<RegularRequestDTO>>> SearchRegularizationAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? roaCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<RegularRequestDTO> regularizationList = new();

            try
            {
                var repoResult = await _repository.SearchRegularizationAsync(requestNo, empNo, costCenter, roaCode, status, startDate, endDate);
                if (!repoResult.Success)
                {
                    return Result<List<RegularRequestDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var e = repoResult.Value;
                if (e != null && e.Any())
                {
                    regularizationList = e.Select(e => new RegularRequestDTO
                    {
                        RegularizationId = e.RegularizationId,
                        AttachmentId = e.AttachmentId,
                        WorkflowId = e.WorkflowId,
                        EmployeeNo = e.EmployeeNo,
                        EmployeeName = e.EmployeeName,
                        CostCenter = e.CostCenter,
                        CostCenterName = e.CostCenterName,
                        AttendanceDate = e.AttendanceDate,
                        ROACode = e.ROACode,
                        ROADescription = e.ROADesc,
                        ActionCode = e.ActionCode,
                        ActionDescription = e.ActionDesc,
                        RegularizedTimeIn = e.RegularizedTimeIn,
                        RegularizedTimeOut = e.RegularizedTimeOut,
                        ShiftPattern = e.ShiftPattern,
                        RegularizedDescription = e.RegularizedDescription,
                        StatusID = e.StatusID,
                        StatusCode = e.StatusCode,
                        StatusDesc = e.StatusDesc,
                        StatusHandlingCode = e.StatusHandlingCode,
                        CreatedDate = e.CreatedDate,
                        CreatedBy = e.CreatedBy,
                        CreatedUserID = e.CreatedUserID,
                        CreatedEmail = e.CreatedEmail,
                        CreatedByName = e.CreatedByName,
                        LastUpdatedDate = e.LastUpdatedDate,
                        LastUpdatedBy = e.LastUpdatedBy,
                        LastUpdatedUserID = e.LastUpdatedUserID,
                        LastUpdatedEmail = e.LastUpdatedEmail,

                        Files = e.AttachmentList!.Select(e => new FileAttachmentDTO
                        {
                            Id = e.Id,
                            RequestType = e.RequestType,
                            AttachmentId = e.AttachmentId,
                            FileName = e.FileName,
                            StoredFileName = e.StoredFileName,
                            ContentType = e.ContentType,
                            FileSize = e.FileSize
                        }).ToList(),
                    }).ToList();
                }

                return Result<List<RegularRequestDTO>>.SuccessResult(regularizationList);
            }
            catch (Exception ex)
            {
                return Result<List<RegularRequestDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the leave requisition records from the database.");
            }
        }

        public async Task<Result<List<TimecardResultDTO>>> SearchTimecardAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? costCenter,
            int? empNo)
        {
            List<TimecardResultDTO> attendanceList = new();

            try
            {
                var repoResult = await _repository.SearchTimecardAsync(startDate, endDate, costCenter, empNo);
                if (!repoResult.Success)
                {
                    return Result<List<TimecardResultDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var e = repoResult.Value;
                if (e != null && e.Any())
                {
                    attendanceList = e.Select(e => new TimecardResultDTO
                    {
                        AutoId = e.AutoId,
                        CostCenter = e.CostCenter,
                        CostCenterName = e.CostCenterName,
                        EmpNo = e.EmpNo,
                        EmployeeName = e.EmployeeName,
                        Position = e.Position,
                        AttendanceDate = e.AttendanceDate,
                        DOW = e.DOW,
                        ShiftPatCode = e.ShiftPatCode,
                        SchedShiftCode = e.SchedShiftCode,
                        ShiftDescription = e.ShiftDescription,
                        ShiftTiming = e.ShiftTiming,
                        FirstTimeIn = e.FirstTimeIn,
                        LastTimeOut = e.LastTimeOut,
                        DurationRequired = e.DurationRequired,
                        WorkDuration = e.WorkDuration,
                        NoPayHours = e.NoPayHours,
                        RemarkCode = e.RemarkCode,
                        LeaveType = e.LeaveType,
                        AbsenceReasonCode = e.AbsenceReasonCode,
                        ROADesc = e.ROADesc,
                        AttendanceStatus = e.AttendanceStatus,
                        AttendanceRemarks = e.AttendanceRemarks,
                        OTDuration = e.OTDuration,
                        OTStatus = e.OTStatus
                    }).ToList();
                }

                return Result<List<TimecardResultDTO>>.SuccessResult(attendanceList);
            }
            catch (Exception ex)
            {
                return Result<List<TimecardResultDTO>>.Failure(ex.Message.ToString() ?? "Unknown error occured when fetching the Time Card data from the database.");
            }
        }

        public async Task<Result<long>> AddOTRequestAsync(
            ExtraTimeRequestDTO dto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                #region Create "ExtraTimeRequestDTO" entity from DTO
                OTRequestWF otRequest = new OTRequestWF
                {
                    EmployeeNo = dto.EmployeeNo,
                    EmployeeName = dto.EmployeeName,
                    CostCenter = dto.CostCenter,
                    AttendanceDate = Convert.ToDateTime(dto.AttendanceDate),
                    OTReasonCode = dto!.OTReasonCode,
                    ActionCode = dto!.ActionCode,
                    OTStartTime = dto.OTStartTime!.Value,
                    OTEndTime = dto.OTEndTime!.Value,
                    ShiftPattern = dto.ShiftPattern,
                    ShiftTiming = dto.ShiftTiming,
                    WorkDuration = dto.WorkDuration,
                    OTDuration = dto.OTDuration,
                    Remarks = dto.Remarks,
                    StatusCode = dto.StatusCode,
                    StatusID = dto.StatusID,
                    StatusHandlingCode = dto.StatusHandlingCode,
                    CreatedDate = dto.CreatedDate,
                    CreatedBy = dto.CreatedBy,
                    CreatedUserID = dto.CreatedUserID,
                    CreatedEmail = dto.CreatedEmail,
                    LastUpdatedDate = dto.LastUpdatedDate,
                    LastUpdatedBy = dto.LastUpdatedBy,
                    LastUpdatedUserID = dto.LastUpdatedUserID,
                    LastUpdatedEmail = dto.LastUpdatedEmail
                };
                #endregion

                var result = await _repository.AddOTRequestAsync(otRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save overtime request due to error. Please check the data entry then try to save again!");
                }

                return Result<long>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<long>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> UpdateOTRequestAsync(
            ExtraTimeRequestDTO dto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                #region Create "OTRequestWF" entity from DTO
                OTRequestWF otRequest = new OTRequestWF
                {
                    ExtratimeId = dto.ExtratimeId,
                    TS_AutoId = dto.TS_AutoId,
                    EmployeeNo = dto.EmployeeNo,
                    AttendanceDate = Convert.ToDateTime(dto.AttendanceDate),
                    OTReasonCode = dto!.OTReasonCode,
                    OTStartTime = dto.OTStartTime!.Value,
                    OTEndTime = dto.OTEndTime!.Value,
                    Remarks = dto.Remarks,
                    StatusCode = dto.StatusCode,
                    StatusID = dto.StatusID,
                    StatusHandlingCode = dto.StatusHandlingCode,
                    LastUpdatedDate = dto.LastUpdatedDate,
                    LastUpdatedBy = dto.LastUpdatedBy,
                    LastUpdatedUserID = dto.LastUpdatedUserID,
                    LastUpdatedEmail = dto.LastUpdatedEmail
                };
                #endregion

                var result = await _repository.UpdateOTRequestAsync(otRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to save overtime request due to error. Please check the data entry then try to save again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<int>> CancelOTRequestAsync(
            ExtraTimeRequestDTO dto,
            CancellationToken cancellationToken = default)
        {
            try
            {
                #region Create "OTRequestWF" entity from DTO
                OTRequestWF otRequest = new OTRequestWF
                {
                    ExtratimeId = dto.ExtratimeId,
                    StatusCode = dto.StatusCode,
                    StatusID = dto.StatusID,
                    StatusHandlingCode = dto.StatusHandlingCode,
                    LastUpdatedDate = dto.LastUpdatedDate,
                    LastUpdatedBy = dto.LastUpdatedBy,
                    LastUpdatedUserID = dto.LastUpdatedUserID,
                    LastUpdatedEmail = dto.LastUpdatedEmail
                };
                #endregion

                var result = await _repository.CancelOTRequestAsync(otRequest, cancellationToken);
                if (!result.Success)
                {
                    if (!string.IsNullOrEmpty(result.Error))
                        throw new Exception(result.Error);
                    else
                        throw new Exception("Unable to cancel overtime request due to unhandled error. Please check the data entry then try again!");
                }

                return Result<int>.SuccessResult(result.Value);
            }
            catch (Exception ex)
            {
                return Result<int>.Failure(ex.Message.ToString());
            }
        }

        public async Task<Result<ExtraTimeRequestDTO?>> GetOTRequestAsync(long requestNo)
        {
            ExtraTimeRequestDTO? regularRequest = null;

            try
            {
                var repoResult = await _repository.GetOTRequestAsync(requestNo);
                if (!repoResult.Success)
                {
                    return Result<ExtraTimeRequestDTO?>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var model = repoResult.Value;
                if (model != null)
                {
                    regularRequest = new ExtraTimeRequestDTO
                    {
                        ExtratimeId = model.ExtratimeId,
                        TS_AutoId = model.TS_AutoId,
                        WorkflowId = model.WorkflowId,
                        EmployeeNo = model.EmployeeNo,
                        EmployeeName = model.EmployeeName,
                        CostCenter = model.CostCenter,
                        CostCenterName = model.CostCenter,
                        AttendanceDate = model.AttendanceDate,
                        OTReasonCode = model.OTReasonCode,
                        OTReasonDesc = model.OTReasonDesc,
                        ActionCode = model.ActionCode,
                        ActionDesc = model.ActionDesc,
                        OTStartTime = model.OTStartTime,
                        OTEndTime = model.OTEndTime,
                        ShiftPattern = model.ShiftPattern,
                        ShiftTiming = model.ShiftTiming,
                        WorkDuration = model.WorkDuration,
                        OTDuration = model.OTDuration,
                        Remarks = model.Remarks,
                        StatusID = model.StatusID,
                        StatusCode = model.StatusCode,
                        StatusDesc = model.StatusDesc,
                        StatusHandlingCode = model.StatusHandlingCode,
                        CreatedDate = model.CreatedDate,
                        CreatedBy = model.CreatedBy,
                        CreatedUserID = model.CreatedUserID,
                        CreatedEmail = model.CreatedEmail,
                        CreatedByName = model.CreatedByName,
                        LastUpdatedDate = model.LastUpdatedDate,
                        LastUpdatedBy = model.LastUpdatedBy,
                        LastUpdatedUserID = model.LastUpdatedUserID,
                        LastUpdatedEmail = model.LastUpdatedEmail,                                                
                    };

                    if (model.SwipeLogList != null && model.SwipeLogList.Any())
                    {
                        regularRequest.SwipeLogList = model.SwipeLogList!.Select(e => new AttendanceSwipeDTO
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                }

                return Result<ExtraTimeRequestDTO?>.SuccessResult(regularRequest);
            }
            catch (Exception ex)
            {
                return Result<ExtraTimeRequestDTO?>.Failure(ex.Message.ToString() ?? "Unknown error while fetching leave request record from the database.");
            }
        }

        public async Task<Result<List<ExtraTimeRequestDTO>>> SearchOvertimeAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? otReasonCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<ExtraTimeRequestDTO> overtimeList = new();

            try
            {
                var repoResult = await _repository.SearchOvertimeAsync(requestNo, empNo, costCenter, otReasonCode, status, startDate, endDate);
                if (!repoResult.Success)
                {
                    return Result<List<ExtraTimeRequestDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                var e = repoResult.Value;
                if (e != null && e.Any())
                {
                    overtimeList = e.Select(e => new ExtraTimeRequestDTO
                    {
                        ExtratimeId = e.ExtratimeId,
                        TS_AutoId = e.TS_AutoId,
                        WorkflowId = e.WorkflowId,
                        EmployeeNo = e.EmployeeNo,
                        EmployeeName = e.EmployeeName,
                        CostCenter = e.CostCenter,
                        CostCenterName = e.CostCenterName,
                        AttendanceDate = e.AttendanceDate,
                        OTReasonCode = e.OTReasonCode,
                        OTReasonDesc = e.OTReasonDesc,
                        ActionCode = e.ActionCode,
                        ActionDesc = e.ActionDesc,
                        OTStartTime = e.OTStartTime,
                        OTEndTime = e.OTEndTime,
                        ShiftPattern = e.ShiftPattern,
                        ShiftTiming = e.ShiftTiming,
                        WorkDuration = e.WorkDuration,
                        OTDuration = e.OTDuration,
                        Remarks = e.Remarks,
                        StatusID = e.StatusID,
                        StatusCode = e.StatusCode,
                        StatusDesc = e.StatusDesc,
                        StatusHandlingCode = e.StatusHandlingCode,
                        CreatedDate = e.CreatedDate,
                        CreatedBy = e.CreatedBy,
                        CreatedUserID = e.CreatedUserID,
                        CreatedEmail = e.CreatedEmail,
                        CreatedByName = e.CreatedByName,
                        LastUpdatedDate = e.LastUpdatedDate,
                        LastUpdatedBy = e.LastUpdatedBy,
                        LastUpdatedUserID = e.LastUpdatedUserID,
                        LastUpdatedEmail = e.LastUpdatedEmail

                        //Files = e.AttachmentList!.Select(e => new FileAttachmentDTO
                        //{
                        //    Id = e.Id,
                        //    RequestType = e.RequestType,
                        //    AttachmentId = e.AttachmentId,
                        //    FileName = e.FileName,
                        //    StoredFileName = e.StoredFileName,
                        //    ContentType = e.ContentType,
                        //    FileSize = e.FileSize
                        //}).ToList(),
                    }).ToList();
                }

                return Result<List<ExtraTimeRequestDTO>>.SuccessResult(overtimeList);
            }
            catch (Exception ex)
            {
                return Result<List<ExtraTimeRequestDTO>>.Failure(ex.Message.ToString() ?? "Unknown error while fetching the leave requisition records from the database.");
            }
        }
        #endregion
    }
}
