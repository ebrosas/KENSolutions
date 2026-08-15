using KenHRApp.Domain.Common;

namespace KenHRApp.Domain.Entities;

public class Position : AuditableEntity
{
    public string Code { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string? TitleAr { get; set; }
    public int Grade { get; set; }
    public Guid? DepartmentId { get; set; }
    public DepartmentMaster? Department { get; set; }
    public decimal? MinSalary { get; set; }
    public decimal? MaxSalary { get; set; }
    public bool IsActive { get; set; } = true;

    public ICollection<Employee> Employees { get; set; } = new List<Employee>();
}
