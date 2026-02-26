using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;
using System.Text.Json;
using KenHRApp.Web.Components.Shared;

namespace KenHRApp.Web.Components.Pages.UserAccount
{
    public partial class AccountRegistration
    {
        #region Parameters and Injections
        [Inject] public IAuthenticationService AuthService { get; set; } = default!;
        [Inject] public NavigationManager Nav { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private IAppState State { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
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
        protected InputType RetypePasswordInputType = InputType.Password;
        protected string PasswordIcon = Icons.Material.Filled.VisibilityOff;
        protected string RetypePasswordIcon = Icons.Material.Filled.VisibilityOff;

        #region Flags
        private bool _hasValidationError = false;
        private bool _showErrorAlert = false;
        private bool _disabled = false;
        private bool _btnProcessing = false;
        #endregion

        #region Objects and Collections
        protected UserAccountDTO Model = new();
        #endregion

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
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

                _ = RegisterUserAccountAsync(async () =>
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

        protected void TogglePassword()
        {
            if (PasswordInputType == InputType.Password)
            {
                PasswordInputType = InputType.Text;
                PasswordIcon = Icons.Material.Filled.Visibility;
            }
            else
            {
                PasswordInputType = InputType.Password;
                PasswordIcon = Icons.Material.Filled.VisibilityOff;
            }
        }

        protected void ToggleRetypePassword()
        {
            if (RetypePasswordInputType == InputType.Password)
            {
                RetypePasswordInputType = InputType.Text;
                RetypePasswordIcon = Icons.Material.Filled.Visibility;
            }
            else
            {
                RetypePasswordInputType = InputType.Password;
                RetypePasswordIcon = Icons.Material.Filled.VisibilityOff;
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

        private async Task ShowSuccessDialog()
        {
            var parameters = new DialogParameters
            {
                { "Message", "Your account has been successfully registered." }
            };

            var options = new DialogOptions
            {
                CloseOnEscapeKey = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true
            };

            //var dialog = DialogService.ShowAsync<RegistrationSuccessDialog>(
            //    "Registration Successful",
            //    parameters,
            //    options);

            var dialog = await DialogService.ShowAsync<InfoDialog>("Registration Successful", parameters, options);
            var result = await dialog.Result;

            if (!result!.Canceled)
            {
                Nav.NavigateTo("/login", true);
            }
        }
        #endregion

        #region Service Methods
        private async Task RegisterUserAccountAsync(Func<Task> callback)
        {
            try
            {
                // Wait for 1 second then gives control back to the runtime
                await Task.Delay(300);

                // Reset error messages
                _errorMessage.Clear();

                // Initialize the cancellation token
                _cts = new CancellationTokenSource();

                var repoResult = await AuthService.RegisterUserAccountAsync(Model);

                if (repoResult.Success)
                {
                    // Hide error message if any
                    ShowHideError(false);

                    // Show notification
                    //ShowNotification("User account has benn registered successfully!", SnackBarTypes.Success);
                    await ShowSuccessDialog();
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
