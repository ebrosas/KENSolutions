using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Features.Employees.Models;

/// <summary>How much of the employee master the signed-in user may see.</summary>
public enum EmployeeAccessScope
{
    /// <summary>No access at all.</summary>
    None = 0,

    /// <summary>Own record only.</summary>
    Self = 1,

    /// <summary>Own record plus the direct and indirect reporting line.</summary>
    Team = 2,

    /// <summary>Every employee record.</summary>
    All = 3,
}

public sealed class EmployeeAccessContext
{
    public EmployeeAccessContext(
        EmployeeAccessScope scope,
        Guid? employeeId,
        IReadOnlyCollection<Guid> visibleEmployeeIds,
        bool canManage)
    {
        Scope = scope;
        EmployeeId = employeeId;
        VisibleEmployeeIds = visibleEmployeeIds;
        CanManage = canManage;
    }

    public EmployeeAccessScope Scope { get; }

    /// <summary>The employee record linked to the signed-in identity, when there is one.</summary>
    public Guid? EmployeeId { get; }

    /// <summary>Employee ids visible under <see cref="EmployeeAccessScope.Team"/> or <see cref="EmployeeAccessScope.Self"/>.</summary>
    public IReadOnlyCollection<Guid> VisibleEmployeeIds { get; }

    /// <summary>True only for HR back-office roles — controls add, edit, deactivate and delete.</summary>
    public bool CanManage { get; }

    public bool CanRead(Guid employeeId) => Scope switch
    {
        EmployeeAccessScope.All => true,
        EmployeeAccessScope.Team or EmployeeAccessScope.Self => VisibleEmployeeIds.Contains(employeeId),
        _ => false,
    };

    public static EmployeeAccessContext Denied { get; } =
        new(EmployeeAccessScope.None, null, Array.Empty<Guid>(), false);
}