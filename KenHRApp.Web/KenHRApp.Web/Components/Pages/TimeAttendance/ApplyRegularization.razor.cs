using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Web.Components.Common.Helpers;
using KenHRApp.Web.Components.Common.Interface;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using KenHRApp.Application.DTOs.TNA;
using KenHRApp.Application.DTOs;
using System.Text;
using KenHRApp.Web.Components.Shared;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class ApplyRegularization : IPageAuthorization, IWorkflowProcess
    {
        #region Parameters and Injections
        [Inject] private IAttendanceService AttendanceService { get; set; } = default!;
        [Inject] private IEmployeeService EmployeeService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState State { get; set; } = default!;
        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;
        [Inject] private IWorkflowService WorkflowService { get; set; } = default!;
        [Inject] private IUserSessionService UserSession { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string ActionType { get; set; } = ActionTypes.View.ToString();

        [Parameter]
        [SupplyParameterFromQuery]
        public long RegularizedRequestId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery]
        public string CallerForm { get; set; } = "";
        #endregion

        #region Fields

        #region General fields
        private EditForm _editForm;
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private string _searchString = string.Empty;
        private StringBuilder _errorMessage = new StringBuilder();
        private decimal _leaveDuration = 0;
        private string _pageTitle = "Apply Regularization";
        private int _currentWFIndex = 0;
        private MudDatePicker _picker;
        private DateTime? _selectedDate = DateTime.Today;
        private Orientation _calOrientation = Orientation.Portrait;
        private string _pickerStyle = "width: 420px;";
        #endregion

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
        private bool _isDisabled = false;
        private bool _isClearable = false;
        private bool _isEditMode = false;
        private bool _saveBtnEnabled = false;
        private bool _btnProcessing = false;
        private bool _forceLoad = false;
        #endregion

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        #endregion

        #region Objects and Collections       
        private RegularRequestDTO _regularRequest = new();
        private IReadOnlyList<IBrowserFile> _files = Array.Empty<IBrowserFile>();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Regularization Request", href: null, disabled: true, @Icons.Material.Filled.CardTravel)
        ];

        private List<UserDefinedCodeDTO> _actionList = new List<UserDefinedCodeDTO>();
        private string[]? _actionArray = null;

        private List<UserDefinedCodeDTO> _roaList = new List<UserDefinedCodeDTO>();
        private string[]? _roaArray = null;

        private string[]? _employeeArray = null;
        private IReadOnlyList<EmployeeResultDTO> _employeeList = new List<EmployeeResultDTO>();

        private List<WorkflowDetailResultDTO> _workflowList = new List<WorkflowDetailResultDTO>();
        private Guid _calendarRenderKey = Guid.NewGuid();
        private List<AttendanceSwipeDTO> _attendanceChips { get; set; } = new();
        #endregion

        #region Constants

        #region Leave Request Statuses
        private readonly string CONST_CANCELLED_BY_USER = "101";         // Cancelled by User
        private readonly string CONST_REQUEST_SENT = "02";              // Request Sent
        private readonly string CONST_WAITING_FOR_APPROVAL = "05";      // Waiting for Approval
        #endregion

        #endregion

        #endregion

        #region Enums
        private enum ActionTypes
        {
            View,
            Edit,
            Add,
            Delete
        }

        private enum NotificationType
        {
            Normal,
            Information,
            Success,
            Warning,
            Error
        }

        private enum MessageBoxTypes
        {
            Info,
            Confirm,
            Warning,
            Error
        }

        private enum UDCKeys
        {
            ROATYPE,            // ROA Types
            ATTENDACT,          // Attendance Action Types
            LEAVEAPORTION,      // Leave Day Portions
            STATUS              // Leave Statuses
        }
        #endregion

        #region IPageAuthorization Implementation
        public string UserName { get; set; } = "";
        public int UserEmpNo { get; set; } = 0;
        public Guid UserId { get; set; }
        public string? UserEmail { get; set; } = "";
        public string? UserCostCenter { get; set; } = "";
        public string UserFullName { get; set; } = "";

        public void GoToLogin()
        {
            Navigation.NavigateTo("/login");
        }
        #endregion

        #region IWorkflowProcess Implementation
        public async Task InitializeWorkflowAsync(long leaveNo, int originatorEmpNo)
        {
            bool isSuccess = false;

            try
            {
                if (leaveNo == 0)
                    throw new ArgumentException("Leave Requisition No. is not defined!");

                // Initialize the cancellation token
                _cts = new CancellationTokenSource();

                var repoResult = await WorkflowService.StartWorkflowAsync(WorkflowHelper.CONST_LEAVE_REQUEST,
                    leaveNo, Environment.WebRootPath, originatorEmpNo, _cts.Token);
                if (repoResult.Success)
                {
                    isSuccess = repoResult.Value;
                }
                else
                {
                    // Show error message
                    _errorMessage.AppendLine(repoResult.Error);

                    ShowHideError(true);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {            
            // Initialize the EditContext 
            _editContext = new EditContext(_regularRequest);

            if (ActionType == ActionTypes.Edit.ToString() ||
                ActionType == ActionTypes.View.ToString())
            {
                _isDisabled = true;
            }
            else if (ActionType == ActionTypes.Add.ToString())
            {
                _isDisabled = false;
                _saveBtnEnabled = true;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                bool isAuthenticated = UserSession.IsAuthenticated();
                if (!isAuthenticated)
                {
                    // Refresh the user session
                    await UserSession.InitializeAsync();
                    isAuthenticated = UserSession.IsAuthenticated();
                }

                if (isAuthenticated)
                {
                    UserId = UserSession.CurrentUser!.UserId;
                    UserName = UserSession.CurrentUser!.Username;
                    UserEmpNo = UserSession.CurrentUser!.UserEmpNo;
                    UserFullName = UserSession.CurrentUser!.UserFullName;
                    UserEmail = UserSession.CurrentUser!.EmailAddress;
                    UserCostCenter = UserSession.CurrentUser!.CostCenter;

                    // Initialize the Leave Request object
                    _regularRequest.CreatedBy = UserEmpNo;
                    _regularRequest.CreatedEmail = UserEmail;
                    _regularRequest.CreatedUserID = UserName;                                        

                    BeginLoadComboboxTask();
                }
                else
                    GoToLogin();
            }
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
                //#region Check if Leave Start Date is public holiday or not
                //bool isStartDateHoliday = LeaveService.CheckIfLeaveDateIsHolidayAsync(_regularRequest.LeaveStartDate!.Value).Result;
                //if (isStartDateHoliday)
                //{
                //    _hasValidationError = true;
                //    _validationMessages.Add("Start Date should not be a public holiday.");
                //}
                //#endregion

                //#region Check if Leave Resume Date is public holiday or not
                //bool isResumeDateHoliday = LeaveService.CheckIfLeaveDateIsHolidayAsync(_regularRequest.LeaveResumeDate!.Value).Result;
                //if (isResumeDateHoliday)
                //{
                //    _hasValidationError = true;
                //    _validationMessages.Add("Resume Date should not be a public holiday.");
                //}
                //#endregion

                //#region Check if leave period exist
                //bool isLeaveExist = LeaveService.CheckIfLeavePeriodExistAsync(
                //    _regularRequest.LeaveEmpNo,
                //    _regularRequest.LeaveResumeDate!.Value).Result;
                //if (isLeaveExist)
                //{
                //    _hasValidationError = true;
                //    _validationMessages.Add("The specified date period overlaps with an existing leave request.");
                //}
                //#endregion

                if (_hasValidationError && _validationMessages.Any())
                    return;

                // If we got here, model is valid
                _hasValidationError = false;
                _validationMessages.Clear();

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Submitting regularization request, please wait...";

                //_ = SubmitLeaveRequestAsync(async () =>
                //{
                //    _isRunning = false;

                //    // Shows the spinner overlay
                //    await InvokeAsync(StateHasChanged);

                //    if (_regularRequest.LeaveRequestId > 0)
                //    {
                //        // Initiate the workflow
                //        await InitializeWorkflowAsync(_regularRequest.LeaveRequestId, _regularRequest.LeaveEmpNo);

                //        BeginLoadLeaveRequest(_regularRequest.LeaveRequestId);
                //    }
                //});
            }
            catch (OperationCanceledException)
            {
                ShowNotification("Save cancelled (navigated away).", NotificationType.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
        }
        #endregion

        #region Button Event Handlers
        private async Task HandleEditButton()
        {
            try
            {
                _isRunning = true;
                overlayMessage = "Entering edit mode, please wait...";
                StateHasChanged(); // immediate render

                // do your async work
                await Task.Delay(500);

                // Set the flags
                _isEditMode = true;
                _saveBtnEnabled = true;
                _isDisabled = false;

                // Hide error message if any
                ShowHideError(false);
            }
            catch (Exception ex)
            {
                overlayMessage = $"Error: {ex.Message}";
            }
            finally
            {
                _isRunning = false;     // ✅ must execute
                StateHasChanged();      // ✅ must re-render
            }
        }

        private void HandleUndoButton()
        {
            // Set flag to display the loading panel
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Undoing changes, please wait...";

            //_ = GetShiftRosterDetailAsync(async () =>
            //{
            //    // Reset the flags
            //    _isEditMode = false;
            //    _allowGridEdit = false;
            //    _isDisabled = true;
            //    _isRunning = false;
            //    _saveBtnEnabled = false;
            //    _hasValidationError = false;
            //    _validationMessages.Clear();

            //    // Reset error messages
            //    _errorMessage.Clear();
            //    ShowHideError(false);

            //    // Shows the spinner overlay
            //    await InvokeAsync(StateHasChanged);
            //}, _shiftPattern.RegularizationId);
        }

        private async Task HandleRefreshButton()
        {
            // Reset Leave Request object
            _regularRequest = new();
            _regularRequest.CreatedBy = UserEmpNo;
            _regularRequest.CreatedEmail = UserEmail;
            _regularRequest.CreatedUserID = UserName;

            #region Reset file attachments
            _files = Array.Empty<IBrowserFile>();

            StateHasChanged();
            #endregion
        }
        
        private void HandleBackButton()
        {
            if (string.IsNullOrEmpty(CallerForm))
                return;

            switch (CallerForm)
            {
                case "TNADashboard":
                    Navigation.NavigateTo("/TimeAttendance/tnadashboard");
                    break;

                case "LeaveInquiry":
                    Navigation.NavigateTo("/TimeAttendance/leaveinquiry");
                    break;

                case "ApprovalDashboard":
                    Navigation.NavigateTo("/Workflow/ApprovalDashboard");
                    break;

                default:
                    Navigation.NavigateTo("/home");
                    break;
            }
        }
        #endregion

        #region Private Methods
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

        private async Task ShowDeleteDialog()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", "Do you really want to delete this leave request? Note that this process cannot be undone." },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                //BeginDeleteShiftRoster();
            }
        }

        private async Task ShowCancelDialog()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Cancel"},
                { "DialogIcon", _iconCancel },
                { "ContentText", "Are you sure you want to cancel regularization request?" },
                { "ConfirmText", "Yes" },
                { "CancelText", "No" },
                { "Color", Color.Success }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                CancelRequest();
            }
        }

        private void CancelRequest()
        {
            if (!string.IsNullOrEmpty(CallerForm))
            {
                switch (CallerForm)
                {
                    case "LeaveInquiry":
                        Navigation.NavigateTo("/TimeAttendance/leaveinquiry");
                        break;

                    default:
                        Navigation.NavigateTo("/TimeAttendance/tnadashboard");
                        break;
                }
            }
            else
                Navigation.NavigateTo("/TimeAttendance/tnadashboard");
        }

        private void ShowNotification(string message, NotificationType type)
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
                case NotificationType.Information:
                    Snackbar.Add(message, Severity.Info);
                    break;

                case NotificationType.Success:
                    Snackbar.Add(message, Severity.Success);
                    break;

                case NotificationType.Warning:
                    Snackbar.Add(message, Severity.Warning);
                    break;

                case NotificationType.Error:
                    Snackbar.Add(message, Severity.Error);
                    break;

                default:
                    Snackbar.Add(message, Severity.Normal);
                    break;
            }
        }

        private void ResetForm()
        {
            //_shiftPattern = new ShiftPatternMasterDTO
            //{
            //    IsActive = true,
            //    IsFlexiTime = false,
            //    ShiftPatternCode = string.Empty
            //};
        }

        private async Task ConfirmDelete(RegularRequestDTO request)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete Regularization Request No. '{request.RegularizedRequestId}'?" },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error },
                { "DialogIconColor", Color.Error }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.TopCenter,
                CloseOnEscapeKey = true,   // Prevent ESC from closing
                BackdropClick = false       // Prevent clicking outside to close
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                //BeginDeleteDepartment(department);
            }
        }

        private async Task ShowErrorMessage(MessageBoxTypes msgboxType, string title, string content)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", title},
                { "DialogIcon", msgboxType == MessageBoxTypes.Error ? _iconError
                        : msgboxType == MessageBoxTypes.Warning ? _iconWarning
                        : _iconInfo  },
                { "ContentText", content },
                {
                    "Color", msgboxType == MessageBoxTypes.Error ? Color.Error
                        : msgboxType == MessageBoxTypes.Info ? Color.Info
                        : msgboxType == MessageBoxTypes.Warning ? Color.Warning
                        : Color.Default
                },
                {
                    "DialogIconColor", msgboxType == MessageBoxTypes.Error ? Color.Error
                        : msgboxType == MessageBoxTypes.Info ? Color.Info
                        : msgboxType == MessageBoxTypes.Warning ? Color.Warning
                        : Color.Default
                }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.Center,
                CloseOnEscapeKey = true,   // Prevent ESC from closing
                BackdropClick = false       // Prevent clicking outside to close
            };

            var dialog = await DialogService.ShowAsync<InfoDialog>(title, parameters, options);
            var result = await dialog.Result;
        }

        private void RemoveFile(IBrowserFile file)
        {
            //_files.Remove(file);
            _files = _files.Where(f => f != file).ToList();
        }               

        private async Task ConfirmCancel()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Cancel"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to cancel leave requsition no. '{_regularRequest.RegularizedRequestId}'?" },
                { "ConfirmText", "Proceed" },
                { "Color", Color.Error },
                { "DialogIconColor", Color.Error }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.TopCenter,
                CloseOnEscapeKey = true,   // Prevent ESC from closing
                BackdropClick = false       // Prevent clicking outside to close
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Leave Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                //BeginLeaveCancellation(_regularRequest);
            }
        }

        private void OnBreakpointChanged(Breakpoint breakpoint)
        {
            if (breakpoint <= Breakpoint.Sm)
            {
                _calOrientation = Orientation.Landscape;
                _pickerStyle = "width: 420px; padding-top: 15px;";                
            }
            else
            {
                _calOrientation = Orientation.Portrait;
                _pickerStyle = "width: 100%;";
            }
        }

        private async void OnDateChanged(DateTime? date)
        {            
            if (date.HasValue)
                await GetAttendanceInfoAsync(UserEmpNo, date!.Value);
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

        private async Task<IEnumerable<string>> SearchAction(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _actionArray!;
            }

            return _actionArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchReasonOfAbsence(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _roaArray!;
            }

            return _roaArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchEmployee(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _employeeArray!;
            }

            return _employeeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion

        #region Database Methods
        private void BeginLoadComboboxTask()
        {
            _isRunning = true;

            // Set the overlay message
            if (!UserSession.IsAuthenticated())
                overlayMessage = "Authentication required. Redirecting to login page...";
            else
                overlayMessage = "Initializing form, please wait...";

            _ = LoadComboboxAsync(async () =>
            {
                // Set the default employee to the current logged in user
                _regularRequest.EmployeeNo = UserEmpNo;
                _regularRequest.EmployeeName = UserFullName;

                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                OnDateChanged(_selectedDate);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                //if (RegularizationId > 0)
                //{
                //    BeginLoadLeaveRequest(RegularizationId);
                //}
            });
        }

        private async Task LoadComboboxAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

            #region Get employee list
            var repoResult = await LookupCache.GetEmployeeAsync();
            if (repoResult.Success)
            {
                _employeeList = repoResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(repoResult.Error);
            }

            if (_employeeList != null)
            {
                _employeeArray = _employeeList.Select(d => d.EmployeeNameWithCode).OrderBy(d => d).ToArray();
            }
            #endregion

            #region Get all UDC group codes
            List<UserDefinedCodeGroupDTO>? udcGroupList = new();
            int groupID = 0;

            var resultUDC = await EmployeeService.GetUserDefinedCodeGroupAsync();
            if (resultUDC.Success)
            {
                udcGroupList = resultUDC!.Value;
            }
            else
                _errorMessage.Append(resultUDC.Error);
            #endregion

            // Get UDC dataset
            var result = await EmployeeService.GetUserDefinedCodeAllAsync();
            if (result.Success)
            {
                var udcData = result.Value;
                if (udcData!.Any() && udcGroupList!.Any())
                {
                    #region Get ROA Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.ROATYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting ROA group id: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _roaList = udcData!.Where(a => a.GroupID == groupID && a.IsActive == true).ToList();
                        if (_roaList != null)
                            _roaArray = _roaList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Attendance Action Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.ATTENDACT.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Action Types group id: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _actionList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_actionList != null)
                            _actionArray = _actionList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion
                }
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task GetAttendanceInfoAsync(int empNo, DateTime attendanceDate)
        {
            try
            {
                // Reset error
                _errorMessage.Clear();
                _showErrorAlert = false;

                // Clear swipe data
                _attendanceChips.Clear();

                // Set calendar selected date
                _selectedDate = attendanceDate;

                var result = await AttendanceService.GetAttendanceInfoAsync(empNo, attendanceDate);
                if (result.Success)
                {
                    AttendanceInfoResultDTO attendanceInfo = result.Value!;
                    if (attendanceInfo != null)
                    {
                        _regularRequest.AttendanceDate = attendanceInfo.AttendanceDate;
                        _regularRequest.ShiftPattern = attendanceInfo.ShiftRoster;
                        _regularRequest.ShiftTiming = attendanceInfo.ShiftTiming;
                        _regularRequest.WorkDuration = attendanceInfo.TotalWorkMinute;

                        #region Populate the raw swipe chips
                        if (attendanceInfo.SwipeLogList != null && attendanceInfo.SwipeLogList.Any())
                            _attendanceChips.AddRange(attendanceInfo.SwipeLogList.ToList());
                        #endregion

                        // Re-render the page
                        StateHasChanged();
                    }
                }
                else
                {
                    // Set the error message
                    _errorMessage.Append(result.Error);

                    ShowHideError(true);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(MessageBoxTypes.Error, "Error", ex.Message.ToString());
            }
        }
        #endregion
    }
}
