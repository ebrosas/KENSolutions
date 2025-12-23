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
        Task<Result<List<MasterShiftPatternTitle>?>> SearchShiftRosterMasterAsync(byte loadType, string? shiftPatternCode, string? shiftCode, byte? activeFlag);
        #endregion
    }
}
