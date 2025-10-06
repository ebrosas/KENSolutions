using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KenHRApp.Application.DTOs;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Models.Common;

namespace KenHRApp.Application.Interfaces
{
    public interface IEmployeeService
    {
        #region Public Methods
        Task<List<EmployeeDTO>> GetAllAsync();
        Task AddAsync(EmployeeDTO employee);
        Task<List<EmergencyContactDTO>> GetEmergencyContactAsync();
        Task<List<QualificationDTO>> GetQualicationsAsync();
        Task<List<EmployeeSkillDTO>> GetEmployeeSkillAsync();
        Task<List<EmployeeCertificationDTO>> GetCertificationAsync();
        Task<List<LanguageSkillDTO>> GetLanguageSkillsAsync();
        Task<List<FamilyMemberDTO>> GetFamilyMembersAsync();
        Task<List<FamilyVisaDTO>> GetFamilyVisasAsync();
        Task<List<EmploymentHistoryDTO>> GetEmploymentHistoryAsync();
        Task<List<OtherDocumentDTO>> GetOtherDocumentAsync();
        Task<List<EmployeeTransactionDTO>> GetEmployeeTransactionAsync();
        Task<List<EmployeeMasterDTO>> GetEmployeeMasterAsync();
        Task<Result<List<UserDefinedCodeDTO>>> GetUserDefinedCodeAsync(string udcKey);
        Task<Result<List<EmployeeMasterDTO>>> SearchEmployeeAsync(int? empNo, string? firstName, string? lastName, int? managerEmpNo,
            DateTime? joiningDate, string? statusCode, string? employmentType, string? departmentCode);
        Task<Result<List<DepartmentDTO>>> GetDepartmentMasterAsync();
        Task<Result<List<EmployeeDTO>>> GetReportingManagerAsync();
        Task<Result<EmployeeDTO>> GetEmployeeDetailAsync(int employeeId);
        Task<Result<List<UserDefinedCodeDTO>>> GetUserDefinedCodeAllAsync();
        Task<Result<List<UserDefinedCodeGroupDTO>>> GetUserDefinedCodeGroupAsync();
        Task<Result<int>> GetMaxEmployeeNoAsync();
        Task<Result<int>> SaveEmployeeAsync(EmployeeDTO employee, CancellationToken cancellationToken = default);
        Task<Result<int>> AddEmployeeAsync(EmployeeDTO employee, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteEmployeeAsync(int employeeId, CancellationToken cancellationToken = default);
        Task<Result<List<DepartmentDTO>>> SearchDepartmentAsync(string departmentName, string description, int? superintendentNo, int? managerEmpNo, bool isActive,
            string departmentCode = "", string groupCode = "");
        Task<Result<int>> SaveDepartmentAsync(DepartmentDTO department, CancellationToken cancellationToken = default);
        Task<Result<bool>> DeleteDepartmentAsync(int departmentID, CancellationToken cancellationToken = default);
        Task<Result<int>> AddDepartmentAsync(DepartmentDTO dto, CancellationToken cancellationToken = default);
        #endregion
    }
}
