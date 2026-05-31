using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;

using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Application.DTOs.TNA;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class TimeCard: ComponentBase, IPageAuthorization
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
        #endregion

        #region Fields
        private string _pageTitle = "Time Card";
        private StringBuilder _errorMessage = new StringBuilder();
        private int? _empNo = null;
        private string _selectedCostCenter = string.Empty;
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
        private bool _hasValidationError = false;
        #endregion

        #region Objects and collections
        private MudDatePicker _startDatePicker;
        private MudDatePicker _endDatePicker;
        private List<TimecardResultDTO> _timeCardList = new List<TimecardResultDTO>();
        private List<string> _validationMessages = new();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Time Card", href: null, icon: @Icons.Material.Filled.ManageSearch, disabled: true)
        ];

        private string[]? _departmentArray = null;
        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        #endregion

        #endregion

        #region Enums
        private enum UDCKeys
        {
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
            if (SearchEmpNo.HasValue && SearchEmpNo > 0)
                _empNo = SearchEmpNo;

            if (SearchStartDate.HasValue)
                _selectedStartDate = SearchStartDate;

            if (SearchEndDate.HasValue)
                _selectedEndDate = SearchEndDate;
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
        private Func<TimecardResultDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (!string.IsNullOrEmpty(x.CostCenter) && x.CostCenter.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.CostCenterName) && x.CostCenterName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.EmployeeName) && x.EmployeeName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.Position) && x.Position!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.ShiftDetails) && x.ShiftDetails!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.AttendanceRemarks) && x.AttendanceRemarks!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void StartedEditingItem(TimecardResultDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(TimecardResultDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(TimecardResultDTO item)
        {

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

                if (ForceLoad)
                {
                    BeginSearchTimeCard(ForceLoad);
                }
            });
        }

        private void HandleRefreshButton()
        {
            // Reset error messages
            _errorMessage.Clear();

            // Clear field mappings
            _pageTitle = "Time Card";
            _empNo = null;
            _selectedCostCenter = string.Empty;
            _selectedStartDate = null;
            _selectedEndDate = null;

            // Clear datagrid datasource
            _timeCardList = new();

            // Reset validation error messages
            _hasValidationError = false;
            _validationMessages.Clear();
        }

        private void BeginSearchTimeCard(bool forceLoad = false)
        {
            // Reset validation errors
            _hasValidationError = false;
            _validationMessages.Clear();

            #region Check if start date is specified
            //if (!_selectedStartDate.HasValue)
            //{
            //    _hasValidationError = true;
            //    _validationMessages.Add("Start Date is required.");
            //}
            #endregion

            #region Check if end date is specified
            //if (!_selectedEndDate.HasValue)
            //{
            //    _hasValidationError = true;
            //    _validationMessages.Add("End Date is required.");
            //}
            #endregion

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
                overlayMessage = "Loading regularization requests, please wait...";

            _ = SearchTimecardAsync(async () =>
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
           
            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task SearchTimecardAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            #region Get the selected Department 
            string costCenter = string.Empty;
            if (!string.IsNullOrEmpty(_selectedCostCenter))
            {
                DepartmentDTO? deptDTO = _departmentList.Where(d => d.DepartmentFullName == _selectedCostCenter).FirstOrDefault();
                if (deptDTO != null)
                    costCenter = deptDTO.DepartmentCode;
            }
            #endregion

            var repoResult = await AttendanceService.SearchTimecardAsync(
               _selectedStartDate,
               _selectedEndDate,
               costCenter,
               _empNo);
            if (repoResult.Success)
            {
                // Set the page title
                if (_selectedStartDate.HasValue && _selectedEndDate.HasValue)
                    _pageTitle = $"Time Card for Attandance Cycle from {_selectedStartDate!.Value.ToString("dd MMMM yyyy")} to {_selectedEndDate!.Value.ToString("dd MMMM yyyy")}";
                else
                    _pageTitle = "Time Card";

                _timeCardList = repoResult.Value!;
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
