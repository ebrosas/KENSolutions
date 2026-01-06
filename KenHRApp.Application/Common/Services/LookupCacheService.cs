using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Services
{
    public class LookupCacheService : ILookupCacheService
    {
        #region Fields
        private readonly IEmployeeService _employeeService;
        private List<EmployeeMasterDTO>? _employeeList;
        private List<UserDefinedCodeDTO>? _userDefinedCodeList;
        private List<UserDefinedCodeDTO>? _employmentTypeList;
        private List<UserDefinedCodeDTO>? _employeeStatusList;
        private List<UserDefinedCodeDTO>? _changeTypeList;
        private List<DepartmentDTO>? _departmentList;
        private List<EmployeeDTO>? _reportingManagerList;

        #region Enums
        private enum UDCKeys
        {
            EMPSTATUS,      // Employee Status
            EMPLOYTYPE,     // Employment Type
            DEPARTMENT,     // Departments
            SHIFTCHANGETYPE // Shift Pattern Change Type
        }
        #endregion

        #endregion

        #region Constructor
        public LookupCacheService(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        #endregion

        #region Properties
        public bool IsEmployeeSearch { get; set; }
        #endregion

        #region Public Methods
        public void ClearCache()
        {
            _employeeList = null;
            _userDefinedCodeList = null;
            _employmentTypeList = null;
            _employeeStatusList = null;
            _departmentList = null;
            _reportingManagerList = null;   
        }

        public void ClearCache(string key)
        {
            if (key == UDCKeys.DEPARTMENT.ToString())
                _departmentList = null;
        }

        public async Task<Result<IReadOnlyList<EmployeeMasterDTO>>> SearchEmployeeAsync(int? empNo, string? firstName, string? lastName, int? managerEmpNo,
            DateTime? joiningDate, string? statusCode, string? employmentType, string? departmentCode, bool forceLoad = false)
        {
            if ((_employeeList == null || !_employeeList.Any()) || forceLoad)
            {
                var repoResult = await _employeeService.SearchEmployeeAsync(empNo, firstName, lastName, managerEmpNo, joiningDate, 
                    statusCode, employmentType, departmentCode);

                if (!repoResult.Success)
                {
                    return Result<IReadOnlyList<EmployeeMasterDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                _employeeList = repoResult.Value!.Select(e => new EmployeeMasterDTO
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeNo = e.EmployeeNo,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Gender = e.Gender,
                    HireDate = e.HireDate,
                    EmploymentTypeCode = e.EmploymentTypeCode,
                    EmploymentType = e.EmploymentType,
                    ReportingManagerCode = e.ReportingManagerCode,
                    ReportingManager = e.ReportingManager,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    EmployeeStatusCode = e.EmployeeStatusCode,
                    EmployeeStatus = e.EmployeeStatus
                }).ToList();
            }

            return Result<IReadOnlyList<EmployeeMasterDTO>>.SuccessResult(_employeeList);
        }

        public async Task<Result<IReadOnlyList<UserDefinedCodeDTO>>> GetUserDefinedCodeAsync(string udcKey, bool forceLoad = false)
        {
            if ((_userDefinedCodeList == null || !_userDefinedCodeList.Any()) || forceLoad)
            {
                var repoResult = await _employeeService.GetUserDefinedCodeAsync(udcKey);
                if (!repoResult.Success)
                {
                    return Result<IReadOnlyList<UserDefinedCodeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                _userDefinedCodeList = repoResult.Value;
            }

            return Result<IReadOnlyList<UserDefinedCodeDTO>>.SuccessResult(_userDefinedCodeList!);
        }

        public async Task<Result<IReadOnlyList<UserDefinedCodeDTO>>> GetEmploymentTypeAsync(bool forceLoad = false)
        {
            if ((_employmentTypeList == null || !_employmentTypeList.Any()) || forceLoad)
            {
                var repoResult = await _employeeService.GetUserDefinedCodeAsync(UDCKeys.EMPLOYTYPE.ToString());
                if (!repoResult.Success)
                {
                    return Result<IReadOnlyList<UserDefinedCodeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                _employmentTypeList = repoResult.Value;
            }

            return Result<IReadOnlyList<UserDefinedCodeDTO>>.SuccessResult(_employmentTypeList!);
        }

        public async Task<Result<IReadOnlyList<UserDefinedCodeDTO>>> GetEmployeeStatusAsync(bool forceLoad = false)
        {
            if ((_employeeStatusList == null || !_employeeStatusList.Any()) || forceLoad)
            {
                var repoResult = await _employeeService.GetUserDefinedCodeAsync(UDCKeys.EMPSTATUS.ToString());
                if (!repoResult.Success)
                {
                    return Result<IReadOnlyList<UserDefinedCodeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                _employeeStatusList = repoResult.Value;
            }

            return Result<IReadOnlyList<UserDefinedCodeDTO>>.SuccessResult(_employeeStatusList!);
        }

        public async Task<Result<IReadOnlyList<DepartmentDTO>>> GetDepartmentMasterAsync(bool forceLoad = false)
        {
            if ((_departmentList == null || !_departmentList.Any()) || forceLoad)
            {
                var repoResult = await _employeeService.GetDepartmentMasterAsync();
                if (!repoResult.Success)
                {
                    return Result<IReadOnlyList<DepartmentDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                _departmentList = repoResult.Value;
            }

            return Result<IReadOnlyList<DepartmentDTO>>.SuccessResult(_departmentList!);
        }

        public async Task<Result<IReadOnlyList<EmployeeDTO>>> GetReportingManagerAsync(bool forceLoad = false)
        {
            if ((_reportingManagerList == null || !_reportingManagerList.Any()) || forceLoad)
            {
                var repoResult = await _employeeService.GetReportingManagerAsync();
                if (!repoResult.Success)
                {
                    return Result<IReadOnlyList<EmployeeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                _reportingManagerList = repoResult.Value;
            }

            return Result<IReadOnlyList<EmployeeDTO>>.SuccessResult(_reportingManagerList!);
        }

        public async Task<Result<IReadOnlyList<UserDefinedCodeDTO>>> GetChangeTypeAsync(bool forceLoad = false)
        {
            if ((_changeTypeList == null || !_changeTypeList.Any()) || forceLoad)
            {
                var repoResult = await _employeeService.GetUserDefinedCodeAsync(UDCKeys.SHIFTCHANGETYPE.ToString());
                if (!repoResult.Success)
                {
                    return Result<IReadOnlyList<UserDefinedCodeDTO>>.Failure(repoResult.Error ?? "Unknown repository error");
                }

                _changeTypeList = repoResult.Value;
            }

            return Result<IReadOnlyList<UserDefinedCodeDTO>>.SuccessResult(_changeTypeList!);
        }
        #endregion
    }
}
