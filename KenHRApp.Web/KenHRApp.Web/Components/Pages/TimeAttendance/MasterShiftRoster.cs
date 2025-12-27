using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Domain.Entities;
using KenHRApp.Web.Components.Pages.Recruitment;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Globalization;
using System.Text;

namespace KenHRApp.Web.Components.Pages.TimeAttendance
{
    public partial class MasterShiftRoster
    {
        #region Parameters and Injections
        [Inject] private IAttendanceService AttendanceService { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState AppState { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string ActionType { get; set; } = ActionTypes.View.ToString();

        [Parameter]
        [SupplyParameterFromQuery]
        public int ShiftPatternId { get; set; } = 0;
        #endregion

        #region Fields

        #region Private Fields
        // The master DTO bound to the form
        private ShiftPatternMasterDTO _shiftPattern = new ShiftPatternMasterDTO
        {
            IsActive = true,
            IsFlexiTime = false,
            ShiftPatternCode = string.Empty
        };

        private EditForm _editForm;
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private string _searchString = string.Empty;
        private StringBuilder _errorMessage = new StringBuilder();

        private MudForm? _form;
        private MudDataGrid<ShiftTimingDTO>? _timingGrid;
        private MudDataGrid<ShiftPointerDTO>? _pointerGrid;

        // The pre-populated shift timing list (code => description)
        private readonly Dictionary<string, string> _shiftTimingLookup = new Dictionary<string, string>
        {
            {"D", "Day"},
            {"E", "Evening"},
            {"M", "Morning"},
            {"N", "Night"},
            {"O", "Weekend"}
        };

        private string? _selectedTimingForSchedule;
        private string? _selectedTimingForSequence;
        #endregion

        #region Constants
        private readonly string CONST_DAYOFF_CODE = "O";
        private readonly string CONST_DAYOFF_DESC = "Weekend";
        #endregion

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
        private bool _enableShiftTimingFilter = false;
        private bool _enableShiftPointerFilter = false;
        private bool _isDisabled = false;
        private bool _isClearable = false;
        private bool _isEditMode = false;
        private bool _saveBtnEnabled = false;
        private bool _forceLoad = false;
        private bool _allowGridEdit = false;
        #endregion

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        #endregion

        #region Collections        
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Shift Roster Master", href: "/TimeAttendance/shiftrostersearch", icon: @Icons.Material.Filled.CalendarMonth),
            new("Shift Roster Detail", href: null, disabled: true, @Icons.Material.Filled.EditNote)
        ];

        private List<ShiftMasterDTO> _shiftMasterList = new List<ShiftMasterDTO>();
        private List<ShiftMasterDTO> _shiftMasterPointerList = new List<ShiftMasterDTO>();
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
        #endregion
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_shiftPattern);

            // Set action to Add mode 
            //ActionType = ActionTypes.Add.ToString();

            if (ActionType == ActionTypes.Edit.ToString() ||
                ActionType == ActionTypes.View.ToString())
            {
                _isDisabled = true;

                if (ShiftPatternId > 0)
                    BeginGetShiftRosterDetail();
            }
            else if (ActionType == ActionTypes.Add.ToString())
            {
                _isDisabled = false;
                _saveBtnEnabled = true;
                _allowGridEdit = true;
            }

                // example: load existing master record (or keep empty for new)
                // For demo, initialize default selection
                _selectedTimingForSchedule = _shiftTimingLookup.Keys.FirstOrDefault();
            //_selectedTimingForSequence = _shiftTimingLookup.Keys.FirstOrDefault();

            #region Populate shift master list
            _shiftMasterList.Add(new ShiftMasterDTO()
            {
                ShiftCode = "D",
                ShiftDescription = "Day",
                ArrivalFrom = new TimeSpan(6,0,0),
                ArrivalTo = new TimeSpan(7, 30, 0),
                DepartFrom = new TimeSpan(16, 0, 0),
                DepartTo = new TimeSpan(16, 30, 0),
                DurationNormal = 8,
                RArrivalFrom = new TimeSpan(6, 0, 0),
                RArrivalTo = new TimeSpan(7, 30, 0),
                RDepartFrom = new TimeSpan(13, 30, 0),
                RDepartTo = new TimeSpan(14, 00, 0),
                DurationRamadan = 6
            });

