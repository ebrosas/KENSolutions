using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;
using System.Text.Json;

namespace KenHRApp.Web.Components.Pages.UserAccount
{
    public partial class PasswordRecovery
    {
        #region Parameters and Injections
        [Inject] public IAuthenticationService AuthService { get; set; } = default!;
        [Inject] public NavigationManager Nav { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private IAppState State { get; set; } = default!;
        #endregion

        #region Fields
        private EditForm _editForm;
        private EditContext? _editContext;
        private StringBuilder _errorMessage = new StringBuilder();
        private List<string> _validationMessages = new();
        private string overlayMessage = "Please wait...";
        private CancellationTokenSource? _cts;
        protected string ErrorMessage;
        protected MudForm _form;
        protected InputType PasswordInputType = InputType.Password;
        protected string PasswordIcon = Icons.Material.Filled.VisibilityOff;

        #region Flags
        private bool _hasValidationError = false;
        private bool _showErrorAlert = false;
        private bool _disabled = false;
        private bool _btnProcessing = false;
        #endregion

        #region Objects and Collections
        protected ForgotPasswordDTO Model = new();
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
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(Model);
        }
        #endregion

        #region Form Validation
        private void HandleInvalidSubmit(EditContext context)
        {
            _hasValidationError = true;
            _validationMessages = context.GetValidationMessages().ToList();
        }

        private void HandleValidSubmit(EditContext context)
        {
            try
            {
                // If we got here, model is valid
                _hasValidationError = false;
                _validationMessages.Clear();

                // Set flag to display the loading button
                _btnProcessing = true;

                // Set the overlay message
                overlayMessage = "Recovering password, please wait...";

                _ = ForgotPasswordAsync(async () =>
                {
                    // Set flag to hide the loading button
                    _btnProcessing = false;

                    //Nav.NavigateTo("/login", true);

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);
                });
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
        #endregion

        #region Private Methods
        private void ShowNotification(string message, SnackBarTypes type, string position = Defaults.Classes.Position.TopCenter)
        {
            Snackbar.Clear();

            Snackbar.Configuration.PositionClass = position;
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

        protected void GoToLogin()
        {
            Nav.NavigateTo("/login", true);
        }

        protected void GoToSupport()
        {
            Nav.NavigateTo("/UserAccount/Support", true);
        }
        #endregion

        #region Service Methods
        private async Task ForgotPasswordAsync(Func<Task> callback)
        {
            try
            {
                // Wait for 1 second then gives control back to the runtime
                await Task.Delay(500);

                // Reset error messages
                _errorMessage.Clear();

                // Initialize the cancellation token
                _cts = new CancellationTokenSource();

                var repoResult = await AuthService.ForgotPasswordAsync(Model);

                if (repoResult.Success)
                {
                    // Hide error message if any
                    ShowHideError(false);

                    // Show notification
                    ShowNotification("Temporary password has been sent to the registered email successfully!", SnackBarTypes.Success);
                }
                else
                {
                    // Set the error message
                    _errorMessage.AppendLine(repoResult.Error!);
                    ShowHideError(true);
                }

                if (callback != null)
                {
                    // Hide the spinner overlay
                    await callback.Invoke();
                }
            }
            catch (Exception ex)
            {
                // Set the error message
                _errorMessage.Append(ex.Message.ToString());

                ShowHideError(true);
            }
        }
        #endregion
    }
}
