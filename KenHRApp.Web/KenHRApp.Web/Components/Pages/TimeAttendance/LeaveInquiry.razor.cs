using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class LeaveInquiry : IPageAuthorization
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
        private MudDatePicker _dojPicker;
        private StringBuilder _errorMessage = new StringBuilder();
        private string? _empNo = null;
        private string? _leaveNo = null;
        private string _selectedLeaveType = string.Empty;
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
        private bool _isSearchHovered = false;
        private bool _isResetHovered = false;
        private bool _isTaskFinished = false;
        private bool _isRunning = false;
        private bool _showErrorAlert = false;
        private bool _enableFilter = false;
        private bool _isActive = true;
        private bool _redirected;
        #endregion

        #region Objects and collections
        private List<LeaveRequestResultDTO> _leaveRequestList = new List<LeaveRequestResultDTO>();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Leave Inquiry", href: null, icon: @Icons.Material.Filled.AccountBalance, disabled: true)
        ];

        private string[]? _leaveTypeArray = null;
        private List<UserDefinedCodeDTO> _leaveTypeList = new List<UserDefinedCodeDTO>();

        private string[]? _leaveStatusArray = null;
        private List<UserDefinedCodeDTO> _leaveStatusList = new List<UserDefinedCodeDTO>();

        private string[]? _departmentArray = null;
        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        #endregion

        #endregion

        #region Enums
        private enum UDCKeys
        {
            EMPSTATUS,      // Employee Status
            EMPLOYTYPE,     // Employment Type
            DEPARTMENT,     // Departments
            DEPTGROUP       // Group Codes
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

                    //BeginLoadComboboxTask();
                }
            }
        }
        #endregion

        #region Grid Events 
        private Func<LeaveRequestResultDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            //if (!string.IsNullOrEmpty(x.DepartmentCode) && x.DepartmentCode.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (!string.IsNullOrEmpty(x.DepartmentName) && x.DepartmentName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (!string.IsNullOrEmpty(x.Description) && x.Description!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (!string.IsNullOrEmpty(x.GroupName) && x.GroupName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (!string.IsNullOrEmpty(x.SuperintendentName) && x.SuperintendentName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (!string.IsNullOrEmpty(x.ManagerName) && x.ManagerName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
            //    return true;

            return false;
        };

        private void StartedEditingItem(LeaveRequestResultDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(LeaveRequestResultDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(LeaveRequestResultDTO item)
        {

        }

        private async Task ConfirmDelete(LeaveRequestResultDTO leaveRequest)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete leave requisition no. '{leaveRequest.LeaveRequestId}'?" },
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

        public void OpenLeaveRequest()
        {
            Navigation.NavigateTo("/TimeAttendance/leaverequest?ActionType=Add&CallerForm=TNADashboard");
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

        private async Task<IEnumerable<string>> SearchLeaveStatus(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _leaveStatusArray!;
            }

            return _leaveStatusArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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
    }
}
