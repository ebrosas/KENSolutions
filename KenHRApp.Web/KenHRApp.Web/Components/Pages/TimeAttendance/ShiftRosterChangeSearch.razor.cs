using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class ShiftRosterChangeSearch
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

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        #endregion

        #region Collections        
        private List<ShiftPatternChangeDTO> _shiftRosterChangeList = new List<ShiftPatternChangeDTO>();
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Shift Roster Change History", href: null, disabled: true, @Icons.Material.Filled.History)
        ];

        private IReadOnlyList<ShiftPatternMasterDTO> _shiftPatternList = new List<ShiftPatternMasterDTO>();
        private List<ShiftPointerDTO> _shiftPointerList = new List<ShiftPointerDTO>();
        private IReadOnlyList<UserDefinedCodeDTO> _changeTypeList = new List<UserDefinedCodeDTO>();
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
            BeginSearchShiftRosterChange();
        }
        #endregion

        #region Grid Events
        private Func<ShiftPatternChangeDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (x.EmpNo > 0 && x.EmpNo.ToString().Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.EmpName) && x.EmpName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.DepartmentCode) && x.DepartmentCode.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.DepartmentName) && x.DepartmentName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.ShiftPatternCode) && x.ShiftPatternCode.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.ChangeTypeDesc) && x.ChangeTypeDesc.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void StartedEditingItem(ShiftPatternChangeDTO item)
        {
            _events.Insert(0, $"Event = StartedEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CanceledEditingItem(ShiftPatternChangeDTO item)
        {
            _events.Insert(0, $"Event = CanceledEditingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void CommittedItemChanges(ShiftPatternChangeDTO item)
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

        private async Task ConfirmDelete(ShiftPatternChangeDTO shiftRoster)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the shift roster changes for the following employee: '{shiftRoster.EmpNo} - {shiftRoster.EmpName}'?" },
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
                BeginDeleteShiftRosterChange(shiftRoster);
            }
        }

        private void OnShiftRosterChanged(ShiftPatternChangeDTO row, string newValue)
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
                //row.ShiftPointerId = 0;
            }
        }

        private void AddShiftRoster()
        {
            Navigation.NavigateTo($"/TimeAttendance/employeeshiftroster?ActionType=Add");
        }

        private void OpenEmployeeShiftRoster()
        {
            Navigation.NavigateTo($"/TimeAttendance/employeeshiftroster?ActionType=Add");
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

        private void BeginSearchShiftRosterChange(bool forceLoad = false)
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading shift roster change, please wait...";

            _ = SearchShiftRosterChangeAsync(async () =>
            {
                _isTaskFinished = true;
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, forceLoad);
        }

        private void BeginDeleteShiftRosterChange(ShiftPatternChangeDTO shiftRoster)
        {
            try
            {
                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Deleting shift roster change, please wait...";

                _ = DeleteShiftRosterChangeAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Remove locally from the list so UI updates immediately
                    _shiftRosterChangeList.Remove(shiftRoster);

                    StateHasChanged();

                }, shiftRoster);
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
        private void GetShiftRosterDetail(ShiftPatternChangeDTO shiftRoster)
        {
            //Navigation.NavigateTo($"/TimeAttendance/mastershiftroster?ShiftPatternId={shiftRoster.ShiftPatternId}&ActionType=View");
        }
        #endregion

        #region Database Methods
        private async Task SearchShiftRosterChangeAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            var repoResult = await AttendanceService.SearchShiftPatternChangeAsync(0, 0, 0, string.Empty, string.Empty, null, null);
            if (repoResult.Success)
            {
                _shiftRosterChangeList = repoResult.Value!;
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

        private async Task DeleteShiftRosterChangeAsync(Func<Task> callback, ShiftPatternChangeDTO shiftRoster)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            //if (shiftRoster.ShiftPatternId == 0)
            //{
            //    errorMsg = "Shift Pattern ID is not defined.";
            //}
            //else
            //{
            //    var deleteResult = await AttendanceService.DeleteShiftRosterMasterAsync(shiftRoster.ShiftPatternId, _cts.Token);
            //    isSuccess = deleteResult.Success;
            //    if (!isSuccess)
            //        errorMsg = deleteResult.Error!;
            //}

            if (isSuccess)
            {
                // Show notification
                ShowNotification("The selected Shift Roster has been deleted successfully!", SnackBarTypes.Success);
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
