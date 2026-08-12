using KenHRApp.Application.Common.Models;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Features.Employees.Queries;
using KenHRApp.Domain.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Features.Employees.Interfaces
{
    public interface ILoveEmployeeService
    {
        Task<PagedResult<EmployeeDTO>> GetEmployeesAsync(GetEmployeesQuery query, CancellationToken cancellationToken = default);

        //Task<Result<EmployeeDetailDto>> GetEmployeeAsync(GetEmployeeByIdQuery query, CancellationToken cancellationToken = default);

        //Task<Result<Guid>> CreateAsync(CreateEmployeeCommand command, CancellationToken cancellationToken = default);

        //Task<Result> UpdateAsync(UpdateEmployeeCommand command, CancellationToken cancellationToken = default);

        //Task<Result> DeactivateAsync(DeactivateEmployeeCommand command, CancellationToken cancellationToken = default);

        //Task<Result> DeleteAsync(DeleteEmployeeCommand command, CancellationToken cancellationToken = default);

        ///// <summary>Authorization boundary for the signed-in user — drives both queries and UI affordances.</summary>
        //Task<EmployeeAccessContext> GetAccessContextAsync(CancellationToken cancellationToken = default);

        ///// <summary>Lightweight manager picker source, scoped to what the caller may see.</summary>
        //Task<IReadOnlyList<EmployeeLookupDto>> GetLookupAsync(string? searchTerm, int take = 25, CancellationToken cancellationToken = default);
    }
}
