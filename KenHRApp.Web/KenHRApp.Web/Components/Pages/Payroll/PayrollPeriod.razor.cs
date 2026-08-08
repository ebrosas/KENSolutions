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

namespace KenHRApp.Web.Components.Pages.Payroll
{
    public partial class PayrollPeriod : ComponentBase, IPageAuthorization
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

        private enum RequestTypeCodes
        {
            RTYPEREGULAR,
            RTYPEOT,
            RTYPEOUTDOOR
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
        #endregion

        #region Asynchronous Tasks
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
        #endregion
    }
}
