using KenHRApp.Application.DTOs;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace KenHRApp.Web.Components.Shared
{
    public partial class ShiftRosterChangeDialog
    {
        #region Fields
        private MudForm _form = new();
        private string[]? _shiftPatternArray = null;
        private string[]? _changeTypeArray = null;

        private List<ShiftPointerDTO> _shiftPointerList = new List<ShiftPointerDTO>();
        #endregion

        #region Parameters and Injections
        [CascadingParameter]
        private IMudDialogInstance MudDialog { get; set; } = default!;

        [Parameter]
        public ShiftPatternChangeDTO ShiftRosterDetail { get; set; } = new();

        [Parameter]
        public IReadOnlyList<ShiftPatternMasterDTO> ShiftPatternList { get; set; } = new List<ShiftPatternMasterDTO>();

        [Parameter]
        public IReadOnlyList<UserDefinedCodeDTO> ChangeTypeList { get; set; } = new List<UserDefinedCodeDTO>();

        [Parameter]
        public bool IsClearable { get; set; } = true;

        [Parameter]
        public bool IsDisabled { get; set; } = true;

        [Parameter]
        public bool IsEditMode { get; set; }

        // Optional UI helpers
        protected string DialogTitle => IsEditMode ? "Edit Employee Shift Roster" : "Add New Employee Shift Roster";
        protected string DialogIcon => IsEditMode ? Icons.Material.Filled.Edit : Icons.Material.Filled.Add;
        protected Color DialogIconColor => IsEditMode ? Color.Info : Color.Success;
        #endregion

        #region Page Methods
        protected override void OnInitialized()
        {
            if (ShiftPatternList != null)
                _shiftPatternArray = ShiftPatternList.Select(s => s.ShiftPatternCode).OrderBy(d => d).ToArray();

            if (ChangeTypeList != null)
                _changeTypeArray = ChangeTypeList.Select(s => s.UDCCode).OrderBy(d => d).ToArray();
        }
        #endregion

        #region Private Methods
        private async Task Save()
        {
            await _form.Validate();
            if (!_form.IsValid) return;

            MudDialog.Close(DialogResult.Ok(ShiftRosterDetail));
        }

        private void Cancel() => MudDialog.Cancel();

        private async Task<IEnumerable<string>> SearchShiftPattern(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _shiftPatternArray!;
            }

            return _shiftPatternArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchChangeType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _changeTypeArray!;
            }

            return _changeTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion
    }
}
