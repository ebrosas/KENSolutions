using KenHRApp.Application.Common.Models;
using KenHRApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>Backs the Employee Directory grid: search, advanced filters, sort and server-side paging.</summary>
namespace KenHRApp.Application.Features.Employees.Queries
{
    public sealed class GetEmployeesQuery : PagedQuery
    {
        //public Guid? DepartmentId { get; set; }
        public string? DepartmentId { get; set; }
        //public Guid? PositionId { get; set; }
        public string? PositionId { get; set; }
        //public Guid? LocationId { get; set; }
        public string? LocationId { get; set; }
        //public Guid? ManagerId { get; set; }
        public int? ManagerId { get; set; }
        //public EmployeeStatus? Status { get; set; }
        public string? Status { get; set; }

        /// <summary>Multi-select status filter from the advanced filter panel.</summary>
        public IReadOnlyCollection<EmployeeStatus> Statuses { get; set; } = Array.Empty<EmployeeStatus>();

        //public EmploymentType? EmploymentType { get; set; }
        public string? EmploymentType { get; set; }
        public DateOnly? JoinedFrom { get; set; }
        public DateOnly? JoinedTo { get; set; }

        public GetEmployeesQuery Clone() => (GetEmployeesQuery)MemberwiseClone();
    }
}
