using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Domain.Entities;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Globalization;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class TNADashboard
    {
        #region Parameters and Injections
        [Inject] private IAttendanceService AttendanceService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState AppState { get; set; } = default!;
        #endregion

        #region Fields

        #region Private Fields
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private string _searchString = string.Empty;
        private StringBuilder _errorMessage = new StringBuilder();
        private List<string> _events = new();
        private int currentPage = 1;
        private int pageSize = 5;
        private int pageCount => (int)Math.Ceiling((double)HolidayList.Count / pageSize);
        #endregion

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
        private bool _enableFilter = false;
        private bool _isTaskFinished = false;
        #endregion

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        #endregion

        #region Collections        
        private List<ShiftPatternMasterDTO> _shiftRosterList = new List<ShiftPatternMasterDTO>();
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Employee Attendance Dashboard", href: null, disabled: true, @Icons.Material.Filled.CalendarMonth)
        ];

        private List<UserDefinedCodeDTO> AttendanceLegends { get; set; } = new();
        private List<HolidayDTO> HolidayList { get; set; } = new();

        private IEnumerable<HolidayDTO> PagedHolidays =>
            HolidayList
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize);
        #endregion

        #endregion

        #region Enums
        private enum SnackBarTypes
        {
            Normal,
            Information,
            Success,
            Warning,
            Error
        }
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            #region Populate attendance legends
            AttendanceLegends.Add(new UserDefinedCodeDTO() { UDCId = 1, UDCCode = "ALABSENT", UDCDesc1 = "Absent" });
            AttendanceLegends.Add(new UserDefinedCodeDTO() { UDCId = 2, UDCCode = "ALPRESENT", UDCDesc1 = "Present" });
            AttendanceLegends.Add(new UserDefinedCodeDTO() { UDCId = 3, UDCCode = "ALLATE", UDCDesc1 = "Late" });
            AttendanceLegends.Add(new UserDefinedCodeDTO() { UDCId = 4, UDCCode = "ALLEAVE", UDCDesc1 = "On-leave" });
            AttendanceLegends.Add(new UserDefinedCodeDTO() { UDCId = 4, UDCCode = "ALSICKLEAVE", UDCDesc1 = "Sick Leave" });
            AttendanceLegends.Add(new UserDefinedCodeDTO() { UDCId = 4, UDCCode = "ALINJURYLEAVE", UDCDesc1 = "Injury Leave" });
            AttendanceLegends.Add(new UserDefinedCodeDTO() { UDCId = 4, UDCCode = "ALBUSTRIP", UDCDesc1 = "Business Trip" });
            #endregion

            #region Populate holiday list
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "New Year Day", HolidayDate = new DateTime(2026, 1, 1) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "Eid Al-Fitr", HolidayDate = new DateTime(2026, 3, 18) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "Eid Al-Fitr", HolidayDate = new DateTime(2026, 3, 19) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "Labour Day", HolidayDate = new DateTime(2026, 5, 3) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "Eid Al-Adha", HolidayDate = new DateTime(2026, 5, 26) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "Eid Al-Adha", HolidayDate = new DateTime(2026, 5, 27) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "Eid Al-Adha", HolidayDate = new DateTime(2026, 5, 28) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "Ashura", HolidayDate = new DateTime(2026, 6, 24) });
            HolidayList.Add(new HolidayDTO() { HolidayId = 1, HolidayDesc = "National Day", HolidayDate = new DateTime(2026, 12, 16) });
            #endregion
        }
        #endregion

        #region Private Methods
        private void ShowNotification(string message, SnackBarTypes type)
        {
            Snackbar.Clear();

            Snackbar.Configuration.PositionClass = Defaults.Classes.Position.TopCenter;
            Snackbar.Configuration.PreventDuplicates = false;
            Snackbar.Configuration.NewestOnTop = false;
            Snackbar.Configuration.ShowCloseIcon = true;
            Snackbar.Configuration.VisibleStateDuration = 5000;
            Snackbar.Configuration.HideTransitionDuration = 500;
            Snackbar.Configuration.ShowTransitionDuration = 500;
            Snackbar.Configuration.SnackbarVariant = Variant.Filled;

            switch (type)
            {
                case SnackBarTypes.Information:
                    Snackbar.Add(message, Severity.Info);
                    break;

                case SnackBarTypes.Success:
                    Snackbar.Add(message, Severity.Success);
                    break;

                case SnackBarTypes.Warning:
                    Snackbar.Add(message, Severity.Warning);
                    break;

                case SnackBarTypes.Error:
                    Snackbar.Add(message, Severity.Error);
                    break;

                default:
                    Snackbar.Add(message, Severity.Normal);
                    break;
            }

            // Snackbar.Add($"Error {message}", Severity.Error, c => c.SnackbarVariant = Variant.Filled);
        }

        private void ShowHideError(bool value)
        {
            if (value)
            {
                _showErrorAlert = true;
            }
            else
            {
                _showErrorAlert = false;

                // Reset error messages
                _errorMessage.Clear();
            }
        }

        private Color GetLegendColor(string legendCode)
        {
            return legendCode switch
            {
                "ALABSENT" => Color.Error,
                "ALPRESENT" => Color.Success,
                "ALLATE" => Color.Warning,
                "ALLEAVE" => Color.Info,
                "ALBUSTRIP" => Color.Tertiary,
                _ => Color.Default
            };
        }

        private void OnPageChanged(int page)
        {
            currentPage = page;
        }
        #endregion
    }
}
