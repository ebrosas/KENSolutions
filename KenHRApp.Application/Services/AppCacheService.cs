using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Interfaces;
using KenHRApp.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Services
{
    public class AppCacheService : IAppCacheService
    {
        #region Fields
        private readonly IAppCacheRepository _repository;
        private const string DepartmentCacheKeyPrefix = "departments_";
        private const string EmployeeCacheKeyPrefix = "employees_";
        #endregion

        #region Constructors
        public AppCacheService(IAppCacheRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region Public Methods  
        public async Task<bool> CheckIfKeyExistAsync(string key)
        {
            return await _repository.ExistsAsync(key);
        }

        public async Task<string> StoreDepartmentsAsync(List<DepartmentDTO> departments)
        {
            var key = $"{DepartmentCacheKeyPrefix}{Guid.NewGuid()}";
            await _repository.SetAsync(key, departments, TimeSpan.FromMinutes(30));
            return key; // return reference key for query string
        }

        public async Task<List<DepartmentDTO>?> GetDepartmentsAsync(string key)
        {
            return await _repository.GetAsync<List<DepartmentDTO>>(key);
        }

        public async Task<string> StoreEmployeesAsync(List<EmployeeDTO> employees)
        {
            var key = $"{EmployeeCacheKeyPrefix}{Guid.NewGuid()}";
            await _repository.SetAsync(key, employees, TimeSpan.FromMinutes(30));
            return key; // return reference key for query string
        }

        public async Task<List<EmployeeDTO>?> GetEmployeesAsync(string key)
        {
            return await _repository.GetAsync<List<EmployeeDTO>>(key);
        }
        #endregion
    }
}
