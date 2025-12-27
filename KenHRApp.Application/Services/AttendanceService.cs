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
    public class AttendanceService : IAttendanceService
    {
        #region Fields
        private readonly IAttendanceRepository _repository;
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
                    LastUpdatedByName = e.LastUpdatedByName
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
        #endregion
    }
}