            _shiftMasterList.Add(new ShiftMasterDTO()
            {
                ShiftCode = "M",
                ShiftDescription = "Morning",
                ArrivalFrom = new TimeSpan(6, 0, 0),
                ArrivalTo = new TimeSpan(7, 0, 0),
                DepartFrom = new TimeSpan(15, 0, 0),
                DepartTo = new TimeSpan(15, 30, 0),
                DurationNormal = 8,
                RArrivalFrom = new TimeSpan(6, 0, 0),
                RArrivalTo = new TimeSpan(7, 0, 0),
                RDepartFrom = new TimeSpan(13, 0, 0),
                RDepartTo = new TimeSpan(13, 30, 0),
                DurationRamadan = 6
            });

            _shiftMasterList.Add(new ShiftMasterDTO()
            {
                ShiftCode = "E",
                ShiftDescription = "Evening",
                ArrivalFrom = new TimeSpan(14, 0, 0),
                ArrivalTo = new TimeSpan(15, 0, 0),
                DepartFrom = new TimeSpan(23, 0, 0),
                DepartTo = new TimeSpan(23, 30, 0),
                DurationNormal = 8,
                RArrivalFrom = new TimeSpan(14, 0, 0),
                RArrivalTo = new TimeSpan(15, 0, 0),
                RDepartFrom = new TimeSpan(21, 0, 0),
                RDepartTo = new TimeSpan(21, 30, 0),
                DurationRamadan = 6
            });

            _shiftMasterList.Add(new ShiftMasterDTO()
            {
                ShiftCode = "N",
                ShiftDescription = "Night",
                ArrivalFrom = new TimeSpan(22, 0, 0),
                ArrivalTo = new TimeSpan(23, 0, 0),
                DepartFrom = new TimeSpan(7, 0, 0),
                DepartTo = new TimeSpan(7, 30, 0),
                DurationNormal = 8,
                RArrivalFrom = new TimeSpan(22, 0, 0),
                RArrivalTo = new TimeSpan(23, 0, 0),
                RDepartFrom = new TimeSpan(5, 0, 0),
                RDepartTo = new TimeSpan(5, 30, 0),
                DurationRamadan = 6
            });
            #endregion

            #region Populate Shift Master Pointer List
            //_shiftMasterPointerList.Add(new ShiftMasterDTO()
            //{
            //    ShiftCode = "D",
            //    ShiftDescription = "Day",
            //    ArrivalFrom = new TimeSpan(6, 0, 0),
            //    ArrivalTo = new TimeSpan(7, 30, 0),
            //    DepartFrom = new TimeSpan(16, 0, 0),
            //    DepartTo = new TimeSpan(16, 30, 0),
            //    DurationNormal = 8,
            //    RArrivalFrom = new TimeSpan(6, 0, 0),
            //    RArrivalTo = new TimeSpan(7, 30, 0),
            //    RDepartFrom = new TimeSpan(13, 30, 0),
            //    RDepartTo = new TimeSpan(14, 00, 0),
            //    DurationRamadan = 6
            //});
            #endregion
        }
        #endregion

        #region Validation Methods
        private void HandleInvalidSubmit(EditContext context)
        {
            _hasValidationError = true;
            _validationMessages = context.GetValidationMessages().ToList();
            // ShowNotification("Please fix the errors and try again.", NotificationType.Error);
        }

