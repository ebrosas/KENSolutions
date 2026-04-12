using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;

namespace KenHRApp.Web.Components.Pages.Workflow
{
    public partial class ApprovalDashboard : IPageAuthorization
    {
        #region Parameters and Injections
        [Inject] private IEmployeeService EmployeeService { get; set; } = default!;
        [Inject] private IWorkflowService WorkflowService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState State { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public bool ForceLoad { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery]
        public string? RequestType { get; set; } = null;

        [Parameter]
        [SupplyParameterFromQuery]
        public int? PeriodType { get; set; } = null;
        #endregion

        #region Fields
        private StringBuilder _errorMessage = new StringBuilder();
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
        private bool _isRunning = false;
        private bool _showErrorAlert = false;
        private bool _enableFilter = false;
        private bool _isActive = true;
        private bool _hasValidationError = false;
        #endregion

        #region Objects and collections
        private List<RequestTypeDTO> _requestTypeList = new List<RequestTypeDTO>();
        private List<RequestApprovalDTO> _approvalList = new List<RequestApprovalDTO>();
        private List<string> _validationMessages = new();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Pending Requests Dashboard", href: null, icon: @Icons.Material.Filled.AccountBalance, disabled: true)
        ];
        #endregion

        #endregion

        #region Enums
        private enum UDCKeys
        {
            LEAVETYPES,     // Leave Types
            STATUS,  // Leave Modes
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

                    BeginLoadComboboxTask();
                }
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
        #endregion

        #region Asynchronous Methods
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
                //_isTaskFinished = true;
                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                //if (ForceLoad)
                //{
                BeginGetPendingRequestTask(ForceLoad);                                
                //}
            });
        }

        private void BeginGetPendingRequestTask(bool forceLoad = false)
        {
            // Reset validation errors
            _hasValidationError = false;
            _validationMessages.Clear();

            #region Check if date period is valid
            //if (_selectedStartDate.HasValue && _selectedResumeDate.HasValue &&
            //    _selectedStartDate > _selectedResumeDate)
            //{
            //    _hasValidationError = true;
            //    _validationMessages.Add("Start Date cannot be greater than End Date.");
            //}
            #endregion

            if (_hasValidationError && _validationMessages.Any())
                return;
                        
            _isRunning = true;

            // Set the overlay message
            if (!State.IsAuthenticated)
                overlayMessage = "Authentication required. Redirecting to login page...";
            else
                overlayMessage = "Loading leave requisitions, please wait...";

            _ = GetPendingRequestAsync(async () =>
            {
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                #region Test the workflow
                //await InitializeWorkflowAsync();
                await ApproveWorkflowAsync(2, 10003634, "Test Approval Leave #2");
                #endregion                

            }, forceLoad);
        }
        #endregion

        #region Database Methods
        private async Task LoadComboboxAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

            #region Get Department list
            //var deptResult = await LookupCache.GetDepartmentMasterAsync();
            //if (deptResult.Success)
            //{
            //    _departmentList = deptResult.Value!;
            //}
            //else
            //{
            //    // Set the error message
            //    _errorMessage.AppendLine(deptResult.Error);
            //}

            //if (_departmentList != null)
            //{
            //    _departmentArray = _departmentList
            //        .OrderBy(d => d.DepartmentCode)
            //        .Select(d => d.DepartmentFullName).ToArray();
            //}
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
                    #region Get Request Types
                    //try
                    //{
                    //    groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.LEAVETYPES.ToString()).FirstOrDefault()!.UDCGroupId;
                    //}
                    //catch (Exception ex)
                    //{
                    //    _errorMessage.Append($"Error getting Leave Types group id: {ex.Message}");
                    //}

                    //if (groupID > 0)
                    //{
                    //    _leaveTypeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                    //    if (_leaveTypeList != null)
                    //        _leaveTypeArray = _leaveTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    //}
                    #endregion
                }
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task GetPendingRequestAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            #region Get the selected Department 
            //string costCenter = string.Empty;
            //if (!string.IsNullOrEmpty(_selectedDepartment))
            //{
            //    DepartmentDTO? deptDTO = _departmentList.Where(d => d.DepartmentFullName == _selectedDepartment).FirstOrDefault();
            //    if (deptDTO != null)
            //        costCenter = deptDTO.DepartmentCode;
            //}
            #endregion

            #region Get the selected Leave Type
            //string leaveType = string.Empty;
            //if (!string.IsNullOrEmpty(_selectedLeaveType))
            //{
            //    UserDefinedCodeDTO? leaveTypeUDC = _leaveTypeList.Where(d => d.UDCDesc1 == _selectedLeaveType).FirstOrDefault();
            //    if (leaveTypeUDC != null)
            //        leaveType = leaveTypeUDC.UDCCode;
            //}
            #endregion

            #region Get the selected status
            //string status = string.Empty;
            //if (!string.IsNullOrEmpty(_selectedStatus))
            //{
            //    UserDefinedCodeDTO? statusUDC = _leaveStatusList.Where(d => d.UDCSpecialHandlingCode == _selectedStatus).FirstOrDefault();
            //    if (statusUDC != null)
            //        status = statusUDC.UDCCode;
            //}
            #endregion

            var repoResult = await WorkflowService.GetPendingRequestAsync(UserEmpNo, string.Empty, 0, null, null);
            if (repoResult.Success)
            {
                _requestTypeList = repoResult.Value!;
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

        private async Task InitializeWorkflowAsync()
        {
            int workflowInstanceID = 0;

            try
            {
                var repoResult = await WorkflowService.StartWorkflowAsync("LeaveRequisitionWF", 10);
                if (repoResult.Success)
                {
                    workflowInstanceID = repoResult.Value;
                }
                else
                {
                    // Show error message
                    _errorMessage.AppendLine(repoResult.Error);

                    ShowHideError(true);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private async Task ApproveWorkflowAsync(int stepInstanceId, int userId, string? comments)
        {
            bool isSuccess = false;

            try
            {
                var repoResult = await WorkflowService.ApproveStepAsync(stepInstanceId, userId, comments);
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
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
    }
}
