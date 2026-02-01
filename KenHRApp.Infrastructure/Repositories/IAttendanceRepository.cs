using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Infrastructure.Repositories
{
    public interface IAttendanceRepository
    {
        #region Public Methods
        Task<Result<int>> AddShiftRosterMasterAsync(MasterShiftPatternTitle shiftRoster, CancellationToken cancellationToken = default);
        Task<Result<int>> UpdateShiftRosterMasterAsync(MasterShiftPatternTitle dto, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteShiftRosterMasterAsync(int shiftPatternId, CancellationToken cancellationToken = default);
        Task<Result<List<MasterShiftPatternTitle>?>> SearchShiftRosterMasterAsync(byte loadType, int? shiftPatternId, string? shiftPatternCode, string? shiftCode, byte? activeFlag);
        Task<Result<MasterShiftPatternTitle?>> GetShiftRosterDetailAsync(int shiftPatternId);
        Task<Result<int>> AddShiftPatternChangeAsync(List<ShiftPatternChange> dto, CancellationToken cancellationToken = default);
        Task<Result<List<ShiftPatternChange>?>> SearchShiftPatternChangeAsync(byte loadType, int? autoID, int? empNo, string? changeType, string? shiftPatternCode, DateTime? startDate, DateTime? endDate);
        Task<Result<ShiftPatternChange?>> GetShiftPatternChangeAsync(int autoID);
        Task<Result<List<ShiftPatternChange>?>> GetShiftRosterChangeLogAsync(int? autoID, int? empNo, string? changeType, string? shiftPatternCode, DateTime? startDate, DateTime? endDate);
        Task<Result<bool>> DeleteShiftPatternChangeAsync(int autoId, CancellationToken cancellationToken = default);
        Task<Result<int>> UpdateShiftPatternChangeAsync(ShiftPatternChange dto, CancellationToken cancellationToken = default);
        Task<Result<List<Holiday>>> GetPublicHolidaysAsync(int? year, byte? holidayType);
        Task<Result<List<UserDefinedCode>>> GetUserDefinedCodeAsync(string? udcCode);
        Task<Result<AttendanceSummaryResult>> GetAttendanceSummaryAsync(int empNo, DateTime? startDate, DateTime? endDate);
        Task<Result<AttendanceDetailResult>> GetAttendanceDetailAsync(int empNo, DateTime attendanceDate);
        Task<Result<int>> AddAttendanceSwipeLogAsync(AttendanceSwipeLog dto, CancellationToken cancellationToken = default);
        Task<Result<AttendanceDurationResult>> GetAttendanceDurationAsync(int empNo, DateTime attendanceDate);
        #endregion
    }
}