        private void HandleValidSubmit(EditContext context)
        {
            try
            {
                #region Check for Shift Timing Schedule
                if (_shiftPattern.ShiftTimingList.Any() == false)   
                {
                    ShowNotification("At least one (1) Shift timing must be added to the Shift Timing Schedule.", NotificationType.Error);
                    return;
                }
                #endregion

                #region Check for Shift Timing Sequence
                if (_shiftPattern.ShiftPointerList.Any() == false)
                {
                    ShowNotification("At least one (1) Shift pointer must be added to the Shift Timing Sequence.", NotificationType.Error);
                    return;
                }
                #endregion

                // If we got here, model is valid
                _hasValidationError = false;
                _validationMessages.Clear();

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
        private Func<ShiftTimingDTO, bool> _shiftTimingFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;

            if (!string.IsNullOrEmpty(x.ShiftCode) && x.ShiftCode.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.ShiftDescription) && x.ShiftDescription.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        //private async Task OnStartedEditingItem(MudDataGridRowEditEventArgs<RecruitmentBudgetDTO> e)
        //{
        //    var item = e.Item;
        //    await EditBudgetAsync(item);
        //    e.Cancel = true; // Prevent the DataGrid's internal edit form from appearing
        //}

        private async Task StartedEditingShiftTimingItem(ShiftTimingDTO item)
        {
            //await EditBudgetAsync(item);
        }

        private async Task StartedEditingShiftPointerItem(ShiftPointerDTO item)
        {
            //await EditBudgetAsync(item);
        }

        private void CommittedShiftTimingItemChanges(ShiftTimingDTO item)
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

        private void CommittedShiftTimingItemChanges(ShiftPointerDTO item)
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

        private async Task DeleteShiftTiming(ShiftTimingDTO item)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the following shift schedule: '{item.ShiftDescription}'?" },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.TopCenter,
                CloseOnEscapeKey = true,    // Prevent ESC from closing
                BackdropClick = false       // Prevent clicking outside to close
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                _shiftPattern.ShiftTimingList.Remove(item);

                // Remove shift timing from the "Shift Timing Sequence"
                ShiftMasterDTO? shiftTimingToDelete = _shiftMasterPointerList.Where(a => a.ShiftCode == item.ShiftCode).FirstOrDefault();
                if (shiftTimingToDelete != null)
                {
                    _shiftMasterPointerList.Remove(shiftTimingToDelete);
                }

                // Refresh grid
                await _timingGrid?.ReloadServerData();

                //BeginDeleteRecruitmentBudget(budget);
            }
        }

        private async Task DeleteShiftSequence(ShiftPointerDTO item)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the following shift timing sequence: '{item.ShiftPointer} - {item.ShiftDescription}'?" },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.TopCenter,
                CloseOnEscapeKey = true,    // Prevent ESC from closing
                BackdropClick = false       // Prevent clicking outside to close
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                _shiftPattern.ShiftPointerList.Remove(item);
                await _pointerGrid?.ReloadServerData();

