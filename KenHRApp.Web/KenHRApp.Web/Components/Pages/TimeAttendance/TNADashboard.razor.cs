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
        private EditForm _editForm;
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private string _searchString = string.Empty;
        private StringBuilder _errorMessage = new StringBuilder();
        private List<string> _events = new();
        private int currentPage = 1;
        private int pageSize = 5;
        private int pageCount => (int)Math.Ceiling((double)_holidayList.Count / pageSize);        
        private int _currentEmpNo = 10003632;
        private DateTime _payrollStartDate = new DateTime(2026, 1, 16);
        private DateTime _payrollEndDate = new DateTime(2026, 2, 15);        
        private int _fiscalYear = DateTime.Now.Year;
        private string? _firstTimeIn = null;
        private string? _lastTimeOut = null;
        private MudDatePicker _picker;
        private DateTime? _selectedDate = DateTime.Today;

        private AttendanceSummaryDTO _attendanceSummary = new AttendanceSummaryDTO();
        private AttendanceDetailDTO _attendanceDetail = new AttendanceDetailDTO();  
        private AttendanceSwipeDTO _swipeLog = new AttendanceSwipeDTO();
        private AttendanceDurationDTO _attendanceDuration = new AttendanceDurationDTO();

        private Orientation _calOrientation = Orientation.Landscape;
        private string _pickerStyle = "width: 420px;";
        #endregion

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
        private bool _enableFilter = false;
        private bool _isTaskFinished = false;
        private bool _showAttendanceDetail = true;
        private bool _isPunchedIn = false;
        private bool _forceLoad = false;
        private bool _btnProcessing = false;
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
        private List<AttendanceSwipeDTO> _attendanceChips { get; set; } = new();

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

        private enum MessageBoxTypes
        {
            Info,
            Confirm,
            Warning,
            Error
        }
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_swipeLog);

            BeginGetAttendanceSummary();                        
        }
        #endregion

        #region Validation Methods
        private void HandleInvalidSubmit(EditContext context)
        {
            _hasValidationError = true;
            _validationMessages = context.GetValidationMessages().ToList();
        }

        private void HandleValidSubmit(EditContext context)
        {
            try
            {
                // If we got here, model is valid
                _hasValidationError = false;
                _validationMessages.Clear();

                // Set flag to display the loading button
                _btnProcessing = true;

                // Set the overlay message
                overlayMessage = "Saving changes, please wait...";

                _ = SaveSwipeDataAsync(async () =>
                {
                    // Set flag to hide the loading button
                    _btnProcessing = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);
                });
            }
            catch (OperationCanceledException)
            {
                ShowNotification("Save cancelled (navigated away).", SnackBarTypes.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", SnackBarTypes.Error);
            }
        }
        #endregion

        #region Private Methods
        private void OnBreakpointChanged(Breakpoint breakpoint)
        {
            if (breakpoint <= Breakpoint.Md)
            {
                _calOrientation = Orientation.Portrait;
                _pickerStyle = "width: 100%;";
            }
            else
            {
                _calOrientation = Orientation.Landscape;
                _pickerStyle = "width: 420px;";
            }
        }

        private void ShowNotification(string message, SnackBarTypes type, string position = Defaults.Classes.Position.TopCenter)
        {
            Snackbar.Clear();

            Snackbar.Configuration.PositionClass = position;
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

            if (!_isPunchedIn && string.IsNullOrEmpty(_firstTimeIn))
                _firstTimeIn = $"{now.Day}{GetOrdinal(now.Day)} {now:MMM yyyy hh:mm:ss tt}";
            else
                _lastTimeOut = $"{now.Day}{GetOrdinal(now.Day)} {now:MMMM yyyy hh:mm:ss tt}";

            #region Initialize DTO
            _swipeLog.EmpNo = _currentEmpNo;
            _swipeLog.SwipeDate = now.Date;
            _swipeLog.SwipeTime = now;
            _swipeLog.SwipeType = !_isPunchedIn ? "IN" : "OUT";
            #endregion

            AddChip(now);
        }

        private void AddChip(DateTime punchTime)
        {
            AttendanceSwipeDTO punchSwipe = new AttendanceSwipeDTO()
            {
                EmpNo = _currentEmpNo,
                SwipeDate = punchTime.Date,
                SwipeTime = punchTime,
                SwipeLogDate = DateTime.Now
            };

            _attendanceChips.Add(punchSwipe);
        }

        private void OnChipClosed(MudChip<AttendanceSwipeDTO> chip) => _attendanceChips.Remove(chip!.Value);

        private void OnDateChanged(DateTime? date)
        {
            //_isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading attendance details, please wait...";
            
            _ = GetAttendanceDetail(async () =>
            {
                //_isRunning = false;
                
                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

            }, date);
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

                // Set calandar to today's date
                _selectedDate = DateTime.Now;

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

            #region Get Public Holidays
            var repoResult = await AttendanceService.GetPublicHolidaysAsync(_fiscalYear, null);
            if (repoResult.Success)
            {
                _holidayList = repoResult!.Value;
            }
            else
                _errorMessage.Append(repoResult.Error);
            #endregion

            #region Get Attendance Legends
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

        private async Task SaveSwipeDataAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = true;
            string errorMsg = string.Empty;

            var addResult = await AttendanceService.SaveSwipeDataAsync(_swipeLog, _cts.Token);
            isSuccess = addResult.Success;
            if (!isSuccess)
                errorMsg = addResult.Error!;
            else
            {
                // Set flag to enable reload of _recruitmentRequests when navigating back to the Employe Search page
                _forceLoad = true;
            }

            if (isSuccess)
            {
                #region Get Attendance Duration
                var attendanceResult = await AttendanceService.GetAttendanceDurationAsync(_currentEmpNo, _selectedDate!.Value.Date);
                if (attendanceResult.Success)
                    _attendanceDuration = attendanceResult!.Value;
                else
                    _errorMessage.Append(attendanceResult.Error);
                #endregion

                // Hide error message if any
                ShowHideError(false);

                // Show notification
                if (!_isPunchedIn)
                    ShowNotification("Punched In successfully!", SnackBarTypes.Success);
                else
                    ShowNotification("Punched Out successfully!", SnackBarTypes.Success);

                // Toggle flag
                _isPunchedIn = !_isPunchedIn;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(errorMsg);
                ShowHideError(true);
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task GetAttendanceDetail(Func<Task> callback, DateTime? selectedDate)
        {
            try
            {
                _selectedDate = selectedDate;

                // Wait for 1 second then gives control back to the runtime
                //await Task.Delay(500);

                // Reset collections
                _errorMessage.Clear();
                _attendanceChips.Clear();

                var attendanceResult = await AttendanceService.GetAttendanceDetailAsync(_currentEmpNo, selectedDate!.Value.Date);
                if (attendanceResult.Success)
                {
                    _attendanceDetail = attendanceResult!.Value;

                    if (_attendanceDetail != null)
                    {
                        #region Get the First Time In and Last Time Out
                        if (_attendanceDetail.FirstTimeIn.HasValue)
                            _firstTimeIn = $"{_attendanceDetail.FirstTimeIn:dd}{GetOrdinal(_attendanceDetail.FirstTimeIn.Value.Day)} {_attendanceDetail.FirstTimeIn:MMM yyyy hh:mm:ss tt}";
                        else
                            _firstTimeIn = string.Empty;

                        if (_attendanceDetail.LastTimeOut.HasValue)
                            _lastTimeOut = $"{_attendanceDetail.LastTimeOut:dd}{GetOrdinal(_attendanceDetail.LastTimeOut.Value.Day)} {_attendanceDetail.LastTimeOut:MMM yyyy hh:mm:ss tt}";
                        else
                            _lastTimeOut = string.Empty;
                        #endregion

                        #region Get Attendance Duration
                        var durationResult = await AttendanceService.GetAttendanceDurationAsync(_currentEmpNo, selectedDate!.Value.Date);
                        if (durationResult.Success)
                            _attendanceDuration = durationResult!.Value;
                        else
                            _errorMessage.Append(durationResult.Error);
                        #endregion

                        #region Populate the raw swipe chips
                        if (_attendanceDetail.SwipeLogList != null && _attendanceDetail.SwipeLogList.Any())
                            _attendanceChips.AddRange(_attendanceDetail.SwipeLogList.ToList());
                        #endregion
                    }
                }
                else
                    _errorMessage.Append(attendanceResult.Error);

                if (callback != null)
                {
                    // Hide the spinner overlay
                    await callback.Invoke();
                }
            }
            catch (Exception ex)
            {
                // Set the error message
                _errorMessage.Append(ex.Message.ToString());

                ShowHideError(true);
            }
        }
        #endregion
    }
}
