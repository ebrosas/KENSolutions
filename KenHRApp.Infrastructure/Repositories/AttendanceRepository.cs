using Azure.Core;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        #region Fields
        private readonly AppDbContext _db;
        #endregion

        #region Constructors                
        public AttendanceRepository(AppDbContext db)
        {
            _db = db;
        }
        #endregion

        #region Public Methods
        public async Task<Result<List<MasterShiftPatternTitle>?>> SearchShiftRosterMasterAsync(byte loadType, int? shiftPatternId, string? shiftPatternCode, string? shiftCode, byte? activeFlag)
        {
            List<MasterShiftPatternTitle> shiftRosterList = new List<MasterShiftPatternTitle>();

            try
            {
                var model = await _db.MasterShiftPatternTitles
                    .FromSqlRaw("EXEC kenuser.Pr_GetShiftRoster @loadType = {0}, @shiftPatternId = {1}, @shiftPatternCode = {2}, @shiftCode = {3}, @activeFlag = {4}",
                    loadType, shiftPatternId!, shiftPatternCode!, shiftCode!, activeFlag!)
                    .ToListAsync();
                if (model != null)
                {
                    MasterShiftPatternTitle shiftRoster = null;
                    foreach (var item in model)
                    {
                        shiftRoster = new MasterShiftPatternTitle()
                        {
                            ShiftPatternId = item.ShiftPatternId,
                            ShiftPatternCode = item.ShiftPatternCode,
                            ShiftPatternDescription = item.ShiftPatternDescription,
                            IsActive = item.IsActive,
                            IsDayShift = item.IsDayShift,
                            IsFlexiTime = item.IsFlexiTime,
                            CreatedByEmpNo = item.CreatedByEmpNo,
                            CreatedByName = item.CreatedByName,
                            CreatedByUserID = item.CreatedByUserID,
                            CreatedDate = item.CreatedDate,
                            LastUpdateDate = item.LastUpdateDate,
                            LastUpdateEmpNo = item.LastUpdateEmpNo,
                            LastUpdateUserID = item.LastUpdateUserID,
                            LastUpdatedByName = item.LastUpdatedByName
                        };

                        #region Get the Shift Pointers
                        var shiftPointerModel = await (from msp in _db.MasterShiftPatterns
                                                       where msp.ShiftPatternCode.Trim() == shiftRoster.ShiftPatternCode.Trim()
                                                       select msp).ToListAsync();
                        if (shiftPointerModel != null)
                        {
                            foreach (var pointer in shiftPointerModel)
                            {
                                shiftRoster.ShiftPointerList.Add(new MasterShiftPattern()
                                {
                                    ShiftCode = pointer.ShiftCode,
                                    ShiftPatternCode = pointer.ShiftPatternCode,
                                    ShiftPointerId = pointer.ShiftPointerId,
                                    ShiftPointer = pointer.ShiftPointer,
                                    ShiftDescription = pointer.ShiftDescription
                                });
                            }
                        }
                        #endregion

                        // Add to the collection
                        shiftRosterList.Add(shiftRoster);
                    }
                }

                return Result<List<MasterShiftPatternTitle>?>.SuccessResult(shiftRosterList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<MasterShiftPatternTitle>?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<MasterShiftPatternTitle?>> GetShiftRosterDetailAsync(int shiftPatternId)
        {
            MasterShiftPatternTitle? shiftRoster = null;

            try
            {
                var model = await _db.MasterShiftPatternTitles
                    .FromSqlRaw("EXEC kenuser.Pr_GetShiftRoster @loadType = {0}, @shiftPatternId = {1}, @shiftPatternCode = {2}, @shiftCode = {3}, @activeFlag = {4}",
                    1, shiftPatternId, string.Empty, string.Empty, 0)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    shiftRoster = new MasterShiftPatternTitle()
                    {
                        ShiftPatternId = model[0].ShiftPatternId,
                        ShiftPatternCode = model[0].ShiftPatternCode,
                        ShiftPatternDescription = model[0].ShiftPatternDescription,
                        IsActive = model[0].IsActive,
                        IsDayShift = model[0].IsDayShift,
                        IsFlexiTime = model[0].IsFlexiTime,
                        CreatedByEmpNo = model[0].CreatedByEmpNo,
                        CreatedByName = model[0].CreatedByName,
                        CreatedByUserID = model[0].CreatedByUserID,
                        CreatedDate = model[0].CreatedDate,
                        LastUpdateDate = model[0].LastUpdateDate,
                        LastUpdateEmpNo = model[0].LastUpdateEmpNo,
                        LastUpdateUserID = model[0].LastUpdateUserID,
                        LastUpdatedByName = model[0].LastUpdatedByName
                    };

                    #region Get Shift Timing Schedule
                    var shiftTimingSchedModel = await (from mst in _db.MasterShiftTimes
                                                       where mst.ShiftPatternCode.Trim() == shiftRoster.ShiftPatternCode.Trim()
                                                       select mst).ToListAsync();
                    if (shiftTimingSchedModel != null)
                    {
                        foreach (var item in shiftTimingSchedModel)
                        {
                            shiftRoster.ShiftTimingList.Add(new MasterShiftTime()
                            {
                                ShiftTimingId = item.ShiftTimingId,
                                ShiftPatternCode = item.ShiftPatternCode,
                                ShiftCode = item.ShiftCode,
                                ShiftDescription = item.ShiftDescription,
                                ArrivalFrom = item.ArrivalFrom,
                                ArrivalTo = item.ArrivalTo,
                                DepartFrom = item.DepartFrom,
                                DepartTo = item.DepartTo,
                                DurationNormal = item.DurationNormal,
                                RArrivalFrom = item.RArrivalFrom,
                                RArrivalTo = item.RArrivalTo,
                                RDepartFrom = item.RDepartFrom,
                                RDepartTo = item.RDepartTo,
                                DurationRamadan = item.DurationRamadan,
                                CreatedByEmpNo = item.CreatedByEmpNo,
                                CreatedByName = item.CreatedByName,
                                CreatedByUserID = item.CreatedByUserID,
                                CreatedDate = item.CreatedDate,
                                LastUpdateDate = item.LastUpdateDate,
                                LastUpdateEmpNo = item.LastUpdateEmpNo,
                                LastUpdateUserID = item.LastUpdateUserID,
                                LastUpdatedByName = item.LastUpdatedByName
                            });
                        }
                    }
                    #endregion

                    #region Get Shift Timing Sequence
                    var shiftTimingSeqModel = await (from msp in _db.MasterShiftPatterns
                                                     where msp.ShiftPatternCode.Trim() == shiftRoster.ShiftPatternCode.Trim()
                                                     select msp).ToListAsync();
                    if (shiftTimingSeqModel != null)
                    {
                        foreach (var item in shiftTimingSeqModel)
                        {
                            shiftRoster.ShiftPointerList.Add(new MasterShiftPattern()
                            {
                                ShiftCode = item.ShiftCode,
                                ShiftPatternCode = item.ShiftPatternCode,
                                ShiftPointerId = item.ShiftPointerId,
                                ShiftPointer = item.ShiftPointer,
                                ShiftDescription = item.ShiftDescription
                            });
                        }
                    }
                    #endregion
                }

                return Result<MasterShiftPatternTitle?>.SuccessResult(shiftRoster);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<MasterShiftPatternTitle?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> AddShiftRosterMasterAsync(MasterShiftPatternTitle shiftRoster, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                MasterShiftPatternTitle? duplicateShiftRoster = await _db.MasterShiftPatternTitles
                    .FirstOrDefaultAsync(x => x.ShiftPatternCode == shiftRoster.ShiftPatternCode, cancellationToken);

                if (duplicateShiftRoster != null)
                    throw new InvalidOperationException("Duplicate Shift Pattern Code found. Please use a different code.");

                // Save to database
                _db.MasterShiftPatternTitles.Add(shiftRoster);

                rowsUpdated = await _db.SaveChangesAsync(cancellationToken);

                return Result<int>.SuccessResult(rowsUpdated);
            }
            catch (InvalidOperationException invEx)
            {
                return Result<int>.Failure($"Data Validation Error: {invEx.Message}");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<int>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<int>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> UpdateShiftRosterMasterAsync(MasterShiftPatternTitle dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                var shiftRoster = await _db.MasterShiftPatternTitles.FirstOrDefaultAsync(x => x.ShiftPatternId == dto.ShiftPatternId, cancellationToken);
                if (shiftRoster == null)
                    throw new InvalidOperationException("Shift Roster record not found");

                #region Update Shift Roster 
                shiftRoster.ShiftPatternCode = dto.ShiftPatternCode;
                shiftRoster.ShiftPatternDescription = dto.ShiftPatternDescription;
                shiftRoster.IsActive = dto.IsActive;
                shiftRoster.IsDayShift = dto.IsDayShift;
                shiftRoster.IsFlexiTime = dto.IsFlexiTime;
                shiftRoster.LastUpdateEmpNo = dto.LastUpdateEmpNo;
                shiftRoster.LastUpdateUserID = dto.LastUpdateUserID;
                shiftRoster.LastUpdatedByName = dto.LastUpdatedByName;
                shiftRoster.LastUpdateDate = dto.LastUpdateDate;
                #endregion

                #region Update Shift Timing
                if (dto.ShiftTimingList != null && dto.ShiftTimingList.Any())
                {
                    #region Delete entity items that don't exist in the DTO
                    var shiftTimingNotInDTO = _db.MasterShiftTimes.AsEnumerable()
                                    .Where(st => st.ShiftPatternCode == dto.ShiftPatternCode)
                                    .ExceptBy(dto.ShiftTimingList.Select(d => d.ShiftTimingId), e => e.ShiftTimingId)
                                    .ToList();
                    if (shiftTimingNotInDTO.Any())
                    {
                        _db.MasterShiftTimes.RemoveRange(shiftTimingNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var shiftTiming in dto.ShiftTimingList)
                    {
                        var existingShiftTiming = await _db.MasterShiftTimes
                            .FirstOrDefaultAsync(st => st.ShiftPatternCode == dto.ShiftPatternCode && st.ShiftCode == shiftTiming.ShiftCode, cancellationToken);

                        if (existingShiftTiming != null)
                        {
                            // Update existing shiftTiming
                            existingShiftTiming.ShiftDescription = shiftTiming.ShiftDescription;
                            existingShiftTiming.ArrivalFrom = shiftTiming.ArrivalFrom;
                            existingShiftTiming.ArrivalTo = shiftTiming.ArrivalTo;
                            existingShiftTiming.DepartFrom = shiftTiming.DepartFrom;
                            existingShiftTiming.DepartTo = shiftTiming.DepartTo;
                            existingShiftTiming.DurationNormal = Convert.ToInt32((shiftTiming.ArrivalTo - shiftTiming.DepartFrom).Duration().TotalMinutes);
                            existingShiftTiming.RArrivalFrom = shiftTiming.RArrivalFrom;
                            existingShiftTiming.RArrivalTo = shiftTiming.RArrivalTo;
                            existingShiftTiming.RDepartFrom = shiftTiming.RDepartFrom;
                            existingShiftTiming.RDepartTo = shiftTiming.RDepartTo;

                            if (shiftTiming.RArrivalTo.HasValue && shiftTiming.RDepartFrom.HasValue)
                                existingShiftTiming.DurationRamadan = Convert.ToInt32((shiftTiming.RArrivalTo.Value - shiftTiming.RDepartFrom.Value).Duration().TotalMinutes);

                            existingShiftTiming.LastUpdateDate = shiftTiming.LastUpdateDate;
                            existingShiftTiming.LastUpdateEmpNo = shiftTiming.LastUpdateEmpNo;
                            existingShiftTiming.LastUpdatedByName = shiftTiming.LastUpdatedByName;
                            existingShiftTiming.LastUpdateUserID = shiftTiming.LastUpdateUserID;
                        }
                        else
                        {
                            // Add new Shift Timing
                            var newShiftTiming = new MasterShiftTime
                            {
                                ShiftPatternCode = shiftRoster.ShiftPatternCode,
                                ShiftCode = shiftTiming.ShiftCode,
                                ShiftDescription = shiftTiming.ShiftDescription,
                                ArrivalFrom = shiftTiming.ArrivalFrom,
                                ArrivalTo = shiftTiming.ArrivalTo,
                                DepartFrom = shiftTiming.DepartFrom,
                                DepartTo = shiftTiming.DepartTo,
                                DurationNormal = Convert.ToInt32((shiftTiming.ArrivalTo - shiftTiming.DepartFrom).Duration().TotalMinutes),
                                RArrivalFrom = shiftTiming.RArrivalFrom,
                                RArrivalTo = shiftTiming.RArrivalTo,
                                RDepartFrom = shiftTiming.RDepartFrom,
                                RDepartTo = shiftTiming.RDepartTo,
                                CreatedDate = shiftTiming.CreatedDate,
                                CreatedByEmpNo = shiftTiming.CreatedByEmpNo,
                                CreatedByName = shiftTiming.CreatedByName,
                                CreatedByUserID = shiftTiming.CreatedByUserID
                            };

                            if (shiftTiming.RArrivalTo.HasValue && shiftTiming.RDepartFrom.HasValue)
                                newShiftTiming.DurationRamadan = Convert.ToInt32((shiftTiming.RArrivalTo.Value - shiftTiming.RDepartFrom.Value).Duration().TotalMinutes);

                            await _db.MasterShiftTimes.AddAsync(newShiftTiming, cancellationToken);
                        }
                    }
                }
                #endregion

                #region Update Shift Pointer
                if (dto.ShiftPointerList != null && dto.ShiftPointerList.Any())
                {
                    #region Delete entity items that don't exist in the DTO
                    var shiftPointerNotInDTO = _db.MasterShiftPatterns.AsEnumerable()
                                    .Where(st => st.ShiftPatternCode == dto.ShiftPatternCode)
                                    .ExceptBy(dto.ShiftPointerList.Select(d => d.ShiftPointerId), e => e.ShiftPointerId)
                                    .ToList();
                    if (shiftPointerNotInDTO.Any())
                    {
                        _db.MasterShiftPatterns.RemoveRange(shiftPointerNotInDTO);
                        await _db.SaveChangesAsync();
                    }
                    #endregion

                    foreach (var shiftPointer in dto.ShiftPointerList)
                    {
                        var existingShiftPointer = await _db.MasterShiftPatterns
                            .FirstOrDefaultAsync(st => st.ShiftPatternCode == dto.ShiftPatternCode
                                && st.ShiftCode == shiftPointer.ShiftCode
                                && st.ShiftPointer == shiftPointer.ShiftPointer, cancellationToken);

                        if (existingShiftPointer != null)
                        {
                            // Update existing Shift Pointer
                            existingShiftPointer.ShiftDescription = shiftPointer.ShiftDescription;
                            existingShiftPointer.ShiftPointer = shiftPointer.ShiftPointer;
                        }
                        else
                        {
                            // Add new Shift Pointer
                            var newShiftPointer = new MasterShiftPattern
                            {
                                ShiftPatternCode = shiftRoster.ShiftPatternCode,
                                ShiftCode = shiftPointer.ShiftCode,
                                ShiftDescription = shiftPointer.ShiftDescription,
                                ShiftPointer = shiftPointer.ShiftPointer
                            };

                            await _db.MasterShiftPatterns.AddAsync(shiftPointer, cancellationToken);
                        }
                    }
                }
                #endregion

                // Save to database
                _db.MasterShiftPatternTitles.Update(shiftRoster);

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

        public async Task<Result<bool>> DeleteShiftRosterMasterAsync(int shiftPatternId, CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;

            try
            {
                var shiftRoster = await _db.MasterShiftPatternTitles.FindAsync(shiftPatternId);
                if (shiftRoster == null)
                    throw new Exception("Could not delete shift roster because record is not found in the database.");

                _db.MasterShiftPatternTitles.Remove(shiftRoster);

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

        public async Task<Result<int>> AddShiftPatternChangeAsync(List<ShiftPatternChange> dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (dto != null && dto.Any())
                {
                    foreach (var shiftRoster in dto)
                    {
                        var existingShiftRoster = await _db.ShiftPatternChanges
                            .FirstOrDefaultAsync(sp => sp.EmpNo == shiftRoster.EmpNo &&
                                sp.ShiftPatternCode == shiftRoster.ShiftPatternCode &&
                                sp.ShiftPointer == shiftRoster.ShiftPointer &&
                                sp.EffectiveDate == shiftRoster.EffectiveDate, cancellationToken);

                        if (existingShiftRoster != null)
                        {
                            // Update existing shift roster
                            existingShiftRoster.ChangeType = shiftRoster.ChangeType;
                            existingShiftRoster.EffectiveDate = shiftRoster.EffectiveDate;
                            existingShiftRoster.EndingDate = shiftRoster.EndingDate;
                            existingShiftRoster.LastUpdateDate = shiftRoster.LastUpdateDate;
                            existingShiftRoster.LastUpdateEmpNo = shiftRoster.LastUpdateEmpNo;
                            existingShiftRoster.LastUpdatedByName = shiftRoster.LastUpdatedByName;
                            existingShiftRoster.LastUpdateUserID = shiftRoster.LastUpdateUserID;

                            // Save to database
                            _db.ShiftPatternChanges.Update(existingShiftRoster);
                        }
                        else
                        {
                            // Add new Shift Timing
                            var newShiftRoster = new ShiftPatternChange
                            {
                                EmpNo = shiftRoster.EmpNo,
                                ShiftPatternCode = shiftRoster.ShiftPatternCode,
                                ShiftPointer = shiftRoster.ShiftPointer,
                                ChangeType = shiftRoster.ChangeType,
                                EffectiveDate = shiftRoster.EffectiveDate,
                                EndingDate = shiftRoster.EndingDate,
                                CreatedDate = shiftRoster.CreatedDate,
                                CreatedByEmpNo = shiftRoster.CreatedByEmpNo,
                                CreatedByName = shiftRoster.CreatedByName,
                                CreatedByUserID = shiftRoster.CreatedByUserID
                            };

                            await _db.ShiftPatternChanges.AddAsync(newShiftRoster, cancellationToken);
                        }
                    }
                }

                // Save to database
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

        public async Task<Result<int>> UpdateShiftPatternChangeAsync(ShiftPatternChange dto, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                // Update existing shift roster
                var rosterToUpdate = await _db.ShiftPatternChanges
                    .FirstOrDefaultAsync(sp => sp.AutoId == dto.AutoId, cancellationToken);
                if (rosterToUpdate != null)
                {
                    rosterToUpdate.ShiftPointer = dto.ShiftPointer;
                    rosterToUpdate.ChangeType = dto.ChangeType;
                    rosterToUpdate.EffectiveDate = dto.EffectiveDate;
                    rosterToUpdate.EndingDate = dto.EndingDate;
                    rosterToUpdate.LastUpdateDate = dto.LastUpdateDate;
                    rosterToUpdate.LastUpdateEmpNo = dto.LastUpdateEmpNo;
                    rosterToUpdate.LastUpdatedByName = dto.LastUpdatedByName;
                    rosterToUpdate.LastUpdateUserID = dto.LastUpdateUserID;

                    // Save to database
                    _db.ShiftPatternChanges.Update(rosterToUpdate);

                    // Save to database
                    rowsUpdated = await _db.SaveChangesAsync(cancellationToken);
                }

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

        public async Task<Result<List<ShiftPatternChange>?>> SearchShiftPatternChangeAsync(byte loadType, int? autoID, int? empNo, string? changeType, string? shiftPatternCode, DateTime? startDate, DateTime? endDate)
        {
            List<ShiftPatternChange> rosterChangeList = new List<ShiftPatternChange>();

            try
            {
                var model = await _db.ShiftPatternChanges
                    .FromSqlRaw("EXEC kenuser.PR_GetShiftPatternChange @loadType = {0}, @autoID = {1}, @empNo = {2}, @changeType = {3}, @shiftPatternCode = {4}, @startDate = {5}, @endDate = {6}",
                    loadType, autoID!, empNo!, changeType!, shiftPatternCode!, startDate!, endDate!)
                    .ToListAsync();
                if (model != null)
                {
                    ShiftPatternChange rosterChange;
                    foreach (var item in model)
                    {
                        rosterChange = new ShiftPatternChange()
                        {
                            AutoId = item.AutoId,
                            EmpNo = item.EmpNo,
                            EmpName = item.EmpName,
                            DepartmentCode = item.DepartmentCode,
                            DepartmentName = item.DepartmentName,
                            ShiftPatternCode = item.ShiftPatternCode,
                            ShiftPointer = item.ShiftPointer,
                            ChangeType = item.ChangeType,
                            ChangeTypeDesc = item.ChangeTypeDesc,
                            EffectiveDate = item.EffectiveDate,
                            EndingDate = item.EndingDate,
                            CreatedByEmpNo = item.CreatedByEmpNo,
                            CreatedByName = item.CreatedByName,
                            CreatedByUserID = item.CreatedByUserID,
                            CreatedDate = item.CreatedDate,
                            LastUpdateDate = item.LastUpdateDate,
                            LastUpdateEmpNo = item.LastUpdateEmpNo,
                            LastUpdateUserID = item.LastUpdateUserID,
                            LastUpdatedByName = item.LastUpdatedByName
                        };

                        // Add to the collection
                        rosterChangeList.Add(rosterChange);
                    }
                }

                return Result<List<ShiftPatternChange>?>.SuccessResult(rosterChangeList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<ShiftPatternChange>?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<ShiftPatternChange>?>> GetShiftRosterChangeLogAsync(int? autoID, int? empNo, string? changeType,
            string? shiftPatternCode, DateTime? startDate, DateTime? endDate)
        {
            List<ShiftPatternChange> rosterChangeList = new List<ShiftPatternChange>();

            try
            {
                var model = await (from spc in _db.ShiftPatternChanges
                                   join emp in _db.Employees on spc.EmpNo equals emp.EmployeeNo        // INNER JOIN
                                   join dep in _db.DepartmentMasters on emp.DepartmentCode equals dep.DepartmentCode
                                   join ct in _db.UserDefinedCodes on spc.ChangeType equals ct.UDCCode
                                   where
                                   (
                                        (spc.AutoId == autoID || (autoID == null || autoID == 0)) &&
                                        (spc.EmpNo == empNo || (empNo == null || empNo == 0)) &&
                                        (spc.ChangeType == changeType || string.IsNullOrEmpty(changeType)) &&
                                        (spc.ShiftPatternCode == shiftPatternCode || string.IsNullOrEmpty(shiftPatternCode)) &&
                                        ((spc.EffectiveDate >= startDate && spc.EffectiveDate <= endDate) || (!startDate.HasValue && !endDate.HasValue))
                                   )
                                   select new
                                   {
                                       ShiftPatternChange = spc,
                                       EmpName = $"{emp.FirstName} {emp.LastName}",
                                       DepartmentCode = dep.DepartmentCode,
                                       DepartmentName = dep.DepartmentName,
                                       ChangeTypeDesc = ct.UDCDesc1
                                   }).ToListAsync();

                if (model != null)
                {
                    #region Initialize entity                     
                    ShiftPatternChange rosterChange;

                    foreach (var item in model)
                    {
                        rosterChange = new ShiftPatternChange()
                        {
                            AutoId = item.ShiftPatternChange.AutoId,
                            EmpNo = item.ShiftPatternChange.EmpNo,
                            EmpName = item.EmpName,
                            ShiftPatternCode = item.ShiftPatternChange.ShiftPatternCode,
                            ShiftPointer = item.ShiftPatternChange.ShiftPointer,
                            ChangeType = item.ShiftPatternChange.ChangeType,
                            ChangeTypeDesc = item.ChangeTypeDesc,
                            EffectiveDate = item.ShiftPatternChange.EffectiveDate,
                            EndingDate = item.ShiftPatternChange.EndingDate,
                            DepartmentCode = item.DepartmentCode,
                            DepartmentName = item.DepartmentName,
                            CreatedByEmpNo = item.ShiftPatternChange.CreatedByEmpNo,
                            CreatedByUserID = item.ShiftPatternChange.CreatedByUserID,
                            CreatedByName = item.ShiftPatternChange.CreatedByName,
                            CreatedDate = item.ShiftPatternChange.CreatedDate,
                            LastUpdateEmpNo = item.ShiftPatternChange.LastUpdateEmpNo,
                            LastUpdateUserID = item.ShiftPatternChange.LastUpdateUserID,
                            LastUpdatedByName = item.ShiftPatternChange.LastUpdatedByName,
                            LastUpdateDate = item.ShiftPatternChange.LastUpdateDate
                        };

                        // Add to the list
                        rosterChangeList.Add(rosterChange);
                    }
                    #endregion
                }

                return Result<List<ShiftPatternChange>?>.SuccessResult(rosterChangeList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<ShiftPatternChange>?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<ShiftPatternChange?>> GetShiftPatternChangeAsync(int autoID)
        {
            ShiftPatternChange? rosterChange = null;

            try
            {
                var model = await _db.ShiftPatternChanges
                    .FromSqlRaw("EXEC kenuser.PR_GetShiftPatternChange @loadType = {0}, @autoID = {1}, @empNo = {2}, @changeType = {3}, @shiftPatternCode = {4}, @startDate = {5}, @endDate = {6}",
                    1, autoID!, 0, string.Empty, string.Empty, DBNull.Value, DBNull.Value)
                    .ToListAsync();

                if (model != null && model.Any())
                {
                    rosterChange = new ShiftPatternChange()
                    {
                        AutoId = model[0].AutoId,
                        EmpNo = model[0].EmpNo,
                        EmpName = model[0].EmpName,
                        DepartmentCode = model[0].DepartmentCode,
                        DepartmentName = model[0].DepartmentName,
                        ShiftPatternCode = model[0].ShiftPatternCode,
                        ShiftPointer = model[0].ShiftPointer,
                        ChangeType = model[0].ChangeType,
                        ChangeTypeDesc = model[0].ChangeTypeDesc,
                        EffectiveDate = model[0].EffectiveDate,
                        EndingDate = model[0].EndingDate,
                        CreatedByEmpNo = model[0].CreatedByEmpNo,
                        CreatedByName = model[0].CreatedByName,
                        CreatedByUserID = model[0].CreatedByUserID,
                        CreatedDate = model[0].CreatedDate,
                        LastUpdateDate = model[0].LastUpdateDate,
                        LastUpdateEmpNo = model[0].LastUpdateEmpNo,
                        LastUpdateUserID = model[0].LastUpdateUserID,
                        LastUpdatedByName = model[0].LastUpdatedByName
                    };
                }

                return Result<ShiftPatternChange?>.SuccessResult(rosterChange);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<ShiftPatternChange?>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<bool>> DeleteShiftPatternChangeAsync(int autoId, CancellationToken cancellationToken = default)
        {
            bool isSuccess = false;

            try
            {
                var shiftRoster = await _db.ShiftPatternChanges.FindAsync(autoId);
                if (shiftRoster == null)
                    throw new Exception("Could not delete shift roster because record is not found in the database.");

                _db.ShiftPatternChanges.Remove(shiftRoster);

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

        public async Task<Result<List<Holiday>>> GetPublicHolidaysAsync(int? year, byte? holidayType)
        {
            List<Holiday> holidayList = new();

            try
            {
                var model = await (from hol in _db.Holidays
                                   where
                                   (
                                        (hol.HolidayDate.Year == year || (year == null || year == 0)) &&
                                        (hol.HolidayType == holidayType || (holidayType == null || holidayType == 0))
                                   )
                                   select hol).ToListAsync();

                if (model != null)
                {
                    holidayList = model.Select(e => new Holiday
                    {
                        HolidayId = e.HolidayId,
                        HolidayDesc = e.HolidayDesc,
                        HolidayDate = e.HolidayDate,
                        HolidayType = e.HolidayType,
                        HolidayTypeDesc = e.HolidayType == 1 ? "Public Holiday" : e.HolidayType == 2 ? "Ramadan" : "Other",
                        CreatedByEmpNo = e.CreatedByEmpNo,
                        CreatedByName = e.CreatedByName,
                        CreatedByUserID = e.CreatedByUserID,
                        CreatedDate = e.CreatedDate,
                        LastUpdateDate = e.LastUpdateDate,
                        LastUpdateEmpNo = e.LastUpdateEmpNo,
                        LastUpdateUserID = e.LastUpdateUserID,
                        LastUpdatedByName = e.LastUpdatedByName
                    }).ToList();
                }

                return Result<List<Holiday>>.SuccessResult(holidayList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<Holiday>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<UserDefinedCode>>> GetUserDefinedCodeAsync(string? udcCode)
        {
            List<UserDefinedCode> udcList = new();
            try
            {
                var model = await (from grp in _db.UserDefinedCodeGroups
                                   join udc in _db.UserDefinedCodes on grp.UDCGroupId equals udc.GroupID
                                   where
                                   (
                                        (grp.UDCGCode == udcCode || string.IsNullOrEmpty(udcCode))
                                   )
                                   select new
                                   {
                                       UDCItem = udc,
                                       UDCGDesc1 = grp.UDCGDesc1,
                                       UDCGSpecialHandlingCode = grp.UDCGSpecialHandlingCode
                                   }).ToListAsync();

                if (model != null)
                {
                    udcList = model.Select(e => new UserDefinedCode
                    {
                        UDCId = e.UDCItem.UDCId,
                        UDCCode = e.UDCItem.UDCCode,
                        UDCDesc1 = e.UDCItem.UDCDesc1,
                        UDCDesc2 = e.UDCItem.UDCDesc2,
                        UDCSpecialHandlingCode = e.UDCItem.UDCSpecialHandlingCode,
                        SequenceNo = e.UDCItem.SequenceNo,
                        IsActive = e.UDCItem.IsActive,
                        Amount = e.UDCItem.Amount
                    }).ToList();
                }

                return Result<List<UserDefinedCode>>.SuccessResult(udcList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<UserDefinedCode>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<AttendanceSummaryResult>> GetAttendanceSummaryAsync(int empNo, DateTime? startDate, DateTime? endDate)
        {
            AttendanceSummaryResult attendanceSummary = new AttendanceSummaryResult();

            try
            {
                var model = await _db.Set<AttendanceSummaryResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetAttendanceSummary @empNo = {0}, @startDate = {1}, @endDate = {2}",
                    empNo, startDate!, endDate!)
                    .AsNoTracking()
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    attendanceSummary.EmployeeNo = model[0].EmployeeNo;
                    attendanceSummary.EmployeeName = model[0].EmployeeName;
                    attendanceSummary.ShiftRoster = model[0].ShiftRoster;
                    attendanceSummary.ShiftRosterDesc = model[0].ShiftRosterDesc;
                    attendanceSummary.ShiftTiming = model[0].ShiftTiming;
                    attendanceSummary.TotalAbsent = model[0].TotalAbsent;
                    attendanceSummary.TotalHalfDay = model[0].TotalHalfDay;
                    attendanceSummary.TotalLeave = model[0].TotalLeave;
                    attendanceSummary.TotalLate = model[0].TotalLate;
                    attendanceSummary.TotalEarlyOut = model[0].TotalEarlyOut;
                    attendanceSummary.TotalDeficitHour = model[0].TotalDeficitHour;
                    attendanceSummary.TotalWorkHour = model[0].TotalWorkHour;
                    attendanceSummary.TotalDaysWorked = model[0].TotalDaysWorked;
                    attendanceSummary.AverageWorkHour = model[0].AverageWorkHour;
                    attendanceSummary.TotalLeaveBalance = model[0].TotalLeaveBalance;
                    attendanceSummary.TotalSLBalance = model[0].TotalSLBalance;
                    attendanceSummary.TotalDILBalance = model[0].TotalDILBalance;
                }

                return Result<AttendanceSummaryResult>.SuccessResult(attendanceSummary);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<AttendanceSummaryResult>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<AttendanceDetailResult>> GetAttendanceDetailAsync(int empNo, DateTime attendanceDate)
        {
            AttendanceDetailResult attendanceDetail = new AttendanceDetailResult();

            try
            {
                var model = await _db.Set<AttendanceDetailResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetAttendanceDetail @empNo = {0}, @attendanceDate = {1}",
                    empNo, attendanceDate)
                    .AsNoTracking()
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    attendanceDetail.EmployeeNo = model[0].EmployeeNo;
                    attendanceDetail.AttendanceDate = model[0].AttendanceDate;
                    attendanceDetail.FirstTimeIn = model[0].FirstTimeIn;
                    attendanceDetail.LastTimeOut = model[0].LastTimeOut;
                    attendanceDetail.WorkDurationDesc = model[0].WorkDurationDesc;
                    attendanceDetail.DeficitHoursDesc = model[0].DeficitHoursDesc;
                    attendanceDetail.RegularizedType = model[0].RegularizedType;
                    attendanceDetail.RegularizedReason = model[0].RegularizedReason;
                    attendanceDetail.LeaveStatus = model[0].LeaveStatus;
                    attendanceDetail.LeaveDetails = model[0].LeaveDetails;
                    attendanceDetail.RawSwipes = model[0].RawSwipes;
                    attendanceDetail.SwipeType = model[0].SwipeType;

                    #region Get the swipe logs
                    List<AttendanceSwipeLog> swipeLogs = new List<AttendanceSwipeLog>();

                    var swipeModel = await (from log in _db.AttendanceSwipeLogs
                                            where log.EmpNo == attendanceDetail.EmployeeNo &&
                                                log.SwipeDate == attendanceDetail.AttendanceDate
                                            select log).ToListAsync();
                    if (swipeModel != null)
                    {
                        attendanceDetail.SwipeLogList = swipeModel.Select(e => new AttendanceSwipeLog
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                    #endregion
                }
                else
                {
                    attendanceDetail.WorkDurationDesc = "-";
                    attendanceDetail.DeficitHoursDesc = "-";
                    attendanceDetail.RegularizedType = "-";
                    attendanceDetail.RegularizedReason = "-";
                    attendanceDetail.LeaveStatus = "-";
                    attendanceDetail.LeaveDetails = "-";
                    attendanceDetail.RawSwipes = "-";
                }

                return Result<AttendanceDetailResult>.SuccessResult(attendanceDetail);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<AttendanceDetailResult>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<int>> AddAttendanceSwipeLogAsync(AttendanceSwipeLog dto, CancellationToken cancellationToken = default)
        {
            int rowsInserted = 0;

            try
            {
                if (dto != null)
                {
                    await _db.AttendanceSwipeLogs.AddAsync(dto, cancellationToken);

                    // Save to database
                    rowsInserted = await _db.SaveChangesAsync(cancellationToken);
                }

                return Result<int>.SuccessResult(rowsInserted);
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

        public async Task<Result<AttendanceDurationResult>> GetAttendanceDurationAsync(int empNo, DateTime attendanceDate)
        {
            AttendanceDurationResult attendanceDuration = new AttendanceDurationResult();

            try
            {
                var model = await _db.Set<AttendanceDurationResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_CalculateWorkDuration @empNo = {0}, @attendanceDate = {1}",
                    empNo, attendanceDate)
                    .AsNoTracking()
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    attendanceDuration.EmpNo = model[0].EmpNo;
                    attendanceDuration.SwipeDate = model[0].SwipeDate;
                    attendanceDuration.TotalWorkDuration = model[0].TotalWorkDuration;
                }

                return Result<AttendanceDurationResult>.SuccessResult(attendanceDuration);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<AttendanceDurationResult>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<PayrollPeriodResult>>> GetPayrollPeriodAsync(int fiscalYear = 0)
        {
            List<PayrollPeriodResult> payrollPeriodList = new();

            try
            {
                var model = await _db.Set<PayrollPeriodResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetPayrollPeriod @fiscalYear = {0}", fiscalYear)
                    .AsNoTracking()
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    payrollPeriodList = model.Select(e => new PayrollPeriodResult
                    {
                        PayrollPeriodId = e.PayrollPeriodId,
                        FiscalYear = e.FiscalYear,
                        FiscalMonth = e.FiscalMonth,
                        PayrollStartDate = e.PayrollStartDate,
                        PayrollEndDate = e.PayrollEndDate,
                        IsActive = e.IsActive
                    }).ToList();
                }

                return Result<List<PayrollPeriodResult>>.SuccessResult(payrollPeriodList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<PayrollPeriodResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<List<AttendanceCalendarResult>>> GetAttendanceCalendarAsync(int empNo, int year, int month, CancellationToken cancellationToken)
        {
            List<AttendanceCalendarResult> attendanceList = new();

            try
            {
                var model = await _db.Set<AttendanceCalendarResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetAttendanceLegend @empNo = {0}, @year = {1}, @month = {2}",
                    empNo, year, month)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);
                if (model != null && model.Any())
                {
                    attendanceList = model.Select(e => new AttendanceCalendarResult
                    {
                        EmpNo = e.EmpNo,
                        AttendanceDate = e.AttendanceDate,
                        LegendCode = e.LegendCode,
                        LegendDesc = e.LegendDesc
                    }).ToList();
                }

                return Result<List<AttendanceCalendarResult>>.SuccessResult(attendanceList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<AttendanceCalendarResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        public async Task<Result<AttendanceInfoResult>> GetAttendanceInfoAsync(
            int empNo, 
            DateTime attendanceDate)
        {
            AttendanceInfoResult attendanceInfo = new AttendanceInfoResult();

            try
            {
                var model = await _db.Set<AttendanceInfoResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetAttendanceDetails @empNo = {0}, @attendanceDate = {1}",
                    empNo, attendanceDate)
                    .AsNoTracking()
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    attendanceInfo.AttendanceDate = model[0].AttendanceDate;
                    attendanceInfo.EmployeeNo = model[0].EmployeeNo;
                    attendanceInfo.EmployeeName = model[0].EmployeeName;
                    attendanceInfo.ShiftRoster = model[0].ShiftRoster;
                    attendanceInfo.ShiftRosterDesc = model[0].ShiftRosterDesc;
                    attendanceInfo.ShiftTiming = model[0].ShiftTiming;
                    attendanceInfo.TotalDeficitHour = model[0].TotalDeficitHour;
                    attendanceInfo.TotalDeficitMinute = model[0].TotalDeficitMinute;
                    attendanceInfo.TotalWorkHour = model[0].TotalWorkHour;
                    attendanceInfo.TotalWorkMinute = model[0].TotalWorkMinute;
                    attendanceInfo.RemarkCode = model[0].RemarkCode;

                    #region Get the swipe logs
                    List<AttendanceSwipeLog> swipeLogs = new List<AttendanceSwipeLog>();

                    var swipeModel = await (from log in _db.AttendanceSwipeLogs
                                            where log.EmpNo == attendanceInfo.EmployeeNo &&
                                                log.SwipeDate == attendanceInfo.AttendanceDate
                                            select log).ToListAsync();
                    if (swipeModel != null)
                    {
                        attendanceInfo.SwipeLogList = swipeModel.Select(e => new AttendanceSwipeLog
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                    #endregion
                }

                return Result<AttendanceInfoResult>.SuccessResult(attendanceInfo);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<AttendanceInfoResult>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Add new regularization request
        /// </summary>
        public async Task<Result<long>> AddRegularRequestAsync(RegularRequestWF dto, CancellationToken cancellationToken)
        {
            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                #region Initialize entity
                var regularRequest = new RegularRequestWF
                {
                    EmployeeNo = dto.EmployeeNo,
                    EmployeeName = dto.EmployeeName,
                    CostCenter = dto.CostCenter,
                    AttendanceDate = dto.AttendanceDate,
                    ROACode = dto.ROACode,
                    ActionCode = dto.ActionCode,
                    RegularizedTimeIn = dto.RegularizedTimeIn,
                    RegularizedTimeOut = dto.RegularizedTimeOut,
                    ShiftPattern = dto.ShiftPattern,
                    ShiftTiming = dto.ShiftTiming,
                    WorkDuration = dto.WorkDuration,
                    NoPayHours = dto.NoPayHours,
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

                #region Initialize attachments
                if (dto.AttachmentList != null && dto.AttachmentList.Any())
                {
                    regularRequest.AttachmentList = dto.AttachmentList.Select(e => new FileAttachment
                    {
                        AttachmentId = e.AttachmentId,
                        RequestType = e.RequestType,
                        FileName = e.FileName,
                        StoredFileName = e.StoredFileName,
                        ContentType = e.ContentType,
                        FileSize = e.FileSize
                    }).ToList();
                }
                #endregion

                // Save to database
                await _db.RegularRequestWFs.AddAsync(regularRequest);
                await _db.SaveChangesAsync(cancellationToken);

                // ✅ EF Core automatically populates identity after SaveChanges
                long generatedId = regularRequest.RegularizationId;

                return Result<long>.SuccessResult(generatedId);

            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<long>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<long>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update regularization request
        /// </summary>
        public async Task<Result<int>> UpdateRegularRequestAsync(RegularRequestWF regularRequest, CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (regularRequest == null)
                    throw new ArgumentNullException(nameof(regularRequest));

                var existing = await _db.RegularRequestWFs
                    .FirstOrDefaultAsync(e =>
                        e.RegularizationId == regularRequest.RegularizationId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find regularization request with the specified no.");

                #region Update leave request information
                existing.ROACode = regularRequest.ROACode;
                existing.RegularizedTimeIn = regularRequest.RegularizedTimeIn;
                existing.RegularizedTimeOut = regularRequest.RegularizedTimeOut;
                existing.RegularizedDescription = regularRequest.RegularizedDescription;
                existing.StatusCode = regularRequest.StatusCode;
                existing.StatusID = regularRequest.StatusID;
                existing.StatusHandlingCode = regularRequest.StatusHandlingCode;
                existing.LastUpdatedDate = regularRequest.LastUpdatedDate;
                existing.LastUpdatedBy = regularRequest.LastUpdatedBy;
                existing.LastUpdatedUserID = regularRequest.LastUpdatedUserID;
                existing.LastUpdatedEmail = regularRequest.LastUpdatedEmail;

                #endregion

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

        /// <summary>
        /// Cancel Regularization Request
        /// </summary>
        public async Task<Result<int>> CancelRegularRequestAsync(
            RegularRequestWF regularRequest, 
            CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (regularRequest == null)
                    throw new ArgumentNullException(nameof(regularRequest));

                var existing = await _db.RegularRequestWFs
                    .FirstOrDefaultAsync(e =>
                        e.RegularizationId == regularRequest.RegularizationId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find regularization request with the specified request no.");

                #region Update leave information to cancel the request
                existing.StatusCode = regularRequest.StatusCode;
                existing.StatusID = regularRequest.StatusID;
                existing.StatusHandlingCode = regularRequest.StatusHandlingCode;
                existing.LastUpdatedDate = regularRequest.LastUpdatedDate;
                existing.LastUpdatedBy = regularRequest.LastUpdatedBy;
                existing.LastUpdatedUserID = regularRequest.LastUpdatedUserID;
                existing.LastUpdatedEmail = regularRequest.LastUpdatedEmail;
                #endregion

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

        /// <summary>
        /// Get Regularization Request details
        /// </summary>
        /// <param name="requestNo"></param>
        /// <returns></returns>
        public async Task<Result<RegularizationResult>> GetRegularRequestAsync(long requestNo)
        {
            RegularizationResult regularRequest = new();

            try
            {
                var model = await _db.Set<RegularizationResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetRegularizationDetail @requestNo = {0}",
                    requestNo)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    regularRequest.RegularizationId = model[0].RegularizationId;
                    regularRequest.AttachmentId = model[0].AttachmentId;
                    regularRequest.WorkflowId = model[0].WorkflowId;
                    regularRequest.EmployeeNo = model[0].EmployeeNo;
                    regularRequest.EmployeeName = model[0].EmployeeName;
                    regularRequest.CostCenter = model[0].CostCenter;
                    regularRequest.CostCenterName = model[0].CostCenter;
                    regularRequest.AttendanceDate = model[0].AttendanceDate;
                    regularRequest.ROACode = model[0].ROACode;
                    regularRequest.ROADesc = model[0].ROADesc;
                    regularRequest.ActionCode = model[0].ActionCode;
                    regularRequest.ActionDesc = model[0].ActionDesc;
                    regularRequest.RegularizedTimeIn = model[0].RegularizedTimeIn;
                    regularRequest.RegularizedTimeOut = model[0].RegularizedTimeOut;
                    regularRequest.ShiftPattern = model[0].ShiftPattern;
                    regularRequest.ShiftTiming = model[0].ShiftTiming;
                    regularRequest.WorkDuration = model[0].WorkDuration;
                    regularRequest.NoPayHours = model[0].NoPayHours;
                    regularRequest.RegularizedDescription = model[0].RegularizedDescription;                    
                    regularRequest.StatusID = model[0].StatusID;
                    regularRequest.StatusCode = model[0].StatusCode;
                    regularRequest.StatusDesc = model[0].StatusDesc;
                    regularRequest.StatusHandlingCode = model[0].StatusHandlingCode;
                    regularRequest.CreatedDate = model[0].CreatedDate;
                    regularRequest.CreatedBy = model[0].CreatedBy;
                    regularRequest.CreatedUserID = model[0].CreatedUserID;
                    regularRequest.CreatedEmail = model[0].CreatedEmail;
                    regularRequest.CreatedByName = model[0].CreatedByName;
                    regularRequest.LastUpdatedDate = model[0].LastUpdatedDate;
                    regularRequest.LastUpdatedBy = model[0].LastUpdatedBy;
                    regularRequest.LastUpdatedUserID = model[0].LastUpdatedUserID;
                    regularRequest.LastUpdatedEmail = model[0].LastUpdatedEmail;

                    #region Get the file attachments
                    List<LeaveAttachment> attachments = new();

                    var attachModel = await (from attach in _db.FileAttachments
                                             where attach.AttachmentId == regularRequest.AttachmentId
                                             select attach).ToListAsync();
                    if (attachModel != null)
                    {
                        regularRequest.AttachmentList = attachModel.Select(e => new FileAttachment
                        {
                            Id = e.Id,
                            RequestType = e.RequestType,
                            AttachmentId = e.AttachmentId,
                            FileName = e.FileName,
                            StoredFileName = e.StoredFileName,
                            ContentType = e.ContentType,
                            FileSize = e.FileSize
                        }).ToList();
                    }
                    #endregion

                    #region Get the swipe logs
                    List<AttendanceSwipeLog> swipeLogs = new List<AttendanceSwipeLog>();

                    var swipeModel = await (from log in _db.AttendanceSwipeLogs
                                            where log.EmpNo == regularRequest.EmployeeNo &&
                                                log.SwipeDate == regularRequest.AttendanceDate
                                            select log).ToListAsync();
                    if (swipeModel != null)
                    {
                        regularRequest.SwipeLogList = swipeModel.Select(e => new AttendanceSwipeLog
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                    #endregion
                }

                return Result<RegularizationResult>.SuccessResult(regularRequest);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<RegularizationResult>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Search Regularization Requests 
        /// </summary>
        /// <param name="requestNo"></param>
        /// <param name="empNo"></param>
        /// <param name="costCenter"></param>
        /// <param name="roaCode"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<Result<List<RegularizationResult>>> SearchRegularizationAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? roaCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<RegularizationResult> regularizationList = new();

            try
            {
                var model = (await _db.Set<RegularizationResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetRegularizationDetail @requestNo = {0}, @empNo = {1}, @costCenter = {2}, @roaCode = {3}, @status = {4}, @startDate = {5}, @endDate = {6}",
                    requestNo!,
                    empNo!,
                    costCenter!,
                    roaCode!,
                    status!,
                    startDate!,
                    endDate!)
                    .ToListAsync()).AsEnumerable().OrderByDescending(a => a.RegularizationId);
                if (model != null && model.Any())
                {
                    regularizationList = model.Select(e => new RegularizationResult
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
                        ROADesc = e.ROADesc,
                        ActionCode = e.ActionCode,
                        ActionDesc = e.ActionDesc,
                        RegularizedTimeIn = e.RegularizedTimeIn,
                        RegularizedTimeOut = e.RegularizedTimeOut,
                        ShiftPattern = e.ShiftPattern,
                        ShiftTiming = e.ShiftTiming,
                        WorkDuration = e.WorkDuration,
                        NoPayHours = e.NoPayHours,
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
                        LastUpdatedEmail = e.LastUpdatedEmail
                    }).ToList();

                    if (regularizationList.Any())
                    {
                        foreach (var item in regularizationList)
                        {
                            #region Get the file attachments
                            var attachModel = await (from attach in _db.FileAttachments
                                                     where attach.AttachmentId == item.AttachmentId
                                                     select attach).ToListAsync();
                            if (attachModel != null)
                            {
                                item.AttachmentList = attachModel.Select(e => new FileAttachment
                                {
                                    Id = e.Id,
                                    AttachmentId = e.AttachmentId,
                                    RequestType = e.RequestType,
                                    FileName = e.FileName,
                                    StoredFileName = e.StoredFileName,
                                    ContentType = e.ContentType,
                                    FileSize = e.FileSize
                                }).ToList();
                            }
                            #endregion
                        }
                    }
                }

                return Result<List<RegularizationResult>>.SuccessResult(regularizationList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<RegularizationResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Search Regularization Requests 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="costCenter"></param>
        /// <param name="empNo"></param>
        /// <returns>List<TimecardResult></returns>
        public async Task<Result<List<TimecardResult>>> SearchTimecardAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? costCenter,
            int? empNo)
        {
            List<TimecardResult> attendanceList = new();

            try
            {
                var model = (await _db.Set<TimecardResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetAttendanceTimeCard @startDate = {0}, @endDate = {1}, @costCenter = {2}, @empNo = {3}",
                    startDate!,
                    endDate!,
                    costCenter!,
                    empNo!)
                    .ToListAsync());
                if (model != null && model.Any())
                {
                    attendanceList = model.Select(e => new TimecardResult
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

                return Result<List<TimecardResult>>.SuccessResult(attendanceList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<TimecardResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Add new overtime request 
        /// </summary>
        /// <param name="OTRequestWF"></param>
        /// <returns>Result<long></returns>
        public async Task<Result<long>> AddOTRequestAsync(
            OTRequestWF dto, 
            CancellationToken cancellationToken)
        {
            try
            {
                if (dto is null)
                    throw new ArgumentNullException(nameof(dto));

                #region Initialize entity
                var otRequest = new OTRequestWF
                {
                    TS_AutoId = dto.TS_AutoId,
                    EmployeeNo = dto.EmployeeNo,
                    EmployeeName = dto.EmployeeName,
                    CostCenter = dto.CostCenter,
                    AttendanceDate = dto.AttendanceDate,
                    OTReasonCode = dto.OTReasonCode,
                    ActionCode = dto.ActionCode,
                    OTStartTime = dto.OTStartTime,
                    OTEndTime = dto.OTEndTime,
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

                // Save to database
                await _db.OTRequestWF.AddAsync(otRequest);
                await _db.SaveChangesAsync(cancellationToken);

                // ✅ EF Core automatically populates identity after SaveChanges
                long generatedId = otRequest.ExtratimeId;

                return Result<long>.SuccessResult(generatedId);

            }
            catch (InvalidOperationException invEx)
            {
                throw new Exception(invEx.Message.ToString());
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                    return Result<long>.Failure($"Database error: {ex.InnerException.Message}");
                else
                    return Result<long>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Update overtime request 
        /// </summary>
        /// <param name="OTRequestWF"></param>
        /// <returns>Result<long></returns>
        public async Task<Result<int>> UpdateOTRequestAsync(
            OTRequestWF otRequest, 
            CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (otRequest == null)
                    throw new ArgumentNullException(nameof(otRequest));

                var existing = await _db.OTRequestWF
                    .FirstOrDefaultAsync(e =>
                        e.ExtratimeId == otRequest.ExtratimeId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find overtime request with the specified no.");

                #region Update request information
                existing.OTReasonCode = otRequest.OTReasonCode;
                existing.OTStartTime = otRequest.OTStartTime;
                existing.OTEndTime = otRequest.OTEndTime;
                existing.Remarks = otRequest.Remarks;
                existing.StatusCode = otRequest.StatusCode;
                existing.StatusID = otRequest.StatusID;
                existing.StatusHandlingCode = otRequest.StatusHandlingCode;
                existing.LastUpdatedDate = otRequest.LastUpdatedDate;
                existing.LastUpdatedBy = otRequest.LastUpdatedBy;
                existing.LastUpdatedUserID = otRequest.LastUpdatedUserID;
                existing.LastUpdatedEmail = otRequest.LastUpdatedEmail;

                #endregion

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

        /// <summary>
        /// Cancel overtime request 
        /// </summary>
        /// <param name="OTRequestWF"></param>
        /// <returns>Result<long></returns>
        public async Task<Result<int>> CancelOTRequestAsync(
            OTRequestWF otRequest,
            CancellationToken cancellationToken = default)
        {
            int rowsUpdated = 0;

            try
            {
                if (otRequest == null)
                    throw new ArgumentNullException(nameof(otRequest));

                var existing = await _db.OTRequestWF
                    .FirstOrDefaultAsync(e =>
                        e.ExtratimeId == otRequest.ExtratimeId,
                        cancellationToken);

                if (existing == null)
                    throw new KeyNotFoundException(
                        "Could not find overtime request with the specified request no.");

                #region Set overtime status to "Cancelled by User"
                existing.StatusCode = otRequest.StatusCode;
                existing.StatusID = otRequest.StatusID;
                existing.StatusHandlingCode = otRequest.StatusHandlingCode;
                existing.LastUpdatedDate = otRequest.LastUpdatedDate;
                existing.LastUpdatedBy = otRequest.LastUpdatedBy;
                existing.LastUpdatedUserID = otRequest.LastUpdatedUserID;
                existing.LastUpdatedEmail = otRequest.LastUpdatedEmail;
                #endregion

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

        /// <summary>
        /// Get OvertimeRequest Request details
        /// </summary>
        /// <param name="requestNo"></param>
        /// <returns></returns>
        public async Task<Result<OTRequestResult>> GetOTRequestAsync(long requestNo)
        {
            OTRequestResult otRequest = new();

            try
            {
                var model = await _db.Set<OTRequestResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetOvertimeDetail @requestNo = {0}",
                    requestNo)
                    .ToListAsync();
                if (model != null && model.Any())
                {
                    otRequest.ExtratimeId = model[0].ExtratimeId;
                    otRequest.TS_AutoId = model[0].TS_AutoId;
                    otRequest.WorkflowId = model[0].WorkflowId;
                    otRequest.EmployeeNo = model[0].EmployeeNo;
                    otRequest.EmployeeName = model[0].EmployeeName;
                    otRequest.CostCenter = model[0].CostCenter;
                    otRequest.CostCenterName = model[0].CostCenter;
                    otRequest.AttendanceDate = model[0].AttendanceDate;
                    otRequest.OTReasonCode = model[0].OTReasonCode;
                    otRequest.OTReasonDesc = model[0].OTReasonDesc;
                    otRequest.ActionCode = model[0].ActionCode;
                    otRequest.ActionDesc = model[0].ActionDesc;
                    otRequest.OTStartTime = model[0].OTStartTime;
                    otRequest.OTEndTime = model[0].OTEndTime;
                    otRequest.ShiftPattern = model[0].ShiftPattern;
                    otRequest.ShiftTiming = model[0].ShiftTiming;
                    otRequest.WorkDuration = model[0].WorkDuration;
                    otRequest.OTDuration = model[0].OTDuration;
                    otRequest.Remarks = model[0].Remarks;
                    otRequest.StatusID = model[0].StatusID;
                    otRequest.StatusCode = model[0].StatusCode;
                    otRequest.StatusDesc = model[0].StatusDesc;
                    otRequest.StatusHandlingCode = model[0].StatusHandlingCode;
                    otRequest.CreatedDate = model[0].CreatedDate;
                    otRequest.CreatedBy = model[0].CreatedBy;
                    otRequest.CreatedUserID = model[0].CreatedUserID;
                    otRequest.CreatedEmail = model[0].CreatedEmail;
                    otRequest.CreatedByName = model[0].CreatedByName;
                    otRequest.LastUpdatedDate = model[0].LastUpdatedDate;
                    otRequest.LastUpdatedBy = model[0].LastUpdatedBy;
                    otRequest.LastUpdatedUserID = model[0].LastUpdatedUserID;
                    otRequest.LastUpdatedEmail = model[0].LastUpdatedEmail;
                    otRequest.ApproverNo = model[0].ApproverNo;
                    otRequest.ApproverName = model[0].ApproverName;

                    #region Get the swipe logs
                    List<AttendanceSwipeLog> swipeLogs = new List<AttendanceSwipeLog>();

                    var swipeModel = await (from log in _db.AttendanceSwipeLogs
                                            where log.EmpNo == otRequest.EmployeeNo &&
                                                log.SwipeDate == otRequest.AttendanceDate
                                            select log).ToListAsync();
                    if (swipeModel != null)
                    {
                        otRequest.SwipeLogList = swipeModel.Select(e => new AttendanceSwipeLog
                        {
                            SwipeID = e.SwipeID,
                            EmpNo = e.EmpNo,
                            SwipeDate = e.SwipeDate,
                            SwipeTime = e.SwipeTime,
                            SwipeType = e.SwipeType,
                            SwipeLogDate = e.SwipeLogDate
                        }).ToList();
                    }
                    #endregion
                }

                return Result<OTRequestResult>.SuccessResult(otRequest);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<OTRequestResult>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Search Regularization Requests 
        /// </summary>
        /// <param name="requestNo"></param>
        /// <param name="empNo"></param>
        /// <param name="costCenter"></param>
        /// <param name="otReasonCode"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<Result<List<OTRequestResult>>> SearchOvertimeAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? otReasonCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<OTRequestResult> regularizationList = new();

            try
            {
                var model = (await _db.Set<OTRequestResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_GetOvertimeDetail @requestNo = {0}, @empNo = {1}, @costCenter = {2}, @otReasonCode = {3}, @status = {4}, @startDate = {5}, @endDate = {6}",
                    requestNo!,
                    empNo!,
                    costCenter!,
                    otReasonCode!,
                    status!,
                    startDate!,
                    endDate!)
                    .ToListAsync()).AsEnumerable().OrderByDescending(a => a.ExtratimeId);
                if (model != null && model.Any())
                {
                    regularizationList = model.Select(e => new OTRequestResult
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
                    }).ToList();
                }

                return Result<List<OTRequestResult>>.SuccessResult(regularizationList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<OTRequestResult>>.Failure($"Database error: {ex.Message}");
            }
        }

        /// <summary>
        /// Search Regularization Requests 
        /// </summary>
        /// <param name="requestNo"></param>
        /// <param name="empNo"></param>
        /// <param name="costCenter"></param>
        /// <param name="requestType"></param>
        /// <param name="status"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public async Task<Result<List<AttendanceCorrectionResult>>> SearchAttendanceCorrectionAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? requestType,
            string? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            List<AttendanceCorrectionResult> correctionList = new();

            try
            {
                var model = (await _db.Set<AttendanceCorrectionResult>()
                    .FromSqlRaw("EXEC kenuser.Pr_SearchAttendanceCorrection @requestNo = {0}, @empNo = {1}, @costCenter = {2}, @requestType = {3}, @status = {4}, @startDate = {5}, @endDate = {6}",
                    requestNo!,
                    empNo!,
                    costCenter!,
                    requestType!,
                    status!,
                    startDate!,
                    endDate!)
                    .ToListAsync());
                if (model != null && model.Any())
                {
                    correctionList = model.Select(e => new AttendanceCorrectionResult
                    {
                        RequestTypeCode = e.RequestTypeCode,
                        RequestTypeDesc = e.RequestTypeDesc,
                        RequestNo = e.RequestNo,
                        RequestDate = e.RequestDate,
                        OrigEmpNo = e.OrigEmpNo,
                        OrigEmpName = e.OrigEmpName,
                        CostCenter = e.CostCenter,
                        CostCenterName = e.CostCenterName,
                        AppliedDate = e.AppliedDate,
                        RequestedByNo = e.RequestedByNo,
                        RequestedByName = e.RequestedByName,
                        RequestDetail = e.RequestDetail,
                        //CreatedByEmpNo = e.CreatedByEmpNo,
                        CurrentStatus = e.CurrentStatus
                    }).ToList();
                }

                return Result<List<AttendanceCorrectionResult>>.SuccessResult(correctionList);
            }
            catch (Exception ex)
            {
                // Log error here if needed (Serilog, NLog, etc.)
                return Result<List<AttendanceCorrectionResult>>.Failure($"Database error: {ex.Message}");
            }
        }
        #endregion
    }
}
