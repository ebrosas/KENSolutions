using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;

namespace KenHRApp.Infrastructure.Repositories
{
    public interface IEmployeeRepository
    {
        #region Public Methods
        Task<List<Employee>> GetAllAsync();
        Task AddAsync(Employee employee);
        Task<Result<List<UserDefinedCode>?>> GetUserDefinedCodeAsync(string udcKey);
        Task<Result<List<EmployeeMaster>?>> SearchEmployeeAsync(int? empNo, string? firstName, string? lastName, int? managerEmpNo,
            DateTime? joiningDate, string? statusCode, string? employmentType, string? departmentCode);
        Task<Result<List<DepartmentMaster>?>> GetDepartmentMasterAsync(string? departmentCode, string? departmentName, string description, string? groupCode,
            int? superintendentNo, int? managerEmpNo, bool isActive);
        Task<Result<List<Employee>?>> GetReportingManagerAsync(string? departmentCode, bool isActive);
        Task<Result<Employee?>> GetEmployeeDetailAsync(int employeeId);
        Task<RepositoryResult<List<UserDefinedCode>>> GetAllUserDefinedCodeAsync();
        Task<Result<List<UserDefinedCode>>> GetUserDefinedCodeAllAsync();
        Task<Result<List<UserDefinedCodeGroup>>> GetUserDefinedCodeGroupAsync();
        Task<Result<int>> GetMaxEmployeeNoAsync();
        Task<Result<int>> SaveEmployeeAsync(Employee dto, CancellationToken cancellationToken = default);
        Task<Result<int>> AddEmployeeAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<Result<int>> SaveDepartmentAsync(DepartmentMaster dto, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteDepartmentAsync(int departmentID, CancellationToken cancellationToken = default);
        Task<Result<int>> AddDepartmentAsync(DepartmentMaster department, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteEmergencyContactAsync(int autoID, CancellationToken cancellationToken = default);
        Task<Result<int>> UpdateEmergencyContactAsync(EmergencyContact dto, CancellationToken cancellationToken = default);
        Task<Result<int>> AddEmergencyContactAsync(EmergencyContact contact, CancellationToken cancellationToken = default);
        Task<Result<Employee?>> GetByUserIDOrEmailAsync(string userIdOrEmail, CancellationToken cancellationToken = default);
        Task<Result<int>> UnlockUserAccountAsync(string userIdOrEmail, CancellationToken cancellationToken = default);
        Task<Result<int>> UpdateAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Result<Employee?>> GetByEmployeeCodeAndHireDateAsync(string employeeCode, DateTime joiningDate, CancellationToken cancellationToken = default);
        Task<Result<int>> RegisterUserAccountAsync(Employee employee, CancellationToken cancellationToken = default);
        Task<Result<Employee?>> GetByEmployeeNoAndHireDateAsync(int? empNo, DateTime doj, CancellationToken cancellationToken = default);
        Task<Result<bool>> VerifyEmailTokenAsync(string token, CancellationToken cancellationToken = default);
        Task<Result<string>> GenerateEmailVerificationTokenAsync(int empNo, DateTime doj, CancellationToken cancellationToken = default);
        #endregion
    }
}
