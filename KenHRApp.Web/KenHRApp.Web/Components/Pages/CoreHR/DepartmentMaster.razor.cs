using KenHRApp.Domain.Entities;
using KenHRApp.Application.Common.Interfaces;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using static MudBlazor.CategoryTypes;
using System.Xml.Linq;
using KenHRApp.Web.Components.Shared;

namespace KenHRApp.Web.Components.Pages.CoreHR
{
    public partial class DepartmentMaster
    {
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private IEmployeeService EmployeeService { get; set; } = default!;

        #region Fields
        private MudDatePicker _dojPicker;
        private StringBuilder _errorMessage = new StringBuilder();
        private List<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        private IReadOnlyList<EmployeeDTO> _employeeList = new List<EmployeeDTO>();
        private List<UserDefinedCodeDTO> _groupList = new List<UserDefinedCodeDTO>();
        private string[]? _groupArray = null;
        private string[]? _employeeArray = null;
        private string _selectedHead = string.Empty;
        private string _cellSelectedHead = string.Empty;
        private string _selectedManager = string.Empty;
        private string _cellSelectedManager = string.Empty;
        private string _selectedGroup = string.Empty;
        private string _searchString = string.Empty;
        private string? _departmentName = null;
        private string? _description = null;
        private string overlayMessage = "Please wait...";
        private List<string> _events = new();
        private UserDefinedCodeDTO? _cellSelectedGroup = null;
        private CancellationTokenSource? _cts;
        private bool _enableFilter = false;
        private bool _isActive = true;

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        #endregion

        #region Flags
        private bool _isSearchHovered = false;
        private bool _isResetHovered = false;
        private bool _isTaskFinished = false;
        private bool _isRunning = false;
        private bool _showErrorAlert = false;
        #endregion

        private enum UDCKeys
        {
            EMPSTATUS,      // Employee Status
            EMPLOYTYPE,     // Employment Type
            DEPARTMENT,     // Departments
            DEPTGROUP       // Group Codes
        }

        private enum SnackBarTypes
        {
            Normal,
            Information,
            Success,
            Warning,
            Error
        }

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Department Master", href: null, icon: @Icons.Material.Filled.AccountBalance, disabled: true)
        ];
        #endregion

        #region Parameters and Injections
        [Parameter]
        [SupplyParameterFromQuery]
        public bool ForceLoad { get; set; } = false;
        #endregion

        #region Page Methods
        public void Dispose()
        {
            // Cleanup resources
            Logger.LogInformation("Department Master page disposed");
        }

        protected override void OnInitialized()
        {
            BeginLoadComboboxTask();
        }
        #endregion

