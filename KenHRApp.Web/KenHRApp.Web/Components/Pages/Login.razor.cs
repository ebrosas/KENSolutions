using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Text.Json;

namespace KenHRApp.Web.Components.Pages
{
    public partial class LoginBase : ComponentBase
    {
        [Inject] public IAuthenticationService AuthService { get; set; }
        [Inject] public NavigationManager Nav { get; set; }

        protected LoginRequestDTO Model = new();
        protected string ErrorMessage;

        protected MudForm _form;

        protected InputType PasswordInputType = InputType.Password;
        protected string PasswordIcon = Icons.Material.Filled.VisibilityOff;

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

        protected async Task HandleLogin()
        {
            await _form.Validate();

            var serviceResult = await AuthService.LoginAsync(Model);
            if (!serviceResult.Success)
            {
                return;
            }

            var result = serviceResult.Value;
            if (result!.IsSuccess)
            {
                if (Model.RememberMe)
                {
                    //await JS.InvokeVoidAsync("localStorage.setItem",
                    //    "login", JsonSerializer.Serialize(Model));
                }

                Nav.NavigateTo("/TimeAttendance/tnadashboard");
            }
            else
            {
                ErrorMessage = result.ErrorMessage;
                //Snackbar.Add(result.ErrorMessage, Severity.Error);
            }
        }

        protected void ShowUnlockDialog()
        {
            // Open MudDialog for unlock
        }

        protected async Task ForgotPassword()
        {
            await AuthService.ForgotPasswordAsync(
                new ForgotPasswordDTO { EmployeeCode = Model.EmployeeCode });

            ErrorMessage = "Temporary password sent to email.";
        }

        protected void GoToSupport()
        {
            Nav.NavigateTo("/support/ticket");
        }
    }
}
