using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;
using System.ComponentModel.DataAnnotations;
using KenHRApp.Application.Services;
using KenHRApp.Domain.Entities;

namespace KenHRApp.Web.Components.Pages.Recruitment
{
    public partial class RecruitmentHome
    {
        #region Parameters and Injections
        [Inject] 
        private IDialogService DialogService { get; set; } = default!;
        
        [Inject] 
        private ISnackbar Snackbar { get; set; } = default!;
        
        [Inject] 
        private IRecruitmentService RecruitmentService { get; set; } = default!;

        [Inject] 
        private ILookupCacheService LookupCache { get; set; } = default!;
        #endregion

        #region Private Fields        
        private RecruitmentBudgetDTO _recruitmentBudget = new();        
        private StringBuilder _errorMessage = new StringBuilder();
        private EditForm _editForm;
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        private string _searchString = string.Empty;

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

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        private readonly string _iconAdd = "fas fa-plus-circle";
        #endregion

        #region Collections        
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Recruitment Management", href: null, disabled: true, @Icons.Material.Outlined.People)
        ];

        private List<RecruitmentBudgetDTO> _budgetList = new List<RecruitmentBudgetDTO>();
        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        private string[]? _departmentArray = null;
        #endregion

        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_recruitmentBudget);

            BeginLoadComboboxTask();            
        }

        protected override void OnParametersSet()
        {
            // Reset error display when navigating to page
            _hasValidationError = false;
        }
        #endregion

        #region Grid Events 
        private Func<RecruitmentBudgetDTO, bool> _quickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_searchString))
                return true;
            
            if (!string.IsNullOrEmpty(x.DepartmentName) && x.DepartmentName.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.BudgetDescription) && x.BudgetDescription.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.Remarks) && x.Remarks.Contains(_searchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void StartedEditingItem(RecruitmentBudgetDTO item)
        {
            //BeginLoadComboboxTask();
        }

        private void CommittedItemChanges(RecruitmentBudgetDTO item)
        {
            try
            {
                if (item == null) return;

                #region Get selected department
                if (!string.IsNullOrEmpty(item.DepartmentName))
                {
                    DepartmentDTO? department = _departmentList.Where(d => d.DepartmentName == item.DepartmentName).FirstOrDefault();
                    if (department != null)
                        item.DepartmentCode = department.DepartmentCode;
                }
                #endregion

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
                ShowNotification("Save cancelled (navigated away).", NotificationType.Warning);
            }
            catch (Exception ex)
            {
                ShowNotification($"Error: {ex.Message}", NotificationType.Error);
            }
        }

        private async Task ConfirmDeleteBudget(RecruitmentBudgetDTO budget)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the recruitment budget for '{budget.DepartmentName}'?" },
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
                BeginDeleteRecruitmentBudget(budget);
            }
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
        #endregion

        #region Async Methods
        private void BeginDeleteRecruitmentBudget(RecruitmentBudgetDTO budget)
        {
            try
            {
                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Deleting budget record, please wait...";

                _ = DeleteRecruitmentBudgetAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Remove locally from the list so UI updates immediately
                    _budgetList.Remove(budget);

                    StateHasChanged();

                }, budget);
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

        private void BeginSearchBudget(bool forceLoad = false)
        {
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading budget list, please wait...";

            _ = SearchBudgetAsync(async () =>
            {
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            }, forceLoad);
        }

        private async Task AddBudgetAsync()
        {
            try
            {
                var parameters = new DialogParameters
                {
                    ["DialogTitle"] = "Add New Budget",
                    ["DialogIcon"] = _iconAdd,
                    ["DialogIconColor"] = Color.Info,
                    ["RecruitmentBudget"] = new RecruitmentBudgetDTO() { OnHold = false },
                    ["DepartmentList"] = _departmentList,
                    ["IsClearable"] = true,
                    ["IsDisabled"] = false
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
                var dialog = await DialogService.ShowAsync<RecruitmentBudgetDialog>("Add New Budget", parameters, options);

                var result = await dialog.Result;
                if (result != null && !result.Canceled)
                {
                    var newBudget = (RecruitmentBudgetDTO)result.Data!;
                    newBudget.BudgetId = 0;

                    #region Get the selected department
                    if (!string.IsNullOrEmpty(newBudget.DepartmentName))
                    {
                        DepartmentDTO? department = _departmentList.Where(d => d.DepartmentName == newBudget.DepartmentName).FirstOrDefault();
                        if (department != null)
                            newBudget.DepartmentCode = department.DepartmentCode;
                    }
                    #endregion

                    #region Check for duplicate entries
                    var duplicateBudget = _budgetList.FirstOrDefault(e => e.DepartmentCode.Trim().ToUpper() == newBudget.DepartmentCode.Trim().ToUpper()
                        && e.BudgetHeadCount == newBudget.BudgetHeadCount);
                    if (duplicateBudget != null)
                    {
                        // Show error
                        await ShowErrorMessage(MessageBoxTypes.Error, "Error", "The specified department and head count budget already exists. Please enter a different value on these fields then try to save again.");
                        return;
                    }
                    #endregion

                    // Set the Update Date
                    newBudget.CreatedDate = DateTime.Now;

                    // Set flag to display the loading panel
                    _isRunning = true;

                    // Set the overlay message
                    overlayMessage = "Adding budget, please wait...";

                    _ = SaveChangeAsync(async () =>
                    {
                        _isRunning = false;

                        // Shows the spinner overlay
                        await InvokeAsync(StateHasChanged);

                    }, newBudget);
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(MessageBoxTypes.Error, "Error", ex.Message.ToString());
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

        private void BeginLoadComboboxTask()
        {
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Initializing form, please wait...";

            _ = LoadComboboxAsync(async () =>
            {
                _isRunning = false;

                if (_errorMessage.Length > 0)
                    ShowHideError(true);

                BeginSearchBudget();

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);                                
            });
        }
        #endregion

        #region Database Methods
        private async Task LoadComboboxAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

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
            }
            #endregion

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task SaveChangeAsync(Func<Task> callback, RecruitmentBudgetDTO budget)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            var result = await RecruitmentService.SaveRecruitmentBudgetAsync(budget, _cts.Token);
            if (!result.Success)
            {
                // Set the error message
                _errorMessage.AppendLine(result.Error!);
                ShowHideError(true);
            }
            else
            {
                if (budget.BudgetId == 0)
                {
                    // Get the new identity seed
                    budget.BudgetId = _budgetList.Max(d => d.BudgetId) + 1;

                    // Add locally to the list so UI updates immediately
                    _budgetList.Add(budget);

                    StateHasChanged();
                }

                // Show notification
                ShowNotification("Budget saved successfully!", NotificationType.Success);
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }                

        private async Task SearchBudgetAsync(Func<Task> callback, bool forceLoad = false)
        {
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            string departmentCode = string.Empty;
            bool? onHold = null;

            #region Get the department code            
            //if (!string.IsNullOrEmpty(_selectedDepartment))
            //{
            //    DepartmentDTO? departmentDTO = _departmentList.Where(d => d.DepartmentName == _selectedDepartment).FirstOrDefault();
            //    if (departmentDTO != null)
            //        _departmentCode = departmentDTO.DepartmentCode;
            //}
            #endregion

            var repoResult = await RecruitmentService.GetRecruitmentBudgetAsync(departmentCode, onHold);
            if (repoResult.Success)
            {
                _budgetList = repoResult.Value!;
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

        private async Task DeleteRecruitmentBudgetAsync(Func<Task> callback, RecruitmentBudgetDTO budget)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (budget.BudgetId == 0)
            {
                errorMsg = "Budget record is not found.";
            }
            else
            {
                var deleteResult = await RecruitmentService.DeleteRecruitmentBudgetAsync(budget.BudgetId, _cts.Token);
                isSuccess = deleteResult.Success;
                if (!isSuccess)
                    errorMsg = deleteResult.Error!;
            }

            if (isSuccess)
            {
                // Show notification
                ShowNotification("The selected budget has been deleted successfully!", NotificationType.Success);
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
