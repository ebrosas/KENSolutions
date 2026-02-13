using KenHRApp.Application.DTOs;
using KenHRApp.Domain.Entities.KeylessModels;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IAttendanceService
    {
        #region Public Methods
        Task<Result<List<ShiftPatternMasterDTO>>> SearchShiftRosterMasterAsync(byte loadType, int? shiftPatternId, string? shiftPatternCode, string? shiftCode, byte? activeFlag);
        Task<Result<ShiftPatternMasterDTO?>> GetShiftRosterDetailAsync(int shiftPatternId);
        Task<Result<int>> AddShiftRosterMasterAsync(ShiftPatternMasterDTO shiftRoster, CancellationToken cancellationToken = default);
        Task<Result<int>> UpdateShiftRosterMasterAsync(ShiftPatternMasterDTO shiftRoster, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteShiftRosterMasterAsync(int shiftPatternId, CancellationToken cancellationToken = default);
        Task<Result<int>> AddShiftPatternChangeAsync(List<EmployeeRosterDTO> shiftRosterList, CancellationToken cancellationToken = default);
        Task<Result<int>> UpdateShiftPatternChangeAsync(EmployeeRosterDTO dto, CancellationToken cancellationToken = default);
        Task<Result<List<ShiftPatternChangeDTO>>> SearchShiftPatternChangeAsync(byte loadType, int? autoID, int? empNo, string? changeType, string? shiftPatternCode, DateTime? startDate, DateTime? endDate);
        Task<Result<ShiftPatternChangeDTO?>> GetShiftPatternChangeAsync(int autoID);
        Task<Result<bool>> DeleteShiftPatternChangeAsync(int autoID, CancellationToken cancellationToken = default);
        Task<Result<List<HolidayDTO>>> GetPublicHolidaysAsync(int? year, byte? holidayType);
        Task<Result<List<UserDefinedCodeDTO>>> GetUserDefinedCodeAsync(string? udcCode);
        Task<Result<AttendanceSummaryDTO>> GetAttendanceSummaryAsync(int empNo, DateTime? startDate, DateTime? endDate);
        Task<Result<AttendanceDetailDTO>> GetAttendanceDetailAsync(int empNo, DateTime attendanceDate);
        Task<Result<int>> SaveSwipeDataAsync(AttendanceSwipeDTO swipeData, CancellationToken cancellationToken = default);
        Task<Result<AttendanceDurationDTO>> GetAttendanceDurationAsync(int empNo, DateTime attendanceDate);
        Task<Result<List<PayrollPeriodResultDTO>>> GetPayrollPeriodAsync(int fiscalYear = 0);
        #endregion
    }
}
