using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.DTOs.TNA;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Common.Helpers;
using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class AttendanceCorrectionInq : ComponentBase, IPageAuthorization
    {
        #region Parameters and Injections
        [Inject] private IAttendanceService AttendanceService { get; set; } = default!;
        [Inject] private IEmployeeService EmployeeService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IUserSessionService UserSession { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public bool ForceLoad { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery]
        public int? SearchEmpNo { get; set; } = null;

        [Parameter]
        [SupplyParameterFromQuery]
        public DateTime? SearchStartDate { get; set; } = null;

        [Parameter]
        [SupplyParameterFromQuery]
        public DateTime? SearchEndDate { get; set; } = null;

        [Parameter]
        [SupplyParameterFromQuery]
        public string? RequestType { get; set; } = null;
        #endregion

        #region Fields
        private StringBuilder _errorMessage = new StringBuilder();
        private int? _empNo = null;
        private int? _requestNo = null;
        private string _selectedRequestType = string.Empty;
        private string _selectedDepartment = string.Empty;
        private string _selectedStatus = string.Empty;
        private DateTime? _selectedStartDate = null;
        private DateTime? _selectedEndDate = null;
        private string _searchString = string.Empty;
        private string overlayMessage = "Please wait...";
        private List<string> _events = new();
        private CancellationTokenSource? _cts;

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        #endregion

        #region Flags
        private bool _isTaskFinished = false;
        private bool _isRunning = false;
        private bool _showErrorAlert = false;
        private bool _enableFilter = false;
        private bool _isActive = true;
        private bool _hasValidationError = false;
        #endregion

        #region Objects and collections
        private MudDatePicker _startDatePicker;
        private MudDatePicker _endDatePicker;
        private List<AttendanceCorrectionDTO> _correctionList = new();
        private List<string> _validationMessages = new();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Attendance Corrections", href: null, icon: @Icons.Material.Filled.ManageSearch, disabled: true)
        ];

        private string[]? _requestTypeArray = null;
        private List<UserDefinedCodeDTO> _requestTypeList = new List<UserDefinedCodeDTO>();

        private string[]? _requestStatusArray = null;
        private List<UserDefinedCodeDTO> _requestStatusList = new List<UserDefinedCodeDTO>();

        private string[]? _departmentArray = null;
        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        #endregion

        #endregion

        #region Enums
        private enum UDCKeys
        {
            REQTYPE,        // Request Types
            STATUS,         // Request Status
            DEPARTMENT      // Departments
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

        private enum WorkflowStatus
        {
            Running,
            Pending,
            InProgress,
            Approved,
            Rejected,
            Cancelled,
            Skipped,
            Completed
        }

        private enum RequestStatus
        {
            Draft,
            Open,
            Approved,
            Closed,
            Rejected,
            Cancelled
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
            #region Process query string values
            if (SearchEmpNo.HasValue && SearchEmpNo > 0)
                _empNo = SearchEmpNo;

            if (SearchStartDate.HasValue)
                _selectedStartDate = SearchStartDate;

            if (SearchEndDate.HasValue)
                _selectedEndDate = SearchEndDate;                       
            #endregion
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

                    BeginLoadComboboxTask();
                }
                else
                    GoToLogin();
            }
        }
        #endregion

        #region Grid Events 
        private Func<AttendanceCorrectionDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (!string.IsNullOrEmpty(x.OrigEmpName) && x.OrigEmpName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.CostCenter) && x.CostCenter.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.CostCenterName) && x.CostCenterName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.RequestTypeDesc) && x.RequestTypeDesc!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.RequestedByName) && x.RequestedByName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.RequestDetail) && x.RequestDetail!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void StartedEditingItem(AttendanceCorrectionDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(AttendanceCorrectionDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(AttendanceCorrectionDTO item)
        {

        }

        private async Task ConfirmDelete(AttendanceCorrectionDTO regularRequest)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete request no. '{regularRequest.RequestNo}'?" },
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
                //BeginDeleteDepartment(leaveRequest);
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

        public void OpenRequestDetail(AttendanceCorrectionDTO item)
        {
            if (item.RequestTypeCode == WorkflowHelper.CONST_EXTRA_TIME)
                Navigation.NavigateTo($"/TimeAttendance/ExtraTime?ActionType=View&RequestNo={item.RequestNo}&CallerForm=AttendanceCorrectInq");

            else if (item.RequestTypeCode == WorkflowHelper.CONST_REGULARIZATION)
                Navigation.NavigateTo($"/TimeAttendance/regularization?ActionType=View&RequestNo={item.RequestNo}&CallerForm=AttendanceCorrectInq");

            else if (item.RequestTypeCode == WorkflowHelper.CONST_OUTDOOR)
                Navigation.NavigateTo($"/TimeAttendance/ApplyOutdoor?ActionType=View&RequestNo={item.RequestNo}&CallerForm=AttendanceCorrectInq");
        }

        private async Task<IEnumerable<string>> SearchRequestType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _requestTypeArray!;
            }

            return _requestTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchRequestStatus(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _requestStatusArray!;
            }

            return _requestStatusArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchDepartment(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _departmentArray!;
            }

            return _departmentArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion

        #region Asynchronous Tasks
        private void BeginLoadComboboxTask()
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            if (!UserSession.IsAuthenticated())
                overlayMessage = "Authentication required. Redirecting to login page...";
            else
                overlayMessage = "Initializing form, please wait...";

            _ = LoadComboboxAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                // Get the selected request type
                if (!string.IsNullOrEmpty(RequestType))
                {
                    UserDefinedCodeDTO? requestTypeUDC = _requestTypeList.Where(d => d.UDCCode == RequestType).FirstOrDefault();
                    if (requestTypeUDC != null)
                        _selectedRequestType = requestTypeUDC.UDCDesc1;
                }

                if (ForceLoad)
                {
                    BeginSearchAttendanceCorrection(ForceLoad);
                }
            });
        }

        private void HandleRefreshButton()
        {
            // Reset error messages
            _errorMessage.Clear();

            // Clear field mappings
            _requestNo = null;
            _empNo = null;
            _selectedDepartment = string.Empty;
            _selectedRequestType = string.Empty;
            _selectedStatus = string.Empty;
            _selectedStartDate = null;
            _selectedEndDate = null;

            // Clear datagrid datasource
            _correctionList = new();

            // Reset validation error messages
            _hasValidationError = false;
            _validationMessages.Clear();
        }

        private void BeginSearchAttendanceCorrection(bool forceLoad = false)
        {
            // Reset validation errors
            _hasValidationError = false;
            _validationMessages.Clear();

            #region Check if date period is valid
            if (_selectedStartDate.HasValue && _selectedEndDate.HasValue &&
                _selectedStartDate > _selectedEndDate)
            {
                _hasValidationError = true;
                _validationMessages.Add("Start Date cannot be greater than End Date.");
            }
            #endregion

            if (_hasValidationError && _validationMessages.Any())
                return;

            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            if (!UserSession.IsAuthenticated())
                overlayMessage = "Authentication required. Redirecting to login page...";
            else
                overlayMessage = "Loading data, please wait...";

            _ = SearchAttendanceCorrectionAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, forceLoad);
        }
        #endregion

        #region Database Methods
        private async Task LoadComboboxAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

            #region Get Department list
            var deptResult = await LookupCache.GetDepartmentMasterAsync();
            if (deptResult.Success)
            {
                _departmentList = deptResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(deptResult.Error);
            }

            if (_departmentList != null)
            {
                _departmentArray = _departmentList
                    .OrderBy(d => d.DepartmentCode)
                    .Select(d => d.DepartmentFullName).ToArray();
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
                    #region Get attendance related request types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.REQTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error fetching data to OT Reasons drop-down box: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _requestTypeList = udcData!.Where(a => a.GroupID == groupID && 
                            a.IsActive == true &&
                            a.UDCSpecialHandlingCode == "ATTENDANCE"
                        ).ToList();
                        if (_requestTypeList != null)
                            _requestTypeArray = _requestTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Request Statuses
                    try
                    {
                        groupID = udcGroupList!
                            .Where(a => a.UDCGCode == UDCKeys.STATUS.ToString())
                            .FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Request Status: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _requestStatusList = udcData!
                            .Where(a => a.GroupID == groupID)
                            .DistinctBy(s => s.UDCSpecialHandlingCode)
                            .ToList();
                        if (_requestStatusList != null)
                            _requestStatusArray = _requestStatusList.Select(s => s.UDCSpecialHandlingCode).OrderBy(s => s).ToArray();
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

        private async Task SearchAttendanceCorrectionAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            #region Get the selected Department 
            string costCenter = string.Empty;
            if (!string.IsNullOrEmpty(_selectedDepartment))
            {
                DepartmentDTO? deptDTO = _departmentList.Where(d => d.DepartmentFullName == _selectedDepartment).FirstOrDefault();
                if (deptDTO != null)
                    costCenter = deptDTO.DepartmentCode;
            }
            #endregion

            #region Get the selected request type
            string requestType = string.Empty;
            if (!string.IsNullOrEmpty(_selectedRequestType))
            {
                UserDefinedCodeDTO? requestTypeUDC = _requestTypeList.Where(d => d.UDCDesc1 == _selectedRequestType).FirstOrDefault();
                if (requestTypeUDC != null)
                    requestType = requestTypeUDC.UDCCode;
            }
            #endregion

            #region Get the selected status
            string status = string.Empty;
            if (!string.IsNullOrEmpty(_selectedStatus))
            {
                UserDefinedCodeDTO? statusUDC = _requestStatusList.Where(d => d.UDCSpecialHandlingCode == _selectedStatus).FirstOrDefault();
                if (statusUDC != null)
                    status = statusUDC.UDCCode;
            }
            #endregion

            var repoResult = await AttendanceService.SearchAttendanceCorrectionAsync(
                _requestNo,
                _empNo,
                costCenter,
                requestType,
                _selectedStatus,
                _selectedStartDate,
                _selectedEndDate);
            if (repoResult.Success)
            {
                _correctionList = repoResult.Value!;
            }
            else
            {
                // Show error message
                _errorMessage.AppendLine(repoResult.Error);

                ShowHideError(true);
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
