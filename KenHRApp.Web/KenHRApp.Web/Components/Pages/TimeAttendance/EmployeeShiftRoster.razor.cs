using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Domain.Entities;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;
using MudBlazor;
using System.Globalization;
using System.IO.Pipelines;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class EmployeeShiftRoster
    {
        #region Parameters and Injections
        [Inject] private IAttendanceService AttendanceService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState AppState { get; set; } = default!;
        [Inject] private IAppCacheService AppCacheService { get; set; } = default!;
        [Inject] private ILogger<Employee> Logger { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public bool ForceLoad { get; set; } = false;

        [Parameter]
        [SupplyParameterFromQuery]
        public string ActionType { get; set; } = ActionTypes.View.ToString();
        #endregion

        #region Fields
        // The master DTO bound to the form
        private EmployeeRosterDTO _shiftRoster = new EmployeeRosterDTO();
        private EditForm _editForm;
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private MudDatePicker _dojPicker;
        private MudDataGrid<EmployeeRosterDTO>? _rosterGrid;        
        private UserDefinedCodeDTO? _selectedEmployeeStatus = null;
        private UserDefinedCodeDTO? _selectedEmploymentType = null;
        private StringBuilder _errorMessage = new StringBuilder();
        private CancellationTokenSource? _cts;

        private string _selectedDepartment = string.Empty;
        private string[]? _departmentArray = null;
        private string? _departmentCode = null;
        private string _selectedManager = string.Empty;
        private string[]? _managerArray = null;

        private int? _employeeCode = null;
        private string? _firstName = null;
        private string? _lastName = null;
        private int? _reportingManager = null;
        private DateTime? _dateOfJoining = null;
        private string? _employeeType = null;
        private string overlayMessage = "Please wait...";
        private string _departmentCacheKey = string.Empty;
        private string _employeeCacheKey = string.Empty;
        private string _shiftPatternCacheKey = string.Empty;

        #region Flags
        private bool _isSearchHovered = false;
        private bool _isResetHovered = false;
        private bool _isTaskFinished = false;
        private bool _isRunning = false;
        private bool _showErrorAlert = false;
        private bool _enableGridFilter = false;
        private bool _allowGridEdit = false;
        private bool _isDisabled = false;
        private bool _saveBtnEnabled = false;
        private bool _hasValidationError = false;
        private bool _isEditMode = false;
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
            EMPSTATUS,  // Employee Status
            EMPLOYTYPE, // Employment Type
            DEPARTMENT  // Departments
        }
        #endregion

        #region Collections
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Shift Roster Master", href: "/TimeAttendance/shiftrostersearch", icon: @Icons.Material.Filled.CalendarMonth),
            new("Manage Shift Roster", href: null, disabled: true, @Icons.Material.Filled.EditNote)
        ];

        private IReadOnlyList<EmployeeRosterDTO> employeeList = new List<EmployeeRosterDTO>();
        private IReadOnlyList<UserDefinedCodeDTO> _employeeStatusList = new List<UserDefinedCodeDTO>();
        private IReadOnlyList<UserDefinedCodeDTO> _employmentTypeList = new List<UserDefinedCodeDTO>();
        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        private IReadOnlyList<EmployeeDTO> _managerList = new List<EmployeeDTO>();
        private IReadOnlyList<ShiftPatternMasterDTO> _shiftPatternList = new List<ShiftPatternMasterDTO>();
        private List<ShiftPointerDTO> _shiftPointerList = new List<ShiftPointerDTO>();
        private IReadOnlyList<UserDefinedCodeDTO> _changeTypeList = new List<UserDefinedCodeDTO>();
        #endregion        

        #endregion

        #region Page Methods
        public void Dispose()
        {
            // Cleanup resources
            Logger.LogInformation("Employee page disposed");
        }

        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(employeeList);

            if (ActionType == ActionTypes.Edit.ToString())
            {
                _isDisabled = true;
            }
            else if (ActionType == ActionTypes.View.ToString())
            {
                _isDisabled = true;
            }
            else if (ActionType == ActionTypes.Add.ToString())
            {
                _isDisabled = false;
                _saveBtnEnabled = true;
                _allowGridEdit = true;
            }

            BeginLoadComboboxTask();

            //if (LookupCache.IsEmployeeSearch)
            //{
            //    LookupCache.IsEmployeeSearch = false;
            //    BeginSearchEmployeeTask(ForceLoad);
            //}
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

                #region Perform data validation
                // Check if there is employee roster defined in the datagrid
                if (!employeeList.Any())
                {                    
                    ShowNotification("Unable to save because there is no employee shift roster record defined in the grid.", NotificationType.Warning);
                    return;
                }
                else
                {
                    foreach (var item in employeeList)
                    {
                        if (string.IsNullOrEmpty(item.ShiftPatternCode))
                        {
                            _hasValidationError = true;
                            _validationMessages.Add($"Employee No. {item.EmployeeNo} has no defined shift roster");
                        }
                        if (item.ShiftPointer == 0)
                        {
                            _hasValidationError = true;
                            _validationMessages.Add($"Employee No. {item.EmployeeNo} has no defined shift pointer");
                        }
                        if (string.IsNullOrEmpty(item.ChangeTypeCode))
                        {
                            _hasValidationError = true;
                            _validationMessages.Add($"Employee No. {item.EmployeeNo} has no defined change type");
                        }
                        if (item.ChangeTypeCode == "CTTEMP" && !item.EndingDate.HasValue)
                        {
                            _hasValidationError = true;
                            _validationMessages.Add($"Ending Date must be defined for employee no. {item.EmployeeNo} if Change Type is set to Temporary.");
                        }
                    }

                    if (_hasValidationError)
                        return;
                }
                #endregion

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Saving changes, please wait...";

                _ = SaveShiftRosterAsync(async () =>
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

        #region Grid Events
        private async Task StartedEditingGridItem(EmployeeRosterDTO item)
        {
            //await EditBudgetAsync(item);
        }

        private void CommittedGridItemChanges(EmployeeRosterDTO item)
        {
            try
            {
                if (item == null) return;

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Saving changes, please wait...";

                //_ = SaveChangeAsync(async () =>
                //{
                //    _isRunning = false;

                //    // Shows the spinner overlay
                //    await InvokeAsync(StateHasChanged);
                //}, item);
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

        private void OnShiftRosterChanged(EmployeeRosterDTO row, string newValue)
        {
            if (row.ShiftPatternCode != newValue)
            {
                row.ShiftPatternCode = newValue;

                // Get the associated shift pointers
                if (_shiftPatternList.Any())
                {
                    ShiftPatternMasterDTO? shiftPattern = _shiftPatternList.Where(s => s.ShiftPatternCode.Trim() == newValue).FirstOrDefault();
                    if (shiftPattern != null)
                    {
                        _shiftPointerList = shiftPattern.ShiftPointerList;
                    }
                }

                // Reset pointer when roster changes
                row.ShiftPointer = 0;
                row.ShiftPointerId = 0;
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

            // Populate Employee Status dropdown
            var repoResult = await LookupCache.GetEmployeeStatusAsync();
            if (repoResult.Success)
            {
                _employeeStatusList = repoResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(repoResult.Error);
            }

            // Populate Employment Type dropdown
            repoResult = await LookupCache.GetEmploymentTypeAsync();
            if (repoResult.Success)
            {
                _employmentTypeList = repoResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(repoResult.Error);
            }

            // Populate Shift Pattern Change Type dropdown
            repoResult = await LookupCache.GetChangeTypeAsync();
            if (repoResult.Success)
            {
                _changeTypeList = repoResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(repoResult.Error);
            }

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
                _departmentArray = _departmentList.Select(d => d.DepartmentName).OrderBy(d => d).ToArray();

                // Save departments into cache via Application service
                _departmentCacheKey = await AppCacheService.StoreDepartmentsAsync(_departmentList.ToList());
            }
            #endregion

            #region Get Reporting Manager list
            var managerResult = await LookupCache.GetReportingManagerAsync();
            if (managerResult.Success)
            {
                _managerList = managerResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(managerResult.Error);
            }

            if (_managerList != null)
            {
                _managerArray = _managerList.Select(d => d.EmployeeFullName).OrderBy(d => d).ToArray();

                // Save employees into cache via Application service
                _employeeCacheKey = await AppCacheService.StoreEmployeesAsync(_managerList.ToList());
            }
            #endregion

            #region Get Shift Pattern list
            var shiftPatternResult = await LookupCache.GetShiftPatternAsync(true);
            if (shiftPatternResult.Success)
            {
                _shiftPatternList = shiftPatternResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(shiftPatternResult.Error);
            }                        
            #endregion

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private void BeginSearchEmployeeTask(bool forceLoad = false)
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading employee list, please wait...";

            /*
            Notes:
            - The underscore makes the method call asynchronous, non-blocking, and explicit that we don’t care about the result.
            - By assigning it to _, you’re telling the compiler: "Yes, I know this returns a Task, but I’m intentionally not awaiting it. Run it in the background."
            - It’s like saying “fire-and-forget”.
            - In short, The underscore _ is a discard assignment — it suppresses compiler warnings when you deliberately ignore the return value of a method.
            */

            _ = SearchEmployeeAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                /* Notes:
                * StateHasChanged → refresh the UI.
                * InvokeAsync → marshal that call to the right Blazor thread.
                * await → make sure the re-render finishes in a safe, async-friendly way.
                */

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, forceLoad);
        }

        private async Task SearchEmployeeAsync(Func<Task> callback, bool forceLoad = false)
        {
            // Simulate async work
            // This creates a task that waits asynchronously for 3000 milliseconds (3 seconds).
            // The method “pauses” here and gives control back to the runtime until the timer completes.
            // Await makes the method asynchronous — the UI stays responsive during the 3-second delay.
            await Task.Delay(1000);

            // Reset error messages
            _errorMessage.Clear();

            // Get the department code
            _departmentCode = string.Empty;
            if (!string.IsNullOrEmpty(_selectedDepartment))
            {
                DepartmentDTO? departmentDTO = _departmentList.Where(d => d.DepartmentName == _selectedDepartment).FirstOrDefault();
                if (departmentDTO != null)
                    _departmentCode = departmentDTO.DepartmentCode;
            }

            _reportingManager = null;
            if (!string.IsNullOrEmpty(_selectedManager))
            {
                EmployeeDTO? selectedManager = _managerList.Where(a => a.EmployeeFullName == _selectedManager).FirstOrDefault();
                if (selectedManager != null)
                    _reportingManager = selectedManager.EmployeeNo;
            }

            var repoResult = await LookupCache.SearchEmployeeAsync(_employeeCode, _firstName, _lastName, _reportingManager,
                _dateOfJoining, _selectedEmployeeStatus?.UDCCode, _selectedEmploymentType?.UDCCode, _departmentCode, forceLoad);
            if (repoResult.Success)
            {
                // Set the flag to indicate that search has been invoked
                LookupCache.IsEmployeeSearch = true;

                employeeList = repoResult.Value!.Select(e => new EmployeeRosterDTO
                {
                    EmployeeId = e.EmployeeId,
                    EmployeeNo = e.EmployeeNo,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    Gender = e.Gender,
                    HireDate = e.HireDate,
                    EmploymentTypeCode = e.EmploymentTypeCode,
                    EmploymentType = e.EmploymentType,
                    ReportingManagerCode = e.ReportingManagerCode,
                    ReportingManager = e.ReportingManager,
                    DepartmentCode = e.DepartmentCode,
                    DepartmentName = e.DepartmentName,
                    EmployeeStatusCode = e.EmployeeStatusCode,
                    EmployeeStatus = e.EmployeeStatus
                }).ToList();
            }
            else
            {
                // Set the error message
                _errorMessage.Append(repoResult.Error);

                ShowHideError(true);
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
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
            await Task.Delay(1000);

            // Reset error messages
            _errorMessage.Clear();

            // Clear field mappings
            _employeeCode = null;
            _firstName = null;
            _lastName = null;
            _reportingManager = null;
            _dateOfJoining = null;
            _employeeType = null;
            _departmentCode = null;
            _selectedEmployeeStatus = null;
            _selectedEmploymentType = null;
            _selectedDepartment = string.Empty;
            employeeList = new List<EmployeeRosterDTO>();

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task SaveShiftRosterAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            //bool isNewRequition = false;    

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = true;
            string errorMsg = string.Empty;

            //if (isNewRequition)
            //{
            //    // Set the user who created the record and the timestamp
            //    _shiftPattern.CreatedDate = DateTime.Now;

            var saveResult = await AttendanceService.AddShiftPatternChangeAsync(employeeList.ToList(), _cts.Token);
            
            isSuccess = saveResult.Success;
            if (!isSuccess)
                errorMsg = saveResult.Error!;
            //else
            //{
            //    // Set flag to enable reload of _recruitmentRequests when navigating back to the Employe Search page
            //    _forceLoad = true;
            //}
            //}
            //else
            //{
            //    // Set the user who created the record and the timestamp
            //    _shiftPattern.LastUpdateDate = DateTime.Now;

            //    var saveResult = await AttendanceService.UpdateShiftRosterMasterAsync(_shiftPattern, _cts.Token);
            //    isSuccess = saveResult.Success;
            //    if (!isSuccess)
            //        errorMsg = saveResult.Error!;
            //}

            if (isSuccess)
            {
                // Reset flags
                _isEditMode = false;
                _allowGridEdit = false;
                _saveBtnEnabled = false;
                _isDisabled = true;

                // Hide error message if any
                ShowHideError(false);

                // Show notification
                ShowNotification("Employee Shift Roster has been saved successfully!", NotificationType.Success);

                // Go back to Shift Roster Master page
                Navigation.NavigateTo("/TimeAttendance/shiftrostersearch");
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

        #region Database Methods
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

        private async Task<IEnumerable<string>> SearchReportingManager(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _managerArray!;
            }

            return _managerArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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

        private void GoToEmployeeDetail(EmployeeRosterDTO employee)
        {
            Navigation.NavigateTo($"/employees?EmployeeId={employee.EmployeeId}&ActionType=View&DepartmentCacheKey={_departmentCacheKey}&EmployeeCacheKey={_employeeCacheKey}");
        }

        private void AddNewEmployee()
        {
            Navigation.NavigateTo($"/employees?EmployeeId=0&ActionType=Add&DepartmentCacheKey={_departmentCacheKey}&EmployeeCacheKey={_employeeCacheKey}");
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
        #endregion
    }
}