                //BeginDeleteRecruitmentBudget(budget);
            }
        }
        #endregion

        #region Button Event Handlers
        private async Task HandleEditButton()
        {
            try
            {
                _isRunning = true;
                overlayMessage = "Entering edit mode, please wait...";
                StateHasChanged(); // immediate render

                // do your async work
                await Task.Delay(500);

                // Set the flags
                _isEditMode = true;
                _allowGridEdit = true;
                _saveBtnEnabled = true;
                _isDisabled = false;

                // Hide error message if any
                ShowHideError(false);
            }
            catch (Exception ex)
            {
                overlayMessage = $"Error: {ex.Message}";
            }
            finally
            {
                _isRunning = false;     // ✅ must execute
                StateHasChanged();      // ✅ must re-render
            }
        }

        private void HandleUndoButton()
        {
            // Set flag to display the loading panel
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Undoing changes, please wait...";

            _ = GetShiftRosterDetailAsync(async () =>
            {
                // Reset the flags
                _isEditMode = false;
                _allowGridEdit = false;
                _isDisabled = true;
                _isRunning = false;
                _saveBtnEnabled = false;
                _hasValidationError = false;
                _validationMessages.Clear();

                // Reset error messages
                _errorMessage.Clear();
                ShowHideError(false);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, _shiftPattern.ShiftPatternId);
        }

        private void HandleRefreshButton()
        {
            if (_shiftPattern.ShiftPatternId > 0)
            {
                BeginGetShiftRosterDetail();
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

        private async Task ShowDeleteDialog()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", "Do you really want to delete this shift roster? Note that this process cannot be undone." },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                BeginDeleteShiftRoster();
            }
        }

        private async Task ShowCancelDialog()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Cancel"},
                { "DialogIcon", _iconCancel },
                { "ContentText", "Do you really want to cancel adding new shift roster?" },
                { "ConfirmText", "Yes" },
                { "CancelText", "No" },
                { "Color", Color.Success }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                CancelAddShiftRoster();
            }
        }

        private void CancelAddShiftRoster()
        {
            Navigation.NavigateTo("/TimeAttendance/shiftrostersearch");
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

        private string FormatTime(TimeSpan? ts)
        {
            if (ts == null) return string.Empty;
            // display hours:minutes (24h)
            return DateTime.Today.Add(ts.Value).ToString("HH:mm", CultureInfo.InvariantCulture);
        }

        private async Task AddTimingToSchedule()
        {
            if (string.IsNullOrEmpty(_shiftPattern.ShiftPatternCode))
            {
                ShowNotification("Shift Pattern Code must be defined.", NotificationType.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(_selectedTimingForSchedule))
            {
                ShowNotification("Shift Timing must be selected from the drop-down box.", NotificationType.Error);
                return;
            }

            // Do not add duplicates (same ShiftCode for this pattern)
            if (_shiftPattern.ShiftTimingList.Any(x => x.ShiftCode == _selectedTimingForSchedule && x.ShiftPatternCode == _shiftPattern.ShiftPatternCode))
            {
                await ShowErrorMessage(MessageBoxTypes.Info, "Duplicate Record", "Timing already exists in schedule.");
                return;
            }

            ShiftMasterDTO? selectedShift = _shiftMasterList.Where(a => a.ShiftCode == _selectedTimingForSchedule).FirstOrDefault();
            if (selectedShift != null)
            {
                #region Add shift timing to the "Shift Timing Sequence" grid
                if (!_shiftMasterPointerList.Any(x => x.ShiftCode == selectedShift.ShiftCode))
                {
                    if (_shiftMasterPointerList.Count == 0)
                    {
                        // Add Day-off shift timing
                        _shiftMasterPointerList.Add(new ShiftMasterDTO()
                        {
                            ShiftCode = CONST_DAYOFF_CODE,
                            ShiftDescription = CONST_DAYOFF_DESC
                        });
                    }

                    _shiftMasterPointerList.Add(new ShiftMasterDTO()
                    {
                        ShiftCode = selectedShift.ShiftCode,
                        ShiftDescription = selectedShift.ShiftDescription,
                        ArrivalFrom = selectedShift.ArrivalFrom,
                        ArrivalTo = selectedShift.ArrivalTo,
                        DepartFrom = selectedShift.DepartFrom,
                        DepartTo = selectedShift.DepartTo,
                        RArrivalFrom = selectedShift.RArrivalFrom,
                        RArrivalTo = selectedShift.RArrivalTo,
                        RDepartFrom = selectedShift.RDepartFrom,
                        RDepartTo = selectedShift.RDepartTo
                    });
                }
                #endregion

                #region Add new shift timing to the grid                                
                var newTiming = new ShiftTimingDTO
                {
                    ShiftPatternCode = _shiftPattern.ShiftPatternCode ?? string.Empty,
                    ShiftCode = _selectedTimingForSchedule,
                    ShiftDescription = selectedShift.ShiftDescription, //_shiftTimingLookup[_selectedTimingForSchedule],
                    ArrivalFrom = selectedShift.ArrivalFrom,
                    ArrivalTo = selectedShift.ArrivalTo,
                    DepartFrom = selectedShift.DepartFrom,
                    DepartTo = selectedShift.DepartTo,
                    RArrivalFrom = selectedShift.RArrivalFrom,
                    RArrivalTo = selectedShift.RArrivalTo,
                    RDepartFrom = selectedShift.RDepartFrom,
                    RDepartTo = selectedShift.RDepartTo,
                    CreatedDate = DateTime.Now,
                    CreatedByEmpNo = 10003632,
                    CreatedByName = "ERVIN OLINAS BROSAS"
                };

                _shiftPattern.ShiftTimingList.Add(newTiming);
                await _timingGrid!.ReloadServerData();
                #endregion
            }
        }

        private async Task RemoveTimingAsync(ShiftTimingDTO timing)
        {
            if (timing == null) return;
            _shiftPattern.ShiftTimingList.Remove(timing);
            await _timingGrid?.ReloadServerData();
        }

        private async Task AddPointerToSequence()
        {
            if (string.IsNullOrEmpty(_shiftPattern.ShiftPatternCode))
            {
                ShowNotification("Shift Roster Code must be defined.", NotificationType.Error);
                return;
            }

            if (string.IsNullOrWhiteSpace(_selectedTimingForSequence))
            {
                ShowNotification("Shift Timing must be selected from the drop-down box.", NotificationType.Error);
                return;
            }

            ShiftMasterDTO? selectedShift = _shiftMasterList.Where(a => a.ShiftCode == _selectedTimingForSchedule).FirstOrDefault();
            if (selectedShift != null)
            {
                var newPointer = new ShiftPointerDTO
                {
                    ShiftPatternCode = _shiftPattern.ShiftPatternCode ?? string.Empty,
                    ShiftCode = _selectedTimingForSequence,
                    ShiftPointer = (_shiftPattern.ShiftPointerList.Count > 0) ? _shiftPattern.ShiftPointerList.Max(x => x.ShiftPointer) + 1 : 1,
                    ShiftDescription = _shiftTimingLookup.GetValueOrDefault(_selectedTimingForSequence)
                };

                _shiftPattern.ShiftPointerList.Add(newPointer);
                await _pointerGrid!.ReloadServerData();
            }
        }

        private async Task RemovePointerAsync(ShiftPointerDTO pointer)
        {
            if (pointer == null) return;
            _shiftPattern.ShiftPointerList.Remove(pointer);
            await _pointerGrid?.ReloadServerData();
        }

        private async Task SaveShiftRosterAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            bool isNewRequition = _shiftPattern.ShiftPatternId == 0;

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = true;
            string errorMsg = string.Empty;

            if (isNewRequition)
            {
                // Set the user who created the record and the timestamp
                _shiftPattern.CreatedDate = DateTime.Now;

                var addResult = await AttendanceService.AddShiftRosterMasterAsync(_shiftPattern, _cts.Token);
                isSuccess = addResult.Success;
                if (!isSuccess)
                    errorMsg = addResult.Error!;
                else
                {
                    // Set flag to enable reload of _recruitmentRequests when navigating back to the Employe Search page
                    _forceLoad = true;
                }
            }
            else
            {
                // Set the user who created the record and the timestamp
                _shiftPattern.CreatedDate = DateTime.Now;

                var saveResult = await AttendanceService.UpdateShiftRosterMasterAsync(_shiftPattern, _cts.Token);
                isSuccess = saveResult.Success;
                if (!isSuccess)
                    errorMsg = saveResult.Error!;
            }

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
                if (isNewRequition)
                    ShowNotification("Shift Roster has been created successfully!", NotificationType.Success);
                else
                    ShowNotification("Shift Roster has been updated successfully!", NotificationType.Success);

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

        private async Task SaveMasterAsync()
        {
            if (_form == null) return;
            await _form.Validate();

            if (!_form.IsValid)
            {
                // show error message; do not save
                Console.WriteLine("Form invalid");
                return;
            }

            // Ensure shift code on child items are in sync with header's ShiftPatternCode
            foreach (var t in _shiftPattern.ShiftTimingList)
                t.ShiftPatternCode = _shiftPattern.ShiftPatternCode;

            foreach (var p in _shiftPattern.ShiftPointerList)
                p.ShiftPatternCode = _shiftPattern.ShiftPatternCode;

            // TODO: call your API/service to save _shiftPattern
            // await _shiftPatternService.SaveAsync(_shiftPattern);

            Console.WriteLine("Saved (replace this with real save)");
        }

        private void ResetForm()
        {
            _shiftPattern = new ShiftPatternMasterDTO
            {
                IsActive = true,
                IsFlexiTime = false,
                ShiftPatternCode = string.Empty
            };
        }

        private async Task ConfirmDelete(ShiftTimingDTO shiftTiming)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the department '{shiftTiming.ShiftDescription}'?" },
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
                //BeginDeleteDepartment(department);
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

        #region Database Methods
        private void BeginGetShiftRosterDetail()
        {
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading shift roster details, please wait...";

            _ = GetShiftRosterDetailAsync(async () =>
            {
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, ShiftPatternId);
        }

        private async Task GetShiftRosterDetailAsync(Func<Task> callback, int shiftPatternId)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(1000);

            // Reset error messages
            _errorMessage.Clear();

            // Clear shift timing items in the "Shift Timing Sequence" grid
            _shiftMasterPointerList.Clear();

            var result = await AttendanceService.GetShiftRosterDetailAsync(shiftPatternId);
            if (result.Success)
            {
                _shiftPattern = result.Value!;

                // Recreate the EditContext with the loaded _shiftPattern
                _editContext = new EditContext(_shiftPattern);
            }
            else
            {
                // Set the error message
                _errorMessage.Append(result.Error);

                ShowHideError(true);
            }

            #region Populate Shift Timing drop-down items in "Shift Timing Sequence" grid
            if (_shiftPattern.ShiftTimingList != null && _shiftPattern.ShiftTimingList.Any())
            {
                foreach (var shiftTiming in _shiftPattern.ShiftTimingList)
                {
                    if (!_shiftMasterPointerList.Any(x => x.ShiftCode == shiftTiming.ShiftCode))
                    {
                        if (_shiftMasterPointerList.Count == 0)
                        {
                            // Add Day-off shift timing
                            _shiftMasterPointerList.Add(new ShiftMasterDTO()
                            {
                                ShiftCode = CONST_DAYOFF_CODE,
                                ShiftDescription = CONST_DAYOFF_DESC
                            });
                        }

                        _shiftMasterPointerList.Add(new ShiftMasterDTO()
                        {
                            ShiftCode = shiftTiming.ShiftCode,
                            ShiftDescription = shiftTiming.ShiftDescription,
                            ArrivalFrom = shiftTiming.ArrivalFrom,
                            ArrivalTo = shiftTiming.ArrivalTo!.Value,
                            DepartFrom = shiftTiming.DepartFrom!.Value,
                            DepartTo = shiftTiming.DepartTo,
                            RArrivalFrom = shiftTiming.RArrivalFrom,
                            RArrivalTo = shiftTiming.RArrivalTo!.Value,
                            RDepartFrom = shiftTiming.RDepartFrom!.Value,
                            RDepartTo = shiftTiming.RDepartTo
                        });
                    }
                }
            }
            #endregion

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private void BeginDeleteShiftRoster()
        {
            try
            {
                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Deleting employee record, please wait...";

                _ = DeleteShiftRosterAsync(async () =>
                {
                    _isRunning = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Go back to Shift Roster Master page
                    Navigation.NavigateTo("/TimeAttendance/shiftrostersearch");
                });
            }
            catch (OperationCanceledException)
            {
                ShowNotification("Delete cancelled (navigated away).", NotificationType.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
        }

        private async Task DeleteShiftRosterAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (_shiftPattern.ShiftPatternId == 0)
            {
                errorMsg = "Shift Pattern ID is not defined.";
            }
            else
            {
                var deleteResult = await AttendanceService.DeleteShiftRosterMasterAsync(_shiftPattern.ShiftPatternId, _cts.Token);
                isSuccess = deleteResult.Success;
                if (!isSuccess)
                    errorMsg = deleteResult.Error!;
            }

            if (isSuccess)
            {
                // Show notification
                ShowNotification("Shift Roster record has been deleted successfully!", NotificationType.Success);                                
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
