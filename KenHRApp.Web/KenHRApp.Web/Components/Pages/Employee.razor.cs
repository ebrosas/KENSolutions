using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.DTOs;
using KenHRApp.Application.Interfaces;
using KenHRApp.Web.Components.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace KenHRApp.Web.Components.Pages
{
    public partial class Employee 
    {
        #region Parameters and Injections
        [Inject] private IEmployeeService EmployeeService { get; set; } = default!;
        [Inject] private IAppCacheService AppCacheService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string DepartmentCacheKey { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery]
        public string EmployeeCacheKey { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery]
        public string EmploymentTypeCacheKey { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery]
        public int EmployeeId { get; set; } = 0;

        [Parameter]
        [SupplyParameterFromQuery]
        public string ActionType { get; set; } = ActionTypes.View.ToString();

        [Parameter]
        public IReadOnlyList<DepartmentDTO> Departments { get; set; } = Array.Empty<DepartmentDTO>();
        #endregion

        #region Fields

        private List<EmployeeDTO> employees = new();
        private EmployeeDTO employee = new();
        private StringBuilder _errorMessage = new StringBuilder();
        private List<string> _notifications = new();
        private string overlayMessage = "Please wait...";
        private string _selectedMenu = string.Empty;
        private string firstName = "";
        private string middleName = "";
        private string lastName = "";
        private string position = "";
        public string HelperText { get; set; } = null!;

        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private CancellationTokenSource? _cts;

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
        #endregion

        #region User Defined Codes
        private List<UserDefinedCodeDTO> _salutationList = new List<UserDefinedCodeDTO>();
        private string[]? _salutationArray = null;

        private List<UserDefinedCodeDTO> _religionList = new List<UserDefinedCodeDTO>();
        private string[]? _religionArray = null;

        private List<UserDefinedCodeDTO> _countryList = new List<UserDefinedCodeDTO>();
        private string[]? _countryArray = null;

        private List<UserDefinedCodeDTO> _genderList = new List<UserDefinedCodeDTO>();
        private string[]? _genderArray = null;

        private List<UserDefinedCodeDTO> _maritalStatusList = new List<UserDefinedCodeDTO>();
        private string[]? _maritalStatusArray = null;

        private List<UserDefinedCodeDTO> _employeeStatusList = new List<UserDefinedCodeDTO>();
        private string[]? _employeeStatusArray = null;

        private List<UserDefinedCodeDTO> _employeeClassList = new List<UserDefinedCodeDTO>();
        private string[]? _employeeClassArray = null;

        private List<UserDefinedCodeDTO> _educationLevelList = new List<UserDefinedCodeDTO>();
        private string[]? _educationLevelArray = null;

        private List<UserDefinedCodeDTO> _accountTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _accountTypeArray = null;

        private List<UserDefinedCodeDTO> _bankList = new List<UserDefinedCodeDTO>();
        private string[]? _bankArray = null;

        private List<UserDefinedCodeDTO> _jobTitleList = new List<UserDefinedCodeDTO>();
        private string[]? _jobTitleArray = null;

        private List<UserDefinedCodeDTO> _payGradeList = new List<UserDefinedCodeDTO>();
        private string[]? _payGradeArray = null;

        private List<UserDefinedCodeDTO> _companyBranchList = new List<UserDefinedCodeDTO>();
        private string[]? _companyBranchArray = null;

        private List<UserDefinedCodeDTO> _visaTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _visaTypeArray = null;

        private List<UserDefinedCodeDTO> _nationalIDTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _nationalIDTypeArray = null;

        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        private string[]? _departmentArray = null;

        private IReadOnlyList<EmployeeDTO> _managerList = new List<EmployeeDTO>();
        private string[]? _managerArray = null;

        private IReadOnlyList<UserDefinedCodeDTO> _employmentTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _employmentTypeArray = null;

        private List<UserDefinedCodeDTO> _attendanceModeList = new List<UserDefinedCodeDTO>();
        private string[]? _attendanceModeArray = null;

        private List<UserDefinedCodeDTO> _roleTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _roleTypeArray = null;

        private List<UserDefinedCodeDTO> _relationTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _relationTypeArray = null;
        #endregion

        #region Enums and Collections
        private enum ActionTypes
        {
            View,
            Edit,
            Add,
            Delete
        }

        private enum UDCGroupCodes
        {
            EMPSTATUS,      // Employee Status
            EMPLOYTYPE,     // Employment Type
            COUNTRY,        // Countries
            RELIGION,       // Religions
            GENDER,         // Gender
            MARSTAT,        // Marital Status
            SALUTE,         // Salutations
            DEPARTMENT,     // Departments
            EMPCLASS,       // Employee Class
            EDUCLEVEL,      // Education LevelDesc
            ACCOUNTTYPE,    // Personal Banking Accounts
            BANKNAME,       // Bank Names
            JOBTITLE,       // Job Titles
            PAYGRADE,       // Pay Grades
            COMPANYBRANCH,  // Company Branches
            VISATYPE,       // Visa Types
            NATIONALITYTYPE,    // Nationality ID Types
            ATTENDANCEMODE,     // Attendance Modes
            ROLETYPES,          // Role Types
            RELATIONTYPE        //Relationship Types
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
            new("Employee Master", href: "/CoreHR/employeesearch", icon: @Icons.Material.Filled.PeopleAlt),
            new("Employee Detail", href: null, disabled: true, @Icons.Material.Filled.EditNote)
        ];
        #endregion

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        #endregion

        #region Grid Fields
        private string _emergencySearchString = string.Empty;
        private bool _emergencyFilter = false;
        #endregion

        #endregion

        #region Page Events
        protected override async Task OnInitializedAsync()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(employee);

            if (ActionType == ActionTypes.Edit.ToString() ||
                ActionType == ActionTypes.View.ToString())
            {
                _isDisabled = true;

                if (EmployeeId > 0)
                    BeginGetEmployeeDetailTask();
            }
            else if (ActionType == ActionTypes.Add.ToString())
            {
                _isDisabled = false;
                _saveBtnEnabled = true;

                #region Get the maximum Employee No. to use
                var maxEmpResult = await EmployeeService.GetMaxEmployeeNoAsync();
                if (maxEmpResult.Success)
                {
                    // Initialize new employee
                    employee = new EmployeeDTO()
                    {
                        EmployeeId = 0,
                        EmployeeNo = maxEmpResult.Value,
                        IsActive = true,
                        HireDate = DateTime.Now,
                        FirstName = string.Empty,
                        LastName = string.Empty,
                        Position = string.Empty
                    };

                    // Reinitialize the EditContext
                    _editContext = new EditContext(employee);
                }
                else
                {
                    // Set the error message
                    _errorMessage.AppendLine(maxEmpResult.Error);
                }
                #endregion

                await PopulateDropDownBoxes();

                if (_errorMessage.Length > 0)
                    ShowHideError(true);
            }
        }

        protected override void OnParametersSet()
        {
            // Reset error display when navigating to page
            _hasValidationError = false;
        }

        // This will be triggered by validation errors automatically
        // Trick: ValidationSummary only shows if there are errors
        // So we set hasErrors = true if any validation fails
        // protected override void OnAfterRender(bool firstRender)
        // {
        //     if (firstRender) return;

        //     var context = new ValidationContext(employee);
        //     var results = new List<ValidationResult>();
        //     _hasValidationError = !Validator.TryValidateObject(employee, context, results, true);
        // }
        #endregion

        #region Grid Events

        #region Emergency Contacts Grid
        private Func<EmergencyContactDTO, bool> _emergencyQuickFilter => x =>
        {
            if (string.IsNullOrWhiteSpace(_emergencySearchString))
                return true;

            if (!string.IsNullOrEmpty(x.ContactPerson) && x.ContactPerson.Contains(_emergencySearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.Relation) && x.Relation.Contains(_emergencySearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.MobileNo) && x.MobileNo!.Contains(_emergencySearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.LandlineNo) && x.LandlineNo!.Contains(_emergencySearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.Address) && x.Address!.Contains(_emergencySearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.CountryDesc) && x.CountryDesc!.Contains(_emergencySearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            if (!string.IsNullOrEmpty(x.City) && x.City!.Contains(_emergencySearchString, StringComparison.OrdinalIgnoreCase))
                return true;

            return false;
        };

        private void EmergencyStartedEditingItem(EmergencyContactDTO item)
        {
            //_events.Insert(0, $"Event = StartedEditingShiftTimingItem, Data = {System.Text.Json.JsonSerializer.Serialize(item)}");
        }

        private void EmergencyCommittedItemChanges(EmergencyContactDTO item)
        {
            try
            {
                if (item == null) return;

                #region Get selected relationship
                if (!string.IsNullOrEmpty(item.RelationCode))
                {
                    UserDefinedCodeDTO? udc = _relationTypeList.Where(d => d.UDCCode == item.RelationCode).FirstOrDefault();
                    if (udc != null)
                        item.Relation = udc.UDCDesc1;
                }
                #endregion

                #region Get selected country
                if (!string.IsNullOrEmpty(item.CountryCode))
                {
                    UserDefinedCodeDTO? udc = _countryList.Where(d => d.UDCCode == item.CountryCode).FirstOrDefault();
                    if (udc != null)
                        item.CountryDesc = udc.UDCDesc1;
                }
                #endregion

                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Saving changes, please wait...";

                _ = SaveEmergencyContactAsync(async () =>
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

        private async Task SaveEmergencyContactAsync(Func<Task> callback, EmergencyContactDTO contactPerson)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            var result = await EmployeeService.SaveEmergencyContactAsync(contactPerson, _cts.Token);
            if (!result.Success)
            {
                // Set the error message
                _errorMessage.AppendLine(result.Error!);
                ShowHideError(true);
            }
            else
            {
                if (contactPerson.AutoId == 0)
                {
                    // Get the new identity seed
                    contactPerson.AutoId = employee.EmergencyContactList.Max(d => d.AutoId) + 1;

                    // Add locally to the list so UI updates immediately
                    employee.EmergencyContactList.Add(contactPerson);

                    StateHasChanged();
                }

                // Show notification
                ShowNotification("Emergency contact saved successfully!", NotificationType.Success);
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task ConfirmDeleteEmergency(EmergencyContactDTO contactPerson)
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", $"Are you sure you want to delete the contact person '{contactPerson.ContactPerson}'?" },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Small,
                Position = DialogPosition.TopCenter,
                CloseOnEscapeKey = true,   // Prevent ESC from closing
                BackdropClick = false       // Prevent clicking outside to close
            };

            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Confirmation", parameters, options);
            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                BeginDeleteEmergencyContact(contactPerson);
            }
        }

        private void BeginDeleteEmergencyContact(EmergencyContactDTO contactPerson)
        {
            try
            {
                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Deleting contact person, please wait...";

                _ = DeleteEmergencyContactAsync(async () =>
                {
                    _isRunning = false;

                    // Hide the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Remove locally from the list so UI updates immediately
                    employee.EmergencyContactList.Remove(contactPerson);

                    StateHasChanged();

                }, contactPerson);
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

        private async Task DeleteEmergencyContactAsync(Func<Task> callback, EmergencyContactDTO contactPerson)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (contactPerson.AutoId == 0)
            {
                errorMsg = "Contact Person ID is not defined.";
            }
            else
            {
                var deleteResult = await EmployeeService.DeleteEmergencyContactAsync(contactPerson.AutoId, _cts.Token);
                isSuccess = deleteResult.Success;
                if (!isSuccess)
                    errorMsg = deleteResult.Error!;
            }

            if (isSuccess)
            {
                // Show notification
                ShowNotification("The selected department has been deleted successfully!", NotificationType.Success);
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

        private async Task AddEmergencyContact()
        {
            try
            {
                var parameters = new DialogParameters
                {
                    ["EmergencyContact"] = new EmergencyContactDTO(),
                    ["RelationTypeList"] = _relationTypeList,
                    ["CountryList"] = _countryList,
                    ["IsClearable"] = true,
                    ["IsDisabled"] = false
                };

                var options = new DialogOptions
                {
                    CloseOnEscapeKey = true,
                    BackdropClick = false,
                    FullWidth = true,
                    MaxWidth = MaxWidth.Large
                };

                // Show the dialog box
                var dialog = await DialogService.ShowAsync<EmergencyContactDialog>("Add New Contact Person", parameters, options);

                var result = await dialog.Result;
                if (result != null && !result.Canceled)
                {
                    var newContact = (EmergencyContactDTO)result.Data!;
                    newContact.AutoId = 0;

                    #region Get the selected relationship type
                    if (!string.IsNullOrEmpty(newContact.Relation))
                    {
                        UserDefinedCodeDTO? udc = _relationTypeList.Where(d => d.UDCDesc1 == newContact.Relation).FirstOrDefault();
                        if (udc != null)
                            newContact.RelationCode = udc.UDCCode;
                    }
                    #endregion

                    #region Get the selected country
                    if (!string.IsNullOrEmpty(newContact.CountryDesc))
                    {
                        UserDefinedCodeDTO? udc = _countryList.Where(d => d.UDCDesc1 == newContact.CountryDesc).FirstOrDefault();
                        if (udc != null)
                            newContact.CountryCode = udc.UDCCode;
                    }
                    #endregion

                    #region Check for duplicate entries
                    var duplicateContact = employee.EmergencyContactList.FirstOrDefault(e => e.ContactPerson.Trim().ToUpper() == newContact.ContactPerson.Trim().ToUpper()
                        && e.RelationCode.Trim().ToUpper() == newContact.RelationCode.Trim().ToUpper()
                        && e.MobileNo.Trim() == newContact.MobileNo.Trim());
                    if (duplicateContact != null)
                    {
                        // Show error
                        await ShowErrorMessage(MessageBoxTypes.Error, "Error", "The specified contact person and relationship already exists. Please enter a different contact name and details.");
                        return;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(MessageBoxTypes.Error, "Error", ex.Message.ToString());
            }
        }
        #endregion

        #endregion

        #region Button Event Handlers
        private void HandleUndoChanges()
        {
            // Set flag to display the loading panel
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Undoing changes, please wait...";

            _ = GetEmployeeDetailAsync(async () =>
            {
                // Reset the flags
                _isEditMode = false;
                _allowGridEdit = false;
                _isDisabled = true;
                _isRunning = false;

                _hasValidationError = false;
                _validationMessages.Clear();

                // Reset error messages
                _errorMessage.Clear();
                ShowHideError(false);

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            });
        }

        private void HandleRefreshPage()
        {
            // NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);

            if (EmployeeId > 0)
            {
                BeginGetEmployeeDetailTask();
            }
        }

        private void CancelAddEmployee()
        {
            Navigation.NavigateTo("/CoreHR/employeesearch");
        }

        private async Task ShowDeleteDialog()
        {
            var parameters = new DialogParameters
            {
                { "DialogTitle", "Confirm Delete"},
                { "DialogIcon", _iconDelete },
                { "ContentText", "Do you really want to delete this record? Note that this process cannot be undone." },
                { "ConfirmText", "Delete" },
                { "Color", Color.Error }
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Delete Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                BeginDeleteEmployee();
            }
        }

        private async Task ShowCancelDialog()
        {
            var parameters = new DialogParameters
        {
            { "DialogTitle", "Confirm Cancel"},
            { "DialogIcon", _iconCancel },
            { "ContentText", "Do you really want to cancel adding new employee record?" },
            { "ConfirmText", "Yes" },
            { "CancelText", "No" },
            { "Color", Color.Success }
        };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, Position = DialogPosition.TopCenter, CloseOnEscapeKey = true };
            var dialog = await DialogService.ShowAsync<ConfirmDialog>("Cancel Confirmation:", parameters, options);

            var result = await dialog.Result;
            if (result != null && !result.Canceled)
            {
                CancelAddEmployee();
            }
        }
        #endregion

        #region Async Events
        private void BeginGetEmployeeDetailTask()
        {
            _isRunning = true;

            // Set the overlay message
            overlayMessage = "Loading employee details, please wait...";

            _ = GetEmployeeDetailAsync(async () =>
            {
                _isRunning = false;

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            });
        }

        private async Task GetEmployeeDetailAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(1000);

            // Reset error messages
            _errorMessage.Clear();

            var result = await EmployeeService.GetEmployeeDetailAsync(EmployeeId);
            if (result.Success)
            {
                employee = result.Value!;

                // Recreate the EditContext with the loaded employee
                _editContext = new EditContext(employee);
            }
            else
            {
                // Set the error message
                _errorMessage.Append(result.Error);

                ShowHideError(true);
            }

            #region Populate datagrid for testing purposes
            employee.EmployeeTransactionList = await EmployeeService.GetEmployeeTransactionAsync();
            #endregion

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task SaveChangeAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            #region Get the combobox selected values
            UserDefinedCodeDTO? udc = null;

            if (!string.IsNullOrEmpty(employee.ReportingManager))
            {
                EmployeeDTO? selectedManager = _managerList.Where(a => a.EmployeeFullName == employee.ReportingManager).FirstOrDefault();
                if (selectedManager != null)
                    employee.ReportingManagerCode = selectedManager.EmployeeNo;
            }

            if (!string.IsNullOrEmpty(employee.SecondReportingManager))
            {
                EmployeeDTO? selectedManager = _managerList.Where(a => a.EmployeeFullName == employee.SecondReportingManager).FirstOrDefault();
                if (selectedManager != null)
                    employee.SecondReportingManagerCode = selectedManager.EmployeeNo;
            }

            if (!string.IsNullOrEmpty(employee.DepartmentName))
            {
                DepartmentDTO? selectedDepartment = _departmentList.Where(a => a.DepartmentName == employee.DepartmentName).FirstOrDefault();
                if (selectedDepartment != null)
                    employee.DepartmentCode = selectedDepartment.DepartmentCode;
            }

            if (!string.IsNullOrEmpty(employee.NationalityDesc))
            {
                udc = _countryList.Where(a => a.UDCDesc1 == employee.NationalityDesc).FirstOrDefault();
                if (udc != null)
                    employee.NationalityCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.ReligionDesc))
            {
                udc = _religionList.Where(a => a.UDCDesc1 == employee.ReligionDesc).FirstOrDefault();
                if (udc != null)
                    employee.ReligionCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.GenderDesc))
            {
                udc = _genderList.Where(a => a.UDCDesc1 == employee.GenderDesc).FirstOrDefault();
                if (udc != null)
                    employee.GenderCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.MaritalStatusDesc))
            {
                udc = _maritalStatusList.Where(a => a.UDCDesc1 == employee.MaritalStatusDesc).FirstOrDefault();
                if (udc != null)
                    employee.MaritalStatusCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.SalutationDesc))
            {
                udc = _salutationList.Where(a => a.UDCDesc1 == employee.SalutationDesc).FirstOrDefault();
                if (udc != null)
                    employee.Salutation = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.EmployeeStatusDesc))
            {
                udc = _employeeStatusList.Where(a => a.UDCDesc1 == employee.EmployeeStatusDesc).FirstOrDefault();
                if (udc != null)
                    employee.EmployeeStatusCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.EmployeeStatusDesc))
            {
                udc = _employeeStatusList.Where(a => a.UDCDesc1 == employee.EmployeeStatusDesc).FirstOrDefault();
                if (udc != null)
                    employee.EmployeeStatusCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.EmploymentType))
            {
                udc = _employmentTypeList.Where(a => a.UDCDesc1 == employee.EmploymentType).FirstOrDefault();
                if (udc != null)
                    employee.EmploymentTypeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.RoleType))
            {
                udc = _roleTypeList.Where(a => a.UDCDesc1 == employee.RoleType).FirstOrDefault();
                if (udc != null)
                    employee.RoleCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.FirstAttendanceMode))
            {
                udc = _attendanceModeList.Where(a => a.UDCDesc1 == employee.FirstAttendanceMode).FirstOrDefault();
                if (udc != null)
                    employee.FirstAttendanceModeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.SecondAttendanceMode))
            {
                udc = _attendanceModeList.Where(a => a.UDCDesc1 == employee.SecondAttendanceMode).FirstOrDefault();
                if (udc != null)
                    employee.SecondAttendanceModeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.ThirdAttendanceMode))
            {
                udc = _attendanceModeList.Where(a => a.UDCDesc1 == employee.ThirdAttendanceMode).FirstOrDefault();
                if (udc != null)
                    employee.ThirdAttendanceModeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.CompanyBranchDesc))
            {
                udc = _companyBranchList.Where(a => a.UDCDesc1 == employee.CompanyBranchDesc).FirstOrDefault();
                if (udc != null)
                    employee.CompanyBranch = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.EducationDesc))
            {
                udc = _educationLevelList.Where(a => a.UDCDesc1 == employee.EducationDesc).FirstOrDefault();
                if (udc != null)
                    employee.EducationCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.EmployeeClassDesc))
            {
                udc = _employeeClassList.Where(a => a.UDCDesc1 == employee.EmployeeClassDesc).FirstOrDefault();
                if (udc != null)
                    employee.EmployeeClassCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.JobTitleDesc))
            {
                udc = _jobTitleList.Where(a => a.UDCDesc1 == employee.JobTitleDesc).FirstOrDefault();
                if (udc != null)
                    employee.JobTitleCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.PayGradeDesc))
            {
                udc = _payGradeList.Where(a => a.UDCDesc1 == employee.PayGradeDesc).FirstOrDefault();
                if (udc != null)
                    employee.PayGrade = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.AccountTypeDesc))
            {
                udc = _accountTypeList.Where(a => a.UDCDesc1 == employee.AccountTypeDesc).FirstOrDefault();
                if (udc != null)
                    employee.AccountTypeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.BankName))
            {
                udc = _bankList.Where(a => a.UDCDesc1 == employee.BankName).FirstOrDefault();
                if (udc != null)
                    employee.BankNameCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.PresentCountryDesc))
            {
                udc = _countryList.Where(a => a.UDCDesc1 == employee.PresentCountryDesc).FirstOrDefault();
                if (udc != null)
                    employee.PresentCountryCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(employee.PermanentCountryDesc))
            {
                udc = _countryList.Where(a => a.UDCDesc1 == employee.PermanentCountryDesc).FirstOrDefault();
                if (udc != null)
                    employee.PermanentCountryCode = udc.UDCCode;
            }

            #region Identity Proof dropdowns
            if (employee.EmpIdentityProof != null)
            {
                if (!string.IsNullOrEmpty(employee.EmpIdentityProof.NationalIDTypeDesc))
                {
                    udc = _countryList.Where(a => a.UDCDesc1 == employee.EmpIdentityProof.NationalIDTypeDesc).FirstOrDefault();
                    if (udc != null)
                        employee.EmpIdentityProof.NationalIDTypeCode = udc.UDCCode;
                }

                if (!string.IsNullOrEmpty(employee.EmpIdentityProof.VisaTypeDesc))
                {
                    udc = _visaTypeList.Where(a => a.UDCDesc1 == employee.EmpIdentityProof.VisaTypeDesc).FirstOrDefault();
                    if (udc != null)
                        employee.EmpIdentityProof.VisaTypeCode = udc.UDCCode;
                }

                if (!string.IsNullOrEmpty(employee.EmpIdentityProof.VisaCountryDesc))
                {
                    udc = _countryList.Where(a => a.UDCDesc1 == employee.EmpIdentityProof.VisaCountryDesc).FirstOrDefault();
                    if (udc != null)
                        employee.EmpIdentityProof.VisaCountryCode = udc.UDCCode;
                }
            }
            #endregion

            #endregion

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (employee.EmployeeId == 0)
            {
                var addResult = await EmployeeService.AddEmployeeAsync(employee, _cts.Token);
                isSuccess = addResult.Success;
                if (!isSuccess)
                    errorMsg = addResult.Error!;
                else
                {
                    // Set flag to enable reload of employees when navigating back to the Employe Search page
                    _forceLoad = true;
                }
            }
            else
            {
                var saveResult = await EmployeeService.SaveEmployeeAsync(employee, _cts.Token);
                isSuccess = saveResult.Success;
                if (!isSuccess)
                    errorMsg = saveResult.Error!;
            }

            if (isSuccess)
            {
                // Reset flags
                _isEditMode = false;
                _allowGridEdit = false;
                _saveBtnEnabled = false;
                _isDisabled = true;

                // Show notification
                ShowNotification("Employee data saved successfully!", NotificationType.Success);
            }
            else
            {
                // Set the error message
                _errorMessage.AppendLine(errorMsg);
                ShowHideError(true);
            }

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task DeleteEmployeeAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (employee.EmployeeId == 0)
            {
                errorMsg = "Employee ID is not defined.";
            }
            else
            {
                var deleteResult = await EmployeeService.DeleteEmployeeAsync(employee.EmployeeId, _cts.Token);
                isSuccess = deleteResult.Success;
                if (!isSuccess)
                    errorMsg = deleteResult.Error!;
            }

            if (isSuccess)
            {
                // Show notification
                ShowNotification("Employee record has been deleted successfully!", NotificationType.Success);
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

        #region Initialize Collections
        private readonly string[] _employeeMenu =
        {
        "About Me", "Address", "Identity Proofs", "Skills & Qualifications", "Family",
        "Employment History", "Documents"
    };
        #endregion

        #region Data Validation Methods
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

        private void BeginDeleteEmployee()
        {
            try
            {
                // Set flag to display the loading panel
                _isRunning = true;

                // Set the overlay message
                overlayMessage = "Deleting employee record, please wait...";

                _ = DeleteEmployeeAsync(async () =>
                {
                    _isRunning = false;

                    // Shows the spinner overlay
                    await InvokeAsync(StateHasChanged);

                    // Go back to the Employee Search page
                    Navigation.NavigateTo($"/CoreHR/employeesearch?ForceLoad={true.ToString()}");
                });
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

        private async Task AddEmployee()
        {
            await EmployeeService.AddAsync(new()
            {
                FirstName = firstName,
                MiddleName = middleName,
                LastName = lastName,
                Position = position,
                NationalityCode = "Filipino",
                ReligionCode = "Born Again",
                GenderCode = "Male",
                MaritalStatusCode = "Married",
                OfficialEmail = "ervin.brosas@yahoo.com",
                EmployeeNo = 10003632
            });

            employees = await EmployeeService.GetAllAsync();
            firstName = middleName = lastName = position = string.Empty;

        }

        private async Task HandleEditEmployee()
        {
            try
            {
                _isRunning = true;
                overlayMessage = "Entering edit mode, please wait...";
                StateHasChanged(); // immediate render

                // do your async work
                await Task.Delay(1000);

                // Set the flags
                _isEditMode = true;
                _allowGridEdit = true;
                _saveBtnEnabled = true;
                _isDisabled = false;                

                // Hide error message if any
                ShowHideError(false);

                #region Get all UDC group codes
                List<UserDefinedCodeGroupDTO>? udcGroupList = new();
                int groupID = 0;

                var resultUDC = await EmployeeService.GetUserDefinedCodeGroupAsync();
                if (resultUDC.Success)
                {
                    udcGroupList = resultUDC!.Value;
                }
                else
                    _errorMessage.Append(resultUDC.Error);
                #endregion

                #region Get all User-defined codes
                var result = await EmployeeService.GetUserDefinedCodeAllAsync();
                if (result.Success)
                {
                    var udcData = result.Value;
                    if (udcData != null && udcData.Count > 0 && udcGroupList != null)
                    {
                        #region Populate Salutation dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.SALUTE.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting salutation group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _salutationList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_salutationList != null)
                                _salutationArray = _salutationList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Nationality dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.COUNTRY.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting nationality group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _countryList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_countryList != null)
                                _countryArray = _countryList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Religion dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.RELIGION.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting religion group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _religionList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_religionList != null)
                                _religionArray = _religionList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Marital Status dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.MARSTAT.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting marital status group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _maritalStatusList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_maritalStatusList != null)
                                _maritalStatusArray = _maritalStatusList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Gender dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.GENDER.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting gender group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _genderList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_genderList != null)
                                _genderArray = _genderList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Employee Status dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.EMPSTATUS.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting employee status group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _employeeStatusList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_employeeStatusList != null)
                                _employeeStatusArray = _employeeStatusList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Employee Class dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.EMPCLASS.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting employee status group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _employeeClassList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_employeeClassList != null)
                                _employeeClassArray = _employeeClassList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Education Level dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.EDUCLEVEL.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting education level group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _educationLevelList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_educationLevelList != null)
                                _educationLevelArray = _educationLevelList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Bank Account Type dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.ACCOUNTTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting account types group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _accountTypeList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_accountTypeList != null)
                                _accountTypeArray = _accountTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Bank Names dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.BANKNAME.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting bank names group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _bankList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_bankList != null)
                                _bankArray = _bankList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Job Titles dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.JOBTITLE.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting job title group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _jobTitleList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_jobTitleList != null)
                                _jobTitleArray = _jobTitleList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Pay Grades dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.PAYGRADE.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting pay grade group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _payGradeList = udcData.Where(a => a.GroupID == groupID).OrderBy(a => a.SequenceNo).ToList();
                            if (_payGradeList != null)
                                _payGradeArray = _payGradeList.Select(s => s.UDCDesc1).ToArray();
                        }
                        #endregion

                        #region Populate Company Branches dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.COMPANYBRANCH.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting company branch group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _companyBranchList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_companyBranchList != null)
                                _companyBranchArray = _companyBranchList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Visa Type dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.VISATYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting visa types group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _visaTypeList = udcData.Where(a => a.GroupID == groupID).ToList();
                            if (_visaTypeList != null)
                                _visaTypeArray = _visaTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                        }
                        #endregion

                        #region Populate Department dropdown
                        bool isDepartmentExist = false;
                        if (!string.IsNullOrEmpty(DepartmentCacheKey))
                        {
                            isDepartmentExist = await AppCacheService.CheckIfKeyExistAsync(DepartmentCacheKey);

                        }
                        if (isDepartmentExist)
                        {
                            _departmentList = await AppCacheService.GetDepartmentsAsync(DepartmentCacheKey);
                        }
                        else
                        {
                            // Get department frrom DB
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
                        }

                        if (_departmentList != null && _departmentList.Count > 0)
                            _departmentArray = _departmentList.Select(d => d.DepartmentName).OrderBy(d => d).ToArray();
                        #endregion

                        #region Populate Reporting Manager dropdown
                        bool isEmployeeExist = false;
                        if (!string.IsNullOrEmpty(EmployeeCacheKey))
                        {
                            isEmployeeExist = await AppCacheService.CheckIfKeyExistAsync(EmployeeCacheKey);

                        }
                        if (isEmployeeExist)
                        {
                            _managerList = await AppCacheService.GetEmployeesAsync(EmployeeCacheKey);
                        }
                        else
                        {
                            // Get reporting managers from DB
                            var managerResult = await LookupCache.GetReportingManagerAsync();
                            if (managerResult.Success)
                            {
                                _managerList = managerResult.Value!;
                            }
                            else
                            {
                                // Set the error message
                                _errorMessage.AppendLine(managerResult.Error);
                            }
                        }

                        if (_managerList != null && _managerList.Count > 0)
                            _managerArray = _managerList.Select(d => d.EmployeeFullName).OrderBy(d => d).ToArray();
                        #endregion

                        #region Populate Employment Type dropdown
                        // Get employent types from DB
                        var employTypeResult = await LookupCache.GetEmploymentTypeAsync();
                        if (employTypeResult.Success)
                        {
                            _employmentTypeList = employTypeResult.Value!;
                        }
                        else
                        {
                            // Set the error message
                            _errorMessage.AppendLine(employTypeResult.Error);
                        }

                        if (_employmentTypeList != null && _employmentTypeList.Count > 0)
                            _employmentTypeArray = _employmentTypeList.Select(d => d.UDCDesc1).OrderBy(d => d).ToArray();
                        #endregion

                        #region Populate Attendance Modes dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.ATTENDANCEMODE.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting attendance modes group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _attendanceModeList = udcData.Where(a => a.GroupID == groupID).OrderBy(a => a.SequenceNo).ToList();
                            if (_attendanceModeList != null)
                                _attendanceModeArray = _attendanceModeList.Select(d => d.UDCDesc1).OrderBy(d => d).ToArray();
                        }
                        #endregion

                        #region Populate Role Types dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.ROLETYPES.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting Role Types group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _roleTypeList = udcData.Where(a => a.GroupID == groupID).OrderBy(a => a.SequenceNo).ToList();
                            if (_roleTypeList != null)
                                _roleTypeArray = _roleTypeList.Select(d => d.UDCDesc1).OrderBy(d => d).ToArray();
                        }
                        #endregion

                        #region Populate Relationship Type dropdown
                        try
                        {
                            groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.RELATIONTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                        }
                        catch (Exception ex)
                        {
                            _errorMessage.Append($"Error getting Relationship Types group ID: {ex.Message}");
                        }

                        if (groupID > 0)
                        {
                            _relationTypeList = udcData.Where(a => a.GroupID == groupID).OrderBy(a => a.UDCDesc1).ToList();
                            if (_relationTypeList != null)
                                _relationTypeArray = _relationTypeList.Select(d => d.UDCDesc1).OrderBy(d => d).ToArray();
                        }
                        #endregion
                    }
                }
                else
                    _errorMessage.Append(result.Error);

                #endregion
            }
            catch (Exception ex)
            {
                overlayMessage = $"Error: {ex.Message}";
            }
            finally
            {
                _isRunning = false;     // ✅ must execute
                StateHasChanged();      // ✅ must re-render
            }
        }

        private async Task PopulateDropDownBoxes()
        {
            #region Get all UDC group codes
            List<UserDefinedCodeGroupDTO>? udcGroupList = new();
            int groupID = 0;

            var resultUDC = await EmployeeService.GetUserDefinedCodeGroupAsync();
            if (resultUDC.Success)
            {
                udcGroupList = resultUDC!.Value;
            }
            else
                _errorMessage.Append(resultUDC.Error);
            #endregion

            #region Get all User-defined codes
            var result = await EmployeeService.GetUserDefinedCodeAllAsync();
            if (result.Success)
            {
                var udcData = result.Value;
                if (udcData != null && udcData.Count > 0 && udcGroupList != null)
                {
                    #region Populate Salutation dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.SALUTE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting salutation group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _salutationList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_salutationList != null)
                            _salutationArray = _salutationList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Nationality dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.COUNTRY.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting nationality group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _countryList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_countryList != null)
                            _countryArray = _countryList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Religion dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.RELIGION.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting religion group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _religionList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_religionList != null)
                            _religionArray = _religionList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Marital Status dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.MARSTAT.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting marital status group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _maritalStatusList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_maritalStatusList != null)
                            _maritalStatusArray = _maritalStatusList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Gender dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.GENDER.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting gender group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _genderList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_genderList != null)
                            _genderArray = _genderList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Employee Status dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.EMPSTATUS.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting employee status group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _employeeStatusList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_employeeStatusList != null)
                            _employeeStatusArray = _employeeStatusList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Employee Class dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.EMPCLASS.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting employee status group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _employeeClassList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_employeeClassList != null)
                            _employeeClassArray = _employeeClassList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Education Level dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.EDUCLEVEL.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting education level group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _educationLevelList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_educationLevelList != null)
                            _educationLevelArray = _educationLevelList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Bank Account Type dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.ACCOUNTTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting account types group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _accountTypeList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_accountTypeList != null)
                            _accountTypeArray = _accountTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Bank Names dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.BANKNAME.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting bank names group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _bankList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_bankList != null)
                            _bankArray = _bankList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Job Titles dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.JOBTITLE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting job title group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _jobTitleList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_jobTitleList != null)
                            _jobTitleArray = _jobTitleList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Pay Grades dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.PAYGRADE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting pay grade group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _payGradeList = udcData.Where(a => a.GroupID == groupID).OrderBy(a => a.SequenceNo).ToList();
                        if (_payGradeList != null)
                            _payGradeArray = _payGradeList.Select(s => s.UDCDesc1).ToArray();
                    }
                    #endregion

                    #region Populate Company Branches dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.COMPANYBRANCH.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting company branch group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _companyBranchList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_companyBranchList != null)
                            _companyBranchArray = _companyBranchList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Visa Type dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.VISATYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting visa types group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _visaTypeList = udcData.Where(a => a.GroupID == groupID).ToList();
                        if (_visaTypeList != null)
                            _visaTypeArray = _visaTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Populate Department dropdown
                    bool isDepartmentExist = false;
                    if (!string.IsNullOrEmpty(DepartmentCacheKey))
                    {
                        isDepartmentExist = await AppCacheService.CheckIfKeyExistAsync(DepartmentCacheKey);

                    }
                    if (isDepartmentExist)
                    {
                        _departmentList = await AppCacheService.GetDepartmentsAsync(DepartmentCacheKey);
                    }
                    else
                    {
                        // Get department frrom DB
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
                    }

                    if (_departmentList != null && _departmentList.Count > 0)
                        _departmentArray = _departmentList.Select(d => d.DepartmentName).OrderBy(d => d).ToArray();
                    #endregion

                    #region Populate Reporting Manager dropdown
                    bool isEmployeeExist = false;
                    if (!string.IsNullOrEmpty(EmployeeCacheKey))
                    {
                        isEmployeeExist = await AppCacheService.CheckIfKeyExistAsync(EmployeeCacheKey);

                    }
                    if (isEmployeeExist)
                    {
                        _managerList = await AppCacheService.GetEmployeesAsync(EmployeeCacheKey);
                    }
                    else
                    {
                        // Get reporting managers from DB
                        var managerResult = await LookupCache.GetReportingManagerAsync();
                        if (managerResult.Success)
                        {
                            _managerList = managerResult.Value!;
                        }
                        else
                        {
                            // Set the error message
                            _errorMessage.AppendLine(managerResult.Error);
                        }
                    }

                    if (_managerList != null && _managerList.Count > 0)
                        _managerArray = _managerList.Select(d => d.EmployeeFullName).OrderBy(d => d).ToArray();
                    #endregion

                    #region Populate Employment Type dropdown
                    // Get employent types from DB
                    var employTypeResult = await LookupCache.GetEmploymentTypeAsync();
                    if (employTypeResult.Success)
                    {
                        _employmentTypeList = employTypeResult.Value!;
                    }
                    else
                    {
                        // Set the error message
                        _errorMessage.AppendLine(employTypeResult.Error);
                    }

                    if (_employmentTypeList != null && _employmentTypeList.Count > 0)
                        _employmentTypeArray = _employmentTypeList.Select(d => d.UDCDesc1).OrderBy(d => d).ToArray();
                    #endregion

                    #region Populate Attendance Modes dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.ATTENDANCEMODE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting attendance modes group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _attendanceModeList = udcData.Where(a => a.GroupID == groupID).OrderBy(a => a.SequenceNo).ToList();
                        if (_attendanceModeList != null)
                            _attendanceModeArray = _attendanceModeList.Select(d => d.UDCDesc1).OrderBy(d => d).ToArray();
                    }
                    #endregion

                    #region Populate Role Types dropdown
                    try
                    {
                        groupID = udcGroupList.Where(a => a.UDCGCode == UDCGroupCodes.ROLETYPES.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Role Types group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _roleTypeList = udcData.Where(a => a.GroupID == groupID).OrderBy(a => a.SequenceNo).ToList();
                        if (_roleTypeList != null)
                            _roleTypeArray = _roleTypeList.Select(d => d.UDCDesc1).OrderBy(d => d).ToArray();
                    }
                    #endregion
                }
            }
            else
                _errorMessage.Append(result.Error);

            #endregion
        }

        private async Task EditEmployeeAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(1000);

            // Set flags
            _isEditMode = true;
            _allowGridEdit = true;
            _isDisabled = false;            

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task<IEnumerable<string>> SearchSalutation(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _salutationArray!;
            }

            return _salutationArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchReligion(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _religionArray!;
            }

            return _religionArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchCountry(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _countryArray!;
            }

            return _countryArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchGender(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _genderArray!;
            }

            return _genderArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchMaritalStatus(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _maritalStatusArray!;
            }

            return _maritalStatusArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchEmployeeStatus(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _employeeStatusArray!;
            }

            return _employeeStatusArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchEmployeeClass(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _employeeClassArray!;
            }

            return _employeeClassArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchEducationLevel(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _educationLevelArray!;
            }

            return _educationLevelArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchAccountType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _accountTypeArray!;
            }

            return _accountTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchBankName(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _bankArray!;
            }

            return _bankArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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

        private async Task<IEnumerable<string>> SearchReportingManager(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _managerArray!;
            }

            return _managerArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchJobTitle(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _jobTitleArray!;
            }

            return _jobTitleArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchPayGrade(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _payGradeArray!;
            }

            return _payGradeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchCompanyBranch(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _companyBranchArray!;
            }

            return _companyBranchArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchVisaType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _visaTypeArray!;
            }

            return _visaTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchNationalIDType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _nationalIDTypeArray!;
            }

            return _nationalIDTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

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

        private async Task<IEnumerable<string>> SearchAttendanceMode(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _attendanceModeArray!;
            }

            return _attendanceModeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchRoleType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _roleTypeArray!;
            }

            return _roleTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchRelationship(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _relationTypeArray!;
            }

            return _relationTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion
    }
}
