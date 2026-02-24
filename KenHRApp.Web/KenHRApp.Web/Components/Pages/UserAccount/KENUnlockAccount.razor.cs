using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;

namespace KenHRApp.Web.Components.Pages.UserAccount
{
    public partial class KENUnlockAccount
    {
        [Inject] public IAuthenticationService AuthService { get; set; } = default!;
        [Inject] public NavigationManager Nav { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private IAppState State { get; set; } = default!;

        private EditContext? _editContext;
        private StringBuilder _errorMessage = new();
        private List<string> _validationMessages = new();
        private bool _hasValidationError = false;
        private bool _showErrorAlert = false;
        private bool _btnProcessing = false;

        protected UnlockAccountDTO Model = new();

        protected override void OnInitialized()
        {
            _editContext = new EditContext(Model);
        }

        private void HandleInvalidSubmit(EditContext context)
        {
            _hasValidationError = true;
            _validationMessages = context.GetValidationMessages().ToList();
        }

        private void HandleValidSubmit(EditContext context)
        {
            _hasValidationError = false;
            _validationMessages.Clear();
            _btnProcessing = true;

            _ = UnlockAccountAsync(async () =>
            {
                _btnProcessing = false;
                await InvokeAsync(StateHasChanged);
            });
        }

        private async Task UnlockAccountAsync(Func<Task> callback)
        {
            try
            {
                _errorMessage.Clear();

                var repoResult = await AuthService.UnlockAccountAsync(Model);

                if (repoResult.Success)
                {
                    ShowHideError(false);
                    ShowNotification("User Account has been unlocked successfully!", SnackBarTypes.Success);
                }
                else
                {
                    _errorMessage.AppendLine(repoResult.Error!);
                    ShowHideError(true);
                }

                if (callback != null)
                    await callback.Invoke();
            }
            catch (Exception ex)
            {
                _errorMessage.AppendLine(ex.Message);
                ShowHideError(true);
            }
        }

        private void ShowHideError(bool value)
        {
            _showErrorAlert = value;
            if (!value) _errorMessage.Clear();
        }

        private void GoToLogin()
        {
            Nav.NavigateTo("/login", true);
        }

        private void ShowSupport()
        {
            Nav.NavigateTo("/UserAccount/Support", true);
        }

        private enum SnackBarTypes
        {
            Normal,
            Information,
            Success,
            Warning,
            Error
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
                case SnackBarTypes.Information: Snackbar.Add(message, Severity.Info); break;
                case SnackBarTypes.Success: Snackbar.Add(message, Severity.Success); break;
                case SnackBarTypes.Warning: Snackbar.Add(message, Severity.Warning); break;
                case SnackBarTypes.Error: Snackbar.Add(message, Severity.Error); break;
                default: Snackbar.Add(message, Severity.Normal); break;
            }
        }
    }
}