using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;
using System.ComponentModel.DataAnnotations;
using KenHRApp.Domain.Entities;

namespace KenHRApp.Web.Components.Pages.Recruitment
{
    public partial class RecruitmentRequest
    {
        #region Parameters and Injections
        [Inject] private IRecruitmentService RecruitmentService { get; set; } = default!;
        [Inject] private IAppCacheService AppCacheService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        #endregion

        #region Fields
        private RecruitmentRequestDTO _recruitmentRequest = new();
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private CancellationTokenSource? _cts;
        private string overlayMessage = "Please wait...";
        private StringBuilder _errorMessage = new StringBuilder();
        private string _searchStringQualification = string.Empty;

        #region System Flags
        private static bool _forceLoad = false;
        private bool _isRunning = false;
        private bool _isDisabled = false;
        private bool _isEditMode = false;
        private bool _isClearable = false;
        private bool _isCboClearable = true;
        private bool _saveBtnEnabled = false;
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _allowGridEdit = false;
        private bool _enableFilter = false;  
        #endregion

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        private readonly string _iconAdd = "fas fa-plus-circle";
        #endregion

        #region Enums and Collections
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

        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Recruitment Management", href: "/Recruitment/recruitmenthome", icon: @Icons.Material.Outlined.People),
            new("Recruitment Request", href: null, disabled: true, @Icons.Material.Outlined.EditCalendar)
        ];
        #endregion

        #region Collection Arrays
        string[] workExperienceLabels = new string[] { "0", "10", "20", "30", "40", "50" };

