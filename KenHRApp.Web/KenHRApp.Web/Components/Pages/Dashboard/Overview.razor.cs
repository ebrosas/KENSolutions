using KenHRApp.Application.Features.Employees.Interfaces;
using KenHRApp.Application.Features.Employees.Queries;
using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Enums;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KenHRApp.Web.Components.Pages.Dashboard
{
    public partial class Overview
    {
        #region Parameters and Injections
        [Inject] private ILoveEmployeeService Employees { get; set; } = default!;
        #endregion

        #region Fields
        private DashboardKpis Kpis { get; set; } = new(0, 0, 0, 0);

        private sealed record DashboardKpis(int Headcount, int PresentToday, int OnLeave, int OpenRequisitions);

        private static readonly string[] Months = { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
        private static readonly string[] Weekdays = { "Sun", "Mon", "Tue", "Wed", "Thu" };
        private static readonly string[] DivisionLabels = { "Operations", "Engineering", "Commercial", "Corporate", "Field" };
        private static readonly double[] DivisionData = { 420, 310, 220, 180, 154 };

        private readonly List<ChartSeries> HeadcountSeries = new()
        {
            new ChartSeries { Name = "Headcount", Data = new double[] { 1180, 1195, 1210, 1218, 1230, 1244, 1250, 1262, 1270, 1278, 1281, 1284 } },
            new ChartSeries { Name = "Attrition", Data = new double[] { 12, 9, 14, 11, 8, 10, 13, 9, 7, 11, 8, 6 } },
        };

        private readonly List<ChartSeries> AttendanceSeries = new()
        {
            new ChartSeries { Name = "Present", Data = new double[] { 1180, 1201, 1195, 1188, 1150 } },
            new ChartSeries { Name = "Late", Data = new double[] { 42, 31, 38, 45, 52 } },
            new ChartSeries { Name = "Absent", Data = new double[] { 18, 12, 15, 20, 24 } },
        };

        private readonly List<Timeline.TimelineEntry> Activity = new()
        {
            new("October payroll approved", "Payroll · 1h ago", Color.Success),
            new("3 leave requests awaiting approval", "Leave Management · 2h ago", Color.Warning),
            new("Offer accepted — Layla Haddad", "Recruitment · 3h ago", Color.Info),
            new("24 performance reviews close Friday", "Employee Lifecycle · Yesterday", Color.Primary),
        };
        #endregion

        protected override async Task OnInitializedAsync()
        {
            // Counts come from the Application layer only — no DbContext in the component.
            var active = await Employees
                .GetEmployeesAsync(new GetEmployeesQuery { Status = EmployeeStatus.Active, PageSize = 1 })
                .ConfigureAwait(false);

            var onLeave = await Employees
                .GetEmployeesAsync(new GetEmployeesQuery { Status = EmployeeStatus.OnLeave, PageSize = 1 })
                .ConfigureAwait(false);

            var all = await Employees
                .GetEmployeesAsync(new GetEmployeesQuery { PageSize = 1 })
                .ConfigureAwait(false);

            //Kpis = new DashboardKpis(
            //    all.TotalCount,
            //    Math.Max(active.TotalCount - onLeave.TotalCount, 0),
            //    onLeave.TotalCount,
            //    OpenRequisitions: 0);

            Kpis = new DashboardKpis(15, 100, 86,3);
        }
    }
}
