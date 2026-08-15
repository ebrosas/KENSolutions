using KenHRApp.Application.Features.Employees.Models;

namespace KenHRApp.Application.Features.Employees.Interfaces;

/// <summary>Resolves the employee-master authorization boundary for the signed-in user.</summary>
public interface IEmployeeAccessService
{
    Task<EmployeeAccessContext> GetAccessContextAsync(CancellationToken cancellationToken = default);
}
