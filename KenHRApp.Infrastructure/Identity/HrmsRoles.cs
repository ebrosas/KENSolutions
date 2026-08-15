namespace KenHRApp.Infrastructure.Identity;

public static class HrmsRoles
{
    public const string SystemAdministrator = "System Administrator";
    public const string HrAdministrator = "HR Administrator";
    public const string HrManager = "HR Manager";
    public const string Manager = "Manager";
    public const string Supervisor = "Supervisor";
    public const string Employee = "Employee";
    public const string PayrollAdministrator = "Payroll Administrator";
    public const string TimeOfficeAdministrator = "Time Office Administrator";

    public static IReadOnlyList<string> All { get; } = new[]
    {
        SystemAdministrator,
        HrAdministrator,
        HrManager,
        Manager,
        Supervisor,
        Employee,
        PayrollAdministrator,
        TimeOfficeAdministrator,
    };
}
