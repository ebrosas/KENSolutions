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

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class LeaveRequisition : IPageAuthorization
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

        [Parameter]
        [SupplyParameterFromQuery]
        public string ActionType { get; set; } = ActionTypes.View.ToString();

        [Parameter]
        [SupplyParameterFromQuery]
        public int ShiftPatternId { get; set; } = 0;
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
            LEAVETYPES,         // Leave Request Types
            LEAVEAPVFLAG,       // Leave Approval Flags
            LEAVEAPORTION,      // Leave Day Portions
            STATUS              // Leave Statuses
        }
        #endregion

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        #endregion

        #region Objects and Collections       
        private LeaveRequisitionDTO _leaveRequest = new();
        private IReadOnlyList<IBrowserFile> _files;

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Leave Request", href: null, disabled: true, @Icons.Material.Filled.CardTravel)
        ];

        private string[]? _leaveTypeArray = null;
        private List<UserDefinedCodeDTO> _leaveTypeList = new List<UserDefinedCodeDTO>();

        private string[]? _leaveModeArray = null;
        private List<UserDefinedCodeDTO> _leaveModeList = new List<UserDefinedCodeDTO>();

        private string[]? _employeeArray = null;
        private IReadOnlyList<EmployeeResultDTO> _employeeList = new List<EmployeeResultDTO>();

        private List<UserDefinedCodeDTO> _leaveStatusList = new();
        #endregion

        #region Constants

        #region Leave Approval Flags
        private readonly char CONST_APPROVED_PAID = 'A';
        private readonly char CONST_WAITING_APPROVAL = 'W';
        private readonly char CONST_APPROVED_NOT_PAID = 'N';
        private readonly char CONST_CANCELLED = 'C';
        private readonly char CONST_DRAFT = 'D';
        private readonly char CONST_REJECTED = 'R';
        #endregion

        #region Leave Request Statuses
        private readonly string CONST_REQUEST_SENT = "02";              // Request Sent
        private readonly string CONST_WAITING_FOR_APPROVAL = "05";      // Waiting for Approval
        #endregion

        #endregion

        #endregion

        #region IPageAuthorization Implementation
        public string UserName { get; set; } = "";
        public string? UserID { get; set; } = "";
        public string? UserEmail { get; set; } = "";
        public int UserEmpNo { get; set; } = 0;
        public string? UserCostCenter { get; set; } = "";

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
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                if (!State.IsAuthenticated)
                    GoToLogin();

                if (State.AuthenticatedUser != null)
                {
                    UserName = State.AuthenticatedUser!.EmployeeFullName;
                    UserEmpNo = State.AuthenticatedUser.EmployeeNo;
                    UserID = State.AuthenticatedUser!.UserID;
                    UserEmail = State.AuthenticatedUser!.OfficialEmail;
                    UserCostCenter = State.AuthenticatedUser!.DepartmentCode;

                    // Initialize the Leave Request object
                    _leaveRequest.LeaveCreatedBy = UserEmpNo;
                    _leaveRequest.LeaveCreatedEmail = UserEmail;
                    _leaveRequest.LeaveCreatedUserID = UserID;
                    _leaveRequest.LeaveStatusCode = CONST_REQUEST_SENT;

                    BeginLoadComboboxTask();
                }
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
                //bool isStartDateHoliday = LeaveService.CheckIfLeaveDateIsHolidayAsync(_leaveRequest.LeaveStartDate).Result;
                //if (isStartDateHoliday)
                //{
                //    _hasValidationError = true;
                //    _validationMessages.Append("Start Date should not be a public holiday.");
                //}
                //#endregion

                //#region Check if Leave Resume Date is public holiday or not
                //bool isResumeDateHoliday = LeaveService.CheckIfLeaveDateIsHolidayAsync(_leaveRequest.LeaveResumeDate).Result;
                //if (isResumeDateHoliday)
                //{
                //    _hasValidationError = true;
                //    _validationMessages.Append("Resume Date should not be a public holiday.");
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
                overlayMessage = "Submitting leave request, please wait...";

                _ = SubmitLeaveRequestAsync(async () =>
                {
                    _isRunning = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);
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
            //}, _shiftPattern.ShiftPatternId);
        }

        private void HandleRefreshButton()
        {
            //if (_shiftPattern.ShiftPatternId > 0)
            //{
            //    BeginGetShiftRosterDetail();
            //}
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

        private async Task<IEnumerable<string>> SearchLeaveTypes(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _leaveTypeArray!;
            }

            return _leaveTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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
        #endregion

        #region Database Methods
        private void BeginLoadComboboxTask()
        {
            _isRunning = true;

            // Set the overlay message
            if (!State.IsAuthenticated)
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
                    #region Get Leave Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.LEAVETYPES.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Leave Types group id: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _leaveTypeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_leaveTypeList != null)
                            _leaveTypeArray = _leaveTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

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

            #region Get the selected leave type
            if (!string.IsNullOrEmpty(_leaveRequest.LeaveTypeDesc))
            {
                UserDefinedCodeDTO? selectedLeaveType = _leaveTypeList
                    .Where(a => a.UDCDesc1 == _leaveRequest.LeaveTypeDesc)
                    .FirstOrDefault();
                if (selectedLeaveType != null)
                    _leaveRequest.LeaveType = selectedLeaveType.UDCCode;
            }
            #endregion

            #region Get the selected employee information 
            if (!string.IsNullOrEmpty(_leaveRequest.LeaveEmpName))
            {
                EmployeeResultDTO? selectedEmployee = _employeeList
                    .Where(a => a.EmployeeNameWithCostCenter == _leaveRequest.LeaveEmpName)
                    .FirstOrDefault();
                if (selectedEmployee != null)
                {
                    _leaveRequest.LeaveEmpNo = selectedEmployee.EmployeeNo;
                    _leaveRequest.LeaveEmpCostCenter = selectedEmployee.DepartmentCode;
                    _leaveRequest.LeaveEmpName = selectedEmployee.EmployeeFullName;
                    _leaveRequest.LeaveEmpEmail = selectedEmployee.EmpEmail;
                    _leaveRequest.LeaveBalance = selectedEmployee.LeaveBalance;
                }
            }
            #endregion

            #region Initialize DTO
            var fileDtos = new List<FileUploadDTO>();

            foreach (var file in _files)
            {
                var stream = file.OpenReadStream(10 * 1024 * 1024);

                fileDtos.Add(new FileUploadDTO
                {
                    FileName = file.Name,
                    ContentType = file.ContentType,
                    Size = file.Size,
                    Content = stream
                });
            }
            #endregion

            if (isNewRequition)
            {
                // Set leave request flags
                _leaveRequest.LeaveCreatedDate = DateTime.Now;
                _leaveRequest.LeaveApprovalFlag = CONST_WAITING_APPROVAL;
                _leaveRequest.LeaveEndDate = _leaveRequest.LeaveResumeDate!.Value.AddDays(-1);

                #region Set leave status to "Request Sent" 
                if (_leaveStatusList != null && _leaveStatusList.Any())
                {
                    UserDefinedCodeDTO? statusFlag = _leaveStatusList.Where(s => s.UDCCode == CONST_REQUEST_SENT).FirstOrDefault();
                    if (statusFlag != null)
                    {
                        _leaveRequest.LeaveStatusCode = statusFlag.UDCCode;
                        _leaveRequest.LeaveStatusID = statusFlag.UDCId;
                        _leaveRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                    }
                }
                #endregion

                var addResult = await LeaveService.AddLeaveRequestAsync(_leaveRequest, fileDtos, Environment.WebRootPath, _cts.Token);
                isSuccess = addResult.Success;
                if (!isSuccess)
                    errorMsg = addResult.Error!;
                else
                {
                    // Set flag to enable reload when navigating back to the caller page
                    _forceLoad = true;
                }
            }
            else
            {
                // Set the user who update the record and the timestamp
                _leaveRequest.LeaveUpdatedDate = DateTime.Now;
                _leaveRequest.LeaveApprovalFlag = CONST_WAITING_APPROVAL;
                _leaveRequest.LeaveEndDate = _leaveRequest.LeaveResumeDate!.Value.AddDays(-1);

                #region Set leave status to "Request Sent" 
                if (_leaveStatusList != null && _leaveStatusList.Any())
                {
                    UserDefinedCodeDTO? statusFlag = _leaveStatusList.Where(s => s.UDCCode == CONST_REQUEST_SENT).FirstOrDefault();
                    if (statusFlag != null)
                    {
                        _leaveRequest.LeaveStatusCode = statusFlag.UDCCode;
                        _leaveRequest.LeaveStatusID = statusFlag.UDCId;
                        _leaveRequest.StatusHandlingCode = statusFlag.UDCSpecialHandlingCode;
                    }
                }
                #endregion

                var saveResult = await LeaveService.UpdateLeaveRequestAsync(_leaveRequest, _cts.Token);
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
                    ShowNotification("Leave request has been submitted successfully!", NotificationType.Success);
                else
                    ShowNotification("Leave request has been updated successfully!", NotificationType.Success);

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

        private async Task CalculateLeaveDurationAsync(Func<Task> callback, string inputDateName, DateTime? selectedDate)
        {
            try
            {
                // Reset errors
                _errorMessage.Clear();

                if (inputDateName == "LeaveStartDate")
                    _leaveRequest.LeaveStartDate = selectedDate;
                else if (inputDateName == "LeaveResumeDate")
                    _leaveRequest.LeaveResumeDate = selectedDate;

                _leaveDuration = await LeaveService.CalculateAsync(
                                _leaveRequest.LeaveEmpNo,
                                Convert.ToDateTime(_leaveRequest.LeaveStartDate).Date,
                                Convert.ToDateTime(_leaveRequest.LeaveResumeDate).Date,
                                _leaveRequest.StartDayMode,
                                _leaveRequest.EndDayMode);

                _leaveRequest.LeaveDuration = Convert.ToDouble(_leaveDuration);

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
        #endregion
    }
}
