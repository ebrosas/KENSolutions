using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;
using KenHRApp.Web.Components.Common.Interface;
using Mono.TextTemplating;
using KenHRApp.Application.Services;
using KenHRApp.Application.DTOs.TNA;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using KenHRApp.Web.Components.Common.Helpers;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class LeavePlanner : ComponentBase, IPageAuthorization
    {
        #region Parameters and Injections
        [Inject] private ILeaveRequestService LeaveService { get; set; } = default!;
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
        public long LeaveRequestNo { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery]
        public string CallerForm { get; set; } = "";

        [Parameter]
        [SupplyParameterFromQuery]
        public string LeaveType { get; set; } = "";
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
        private string _pageTitle = "Leave Planner";
        private int _currentWFIndex = 0;
        private string _approverRemarks = string.Empty;
        private bool _isCurrentApprover = false;
        private bool _isCreator = false;
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
        private ApprovalRequestResultDTO? _requestItem;
        private LeaveRequisitionDTO _leaveRequest = new();
        private IReadOnlyList<IBrowserFile> _files = Array.Empty<IBrowserFile>();
        private MudSelect<string> _endDayMode = new();
        private MudSelect<string> _startDayMode = new();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Planned Leave Request", href: null, disabled: true, @Icons.Material.Filled.CardTravel)
        ];

        private string[]? _leaveModeArray = null;
        private List<UserDefinedCodeDTO> _leaveModeList = new List<UserDefinedCodeDTO>();

        private string[]? _employeeArray = null;
        private IReadOnlyList<EmployeeResultDTO> _employeeList = new List<EmployeeResultDTO>();

        private List<UserDefinedCodeDTO> _leaveStatusList = new();
        #endregion

        #region Constants

        #region Leave Day Mode
        private const string CONST_LEAVE_FULL_DAY = "LEAVEFD";
        private const string CONST_LEAVE_FIRST_HALF = "LEAVEFH";
        private const string CONST_LEAVE_SECOND_HALF = "LEAVESH";
        #endregion

        #region Leave Approval Flags
        private readonly char CONST_APPROVED_PAID = 'A';
        private readonly char CONST_WAITING_APPROVAL = 'W';
        private readonly char CONST_APPROVED_NOT_PAID = 'N';
        private readonly char CONST_CANCELLED = 'C';
        private readonly char CONST_DRAFT = 'D';
        private readonly char CONST_REJECTED = 'R';
        #endregion

        #region Leave Request Statuses
        private readonly string CONST_CANCELLED_BY_USER = "101";        // Cancelled by User
        private readonly string CONST_REQUEST_SENT = "02";              // Request Sent
        private readonly string CONST_WAITING_FOR_APPROVAL = "05";      // Waiting for Approval
        private readonly string CONST_CLOSED_BY_USER = "99";            // Closed by User
        #endregion

        #endregion

        #endregion

        #region Enums
        private enum ActionTypes
        {
            View,
            Edit,
            Add,
            Delete,
            Approval
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
            LEAVETYPES,         // Leave Request Types
            LEAVEAPVFLAG,       // Leave Approval Flags
            LEAVEAPORTION,      // Leave Day Portions
            STATUS              // Leave Statuses
        }

        public enum LeaveDayMode : byte
        {
            NotDefined,
            FullDay,
            FirstHalf,
            SecondHalf
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

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_leaveRequest);

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

            if (CallerForm == "LeaveInquiry")
                _forceLoad = true;
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

                    // Get the request item from the application state
                    _requestItem = State.RequestItem!;

                    // Initialize the Leave Request object
                    _leaveRequest.LeaveCreatedBy = UserEmpNo;
                    _leaveRequest.LeaveCreatedEmail = UserEmail;
                    _leaveRequest.LeaveCreatedUserID = UserName;
                    _leaveRequest.LeaveType = "AL";
                    _leaveRequest.LeaveTypeDesc = "Annual Leave";

                    //BeginLoadComboboxTask();

                    if (ActionType == ActionTypes.Edit.ToString() ||
                        ActionType == ActionTypes.View.ToString() ||
                        ActionType == ActionTypes.Approval.ToString())
                    {
                        if (LeaveRequestNo > 0)
                        {
                            #region Load request details and workflow

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
                                _employeeArray = _employeeList.Select(d => d.EmployeeNameWithCostCenter).OrderBy(d => d).ToArray();
                            }
                            #endregion

                            #region Get UDCs

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
                                    #region Leave Day Portions
                                    try
                                    {
                                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.LEAVEAPORTION.ToString()).FirstOrDefault()!.UDCGroupId;
                                    }
                                    catch (Exception ex)
                                    {
                                        _errorMessage.Append($"Error getting Leave Mode group id: {ex.Message}");
                                    }

                                    if (groupID > 0)
                                    {
                                        _leaveModeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                                        if (_leaveModeList != null)
                                            _leaveModeArray = _leaveModeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                                    }
                                    #endregion

                                    #region Leave Statuses
                                    try
                                    {
                                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.STATUS.ToString()).FirstOrDefault()!.UDCGroupId;
                                    }
                                    catch (Exception ex)
                                    {
                                        _errorMessage.Append($"Error getting Leave Status group id: {ex.Message}");
                                    }

                                    if (groupID > 0)
                                    {
                                        _leaveStatusList = udcData!.Where(a => a.GroupID == groupID).ToList();
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            // Load request details
                            await GetLeaveRequestDetail(LeaveRequestNo);

                            #endregion

                            _isDisabled = true;

                            // Refresh the page
                            StateHasChanged();
                        }
                    }
                    else
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
                #region Check if Leave Start Date is public holiday or not
                bool isStartDateHoliday = LeaveService.CheckIfLeaveDateIsHolidayAsync(_leaveRequest.LeaveStartDate!.Value).Result;
                if (isStartDateHoliday)
                {
                    _hasValidationError = true;
                    _validationMessages.Add("Start Date should not be a public holiday.");
                }
                #endregion

                #region Check if Leave Resume Date is public holiday or not
                bool isResumeDateHoliday = LeaveService.CheckIfLeaveDateIsHolidayAsync(_leaveRequest.LeaveResumeDate!.Value).Result;
                if (isResumeDateHoliday)
                {
                    _hasValidationError = true;
                    _validationMessages.Add("Resume Date should not be a public holiday.");
                }
                #endregion

                #region Check if leave period exist
                //bool isLeaveExist = LeaveService.CheckIfLeavePeriodExistAsync(
                //    _leaveRequest.LeaveEmpNo,
                //    _leaveRequest.LeaveResumeDate!.Value).Result;
                //if (isLeaveExist)
                //{
                //    _hasValidationError = true;
                //    _validationMessages.Add("The specified date period overlaps with an existing leave request.");
                //}
                #endregion

                if (_hasValidationError && _validationMessages.Any())
                    return;

                // If we got here, model is valid
                _hasValidationError = false;
                _validationMessages.Clear();

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Submitting planned leave request, please wait...";

                _ = SubmitLeaveRequestAsync(async () =>
                {
                    _isRunning = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    if (_leaveRequest.LeaveRequestId > 0)
                    {
                        HandleBackButton();
                        //BeginLoadLeaveRequest(_outdoorRequest.PlannedLeaveId);
                    }
                });
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
        }

        private async Task HandleRefreshButton()
        {
            // Reset Leave Request object
            _leaveRequest = new();
            _leaveRequest.LeaveCreatedBy = UserEmpNo;
            _leaveRequest.LeaveCreatedEmail = UserEmail;
            _leaveRequest.LeaveCreatedUserID = UserName;
            _leaveRequest.StartDayMode = null;
            _leaveRequest.EndDayMode = null;
            _hasValidationError = false;
            _validationMessages.Clear();

            ShowHideError(false);

            #region Reset file attachments
            _files = Array.Empty<IBrowserFile>();
            await _endDayMode.ClearAsync();
            await _startDayMode.ClearAsync();

            StateHasChanged();
            #endregion
        }

        private void HandleBackButton()
        {
            switch (CallerForm)
            {
                case "TNADashboard":
                    Navigation.NavigateTo("/TimeAttendance/tnadashboard");
                    break;

                case "LeavePlanner":
                    Navigation.NavigateTo($"/TimeAttendance/leaveinquiry?ForceLoad={_forceLoad}");
                    break;

                case "ApprovalDashboard":
                    Navigation.NavigateTo("/Workflow/ApprovalDashboard?RequestType=RTYPEREGULAR");
                    break;

                default:
                    Navigation.NavigateTo("/TimeAttendance/tnadashboard");
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
                { "ContentText", "Are you sure you want to cancel submitting leave request?" },
                { "ConfirmText", "Yes" },
                { "CancelText", "No" },
                { "Color", Color.Success }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                CancelAddingLeaveRequest();
            }
        }

        private void CancelAddingLeaveRequest()
        {
            if (!string.IsNullOrEmpty(CallerForm))
            {
                switch (CallerForm)
                {
                    case "TNADashboard":
                        Navigation.NavigateTo("/TimeAttendance/tnadashboard");
                        break;

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

        private async Task ConfirmDelete(LeaveRequisitionDTO leaveRequest)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete Leave Request No. '{leaveRequest.LeaveRequestId}'?" },
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

        private async Task<IEnumerable<string>> SearchLeaveMode(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _leaveModeArray!;
            }

            return _leaveModeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private void OnStartDateChanged(DateTime? date)
        {
            //_isRunning = true;

            //Set the overlay message
            overlayMessage = "Calculating leave duration, please wait...";

            _ = CalculateLeaveDurationAsync(async () =>
            {
                //_isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

            }, "LeaveStartDate", date);
        }

        private void OnResumeDateChanged(DateTime? date)
        {
            //_isRunning = true;

            //Set the overlay message
            overlayMessage = "Calculating leave duration, please wait...";

            _ = CalculateLeaveDurationAsync(async () =>
            {
                //_isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

            }, "LeaveResumeDate", date);
        }

        private void OnStartDayModeChanged(string newValue)
        {
            if (_leaveRequest.StartDayMode != newValue)
            {
                _leaveRequest.StartDayMode = newValue;

                // Get the description
                if (_leaveModeList.Any())
                {
                    UserDefinedCodeDTO? udc = _leaveModeList.Where(s => s.UDCCode.Trim() == newValue).FirstOrDefault();
                    if (udc != null)
                    {
                        _leaveRequest.StartDayModeDesc = udc.UDCDesc1;
                    }
                }

                #region Calculate duration
                if (_leaveRequest.LeaveStartDate.HasValue &&
                    _leaveRequest.LeaveResumeDate.HasValue)
                {
                    _ = CalculateLeaveDurationAsync(async () =>
                    {
                        // Shows the spinner overlay
                        await InvokeAsync(StateHasChanged);

                    }, "LeaveStartDate", _leaveRequest.LeaveStartDate);
                }
                #endregion
            }
        }

        private void OnResumeDayModeChanged(string newValue)
        {
            if (_leaveRequest.EndDayMode != newValue)
            {
                _leaveRequest.EndDayMode = newValue;

                // Get the description
                if (_leaveModeList.Any())
                {
                    UserDefinedCodeDTO? udc = _leaveModeList.Where(s => s.UDCCode.Trim() == newValue).FirstOrDefault();
                    if (udc != null)
                    {
                        _leaveRequest.EndDayModeDesc = udc.UDCDesc1;
                    }
                }

                #region Calculate leave duration
                if (_leaveRequest.LeaveStartDate.HasValue &&
                    _leaveRequest.LeaveResumeDate.HasValue)
                {
                    _ = CalculateLeaveDurationAsync(async () =>
                    {
                        // Shows the spinner overlay
                        await InvokeAsync(StateHasChanged);

                    }, "LeaveResumeDate", _leaveRequest.LeaveResumeDate);
                }
                #endregion
            }
        }

        private void OnEmployeeChanged(int newValue)
        {
            if (_leaveRequest.LeaveEmpNo != newValue)
            {
                _leaveRequest.LeaveEmpNo = newValue;

                // Get the employee details
                if (_employeeList.Any())
                {
                    EmployeeResultDTO? employee = _employeeList.Where(e => e.EmployeeNo == newValue).FirstOrDefault();
                    if (employee != null)
                    {
                        _leaveRequest.LeaveEmpName = employee.EmployeeFullName;
                        _leaveRequest.LeaveEmpCostCenter = employee.DepartmentCode;
                        _leaveRequest.LeaveEmpEmail = employee.EmpEmail;
                        _leaveRequest.LeaveBalance = employee.LeaveBalance.HasValue ? Convert.ToDouble(employee.LeaveBalance) : 0;
                    }
                }
            }
        }

        private async Task ConfirmCancel()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Cancel"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to cancel request no. '{_leaveRequest.LeaveRequestId}'?" },
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

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Planned Leave Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                BeginLeaveCancellation(_leaveRequest);
            }
        }

        private void BeginLeaveCancellation(LeaveRequisitionDTO leaveRequest)
        {
            try
            {
                // Exit process if leave status is Cancelled
                if (_leaveRequest.StatusHandlingCode == "Cancelled")
                {
                    _hasValidationError = true;
                    _validationMessages.Add("Unable to cancel leave request because it was already been cancelled!");
                    return;
                }

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Cancelling planeed leave request, please wait...";

                _ = CancelLeaveRequestAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    StateHasChanged();

                }, leaveRequest);
            }
            catch (OperationCanceledException)
            {
                ShowNotification("Leave cancellation aborted (navigated away).", NotificationType.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
        }
        #endregion

        #region Database Methods
        private void BeginLoadComboboxTask()
        {
            _isRunning = true;

            // Set the overlay message
            //if (!State.IsAuthenticated)
            if (!UserSession.IsAuthenticated())
                overlayMessage = "Authentication required. Redirecting to login page...";
            else
                overlayMessage = "Initializing form, please wait...";

            _ = LoadComboboxAsync(async () =>
            {
                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                if (LeaveRequestNo > 0)
                {
                    BeginLoadLeaveRequest(LeaveRequestNo);
                }
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
                _employeeArray = _employeeList.Select(d => d.EmployeeNameWithCostCenter).OrderBy(d => d).ToArray();
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
                    #region Leave Day Portions
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.LEAVEAPORTION.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Leave Mode group id: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _leaveModeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_leaveModeList != null)
                            _leaveModeArray = _leaveModeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Leave Statuses
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.STATUS.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Leave Status group id: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _leaveStatusList = udcData!.Where(a => a.GroupID == groupID).ToList();
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

        private async Task SubmitLeaveRequestAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            bool isNewRequition = _leaveRequest.LeaveRequestId == 0;

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = true;
            string errorMsg = string.Empty;

            if (isNewRequition)
            {
                // Set leave request information and flags 
                _leaveRequest.LeaveCreatedDate = DateTime.Now;
                //_leaveRequest.LeaveApprovalFlag = CONST_WAITING_APPROVAL;
                _leaveRequest.LeaveEndDate = _leaveRequest.LeaveResumeDate!.Value.AddDays(-1);

                #region Set request status to "Closed By User" 
                if (_leaveStatusList != null
                    && _leaveStatusList.Any())
                {
                    UserDefinedCodeDTO? statusFlag = _leaveStatusList.Where(s => s.UDCCode == CONST_CLOSED_BY_USER).FirstOrDefault();
                    if (statusFlag != null)
                    {
                        _leaveRequest.LeaveStatusCode = statusFlag.UDCCode;
                        _leaveRequest.LeaveStatusID = statusFlag.UDCId;
                        _leaveRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                    }
                }
                #endregion

                var addResult = await LeaveService.AddPlannedLeaveRequestAsync(_leaveRequest, _cts.Token);
                isSuccess = addResult.Success;
                if (!isSuccess)
                    errorMsg = addResult.Error!;
                else
                {
                    // Set flag to enable reload when navigating back to the caller page
                    _forceLoad = true;

                    // Set action type flag
                    ActionType = ActionTypes.Edit.ToString();

                    // Get the new identity seed value
                    _leaveRequest.LeaveRequestId = addResult.Value;

                    // Display the requisition number in the page title
                    //_pageTitle = $" Submitted Leave Request No. {addResult.Value}";
                }
            }
            else
            {
                // Set the user who update the record and the timestamp
                _leaveRequest.LeaveUpdatedDate = DateTime.Now;
                _leaveRequest.LeaveEndDate = _leaveRequest.LeaveResumeDate!.Value.AddDays(-1);

                #region Set leave status to "Closed By User" 
                if (_leaveStatusList != null && _leaveStatusList.Any())
                {
                    UserDefinedCodeDTO? statusFlag = _leaveStatusList.Where(s => s.UDCCode == CONST_CLOSED_BY_USER).FirstOrDefault();
                    if (statusFlag != null)
                    {
                        _leaveRequest.LeaveStatusCode = statusFlag.UDCCode;
                        _leaveRequest.LeaveStatusID = statusFlag.UDCId;
                        _leaveRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                    }
                }
                #endregion

                var saveResult = await LeaveService.UpdatePlannedLeaveRequestAsync(_leaveRequest, _cts.Token);
                isSuccess = saveResult.Success;
                if (!isSuccess)
                    errorMsg = saveResult.Error!;
            }

            if (isSuccess)
            {
                // Reset flags
                _isEditMode = false;
                _saveBtnEnabled = false;
                _isDisabled = true;

                // Hide error message if any
                ShowHideError(false);

                // Show notification
                if (isNewRequition)
                    ShowNotification("Planned leave request has been submitted successfully!", NotificationType.Success);
                else
                    ShowNotification("Planned leave request has been updated successfully!", NotificationType.Success);

                // Go back to T&A dashboard
                //Navigation.NavigateTo("/TimeAttendance/tnadashboard");
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

        private async Task CalculateLeaveDurationAsync(Func<Task> callback, string inputDateName, DateTime? selectedDate)
        {
            try
            {
                // Reset errors
                _errorMessage.Clear();

                // Get the Full Day udc key
                UserDefinedCodeDTO udc = _leaveModeList.Where(a => a.UDCCode == CONST_LEAVE_FULL_DAY).FirstOrDefault();

                if (inputDateName == "LeaveStartDate")
                {
                    _leaveRequest.LeaveStartDate = selectedDate;
                    if (_leaveRequest.StartDayMode == null)
                    {
                        if (_leaveModeList != null && _leaveModeList.Any())
                        {
                            if (udc != null)
                            {
                                _leaveRequest.StartDayMode = udc.UDCCode;
                                _leaveRequest.StartDayModeDesc = udc.UDCDesc1;
                            }
                        }
                    }
                }
                else if (inputDateName == "LeaveResumeDate")
                {
                    _leaveRequest.LeaveResumeDate = selectedDate;
                    if (_leaveRequest.EndDayMode == null)
                    {
                        if (_leaveModeList != null && _leaveModeList.Any())
                        {
                            if (udc != null)
                            {
                                _leaveRequest.EndDayMode = udc.UDCCode;
                                _leaveRequest.EndDayModeDesc = udc.UDCDesc1;
                            }
                        }
                    }
                }

                if (_leaveRequest.LeaveStartDate.HasValue &&
                    _leaveRequest.LeaveResumeDate.HasValue)
                {
                    _leaveDuration = await LeaveService.CalculateAsync(
                                    _leaveRequest.LeaveEmpNo,
                                    Convert.ToDateTime(_leaveRequest.LeaveStartDate).Date,
                                    Convert.ToDateTime(_leaveRequest.LeaveResumeDate).Date,
                                    _leaveRequest.StartDayMode,
                                    _leaveRequest.EndDayMode);

                    _leaveRequest.LeaveDuration = Convert.ToDouble(_leaveDuration);
                    _leaveRequest.NoOfHolidays = await LeaveService.HolidayCount();
                    _leaveRequest.NoOfWeekends = await LeaveService.WeekendCount();
                }

                if (callback != null)
                {
                    // Hide the spinner overlay
                    await callback.Invoke();
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(MessageBoxTypes.Error, "Error", ex.Message.ToString());
            }
        }

        private async Task CancelLeaveRequestAsync(Func<Task> callback, LeaveRequisitionDTO leaveRequest)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = true;
            string errorMsg = string.Empty;

            // Set the user who update the record and the timestamp
            leaveRequest.LeaveUpdatedDate = DateTime.Now;
            //leaveRequest.LeaveApprovalFlag = CONST_CANCELLED;

            #region Set workflow status to "101 - Cancelled by User" 
            if (_leaveStatusList != null && _leaveStatusList.Any())
            {
                UserDefinedCodeDTO? statusFlag = _leaveStatusList.Where(s => s.UDCCode == CONST_CANCELLED_BY_USER).FirstOrDefault();
                if (statusFlag != null)
                {
                    leaveRequest.LeaveStatusCode = statusFlag.UDCCode;
                    leaveRequest.LeaveStatusID = statusFlag.UDCId;
                    leaveRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                }
            }
            #endregion

            var saveResult = await LeaveService.CancelPlannedLeaveRequestAsync(leaveRequest, _cts.Token);
            isSuccess = saveResult.Success;
            if (!isSuccess)
                errorMsg = saveResult.Error!;

            if (isSuccess)
            {
                // Reset flags
                _isEditMode = false;
                _saveBtnEnabled = false;
                _isDisabled = true;

                // Hide error message if any
                ShowHideError(false);

                // Show notification
                ShowNotification("Planned leave request has been cancelled successfully!", NotificationType.Success);

                // Go back to T&A dashboard
                Navigation.NavigateTo("/TimeAttendance/tnadashboard");
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

        private async void BeginLoadLeaveRequest(long requestNo)
        {
            // Load regularization details
            await GetLeaveRequestDetail(requestNo);
        }

        private async Task GetLeaveRequestDetail(long leaveRequestNo)
        {
            // Reset error messages and flags
            _errorMessage.Clear();
            _isCurrentApprover = false;
            _isCreator = false;

            // Clear attachment list
            _files = Array.Empty<IBrowserFile>();

            var result = await LeaveService.GetPlannedLeaveRequestAsync(leaveRequestNo);
            if (result.Success)
            {
                _leaveRequest = result.Value!;

                if (_leaveRequest.LeaveCreatedBy == UserEmpNo ||
                    _leaveRequest.LeaveEmpNo == UserEmpNo)
                    _isCreator = true;

                // Recreate the EditContext with the loaded request item
                _editContext = new EditContext(_leaveRequest);
            }
            else
            {
                // Set the error message
                _errorMessage.Append(result.Error);

                ShowHideError(true);
            }
        }
        #endregion
    }
}
