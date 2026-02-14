using KenHRApp.Application.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace KenHRApp.Web.Components.Pages
{
    public partial class Home
    {
        #region Parameters and Injections
        [Inject] private IAttendanceService AttendanceService { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IAppState AppState { get; set; } = default!;
        #endregion

        #region Fields
        private bool _redirected;
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
                //Navigation.NavigateTo("/TimeAttendance/tnadashboard", replace: true);   //(Notes: replace: true prevents back-button loop)
                //Navigation.NavigateTo("/TimeAttendance/tnadashboard", true);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && !_redirected)
            {
                _redirected = true;
                Navigation.NavigateTo("/TimeAttendance/tnadashboard", replace: true);
            }
        }
        #endregion
    }
}
