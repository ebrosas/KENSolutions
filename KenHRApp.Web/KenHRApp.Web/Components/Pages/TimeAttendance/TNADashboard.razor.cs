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
        private int pageCount => (int)Math.Ceiling((double)_holidayList.Count / pageSize);
        private DateTime? _selectedDate = DateTime.Now;
        private int _currentEmpNo = 10003632;
        private DateTime _payrollStartDate = new DateTime(2026, 1, 16);
        private DateTime _payrollEndDate = new DateTime(2026, 2, 15);        
        private int _fiscalYear = DateTime.Now.Year;
        private string _firstTimeIn = "-";
        private string _lastTimeOut = "-";

        private AttendanceSummaryDTO _attendanceSummary = new AttendanceSummaryDTO();
        private AttendanceDetailDTO _attendanceDetail = new AttendanceDetailDTO();  
        #endregion

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
        private bool _enableFilter = false;
        private bool _isTaskFinished = false;
        private bool _showAttendanceDetail = true;
        private bool _isPunchedIn = false;
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

        private List<UserDefinedCodeDTO> _attendanceLegends { get; set; } = new();
        private List<HolidayDTO> _holidayList { get; set; } = new();

        private IEnumerable<HolidayDTO> PagedHolidays =>
            _holidayList
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

        private enum UDCKeys
        {
            ATTENDLEGEND             // Attendance Legend
        }
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            //RefreshHolidays();
            BeginGetAttendanceSummary();
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

        private void ShowAttendanceDetailToggle()
        {
            _showAttendanceDetail = !_showAttendanceDetail;
        }

        private string GetOrdinal(int day)
        {
            if (day >= 11 && day <= 13)
                return "th";

            return (day % 10) switch
            {
                1 => "st",
                2 => "nd",
                3 => "rd",
                _ => "th"
            };
        }

        private void PunchInOrOut()
        {
            DateTime now = DateTime.Now;

            if (!_isPunchedIn)
                _firstTimeIn = $"{now.Day}{GetOrdinal(now.Day)} {now:MMM yyyy hh:mm tt}";
            else
                _lastTimeOut = $"{now.Day}{GetOrdinal(now.Day)} {now:MMMM yyyy hh:mm tt}";

            if (!_isPunchedIn)
                _isPunchedIn = true;
        }
        #endregion

        #region Database Methods
        private async Task RefreshHolidays()
        {

            var repoResult = await AttendanceService.GetPublicHolidaysAsync(_fiscalYear, null);
            if (repoResult.Success)
            {
                _holidayList = repoResult!.Value;
            }
            else
                _errorMessage.Append(repoResult.Error);

            if (_errorMessage.Length > 0)
                ShowHideError(true);
        }

        private async Task LoadComboboxAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

            var repoResult = await AttendanceService.GetPublicHolidaysAsync(_fiscalYear, null);
            if (repoResult.Success)
            {
                _holidayList = repoResult!.Value;
            }
            else
                _errorMessage.Append(repoResult.Error);

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private void BeginGetAttendanceSummary()
        {
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading attendance summary, please wait...";

            _ = GetAttendanceSummaryAsync(async () =>
            {
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, _currentEmpNo, _payrollStartDate, _payrollEndDate);
        }

        private async Task GetAttendanceSummaryAsync(Func<Task> callback, int empNo, DateTime? startDate, DateTime? endDate)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Clear shift timing items in the "Shift Timing Sequence" grid
            //_shiftMasterPointerList.Clear();

            var result = await AttendanceService.GetAttendanceSummaryAsync(empNo, startDate, endDate);
            if (result.Success)
            {
                _attendanceSummary = result.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.Append(result.Error);

                ShowHideError(true);
            }

            #region Get public holidays and attendance legends
            
            //Get Public Holidays
            var repoResult = await AttendanceService.GetPublicHolidaysAsync(_fiscalYear, null);
            if (repoResult.Success)
            {
                _holidayList = repoResult!.Value;
            }
            else
                _errorMessage.Append(repoResult.Error);

            // Get Attendance Legends
            var udcResult = await AttendanceService.GetUserDefinedCodeAsync(UDCKeys.ATTENDLEGEND.ToString());
            if (udcResult.Success)
                _attendanceLegends = udcResult!.Value;
            else
                _errorMessage.Append(udcResult.Error);

            #endregion

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }
        #endregion
    }
}
