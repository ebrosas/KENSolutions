using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Domain.Entities;

namespace KenHRApp.Application.Features.Employees.Interfaces;

public interface ILoveEmployeeRepository: IRepository<Employee>
{
    Task<Employee?> GetByEmployeeNumberAsync(string employeeNumber, CancellationToken cancellationToken = default);

    Task<bool> EmployeeNumberExistsAsync(string employeeNumber, Guid? excludeId = null, CancellationToken cancellationToken = default);

    Task<Employee?> GetDetailAsync(Guid id, CancellationToken cancellationToken = default);
}
