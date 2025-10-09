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
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        #endregion

        #region Fields        
        private EmployeeDTO _employee = new();
        private StringBuilder _errorMessage = new StringBuilder();
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;

        #region Flags
        private bool _showErrorAlert = false;
        private bool _hasValidationError = false;
        private bool _isRunning = false;
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

        #region Collections
        private List<BreadcrumbItem> _breadcrumbItems =
        [
            new("Home", href: "/", icon: Icons.Material.Filled.Home),
            new("Recruitment Management", href: null, disabled: true, @Icons.Material.Outlined.People)
        ];
        #endregion

        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_employee);
            
        }

        protected override void OnParametersSet()
        {
            // Reset error display when navigating to page
            _hasValidationError = false;
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

                _ = SaveChangeAsync(async () =>
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
        #endregion

        #region Database Methods
        private async Task SaveChangeAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }
        #endregion
    }
}
