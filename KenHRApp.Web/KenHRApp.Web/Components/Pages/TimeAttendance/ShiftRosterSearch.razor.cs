using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Domain.Entities;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Globalization;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class ShiftRosterSearch
    {
        #region Parameters and Injections
        [Inject] private IAttendanceService AttendanceService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState AppState { get; set; } = default!;
        #endregion

        #region Fields

        #region Private Fields
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private string _searchString = string.Empty;
        private StringBuilder _errorMessage = new StringBuilder();        
        private List<string> _events = new();
        #endregion

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
        private bool _enableFilter = false;
        private bool _isTaskFinished = false;
        #endregion

        #region Collections        
        private List<ShiftPatternMasterDTO> _shiftRosterList = new List<ShiftPatternMasterDTO>();
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Shift Roster", href: null, disabled: true, @Icons.Material.Outlined.Shield)
        ];
        #endregion

        #endregion

        #region Enums
        private enum SnackBarTypes
        {
            Normal,
            Information,
            Success,
            Warning,
            Error
        }
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            BeginSearchShiftRoster();
        }
        #endregion

        #region Grid Events
        private Func<ShiftPatternMasterDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (!string.IsNullOrEmpty(x.ShiftPatternCode) && x.ShiftPatternCode.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.ShiftPatternDescription) && x.ShiftPatternDescription.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;
                        
            return false;
        };

        private void StartedEditingItem(ShiftPatternMasterDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(ShiftPatternMasterDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(ShiftPatternMasterDTO item)
        {
            try
            {
                if (item == null) return;
               
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

        private async Task AddShiftRoster()
        {
            //var parameters = new DialogParameters
            //{
            //    ["Department"] = new ShiftPatternMasterDTO(),
            //    ["GroupList"] = _groupList,
            //    ["EmployeeList"] = _employeeList,
            //    ["IsClearable"] = true,
            //    ["IsDisabled"] = false
            //};

            //var options = new DialogOptions
            //{
            //    CloseOnEscapeKey = true,
            //    BackdropClick = false,
            //    FullWidth = true,
            //    MaxWidth = MaxWidth.Large
            //};

            //var dialog = await DialogService.ShowAsync<DepartmentEditDialog>("Add New Department", parameters, options);

            //var result = await dialog.Result;
            //if (result != null && !result.Canceled)
            //{
            //    var newDept = (ShiftPatternMasterDTO)result.Data!;
            //    newDept.DepartmentId = 0;

            //    #region Check for duplicate entries
            //    var duplicateDepartment = _departmentList.FirstOrDefault(g => g.DepartmentCode.Trim().ToUpper() == newDept.DepartmentCode.Trim().ToUpper()
            //        && g.DepartmentName.Trim().ToUpper() == newDept.DepartmentName.Trim().ToUpper());
            //    if (duplicateDepartment != null)
            //    {
            //        // Show error
            //        await ShowErrorMessage(MessageBoxTypes.Error, "Error", "The Department Code and Name already exists. Please enter a different Department Code and Name.");
            //        return;
            //    }
            //    #endregion

            //    #region Get the selected Group Name
            //    if (!string.IsNullOrEmpty(newDept.GroupCode))
            //    {
            //        UserDefinedCodeDTO? udc = _groupList.Where(d => d.UDCCode == newDept.GroupCode).FirstOrDefault();
            //        if (udc != null)
            //            newDept.GroupName = udc.UDCDesc1;
            //    }
            //    #endregion

            //    #region Get the selected Department Head
            //    if (!string.IsNullOrEmpty(newDept.SuperintendentName))
            //    {
            //        EmployeeDTO? headEmp = _employeeList.Where(d => d.EmployeeFullName == newDept.SuperintendentName).FirstOrDefault();
            //        if (headEmp != null)
            //            newDept.SuperintendentEmpNo = headEmp.EmployeeNo;
            //    }
            //    #endregion

            //    #region Get the selected Department Manager
            //    if (!string.IsNullOrEmpty(newDept.ManagerName))
            //    {
            //        EmployeeDTO? managerEmp = _employeeList.Where(d => d.EmployeeFullName == newDept.ManagerName).FirstOrDefault();
            //        if (managerEmp != null)
            //            newDept.ManagerEmpNo = managerEmp.EmployeeNo;
            //    }
            //    #endregion

            //    newDept.IsActiveDesc = newDept.IsActive ? "Yes" : "No";

            //    // Set the Update Date
            //    newDept.CreatedAt = DateTime.Now;

            //    // Set flag to display the loading panel
            //    _isRunning = true;

            //    // Set the overlay message
            //    overlayMessage = "Adding department, please wait...";

            //    _ = SaveChangeAsync(async () =>
            //    {
            //        _isRunning = false;

            //        // Shows the spinner overlay
            //        await InvokeAsync(StateHasChanged);

            //    }, newDept);
            //}
        }
        #endregion

        #region Private Methods
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

        private void BeginSearchShiftRoster(bool forceLoad = false)
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading shift rosters, please wait...";

            _ = SearchShiftRosterAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, forceLoad);
        }
        #endregion

        #region Database Methods
        private async Task SearchShiftRosterAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            var repoResult = await AttendanceService.SearchShiftRosterMasterAsync(0, null, null, null);
            if (repoResult.Success)
            {
                _shiftRosterList = repoResult.Value!;
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
