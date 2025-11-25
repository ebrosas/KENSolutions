using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
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
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState AppState { get; set; } = default!;
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

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
        private bool _enableFilter = false;
        #endregion

        #region Enums
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

        #region Collections        
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Shift Roster", href: null, disabled: true, @Icons.Material.Outlined.Shield)
        ];
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_shiftPattern);

            // example: load existing master record (or keep empty for new)
            // For demo, initialize default selection
            _selectedTimingForSchedule = _shiftTimingLookup.Keys.FirstOrDefault();
            _selectedTimingForSequence = _shiftTimingLookup.Keys.FirstOrDefault();

            // if editing existing, load lists into _shiftPattern.ShiftTimingList and ShiftPointerList
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
                // If we got here, model is valid
                _hasValidationError = false;
                _validationMessages.Clear();

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Saving changes, please wait...";

                //_ = SaveQualificationAsync(async () =>
                //{
                //    _isRunning = false;

                //    // Shows the spinner overlay
                //    await InvokeAsync(StateHasChanged);
                //});
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
        private Func<ShiftTimingDTO, bool> _quickFilter => x =>
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

        private async Task StartedEditingItem(ShiftTimingDTO item)
        {
            //await EditBudgetAsync(item);
        }

        private void CommittedItemChanges(ShiftTimingDTO item)
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

        private string FormatTime(TimeSpan? ts)
        {
            if (ts == null) return string.Empty;
            // display hours:minutes (24h)
            return DateTime.Today.Add(ts.Value).ToString("HH:mm", CultureInfo.InvariantCulture);
        }

        private async Task AddTimingToSchedule()
        {
            if (string.IsNullOrWhiteSpace(_selectedTimingForSchedule))
                return;

            // Do not add duplicates (same ShiftCode for this pattern)
            if (_shiftPattern.ShiftTimingList.Any(x => x.ShiftCode == _selectedTimingForSchedule && x.ShiftPatternCode == _shiftPattern.ShiftPatternCode))
            {
                // show a snackbar or message - for brevity using Console
                Console.WriteLine("Timing already exists in schedule");
                return;
            }

            var newTiming = new ShiftTimingDTO
            {
                ShiftPatternCode = _shiftPattern.ShiftPatternCode ?? string.Empty,
                ShiftCode = _selectedTimingForSchedule,
                ShiftDescription = _shiftTimingLookup[_selectedTimingForSchedule],
                CreatedDate = DateTime.Now,
                CreatedByEmpNo = null,
                CreatedByName = "System"
                // other defaults as needed
            };

            _shiftPattern.ShiftTimingList.Add(newTiming);
            await _timingGrid?.ReloadServerData();
        }

        private async Task RemoveTimingAsync(ShiftTimingDTO timing)
        {
            if (timing == null) return;
            _shiftPattern.ShiftTimingList.Remove(timing);
            await _timingGrid?.ReloadServerData();
        }

        private async Task AddPointerToSequence()
        {
            if (string.IsNullOrWhiteSpace(_selectedTimingForSequence))
                return;

            var newPointer = new ShiftPointerDTO
            {
                ShiftPatternCode = _shiftPattern.ShiftPatternCode ?? string.Empty,
                ShiftCode = _selectedTimingForSequence,
                ShiftPointer = (_shiftPattern.ShiftPointerList.Count > 0) ? _shiftPattern.ShiftPointerList.Max(x => x.ShiftPointer) + 1 : 1,
                Remarks = string.Empty
            };

            _shiftPattern.ShiftPointerList.Add(newPointer);
            await _pointerGrid?.ReloadServerData();
        }

        private async Task RemovePointerAsync(ShiftPointerDTO pointer)
        {
            if (pointer == null) return;
            _shiftPattern.ShiftPointerList.Remove(pointer);
            await _pointerGrid?.ReloadServerData();
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
        #endregion
    }
}