        private List<UserDefinedCodeDTO> _employmentTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _employmentTypeArray = null;
        private List<UserDefinedCodeDTO> _qualificationModeList = new List<UserDefinedCodeDTO>();
        private string[]? _qualificationModeArray = null;
        #endregion

        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_recruitmentRequest);

            //BeginLoadComboboxTask();
        }

        protected override void OnParametersSet()
        {
            // Reset error display when navigating to page
            _hasValidationError = false;
        }
        #endregion

        #region Form Events
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

                //_ = SaveChangeAsync(async () =>
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

        private void HandleInvalidSubmit(EditContext context)
        {
            _hasValidationError = true;
            _validationMessages = context.GetValidationMessages().ToList();
        }
        #endregion

        #region Grid Events
        private Func<RecruitmentRequestDTO, bool> _quickFilterQualification => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchStringQualification))
                return true;

            //if (!string.IsNullOrEmpty(x.DepartmentName) && x.DepartmentName.Contains(_searchStringQualification, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (!string.IsNullOrEmpty(x.BudgetDescription) && x.BudgetDescription.Contains(_searchStringQualification, StringComparison.OrdinalIgnoreCase))
            //    return true;

            //if (!string.IsNullOrEmpty(x.Remarks) && x.Remarks.Contains(_searchStringQualification, StringComparison.OrdinalIgnoreCase))
            //    return true;

            return false;
        };

        private async Task StartedEditingItem(RecruitmentRequestDTO item)
        {
            //await EditBudgetAsync(item);
        }

        private void CommittedItemChanges(RecruitmentRequestDTO item)
        {

        }
        #endregion

        #region Async Methods
        private async Task AddQualificationAsync()
        {
            try
            {
                var parameters = new DialogParameters
                {
                    ["RecruitmentBudget"] = new RecruitmentBudgetDTO() { OnHold = false },
                    //["DepartmentList"] = _departmentList,
                    ["IsClearable"] = true,
                    ["IsDisabled"] = false,
                    ["IsEditMode"] = false
                };

                var options = new DialogOptions
                {
                    CloseOnEscapeKey = true,
                    BackdropClick = false,
                    FullWidth = true,
                    MaxWidth = MaxWidth.Medium,
                    CloseButton = false
                };

                // Show the dialog box
                var dialog = await DialogService.ShowAsync<RecruitmentBudgetDialog>("Add Recruitment Budget", parameters, options);
                var result = await dialog.Result;

                //if (result != null && !result.Canceled)
                //{
                //    var newBudget = (RecruitmentBudgetDTO)result.Data!;
                //    newBudget.BudgetId = 0;

                //    #region Get the selected department
                //    if (!string.IsNullOrEmpty(newBudget.DepartmentName))
                //    {
                //        DepartmentDTO? department = _departmentList.Where(d => d.DepartmentName == newBudget.DepartmentName).FirstOrDefault();
                //        if (department != null)
                //            newBudget.DepartmentCode = department.DepartmentCode;
                //    }
                //    #endregion

                //    #region Check for duplicate entries
                //    var duplicateBudget = _budgetList.FirstOrDefault(e => e.DepartmentCode.Trim().ToUpper() == newBudget.DepartmentCode.Trim().ToUpper()
                //        && e.BudgetHeadCount == newBudget.BudgetHeadCount);
                //    if (duplicateBudget != null)
                //    {
                //        // Show error
                //        await ShowErrorMessage(MessageBoxTypes.Error, "Error", "The specified department and head count budget already exists. Please enter a different value on these fields then try to save again.");
                //        return;
                //    }
                //    #endregion

                //    // Set the Update Date
                //    newBudget.CreatedDate = DateTime.Now;

                //    // Set flag to display the loading panel
                //    _isRunning = true;

                //    // Set the overlay message
                //    overlayMessage = "Adding budget, please wait...";

                //    _ = SaveChangeAsync(async () =>
                //    {
                //        _isRunning = false;

                //        // Shows the spinner overlay
                //        await InvokeAsync(StateHasChanged);

                //    }, newBudget);
                //}
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(MessageBoxTypes.Error, "Error", ex.Message.ToString());
            }
        }

        private async Task EditQualificationAsync(JobQualificationDTO qualification)
        {
            try
            {
                // Clone the object so the dialog can edit without affecting the grid until Save
                var editableCopy = new JobQualificationDTO
                {
                    RequisitionId = qualification.RequisitionId,
                    AutoId = qualification.AutoId,
                    QualificationCode = qualification.QualificationCode,
                    Qualification = qualification.Qualification,
                    StreamCode = qualification.StreamCode,
                    Stream = qualification.Stream,
                    SpecializationCode = qualification.SpecializationCode,
                    Specialization = qualification.Specialization
                };

                var parameters = new DialogParameters
                {
                    ["RecruitmentBudget"] = editableCopy,
                    //["DepartmentList"] = _departmentList,
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

                var dialog = await DialogService.ShowAsync<RecruitmentBudgetDialog>("Edit Recruitment Budget", parameters, options);
                var result = await dialog.Result;

                if (result != null && !result.Canceled)
                {
                    var updated = (RecruitmentBudgetDTO)result.Data!;

                    #region Get selected department
                    //if (!string.IsNullOrEmpty(updated.DepartmentName))
                    //{
                    //    DepartmentDTO? department = _departmentList.Where(d => d.DepartmentName == updated.DepartmentName).FirstOrDefault();
                    //    if (department != null)
                    //        updated.DepartmentCode = department.DepartmentCode;
                    //}
                    //#endregion

                    //// Update in-memory grid item
                    //var index = _qualificationList.FindIndex(x => x.BudgetId == updated.BudgetId);
                    //if (index >= 0)
                    //{
                    //    _qualificationList[index] = updated;
                    //    await InvokeAsync(StateHasChanged);
                    //}

                    //#region Persist changes to DB
                    //// Set flag to display the loading panel
                    //_isRunning = true;

                    //// Set the overlay message
                    //overlayMessage = "Saving qualification changes, please wait...";

                    //_ = SaveChangeAsync(async () =>
                    //{
                    //    _isRunning = false;

                    //    // Shows the spinner overlay
                    //    await InvokeAsync(StateHasChanged);
                    //}, updated);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(MessageBoxTypes.Error, "Error", ex.Message.ToString());
            }
        }

        private async Task DeleteQualification(JobQualificationDTO qualification)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the qualification '{qualification.Qualification}'?" },
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
                //BeginDeleteRecruitmentBudget(budget);
            }
        }
        #endregion

        #region Private Methods
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
        #endregion

        #region Drop-down Boxes Search Methods
        private async Task<IEnumerable<string>> SearchEmploymentType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _employmentTypeArray!;
            }

            return _employmentTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchQualficationMode(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _qualificationModeArray!;
            }

            return _qualificationModeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion
    }
}
