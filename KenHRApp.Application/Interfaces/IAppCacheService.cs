using KenHRApp.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Interfaces
{
    public interface IAppCacheService
    {
        Task<bool> CheckIfKeyExistAsync(string key);

        Task<string> StoreDepartmentsAsync(List<DepartmentDTO> departments);

        Task<List<DepartmentDTO>?> GetDepartmentsAsync(string key);

        Task<string> StoreEmployeesAsync(List<EmployeeDTO> employees);

        Task<List<EmployeeDTO>?> GetEmployeesAsync(string key);
    }
}
