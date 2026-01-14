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
            new("Shift Roster Master", href: "/TimeAttendance/shiftrostersearch", icon: @Icons.Material.Filled.CalendarMonth),
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

        private enum MessageBoxTypes
        {
            Info,
            Confirm,
            Warning,
            Error
        }

        private enum NotificationType
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
                { "ContentText", $"Are you sure you want to delete the shift roster for the following employee: '{shiftRoster.EmpNo} - {shiftRoster.EmpName}'?" },
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

        private async Task EditRosterChangeAsync(ShiftPatternChangeDTO rosterChange)
        {
            try
            {
                //Clone the object so the dialog can edit without affecting the grid until Save
                var editableCopy = new ShiftPatternChangeDTO
                {
                    ShiftPatternChangeId = rosterChange.ShiftPatternChangeId,
                    EmpNo = rosterChange.EmpNo,
                    EmpName = rosterChange.EmpName,
                    DepartmentCode = rosterChange.DepartmentCode,
                    DepartmentName = rosterChange.DepartmentName,
                    EffectiveDate = rosterChange.EffectiveDate,
                    EndingDate = rosterChange.EndingDate,
                    ShiftPatternCode = rosterChange.ShiftPatternCode,
                    ShiftPointer = rosterChange.ShiftPointer,
                    ChangeTypeCode = rosterChange.ChangeTypeCode,
                    ChangeTypeDesc = rosterChange.ChangeTypeDesc,
                    CreatedByEmpNo = rosterChange.CreatedByEmpNo,
                    CreatedByName = rosterChange.CreatedByName,
                    CreatedByUserID = rosterChange.CreatedByUserID,
                    CreatedDate = rosterChange.CreatedDate,
                    LastUpdateDate = rosterChange.LastUpdateDate,
                    LastUpdateEmpNo = rosterChange.LastUpdateEmpNo,
                    LastUpdateUserID = rosterChange.LastUpdateUserID,
                    LastUpdatedByName = rosterChange.LastUpdatedByName
                };

                // Get the associated shift pointers
                if (_shiftPatternList.Any())
                {
                    ShiftPatternMasterDTO? shiftPattern = _shiftPatternList.Where(s => s.ShiftPatternCode.Trim() == rosterChange.ShiftPatternCode).FirstOrDefault();
                    if (shiftPattern != null)
                    {
                        editableCopy.ShiftPointerList = shiftPattern.ShiftPointerList;
                    }
                }

                var parameters = new DialogParameters
                {
                    ["ShiftRosterDetail"] = editableCopy,
                    ["ShiftPatternList"] = _shiftPatternList,
                    ["ChangeTypeList"] = _changeTypeList,
                    ["IsClearable"] = true,
                    ["IsDisabled"] = false,
                    ["IsEditMode"] = true
                };

                var options = new DialogOptions
                {
                    CloseOnEscapeKey = true,
                    BackdropClick = false,
                    FullWidth = true,
                    MaxWidth = MaxWidth.Medium,
                    CloseButton = false
                };

                var dialog = await DialogService.ShowAsync<ShiftRosterChangeDialog>("Edit Shift Roster Details", parameters, options);
                var result = await dialog.Result;

                if (result != null && !result.Canceled)
                {
                    var updated = (ShiftPatternChangeDTO)result.Data!;

                    // Set the update time stamp
                    updated.LastUpdateDate = DateTime.Now;

                    // Update in-memory grid item
                    var index = _shiftRosterChangeList.FindIndex(x => x.ShiftPatternChangeId == updated.ShiftPatternChangeId);
                    if (index >= 0)
                    {
                        _shiftRosterChangeList[index] = updated;
                        await InvokeAsync(StateHasChanged);
                    }

                    #region Persist changes to DB
                    // Set flag to display the loading panel
                    _isRunning = true;

                    // Set the overlay message
                    overlayMessage = "Saving employee shift roster, please wait...";

                    _ = SaveShiftRosterAsync(async () =>
                    {
                        _isRunning = false;

                        // Shows the spinner overlay
                        await InvokeAsync(StateHasChanged);
                    }, updated);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(MessageBoxTypes.Error, "Error", ex.Message.ToString());
            }
        }
        #endregion

        #region Asynchronous Tasks
        private void BeginLoadComboboxTask()
        {
            _isTaskFinished = false;
            _isRunning = true;

            // Set the overlay message
            //overlayMessage = "Initializing form, please wait...";

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
        #endregion

        #region Private Methods
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
                },
                {
                    "OkBtnColor", msgboxType == MessageBoxTypes.Error ? Color.Error
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

                _ = DeleteShiftRosterAsync(async () =>
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
        private void GetShiftRosterChangeDetail(ShiftPatternChangeDTO shiftRoster)
        {
            Navigation.NavigateTo($"/TimeAttendance/employeeshiftroster?AutoId={shiftRoster.ShiftPatternChangeId}&ActionType=Edit");
        }
        #endregion

        #region Database Methods
        private async Task SearchShiftRosterChangeAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            #region Get Change Type list
            var changeTypeResult = await LookupCache.GetChangeTypeAsync();
            if (changeTypeResult.Success)
            {
                _changeTypeList = changeTypeResult.Value!;
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(changeTypeResult.Error);
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

        private async Task DeleteShiftRosterAsync(Func<Task> callback, ShiftPatternChangeDTO shiftRoster)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (shiftRoster.ShiftPatternChangeId == 0)
            {
                errorMsg = "The grid selected row identity is not defined. Please refresh the page and select the row to delete from the grid.";
            }
            else
            {
                var deleteResult = await AttendanceService.DeleteShiftPatternChangeAsync(shiftRoster.ShiftPatternChangeId, _cts.Token);
                isSuccess = deleteResult.Success;
                if (!isSuccess)
                    errorMsg = deleteResult.Error!;
            }

            if (isSuccess)
            {
                // Show notification
                ShowNotification("The selected employee roster has been deleted successfully!", SnackBarTypes.Success);
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

        private async Task SaveShiftRosterAsync(Func<Task> callback, ShiftPatternChangeDTO dto)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = true;
            string errorMsg = string.Empty;

            #region Initialize entity
            EmployeeRosterDTO rosterChange = new EmployeeRosterDTO()
            {
                AutoId = dto.ShiftPatternChangeId,
                EmployeeNo = dto.EmpNo,
                ShiftPatternCode = dto.ShiftPatternCode,
                ShiftPointer = dto.ShiftPointer,
                ChangeTypeCode = dto.ChangeTypeCode,
                EffectiveDate = dto.EffectiveDate,
                EndingDate = dto.EndingDate,
                LastUpdateDate = dto.LastUpdateDate

            };
            #endregion

            var saveResult = await AttendanceService.UpdateShiftPatternChangeAsync(rosterChange, _cts.Token);
            isSuccess = saveResult.Success;
            if (!isSuccess)
                errorMsg = saveResult.Error!;

            if (isSuccess)
            {
                // Hide error message if any
                ShowHideError(false);

                // Show notification
                ShowNotification("Employee roster has been saved successfully!", SnackBarTypes.Success);

                // Go back to Shift Roster Master page
                //Navigation.NavigateTo("/TimeAttendance/shiftrostersearch");
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
    }
}
