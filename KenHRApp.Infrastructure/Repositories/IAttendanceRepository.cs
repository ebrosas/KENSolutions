using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Entities.KeylessModels;
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
        Task<Result<List<PayrollPeriodResult>>> GetPayrollPeriodAsync(int fiscalYear = 0);
        Task<Result<List<AttendanceCalendarResult>>> GetAttendanceCalendarAsync(int empNo, int year, int month, CancellationToken cancellationToken);

        Task<Result<AttendanceInfoResult>> GetAttendanceInfoAsync(
            int empNo,
            DateTime attendanceDate);

        Task<Result<long>> AddRegularRequestAsync(RegularRequestWF dto, CancellationToken cancellationToken);
        Task<Result<int>> UpdateRegularRequestAsync(RegularRequestWF regularRequest, CancellationToken cancellationToken = default);
        Task<Result<int>> CancelRegularRequestAsync(RegularRequestWF regularRequest, CancellationToken cancellationToken = default);
        Task<Result<RegularizationResult>> GetRegularRequestAsync(long requestNo);

        Task<Result<List<RegularizationResult>>> SearchRegularizationAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? roaCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<List<TimecardResult>>> SearchTimecardAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? costCenter,
            int? empNo);

        Task<Result<long>> AddOTRequestAsync(
            OTRequestWF dto,
            CancellationToken cancellationToken);

        Task<Result<int>> UpdateOTRequestAsync(
            OTRequestWF otRequest,
            CancellationToken cancellationToken = default);

        Task<Result<int>> CancelOTRequestAsync(
            OTRequestWF otRequest,
            CancellationToken cancellationToken = default);

        Task<Result<OTRequestResult>> GetOTRequestAsync(long requestNo);

        Task<Result<List<OTRequestResult>>> SearchOvertimeAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? otReasonCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<List<AttendanceCorrectionResult>>> SearchAttendanceCorrectionAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? requestType,
            string? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<OutdoorRequestWF>> GetOutdoorRequestAsync(long requestNo);

        Task<Result<long>> AddOutdoorRequestAsync(
            OutdoorRequestWF dto,
            CancellationToken cancellationToken);

        Task<Result<int>> UpdateOutdoorRequestAsync(
            OutdoorRequestWF outdoorRequest,
            CancellationToken cancellationToken = default);

        Task<Result<int>> CancelOutdoorRequestAsync(
            OutdoorRequestWF outdoorRequest,
            CancellationToken cancellationToken = default);
        #endregion
    }
}
