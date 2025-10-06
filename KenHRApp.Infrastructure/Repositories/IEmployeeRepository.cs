﻿using System;
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
        #endregion
    }
}
