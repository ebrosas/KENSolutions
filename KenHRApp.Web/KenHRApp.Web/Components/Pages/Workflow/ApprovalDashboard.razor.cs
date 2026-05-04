using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Common.Interface;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;
using KenHRApp.Web.Components.Common.Helpers;
using System.Net.Http.Headers;
using Azure.Core;

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
        [Inject] private IWebHostEnvironment Environment { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public bool ForceLoad { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery]
        public string? RequestType { get; set; } = null;
        #endregion

        #region Constants
        private static string CONST_DEFAULT_LIST_ITEM = "<ALL>";
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

        private enum SearchType
        {
            All,
            PendingRequest,
            ApprovedRequest,
            RejectedRequest,
            OnholdRequest
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
        #endregion

        #region Fields
        private StringBuilder _errorMessage = new StringBuilder();
        private string _searchString = string.Empty;
        private string overlayMessage = "Please wait...";
        private List<string> _events = new();
        private CancellationTokenSource? _cts;
        private RequestTypeDTO? _selectedRequestType = null;
        private SearchType _searchType = SearchType.PendingRequest;

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
        private bool _isSearchHovered = false;
        private bool _isResetHovered = false;
        private bool _isPendingClicked = true;
        private bool _isApprovedClicked = false;
        private bool _isRejectedClicked = false;
        private bool _isOnHoldClicked = false;
        #endregion

        #region Objects and collections
        private List<RequestTypeDTO> _requestTypeList = new List<RequestTypeDTO>();
        private List<ApprovalRequestResultDTO> _approvalList = new List<ApprovalRequestResultDTO>();
        private List<string> _validationMessages = new();

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/TimeAttendance/tnadashboard", icon: Icons.Material.Filled.Home),
            new("Pending Requests Dashboard", href: null, icon: @Icons.Material.Filled.AccountBalance, disabled: true)
        ];
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

                    BeginLoadComboboxTask();
                }
            }
        }
        #endregion

        #region Grid Events 
        private Func<ApprovalRequestResultDTO, bool> _quickFilter => x =>
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

        private void StartedEditingItem(ApprovalRequestResultDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(ApprovalRequestResultDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(ApprovalRequestResultDTO item)
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

        public void OpenLeaveRequest(ApprovalRequestResultDTO item)
        {
            Navigation.NavigateTo($"/TimeAttendance/leaverequest?ActionType=View&LeaveRequestNo={item.RequestNo}&CallerForm=ApprovalDashboard");
        }

        private void OnSelectedValueChanged(RequestTypeDTO value)
        {
            _selectedRequestType = value;

            BeginGetPendingRequestByType(_searchType);

            switch (_searchType)
            {
                case SearchType.PendingRequest:
                    overlayMessage = "Loading pending requests, please wait...";
                    _isPendingClicked = true;
                    break;

                case SearchType.ApprovedRequest:
                    overlayMessage = "Loading approved requests, please wait...";
                    _isApprovedClicked = true;
                    break;

                case SearchType.RejectedRequest:
                    overlayMessage = "Loading rejected requests, please wait...";
                    _isRejectedClicked = true;
                    break;

                case SearchType.OnholdRequest:
                    overlayMessage = "Loading on-hold requests, please wait...";
                    _isOnHoldClicked = true;
                    break;
            }
        }

        private void BeginGetPendingRequestByType(SearchType searchType)
        {
            // Set flag to display the loading panel
            _isRunning = true;

            //Reset button flags
            _isPendingClicked = false;
            _isApprovedClicked = false;
            _isRejectedClicked = false;
            _isOnHoldClicked = false;

            // Update the flag
            _searchType = searchType;

            switch (_searchType)
            {
                case SearchType.PendingRequest:
                    overlayMessage = "Loading pending requests, please wait...";
                    _isPendingClicked = true;
                    break;

                case SearchType.ApprovedRequest:
                    overlayMessage = "Loading approved requests, please wait...";
                    _isApprovedClicked = true;
                    break;

                case SearchType.RejectedRequest:
                    overlayMessage = "Loading rejected requests, please wait...";
                    _isRejectedClicked = true;
                    break;

                case SearchType.OnholdRequest:
                    overlayMessage = "Loading on-hold requests, please wait...";
                    _isOnHoldClicked = true;
                    break;
            }

            _ = GetPendingRequestByType(async () =>
            {
                _isRunning = false;

                // Hide the spinner overlay
                await InvokeAsync(StateHasChanged);

                StateHasChanged();

            }, searchType);
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
                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                // Load the request types
                await GetRequestListAsync();
                                
                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);

                //BeginGetRequestTypesTask(ForceLoad);
            });
        }

        private void BeginGetRequestTypesTask(bool forceLoad = false)
        {
            // Reset validation errors
            _hasValidationError = false;
            _validationMessages.Clear();
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
                //await ApproveWorkflowAsync(15, 10003632, "ervin", "Test Supervisor Approval");
                //await ApproveWorkflowAsync(18, 10003685, "shahbaz", "Test Department Manager Approval");
                //await ApproveWorkflowAsync(6, 10003636, "nagendra", "Test Cost Center Manager Approval");
                //await ApproveWorkflowAsync(7, 10003635, "Tester", "Test General Manager Approval");
                #endregion                

            }, forceLoad);
        }     
        
        private void BeginApproveRequest(ApprovalRequestResultDTO requestItem)
        {
            try
            {
                if (requestItem == null)
                {
                    throw new Exception("The selected request workflow instance is not configured correctly!");
                }
                else
                {
                    if (requestItem.StepInstanceId == null)
                        throw new Exception("The current workflow instance is not defined!");
                }

                // Get the current WF activity instance id
                int stepInstanceId = requestItem.StepInstanceId ?? 0;

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Approving request, please wait...";

                _ = ApproveWorkflowAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Remove locally from the list so UI updates immediately
                    _approvalList.Remove(requestItem);

                    StateHasChanged();

                }, stepInstanceId, requestItem.ApproverNo, UserID!,
                requestItem.ApproverRemarks, requestItem.RequestNo);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
        }

        private async Task ConfirmReject(ApprovalRequestResultDTO requestItem)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Reject"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to reject request no. '{requestItem.RequestNo}'? Click Proceed to continue, otherwise click Cancel button." },
                { "ConfirmText", "Proceed" },
                { "Color", Color.Error },
                { "DialogIconColor", Color.Error }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.Center,
                CloseOnEscapeKey = true,   // Prevent ESC from closing
                BackdropClick = false       // Prevent clicking outside to close
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Reject Request", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                BeginRejectRequest(requestItem);
            }
        }
        private void BeginRejectRequest(ApprovalRequestResultDTO requestItem)
        {
            try
            {
                if (requestItem == null)
                {
                    throw new Exception("The selected request workflow instance is not configured correctly!");
                }
                else
                {
                    if (requestItem.StepInstanceId == null)
                        throw new Exception("The current workflow instance is not defined!");

                    if (string.IsNullOrWhiteSpace(requestItem.ApproverRemarks))
                        throw new Exception("Remarks is required when rejecting the request!");
                }

                // Get the current WF activity instance id
                int stepInstanceId = requestItem.StepInstanceId ?? 0;

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Rejecting request, please wait...";

                //bool isSuccess = await RejectWorkflowAsync(stepInstanceId, requestItem.CreatedByEmpNo, requestItem.ApproverNo, UserID!,
                //    requestItem.ApproverRemarks, requestItem.RequestNo);
                //if (isSuccess)
                //{
                //    ShowNotification("The selected request has been rejected successfully!", NotificationType.Success);
                //}

                _ = RejectWorkflowAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Remove locally from the list so UI updates immediately
                    _approvalList.Remove(requestItem);

                    StateHasChanged();

                }, stepInstanceId, requestItem.CreatedByEmpNo, requestItem.ApproverNo, UserID!,
                requestItem.ApproverRemarks, requestItem.RequestNo);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
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
            bool isSuccess = false;

            try
            {
                // Initialize the cancellation token
                _cts = new CancellationTokenSource();

                var repoResult = await WorkflowService.StartWorkflowAsync(WorkflowHelper.CONST_LEAVE_REQUEST, 15, Environment.WebRootPath, _cts.Token);
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
                ShowNotification("The selected request has been approved successfully!", NotificationType.Success);
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

        private async Task RejectWorkflowAsync(
            Func<Task> callback,
            int stepInstanceId, 
            int? creatorEmpNo,
            int approverNo, 
            string? userID, 
            string comments,
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

            var repoResult = await WorkflowService.RejectStepAsync(stepInstanceId, creatorEmpNo, approverNo, userID, comments,
                WorkflowHelper.CONST_LEAVE_REQUEST, requestNo, Environment.WebRootPath, _cts.Token);

            isSuccess = repoResult.Success;
            if (!isSuccess)
                errorMsg = repoResult.Error!;

            if (isSuccess)
            {
                // Show notification
                ShowNotification("The selected request has been rejected successfully!", NotificationType.Success);
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

        private async Task GetRequestListAsync()
        {
            // Reset error messages
            _errorMessage.Clear();

            var repoResult = await WorkflowService.GetPendingRequestAsync(UserEmpNo, string.Empty, 0, null, null);
            if (repoResult.Success)
            {
                _requestTypeList = repoResult.Value!;

                // Set the default selected list item to "All"
                if (_requestTypeList != null && _requestTypeList.Any())
                {
                    if (!string.IsNullOrWhiteSpace(this.RequestType))
                        _selectedRequestType = _requestTypeList.FirstOrDefault(r => r.RequestTypeCode == this.RequestType);
                    else
                        _selectedRequestType = _requestTypeList.FirstOrDefault(r => r.RequestTypeCode == CONST_DEFAULT_LIST_ITEM);
                }

                BeginGetPendingRequestByType(_searchType);
            }
            else
            {
                // Show error message
                _errorMessage.AppendLine(repoResult.Error);

                ShowHideError(true);
            }
        }

        private async Task GetPendingRequestByType(
            Func<Task> callback, 
            SearchType searchType)
        {
            try
            {
                // Wait for 1 second then gives control back to the runtime
                await Task.Delay(300);

                // Reset error messages
                _errorMessage.Clear();

                // Reset datagrid datasource
                _approvalList.Clear();

                var repoResult = await WorkflowService.GetApprovalRequestAsync(
                    Convert.ToByte(searchType),
                    UserEmpNo,
                    _selectedRequestType != null ? _selectedRequestType!.RequestTypeCode : null);
                if (repoResult.Success)
                {
                    _approvalList = repoResult.Value!;
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
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
        }
        #endregion
    }
}
