using KenHRApp.Application.DTOs;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Interfaces
{
    public interface ILookupCacheService
    {
        #region Properties
        bool IsEmployeeSearch { get; set; }
        #endregion
        
        #region Public Methods
        void ClearCache(); // optional, e.g. after admin updates lookup data

        Task<Result<IReadOnlyList<EmployeeMasterDTO>>> SearchEmployeeAsync(int? empNo, string? firstName, string? lastName, int? managerEmpNo,
            DateTime? joiningDate, string? statusCode, string? employmentType, string? departmentCode, bool forceLoad = false);

        Task<Result<IReadOnlyList<UserDefinedCodeDTO>>> GetUserDefinedCodeAsync(string udcKey, bool forceLoad = false);

        Task<Result<IReadOnlyList<UserDefinedCodeDTO>>> GetEmploymentTypeAsync(bool forceLoad = false);

        Task<Result<IReadOnlyList<UserDefinedCodeDTO>>> GetEmployeeStatusAsync(bool forceLoad = false);

        Task<Result<IReadOnlyList<DepartmentDTO>>> GetDepartmentMasterAsync(bool forceLoad = false);

        Task<Result<IReadOnlyList<EmployeeDTO>>> GetReportingManagerAsync(bool forceLoad = false);
        #endregion
    }
}
