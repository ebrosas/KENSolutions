using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class ManageLeaveEntitlement : IPageAuthorization
    {
        #region Parameters and Injections
        [Inject] private ILeaveRequestService LeaveService { get; set; } = default!;
        [Inject] private IEmployeeService EmployeeService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState State { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public bool ForceLoad { get; set; } = false;
        #endregion

        #region Fields
        private StringBuilder _errorMessage = new StringBuilder();
        private string _searchString = string.Empty;
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private int? _empNo = null;
        private string _selectedDepartment = string.Empty;
        private DateTime? _selectedStartDate = null;
        private DateTime? _selectedEndDate = null;
        private MudDatePicker _startDatePicker;
        private MudDatePicker _endDatePicker;

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
        private List<string> _validationMessages = new();
        private List<string> _events = new();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Leave Entitlement", href: null, icon: @Icons.Material.Filled.AccountBalance, disabled: true)
        ];

        private List<LeaveEntitlementDTO> _leaveEntitlementList = new List<LeaveEntitlementDTO>();
        private IReadOnlyList<EmployeeDTO> _employeeList = new List<EmployeeDTO>();
        private string[]? _employeeArray = null;

        private List<UserDefinedCodeDTO> _uomList = new List<UserDefinedCodeDTO>();
        private string[]? _uomArray = null;

        private List<UserDefinedCodeDTO> _renewalList = new List<UserDefinedCodeDTO>();
        private string[]? _renewalArray = null;
                
        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        private string[]? _departmentArray = null;
        #endregion

        #endregion

        #region Enums
        private enum UDCKeys
        {
            RENEWTYPE,      // Renewal Types
            LEAVEUOM,       // Leave Entitlement Unit of Measure
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
            //if (LeaveEmpNo.HasValue && LeaveEmpNo > 0)
            //    _empNo = LeaveEmpNo;

            //if (LeaveStartDate.HasValue)
            //    _selectedStartDate = LeaveStartDate;

            //if (LeaveEndDate.HasValue)
            //    _selectedResumeDate = LeaveEndDate;
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

                    LoadComboboxTask();
                }
            }
        }
        #endregion

        #region Grid Events 
        private Func<LeaveEntitlementDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.EmployeeNo > 0 && x.EmployeeNo.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.ALEntitlementCount > 0 && x.ALEntitlementCount.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.SLEntitlementCount > 0 && x.SLEntitlementCount!.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.LeaveBalance > 0 && x.LeaveBalance!.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.SLBalance > 0 && x.SLBalance!.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (x.DILBalance > 0 && x.DILBalance.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.EmployeeName) && x.EmployeeName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.ALRenewalTypeDesc) && x.ALRenewalTypeDesc!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.SLRenewalTypeDesc) && x.SLRenewalTypeDesc!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.CreatedUserID) && x.CreatedUserID!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.LastUpdatedUserID) && x.LastUpdatedUserID!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void StartedEditingItem(LeaveEntitlementDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(LeaveEntitlementDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(LeaveEntitlementDTO item)
        {

        }

        private async Task ConfirmDelete(LeaveEntitlementDTO entitlement)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete entitlement information for employee '{entitlement.EmployeeName}'?" },
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
                //BeginDeleteDepartment(entitlement);
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

        private void HandleRefreshButton()
        {
            // Reset error messages
            _errorMessage.Clear();

            // Clear field mappings
            //_leaveNo = null;
            //_empNo = null;
            //_selectedDepartment = string.Empty;
            //_selectedLeaveType = string.Empty;
            //_selectedStatus = string.Empty;
            //_selectedStartDate = null;
            //_selectedResumeDate = null;

            // Clear datagrid datasource
            //_leaveRequestList = new();

            // Reset validation error messages
            _hasValidationError = false;
            _validationMessages.Clear();
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
        private void LoadComboboxTask()
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            if (!State.IsAuthenticated)
                overlayMessage = "Authentication required. Redirecting to login page...";
            else
                overlayMessage = "Initializing form, please wait...";

            _ = GetDepartmentMasterAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                LoadLeaveEntitlementTask();
            });
        }

        //private void BeginLoadEntitlementTask(bool forceLoad = false)
        //{
        //    // Reset validation errors
        //    _hasValidationError = false;
        //    _validationMessages.Clear();

        //    #region Check if date period is valid
        //    if (_selectedStartDate.HasValue && _selectedEndDate.HasValue &&
        //        _selectedStartDate > _selectedEndDate)
        //    {
        //        _hasValidationError = true;
        //        _validationMessages.Add("Start Date cannot be greater than End Date.");
        //    }
        //    #endregion

        //    if (_hasValidationError && _validationMessages.Any())
        //        return;

        //    _isTaskFinished = false;
        //    _isRunning = true;

        //    // Set the overlay message
        //    if (!State.IsAuthenticated)
        //        overlayMessage = "Authentication required. Redirecting to login page...";
        //    else
        //        overlayMessage = "Loading leave requisitions, please wait...";

        //    _ = GetLeaveEntitlementAsync(async () =>
        //    {
        //        _isTaskFinished = true;
        //        _isRunning = false;

        //        // Shows the spinner overlay
        //        await InvokeAsync(StateHasChanged);
        //    }, forceLoad);
        //}

        private void LoadLeaveEntitlementTask()
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
            if (!State.IsAuthenticated)
                overlayMessage = "Authentication required. Redirecting to login page...";
            else
                overlayMessage = "Loading leave entitlements, please wait...";

            _ = GetLeaveEntitlementAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            });
        }
        #endregion

        #region Database Methods
        private async Task GetDepartmentMasterAsync(Func<Task> callback)
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
                    #region Get Leave Renewal Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.RENEWTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Renewal Type group id: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _renewalList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_renewalList != null)
                            _renewalArray = _renewalList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Leave Unit of Measure
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.LEAVEUOM.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Leave UOM group id: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _uomList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_uomList != null)
                            _uomArray = _uomList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
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

        private async Task GetLeaveEntitlementAsync(Func<Task> callback)
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

            var repoResult = await LeaveService.GetLeaveEntitlementAsync(
                null,
                _empNo,
                costCenter,
                _selectedStartDate,
                _selectedEndDate);
            if (repoResult.Success)
            {
                _leaveEntitlementList = repoResult.Value!;
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
