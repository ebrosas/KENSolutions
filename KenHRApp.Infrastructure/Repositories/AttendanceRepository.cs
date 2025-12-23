using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using KenHRApp.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<Result<List<MasterShiftPatternTitle>?>> SearchShiftRosterMasterAsync(byte loadType, string? shiftPatternCode, string? shiftCode, byte? activeFlag)
        {
            List<MasterShiftPatternTitle> shiftRosterList = new List<MasterShiftPatternTitle>();

            try
            {
                var model = await _db.MasterShiftPatternTitles
                    .FromSqlRaw("EXEC kenuser.Pr_GetShiftRoster @loadType = {0}, @shiftPatternCode = {1}, @shiftCode = {2}, @activeFlag = {3}",
                    loadType, shiftPatternCode!, shiftCode!, activeFlag!)
                    .ToListAsync();
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        shiftRosterList.Add(new MasterShiftPatternTitle()
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
                        });
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
        #endregion
    }
}
