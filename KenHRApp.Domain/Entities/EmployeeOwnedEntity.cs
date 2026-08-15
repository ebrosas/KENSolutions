using KenHRApp.Domain.Common;

namespace KenHRApp.Domain.Entities;

/// <summary>Base type for every child record that belongs to a single employee.</summary>
public abstract class EmployeeOwnedEntity : AuditableEntity
{
    public Guid EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
