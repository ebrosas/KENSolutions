using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.DTOs.TNA;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Common.Helpers;
using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class ExtraTime : ComponentBase, IPageAuthorization, IWorkflowProcess
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
        public long RequestNo { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery]
        public string CallerForm { get; set; } = "";

        [Parameter]
        [SupplyParameterFromQuery]
        public DateTime? AttendanceDate { get; set; }
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
        private string _pageTitle = "Apply Extra Time";
        private int _currentWFIndex = 0;
        private MudDatePicker _picker;
        private DateTime? _selectedDate;
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
        private ApprovalRequestResultDTO? _requestItem;
        private ExtraTimeRequestDTO _overtimeRequest = new();
        private IReadOnlyList<IBrowserFile> _files = Array.Empty<IBrowserFile>();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            //new("Extra Time Inquiry", href: "/TimeAttendance/ExtraTimeInq?ForceLoad=true", icon: Icons.Material.Filled.YoutubeSearchedFor),
            new("Attendance Corrections", href: "/TimeAttendance/AttendanceCorrectInq?ForceLoad=true", icon: Icons.Material.Filled.ManageSearch),
            new("Apply Extra Time", href: null, disabled: true, @Icons.Material.Filled.CardTravel)
        ];

        private List<UserDefinedCodeDTO> _actionList = new List<UserDefinedCodeDTO>();
        private string[]? _actionArray = null;

        private List<UserDefinedCodeDTO> _otReasonList = new List<UserDefinedCodeDTO>();
        private string[]? _otReasonArray = null;

        private string[]? _employeeArray = null;
        private IReadOnlyList<EmployeeResultDTO> _employeeList = new List<EmployeeResultDTO>();

        private Guid _calendarRenderKey = Guid.NewGuid();
        private List<AttendanceSwipeDTO> _attendanceChips { get; set; } = new();
        private List<UserDefinedCodeDTO> _requestStatusList = new();
        private List<WorkflowDetailResultDTO> _workflowList = new List<WorkflowDetailResultDTO>();
        #endregion

        #region Constants
        private readonly string CONST_CANCELLED_BY_USER = "101";         // Cancelled by User
        private readonly string CONST_REQUEST_SENT = "02";              // Request Sent
        private readonly string CONST_WAITING_FOR_APPROVAL = "05";      // Waiting for Approval
        private readonly string CONST_EXTRA_TIME = "Extra Time";
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
            OTREASON,           // OvertimeRequest Reasons
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
        public async Task InitializeWorkflowAsync(long requestNo, int originatorEmpNo)
        {
            bool isSuccess = false;

            try
            {
                if (requestNo == 0)
                    throw new ArgumentException("Request No. is not defined!");

                // Initialize the cancellation token
                _cts = new CancellationTokenSource();

                var repoResult = await WorkflowService.StartWorkflowAsync(WorkflowHelper.CONST_EXTRA_TIME,
                    requestNo, Environment.WebRootPath, originatorEmpNo, _cts.Token);
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
            _editContext = new EditContext(_overtimeRequest);

            if (AttendanceDate.HasValue)
                _selectedDate = AttendanceDate.Value;
            else
                _selectedDate = DateTime.Today;

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

                    _requestItem = State.RequestItem!;

                    // Initialize the request object
                    _overtimeRequest.CreatedBy = UserEmpNo;
                    _overtimeRequest.CreatedEmail = UserEmail;
                    _overtimeRequest.CreatedUserID = UserName;
                    _overtimeRequest.ActionDesc = CONST_EXTRA_TIME;

                    if (ActionType == ActionTypes.Edit.ToString() ||
                        ActionType == ActionTypes.View.ToString() ||
                        ActionType == ActionTypes.Approval.ToString())
                    {
                        if (RequestNo > 0)
                        {
                            #region Load regularization request details and workflow

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
                                    #region Get ROA Types
                                    //try
                                    //{
                                    //    groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.OTREASON.ToString()).FirstOrDefault()!.UDCGroupId;
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    _errorMessage.Append($"Error getting ROA group id: {ex.Message}");
                                    //}

                                    //if (groupID > 0)
                                    //{
                                    //    _requestTypeList = udcData!.Where(a => a.GroupID == groupID && a.IsActive == true).ToList();
                                    //    if (_requestTypeList != null)
                                    //        _requestTypeArray = _requestTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                                    //}
                                    #endregion

                                    #region Attendance Action Types
                                    //try
                                    //{
                                    //    groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.ATTENDACT.ToString()).FirstOrDefault()!.UDCGroupId;
                                    //}
                                    //catch (Exception ex)
                                    //{
                                    //    _errorMessage.Append($"Error getting Action Types group id: {ex.Message}");
                                    //}

                                    //if (groupID > 0)
                                    //{
                                    //    _actionList = udcData!.Where(a => a.GroupID == groupID).ToList();
                                    //    if (_actionList != null)
                                    //        _actionArray = _actionList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                                    //}
                                    #endregion

                                    #region Request Statuses
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
                                        _requestStatusList = udcData!.Where(a => a.GroupID == groupID).ToList();
                                    }
                                    #endregion
                                }
                            }
                            #endregion

                            // Load regularization details
                            await GetExtraTimeDetail(RequestNo);

                            #region Get the workflow data
                            var wfRepo = await WorkflowService.GetWorkflowStatusAsync(WorkflowHelper.CONST_EXTRA_TIME, RequestNo);
                            if (wfRepo.Success)
                            {
                                _workflowList = wfRepo.Value!;

                                if (_workflowList.Any())
                                {
                                    #region Find the current pending activity
                                    WorkflowDetailResultDTO currentAct = _workflowList.Where(w => w.IsCurrent == true).FirstOrDefault()!;
                                    if (currentAct != null)
                                    {
                                        _currentWFIndex = _workflowList.IndexOf(currentAct);
                                    }
                                    #endregion
                                }
                            }
                            else
                            {
                                // Set the error message
                                _errorMessage.Append(result.Error);

                                ShowHideError(true);
                            }
                            #endregion

                            #endregion

                            //BeginLoadOvertimeRequest(RequestNo);

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
                _hasValidationError = false;
                _validationMessages.Clear();

                #region Perform Data Validation
                // Check if Absent or has NPH
                //if (!(_overtimeRequest.RemarkCode == "A" || _overtimeRequest.NoPayHours > 0))
                //{
                //    _hasValidationError = true;
                //    _validationMessages.Add("Regularization is applicable only if the employee is either Absent or has deficit hours.");
                //}

                if (_hasValidationError && _validationMessages.Any())
                    return;
                #endregion

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Submitting extratime request, please wait...";

                _ = SubmitOvertimeRequestAsync(async () =>
                {
                    _isRunning = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    if (_overtimeRequest.ExtratimeId > 0)
                    {
                        // Initiate the workflow
                        await InitializeWorkflowAsync(_overtimeRequest.ExtratimeId, _overtimeRequest.EmployeeNo);

                        BeginLoadOvertimeRequest(_overtimeRequest.ExtratimeId);
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
            //}, _shiftPattern.ExtratimeId);
        }

        private void HandleRefreshButton()
        {
            // Reset Leave Request object
            _overtimeRequest = new();
            _overtimeRequest.CreatedBy = UserEmpNo;
            _overtimeRequest.CreatedEmail = UserEmail;
            _overtimeRequest.CreatedUserID = UserName;

            #region Reset file attachments
            _files = Array.Empty<IBrowserFile>();

            StateHasChanged();
            #endregion
        }

        private void HandleBackButton()
        {
            //if (string.IsNullOrEmpty(CallerForm))
            //    return;

            switch (CallerForm)
            {
                case "TNADashboard":
                    Navigation.NavigateTo("/TimeAttendance/tnadashboard");
                    break;

                case "ApprovalDashboard":
                    Navigation.NavigateTo("/Workflow/ApprovalDashboard");
                    break;

                case "ExtraTimeInq":
                    Navigation.NavigateTo("/TimeAttendance/ExtraTimeInq?ForceLoad=true");
                    break;

                case "AttendanceCorrectInq":
                    Navigation.NavigateTo("/TimeAttendance/AttendanceCorrectInq?ForceLoad=true");
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
                { "ContentText", "Do you really want to delete this extra time request? Note that this process cannot be undone." },
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
                { "ContentText", "Are you sure you want to cancel extra time application?" },
                { "ConfirmText", "Yes" },
                { "CancelText", "No" },
                { "Color", Color.Success }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                HandleBackButton();
            }
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
        private async Task ConfirmDelete(ExtraTimeRequestDTO request)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete Extra Time Request No. '{request.ExtratimeId}'?" },
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
                { "ContentText", $"Are you sure you want to cancel Extra Time Request No. '{_overtimeRequest.ExtratimeId}'?" },
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

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                BeginRequestCancellation(_overtimeRequest);
            }
        }

        private async Task ConfirmReject()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Reject"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to reject Extra Time Request No. '{_overtimeRequest.ExtratimeId}'?" },
                { "ConfirmText", "Ok" },
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

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Reject Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                //BeginRequestCancellation(_overtimeRequest);
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
            if (date.HasValue &&
                _overtimeRequest.EmployeeNo > 0)
            {
                await GetAttendanceInfoAsync(_overtimeRequest.EmployeeNo, date!.Value);
            }
        }

        private void OnEmployeeChanged(int newValue)
        {
            if (_overtimeRequest.EmployeeNo != newValue)
            {
                _overtimeRequest.EmployeeNo = newValue;

                // Get the employee details
                if (_employeeList.Any())
                {
                    EmployeeResultDTO? employee = _employeeList.Where(e => e.EmployeeNo == newValue).FirstOrDefault();
                    if (employee != null)
                    {
                        _overtimeRequest.EmployeeNo = employee.EmployeeNo;
                        _overtimeRequest.CostCenter = employee!.DepartmentCode;
                        _overtimeRequest.EmployeeName = employee.EmployeeFullName;
                    }
                }
            }
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

        private async Task<IEnumerable<string>> SearchOTReason(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _otReasonArray!;
            }

            return _otReasonArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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

        private bool DisableAllDates(DateTime date)
        {
            return _isDisabled;
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
                _overtimeRequest.EmployeeNo = UserEmpNo;
                _overtimeRequest.EmployeeName = UserFullName;

                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                OnDateChanged(_selectedDate);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                if (RequestNo > 0)
                {
                    BeginLoadOvertimeRequest(RequestNo);
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
                    #region Get OT Reasons
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.OTREASON.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error fetching data to OT Reasons drop-down box: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _otReasonList = udcData!.Where(a => a.GroupID == groupID && a.IsActive == true).ToList();
                        if (_otReasonList != null)
                            _otReasonArray = _otReasonList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
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

                    #region Request Statuses
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
                        _requestStatusList = udcData!.Where(a => a.GroupID == groupID).ToList();
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
                        _overtimeRequest.AttendanceDate = attendanceInfo.AttendanceDate;
                        _overtimeRequest.ShiftPattern = attendanceInfo.ShiftRoster;
                        _overtimeRequest.ShiftTiming = attendanceInfo.ShiftTiming;
                        _overtimeRequest.WorkDuration = attendanceInfo.TotalWorkMinute;

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

        private async Task SubmitOvertimeRequestAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            bool isNewRequition = _overtimeRequest.ExtratimeId == 0;

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = true;
            string errorMsg = string.Empty;

            #region Get the selected employee information 
            if (String.IsNullOrWhiteSpace(_overtimeRequest.EmployeeName) ||
                String.IsNullOrWhiteSpace(_overtimeRequest.CostCenter))
            {
                EmployeeResultDTO? selectedEmployee = _employeeList
                    .Where(a => a.EmployeeNo == _overtimeRequest.EmployeeNo)
                    .FirstOrDefault();
                if (selectedEmployee != null)
                {
                    _overtimeRequest.EmployeeNo = selectedEmployee.EmployeeNo;
                    _overtimeRequest.CostCenter = selectedEmployee!.DepartmentCode;
                    _overtimeRequest.EmployeeName = selectedEmployee.EmployeeFullName;
                }
            }
            #endregion

            #region Get the selected OT Reason
            if (!string.IsNullOrEmpty(_overtimeRequest.OTReasonDesc))
            {
                UserDefinedCodeDTO? selectedReason = _otReasonList
                    .Where(a => a.UDCDesc1 == _overtimeRequest.OTReasonDesc)
                    .FirstOrDefault();
                if (selectedReason != null)
                    _overtimeRequest.OTReasonCode = selectedReason.UDCCode;
            }
            #endregion

            #region Get the selected Action
            if (!string.IsNullOrEmpty(_overtimeRequest.ActionDesc))
            {
                UserDefinedCodeDTO? selectedAction = _actionList
                    .Where(a => a.UDCDesc1 == _overtimeRequest.ActionDesc)
                    .FirstOrDefault();
                if (selectedAction != null)
                    _overtimeRequest.ActionCode = selectedAction.UDCCode;
            }
            #endregion

            if (isNewRequition)
            {
                // Set leave request information and flags 
                _overtimeRequest.CreatedDate = DateTime.Now;

                #region Set status to "Waiting for Approval" 
                if (_requestStatusList != null && _requestStatusList.Any())
                {
                    UserDefinedCodeDTO? statusFlag = _requestStatusList.Where(s => s.UDCCode == CONST_WAITING_FOR_APPROVAL).FirstOrDefault();
                    if (statusFlag != null)
                    {
                        _overtimeRequest.StatusCode = statusFlag.UDCCode;
                        _overtimeRequest.StatusID = statusFlag.UDCId;
                        _overtimeRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                    }
                }
                #endregion

                var addResult = await AttendanceService.AddOTRequestAsync(_overtimeRequest, _cts.Token);
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
                    _overtimeRequest.ExtratimeId = addResult.Value;

                    // Display the requisition number in the page title
                    _pageTitle = $" Extra Time Request No. {addResult.Value}";
                }
            }
            else
            {
                // Set the user who update the record and the timestamp
                _overtimeRequest.LastUpdatedDate = DateTime.Now;

                #region Set request status to "Request Sent" 
                if (_requestStatusList != null && _requestStatusList.Any())
                {
                    UserDefinedCodeDTO? statusFlag = _requestStatusList.Where(s => s.UDCCode == CONST_WAITING_FOR_APPROVAL).FirstOrDefault();
                    if (statusFlag != null)
                    {
                        _overtimeRequest.StatusCode = statusFlag.UDCCode;
                        _overtimeRequest.StatusID = statusFlag.UDCId;
                        _overtimeRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                    }
                }
                #endregion

                var saveResult = await AttendanceService.UpdateOTRequestAsync(_overtimeRequest, _cts.Token);
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
                    ShowNotification("Extra time request has been submitted successfully!", NotificationType.Success);
                else
                    ShowNotification("Extra time request has been updated successfully!", NotificationType.Success);
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

        private async void BeginLoadOvertimeRequest(long requestNo)
        {
            // Load regularization details
            await GetExtraTimeDetail(requestNo);

            #region Get the workflow data
            var result = await WorkflowService.GetWorkflowStatusAsync(WorkflowHelper.CONST_EXTRA_TIME, requestNo);
            if (result.Success)
            {
                _workflowList = result.Value!;

                if (_workflowList.Any())
                {
                    #region Find the current pending activity
                    WorkflowDetailResultDTO currentAct = _workflowList.Where(w => w.IsCurrent == true).FirstOrDefault()!;
                    if (currentAct != null)
                    {
                        _currentWFIndex = _workflowList.IndexOf(currentAct);
                    }
                    #endregion
                }
            }
            else
            {
                // Set the error message
                _errorMessage.Append(result.Error);

                ShowHideError(true);
            }
            #endregion
        }

        private async Task GetExtraTimeDetail(long requestNo)
        {
            // Reset error messages
            _errorMessage.Clear();

            // Clear attachment list
            _files = Array.Empty<IBrowserFile>();

            var result = await AttendanceService.GetOTRequestAsync(requestNo);
            if (result.Success)
            {
                _overtimeRequest = result.Value!;

                #region Populate raw swipe chips
                if (_overtimeRequest.SwipeLogList != null && _overtimeRequest.SwipeLogList.Any())
                    _attendanceChips.AddRange(_overtimeRequest.SwipeLogList.ToList());
                #endregion

                // Set the calendar's selected date
                _selectedDate = _overtimeRequest.AttendanceDate;

                // Display the requisition number in the page title
                _pageTitle = $" Extra Time Request #{_overtimeRequest.ExtratimeId} (Created On: {_overtimeRequest.CreatedDate?.ToString("MMM dd, yyyy hh:mm tt")} | Status: {_overtimeRequest.StatusSummary})";

                // Recreate the EditContext with the loaded _overtimeRequest
                _editContext = new EditContext(_overtimeRequest);
            }
            else
            {
                // Set the error message
                _errorMessage.Append(result.Error);

                ShowHideError(true);
            }
        }

        private void BeginRequestCancellation(ExtraTimeRequestDTO regularRequest)
        {
            try
            {
                // Exit process if leave status is Cancelled
                if (regularRequest.StatusHandlingCode == "Cancelled")
                {
                    _hasValidationError = true;
                    _validationMessages.Add("Unable to cancel request because it was already been cancelled!");
                    return;
                }

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Cancelling request, please wait...";

                _ = CancelRequestAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    StateHasChanged();

                }, regularRequest);
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

        private async Task CancelRequestAsync(Func<Task> callback, ExtraTimeRequestDTO regularRequest)
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
            regularRequest.LastUpdatedDate = DateTime.Now;

            #region Set workflow status to "101 - Cancelled by User" 
            if (_requestStatusList != null && _requestStatusList.Any())
            {
                UserDefinedCodeDTO? statusFlag = _requestStatusList.Where(s => s.UDCCode == CONST_CANCELLED_BY_USER).FirstOrDefault();
                if (statusFlag != null)
                {
                    regularRequest.StatusCode = statusFlag.UDCCode;
                    regularRequest.StatusID = statusFlag.UDCId;
                    regularRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                }
            }
            #endregion

            var saveResult = await AttendanceService.CancelOTRequestAsync(regularRequest, _cts.Token);
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
                ShowNotification("Extra time request has been cancelled successfully!", NotificationType.Success);

                HandleBackButton();
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
        #endregion

        #region Workflow Methods
        private void BeginApproveRequest()
        {
            try
            {
                if (_requestItem == null)
                {
                    throw new Exception("The selected request workflow instance is not configured correctly!");
                }
                else
                {
                    if (_requestItem.StepInstanceId == null)
                        throw new Exception("The current workflow instance is not defined!");
                }

                // Get the current WF activity instance id
                int stepInstanceId = _requestItem.StepInstanceId ?? 0;

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Approving request, please wait...";

                _ = ApproveWorkflowAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    StateHasChanged();

                }, stepInstanceId, _requestItem.ApproverNo, UserName!, _requestItem.Remarks, _requestItem.RequestNo);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
        }

        private async Task ApproveWorkflowAsync(
           Func<Task> callback,
           int stepInstanceId,
           int approverNo,
           string userID,
           string? comments,
           long requestNo)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            var repoResult = await WorkflowService.ApproveStepAsync(stepInstanceId, approverNo, userID, comments,
                WorkflowHelper.CONST_LEAVE_REQUEST, requestNo, Environment.WebRootPath, _cts.Token);

            isSuccess = repoResult.Success;
            if (!isSuccess)
                errorMsg = repoResult.Error!;

            if (isSuccess)
            {
                // Show notification
                ShowNotification("Extra Time request has been approved successfully!", NotificationType.Success);
            }
            else
            {
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    // Display error message
                    _errorMessage.AppendLine(errorMsg);
                    ShowHideError(true);
                }
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }
        #endregion
    }
}