        #region Grid Events 
        private Func<DepartmentDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (!string.IsNullOrEmpty(x.DepartmentCode) && x.DepartmentCode.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.DepartmentName) && x.DepartmentName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.Description) && x.Description!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.GroupName) && x.GroupName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.SuperintendentName) && x.SuperintendentName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.ManagerName) && x.ManagerName!.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void StartedEditingItem(DepartmentDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(DepartmentDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(DepartmentDTO item)
        {
            try
            {
                if (item == null) return;

                #region Get the selected Group Name
                if (!string.IsNullOrEmpty(item.GroupCode))
                {
                    UserDefinedCodeDTO? udc = _groupList.Where(d => d.UDCCode == item.GroupCode).FirstOrDefault();
                    if (udc != null)
                        item.GroupName = udc.UDCDesc1;
                }
                #endregion

                #region Get the selected Department Head
                if (!string.IsNullOrEmpty(item.SuperintendentName))
                {
                    EmployeeDTO? headEmp = _employeeList.Where(d => d.EmployeeFullName == item.SuperintendentName).FirstOrDefault();
                    if (headEmp != null)
                        item.SuperintendentEmpNo = headEmp.EmployeeNo;
                }
                #endregion

                #region Get the selected Department Manager
                if (!string.IsNullOrEmpty(item.ManagerName))
                {
                    EmployeeDTO? managerEmp = _employeeList.Where(d => d.EmployeeFullName == item.ManagerName).FirstOrDefault();
                    if (managerEmp != null)
                        item.ManagerEmpNo = managerEmp.EmployeeNo;
                }
                #endregion

                item.IsActiveDesc = item.IsActive ? "Yes" : "No";

                // Set the Update Date
                item.UpdatedAt = DateTime.Today;

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Saving changes, please wait...";

                _ = SaveChangeAsync(async () =>
                {
                    _isRunning = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);
                }, item);
            }
            catch (OperationCanceledException)
            {
                ShowNotification("Save cancelled (navigated away).", SnackBarTypes.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", SnackBarTypes.Error);
            }
        }

        private async Task ConfirmDelete(DepartmentDTO department)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the department '{department.DepartmentName}'?" },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error }
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
                BeginDeleteDepartment(department);
            }
        }

        private async Task AddDepartment()
        {
            var parameters = new DialogParameters { ["Department"] = new DepartmentDTO() };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = false,
                BackdropClick = false,
                FullWidth = true,
                MaxWidth = MaxWidth.Medium
            };

            var dialog = await DialogService.ShowAsync<DepartmentEditDialog>("Add Department", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                var newDept = (DepartmentDTO)result.Data!;
                newDept.DepartmentId = _departmentList.Max(d => d.DepartmentId) + 1;
                _departmentList.Add(newDept);
            }
        }
        #endregion

        #region Asynchronous Tasks
        private void BeginLoadComboboxTask()
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Initializing form, please wait...";

            _ = LoadComboboxAsync(async () =>
            {
                _isTaskFinished = true;
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
            var repoResult = await LookupCache.GetReportingManagerAsync();
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
                _employeeArray = _employeeList.Select(d => d.EmployeeFullName).OrderBy(d => d).ToArray();
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
                    #region Get Department Groups
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.DEPTGROUP.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Department Group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _groupList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_groupList != null)
                            _groupArray = _groupList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
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

        private void BeginSearchDepartmentTask(bool forceLoad = false)
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading departments, please wait...";                       

            _ = SearchDepartmentAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;                                

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, forceLoad);
        }

        private async Task SearchDepartmentAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            #region Get the selected Group Code
            string groupCode = string.Empty;
            if (!string.IsNullOrEmpty(_selectedGroup))
            {
                UserDefinedCodeDTO? udc = _groupList.Where(d => d.UDCDesc1 == _selectedGroup).FirstOrDefault();
                if (udc != null)
                    groupCode = udc.UDCCode;
            }
            #endregion

            #region Get the selected Department Head
            int superintendentEmpNo = 0;
            if (!string.IsNullOrEmpty(_selectedHead))
            {
                EmployeeDTO? headEmp = _employeeList.Where(d => d.EmployeeFullName == _selectedHead).FirstOrDefault();
                if (headEmp != null)
                    superintendentEmpNo = headEmp.EmployeeNo;
            }
            #endregion

            #region Get the selected Department Manager
            int managerEmpNo = 0;
            if (!string.IsNullOrEmpty(_selectedManager))
            {
                EmployeeDTO? managerEmp = _employeeList.Where(d => d.EmployeeFullName == _selectedManager).FirstOrDefault();
                if (managerEmp != null)
                    managerEmpNo = managerEmp.EmployeeNo;
            }
            #endregion

            var reportResult = await EmployeeService.SearchDepartmentAsync(_departmentName!, _description!, superintendentEmpNo, 
                managerEmpNo, _isActive, string.Empty, groupCode);
            if (reportResult.Success)
            {
                _departmentList = reportResult.Value!;
            }
            else
            {
                // Show error message
                _errorMessage.AppendLine(reportResult.Error);

                ShowHideError(true);
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task<IEnumerable<string>> SearchDepartmentGroup(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _groupArray!;
            }

            return _groupArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private void BeginRefreshPageTask()
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Refreshing form, please wait...";

            _ = RefreshPageAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            });
        }

        private async Task RefreshPageAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

            // Reset error messages
            _errorMessage.Clear();

            // Clear field mappings
            _departmentName = null;
            _description = null;
            _selectedGroup = string.Empty;
            _selectedHead = string.Empty;
            _selectedManager = string.Empty;
            _departmentList = new List<DepartmentDTO>();

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }
        #endregion

        #region Private Methods
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

        private void GoToEmployeeDetail(EmployeeMasterDTO employee)
        {
            //NavigationManager.NavigateTo($"/employees?EmployeeId={employee.EmployeeId}&ActionType=View&DepartmentCacheKey={_departmentCacheKey}&EmployeeCacheKey={_employeeCacheKey}");
        }

        private void AddNewEmployee()
        {
            //NavigationManager.NavigateTo($"/employees?EmployeeId=0&ActionType=Add&DepartmentCacheKey={_departmentCacheKey}&EmployeeCacheKey={_employeeCacheKey}");
        }

        private void ShowNotification(string message, SnackBarTypes type)
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
                case SnackBarTypes.Information:
                    Snackbar.Add(message, Severity.Info);
                    break;

                case SnackBarTypes.Success:
                    Snackbar.Add(message, Severity.Success);
                    break;

                case SnackBarTypes.Warning:
                    Snackbar.Add(message, Severity.Warning);
                    break;

                case SnackBarTypes.Error:
                    Snackbar.Add(message, Severity.Error);
                    break;

                default:
                    Snackbar.Add(message, Severity.Normal);
                    break;
            }

            // Snackbar.Add($"Error {message}", Severity.Error, c => c.SnackbarVariant = Variant.Filled);
        }

        private void BeginDeleteDepartment(DepartmentDTO department)
        {
            try
            {
                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Deleting department, please wait...";

                _ = DeleteDepartmentAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Remove locally from the list so UI updates immediately
                    _departmentList.Remove(department);

                    StateHasChanged();

                }, department);
            }
            catch (OperationCanceledException)
            {
                ShowNotification("Delete cancelled (navigated away).", SnackBarTypes.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", SnackBarTypes.Error);
            }
        }
        #endregion

        #region Database Methods
        private async Task SaveChangeAsync(Func<Task> callback, DepartmentDTO department)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (department.DepartmentId == 0)
            {
                //var addResult = await EmployeeService.AddEmployeeAsync(department, _cts.Token);
                //isSuccess = addResult.Success;
                //if (!isSuccess)
                //    errorMsg = addResult.Error!;
            }
            else
            {
                var saveResult = await EmployeeService.SaveDepartmentAsync(department, _cts.Token);
                isSuccess = saveResult.Success;
                if (!isSuccess)
                    errorMsg = saveResult.Error!;
            }

            if (isSuccess)
            {
                // Reset flags
                //_isEditMode = false;
                //_saveBtnEnabled = false;
                //_isDisabled = true;

                // Show notification
                ShowNotification("Department data saved successfully!", SnackBarTypes.Success);
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

        private async Task DeleteDepartmentAsync(Func<Task> callback, DepartmentDTO department)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (department.DepartmentId == 0)
            {
                errorMsg = "Department ID is not defined.";
            }
            else
            {
                var deleteResult = await EmployeeService.DeleteDepartmentAsync(department.DepartmentId, _cts.Token);
                isSuccess = deleteResult.Success;
                if (!isSuccess)
                    errorMsg = deleteResult.Error!;
            }

            if (isSuccess)
            {
                // Show notification
                ShowNotification("The selected department has been deleted successfully!", SnackBarTypes.Success);
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
