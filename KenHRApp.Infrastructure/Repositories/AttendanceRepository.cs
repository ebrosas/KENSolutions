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
        #endregion
    }
}
