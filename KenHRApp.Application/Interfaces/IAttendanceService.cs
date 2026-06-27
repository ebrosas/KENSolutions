using KenHRApp.Application.DTOs;
using KenHRApp.Application.DTOs.TNA;
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
        Task<Result<List<AttendanceCalendarDTO>>> GetAttendanceCalendarAsync(int empNo, int year, int month, CancellationToken cancellationToken = default);

        Task<Result<AttendanceInfoResultDTO>> GetAttendanceInfoAsync(
            int empNo,
            DateTime attendanceDate);

        Task<Result<long>> AddRegularRequestAsync(
            RegularRequestDTO dto,
            List<FileUploadDTO> files,
            string webRootPath,
            CancellationToken cancellationToken = default);

        Task<Result<int>> UpdateRegularizRequestAsync(
            RegularRequestDTO dto,
            CancellationToken cancellationToken = default);

        Task<Result<int>> CancelRegularRequestAsync(
            RegularRequestDTO dto,
            CancellationToken cancellationToken = default);

        Task<Result<RegularRequestDTO?>> GetRegularRequestAsync(long requestNo);

        Task<Result<List<RegularRequestDTO>>> SearchRegularizationAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? roaCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<List<TimecardResultDTO>>> SearchTimecardAsync(
            DateTime? startDate,
            DateTime? endDate,
            string? costCenter,
            int? empNo);

        Task<Result<long>> AddOTRequestAsync(
            ExtraTimeRequestDTO dto,
            CancellationToken cancellationToken = default);

        Task<Result<int>> UpdateOTRequestAsync(
            ExtraTimeRequestDTO dto,
            CancellationToken cancellationToken = default);

        Task<Result<int>> CancelOTRequestAsync(
            ExtraTimeRequestDTO dto,
            CancellationToken cancellationToken = default);

        Task<Result<ExtraTimeRequestDTO?>> GetOTRequestAsync(long requestNo);

        Task<Result<List<ExtraTimeRequestDTO>>> SearchOvertimeAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? otReasonCode,
            string? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<List<AttendanceCorrectionDTO>>> SearchAttendanceCorrectionAsync(
            long? requestNo,
            int? empNo,
            string? costCenter,
            string? requestType,
            string? status,
            DateTime? startDate,
            DateTime? endDate);

        Task<Result<OutdoorRequestDTO?>> GetOutdoorRequestAsync(long requestNo);
        Task<Result<long>> AddOutdoorRequestAsync(
            OutdoorRequestDTO dto,
            List<FileUploadDTO> files,
            string webRootPath,
            CancellationToken cancellationToken = default);

        Task<Result<int>> UpdateOutdoorRequestAsync(
           OutdoorRequestDTO dto,
           CancellationToken cancellationToken = default);

        Task<Result<int>> CancelOutdoorRequestAsync(
            OutdoorRequestDTO dto,
            CancellationToken cancellationToken = default);
        #endregion
    }
}
