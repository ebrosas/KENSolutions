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
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace KenHRApp.Web.Components.Pages.Recruitment
{
    public partial class RecruitmentRequest
    {
        #region Parameters and Injections
        [Inject] private IRecruitmentService RecruitmentService { get; set; } = default!;
        [Inject] private IEmployeeService EmployeeService { get; set; } = default!;
        [Inject] private IAppCacheService AppCacheService { get; set; } = default!;
        [Inject] private ISnackbar Snackbar { get; set; } = default!;
        [Inject] private ILookupCacheService LookupCache { get; set; } = default!;
        [Inject] private NavigationManager Navigation { get; set; } = default!;
        [Inject] private IDialogService DialogService { get; set; } = default!;
        [Inject] private IAppState AppState { get; set; } = default!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string DepartmentName { get; set; } = string.Empty;

        [Parameter]
        [SupplyParameterFromQuery]
        public string ActionType { get; set; } = ActionTypes.View.ToString();
        #endregion

        #region Fields
        private RecruitmentRequestDTO _recruitmentRequest = new();
        private EditContext? _editContext;
        private List<string> _validationMessages = new();
        private CancellationTokenSource? _cts;
        private string overlayMessage = "Please wait...";
        private StringBuilder _errorMessage = new StringBuilder();
        private string _searchStringQualification = string.Empty;        
        private readonly List<string> _skillChips = new();
        private int _numberOfColors = Enum.GetValues(typeof(Color)).Length;
        private int _skillChipCounter = 0;
        private string _skillName = string.Empty;  

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

        private enum UDCKeys
        {
            EMPLOYTYPE,             // Employment Type
            QUALIFACTIONMODE,       // Qualification Modes
            QUALIFACTIONTYPE,       // Qualification Types
            POSITIONTYPE,           // Position Types
            INTERVIEWWF,            // Interview Process
            DEPARTMENT,             // Departments
            COUNTRY,                // Countries
            EDUCLEVEL,              // Education Levels
            EMPCLASS,               // Employee Class
            JOBTITLE,               // Job Titles
            PAYGRADE,               // Pay Grades
            ETHNICTYPE,             // Ethnicity Types
            STREAMTYPE,             // Stream Types
            SPECIALIZATION,         // Specialization Types
            CUSTOMERLIST            // Customer List
        }

        #region Dialog Box Button Icons
        private readonly string _iconDelete = "fas fa-trash-alt";
        private readonly string _iconCancel = "fas fa-window-close";
        private readonly string _iconError = "fas fa-times-circle";
        private readonly string _iconInfo = "fas fa-info-circle";
        private readonly string _iconWarning = "fas fa-exclamation-circle";
        private readonly string _iconAdd = "fas fa-plus-circle";
        #endregion

        #region Enums 
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
        string[] ageRangeLabels = new string[] { "18", "32", "47", "61", "75" };

        private List<UserDefinedCodeDTO> _employmentTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _employmentTypeArray = null;
        private List<UserDefinedCodeDTO> _qualificationModeList = new List<UserDefinedCodeDTO>();
        private string[]? _qualificationModeArray = null;
        private List<UserDefinedCodeDTO> _positionTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _positionTypeArray = null;
        private List<UserDefinedCodeDTO> _interviewProcessList = new List<UserDefinedCodeDTO>();
        private string[]? _interviewProcessArray = null;
        private IReadOnlyList<DepartmentDTO> _departmentList = new List<DepartmentDTO>();
        private string[]? _departmentArray = null;
        private List<UserDefinedCodeDTO> _countryList = new List<UserDefinedCodeDTO>();
        private string[]? _countryArray = null;
        private List<UserDefinedCodeDTO> _educationLevelList = new List<UserDefinedCodeDTO>();
        private string[]? _educationLevelArray = null;
        private List<UserDefinedCodeDTO> _employeeClassList = new List<UserDefinedCodeDTO>();
        private string[]? _employeeClassArray = null;
        private List<UserDefinedCodeDTO> _jobTitleList = new List<UserDefinedCodeDTO>();
        private string[]? _jobTitleArray = null;
        private List<UserDefinedCodeDTO> _payGradeList = new List<UserDefinedCodeDTO>();
        private string[]? _payGradeArray = null;
        private List<UserDefinedCodeDTO> _ethnicityTypeList = new List<UserDefinedCodeDTO>();
        private string[]? _ethnicityTypeArray = null;
        private List<UserDefinedCodeDTO> _customerList = new List<UserDefinedCodeDTO>();
        private string[]? _customerArray = null;
        private List<UserDefinedCodeDTO> _qualificationTypeList = new List<UserDefinedCodeDTO>();
        private List<UserDefinedCodeDTO> _streamTypeList = new List<UserDefinedCodeDTO>();
        private List<UserDefinedCodeDTO> _specializationList = new List<UserDefinedCodeDTO>();
        #endregion

        #endregion

        #region Properties
        private string SalaryRangeType
        {
            get => _recruitmentRequest.SalaryRangeType;
            set
            {
                if (_recruitmentRequest.SalaryRangeType == value) return;
                _recruitmentRequest.SalaryRangeType = value;
                UpdateSliderStates(value);
                StateHasChanged();
            }
        }
        #endregion

        #region Page Events
        protected override void OnInitialized()
        {
            // Initialize the EditContext 
            _editContext = new EditContext(_recruitmentRequest);

            if (ActionType == ActionTypes.Edit.ToString() ||
                ActionType == ActionTypes.View.ToString())
            {
                _isDisabled = true;

                _recruitmentRequest = AppState.RecruitmentRequest!;
            }
            else if (ActionType == ActionTypes.Add.ToString())
            {
                _isDisabled = false;
                _saveBtnEnabled = true;

                if (AppState.RecruitmentRequest != null)
                {
                    #region Initialize recruitment request based on the AppState
                    _recruitmentRequest.DepartmentCode = AppState.RecruitmentRequest.DepartmentCode;
                    _recruitmentRequest.DepartmentName = AppState.RecruitmentRequest.DepartmentName;
                    _recruitmentRequest.EmploymentTypeCode = AppState.RecruitmentRequest.EmploymentTypeCode;
                    _recruitmentRequest.EmploymentType = AppState.RecruitmentRequest.EmploymentType;
                    _recruitmentRequest.QualificationModeCode = AppState.RecruitmentRequest.QualificationModeCode;
                    _recruitmentRequest.QualificationMode = AppState.RecruitmentRequest.QualificationMode;
                    _recruitmentRequest.PositionTypeCode = AppState.RecruitmentRequest.PositionTypeCode;
                    _recruitmentRequest.PositionType = AppState.RecruitmentRequest.PositionType;
                    _recruitmentRequest.InterviewProcessCode = AppState.RecruitmentRequest.InterviewProcessCode;
                    _recruitmentRequest.InterviewProcess = AppState.RecruitmentRequest.InterviewProcess;
                    _recruitmentRequest.CompanyCode = AppState.RecruitmentRequest.CompanyCode;
                    _recruitmentRequest.Company = AppState.RecruitmentRequest.Company;
                    _recruitmentRequest.MinAge = AppState.RecruitmentRequest.MinAge;
                    _recruitmentRequest.MaxAge = AppState.RecruitmentRequest.MaxAge;
                    _recruitmentRequest.MinRelevantExperience = AppState.RecruitmentRequest.MinRelevantExperience;
                    _recruitmentRequest.MaxRelevantExperience = AppState.RecruitmentRequest.MaxRelevantExperience;
                    _recruitmentRequest.MinWorkExperience = AppState.RecruitmentRequest.MinWorkExperience;
                    _recruitmentRequest.MaxWorkExperience = AppState.RecruitmentRequest.MaxWorkExperience;
                    _recruitmentRequest.SalaryRangeType = AppState.RecruitmentRequest.SalaryRangeType;
                    _recruitmentRequest.YearlySalaryRangeMin = AppState.RecruitmentRequest.YearlySalaryRangeMin;
                    _recruitmentRequest.YearlySalaryRangeMax = AppState.RecruitmentRequest.YearlySalaryRangeMax;
                    _recruitmentRequest.YearlySalaryRangeCurrency = AppState.RecruitmentRequest.YearlySalaryRangeCurrency;
                    _recruitmentRequest.MonthlySalaryRangeMin = AppState.RecruitmentRequest.MonthlySalaryRangeMin;
                    _recruitmentRequest.MonthlySalaryRangeMax = AppState.RecruitmentRequest.MonthlySalaryRangeMax;
                    _recruitmentRequest.MonthlySalaryRangeCurrency = AppState.RecruitmentRequest.MonthlySalaryRangeCurrency;
                    _recruitmentRequest.DailySalaryRangeMin = AppState.RecruitmentRequest.DailySalaryRangeMin;
                    _recruitmentRequest.DailySalaryRangeMax = AppState.RecruitmentRequest.DailySalaryRangeMax;
                    _recruitmentRequest.DailySalaryRangeCurrency = AppState.RecruitmentRequest.DailySalaryRangeCurrency;
                    _recruitmentRequest.HourlySalaryRangeMin = AppState.RecruitmentRequest.HourlySalaryRangeMin;
                    _recruitmentRequest.HourlySalaryRangeMax = AppState.RecruitmentRequest.HourlySalaryRangeMax;
                    _recruitmentRequest.HourlySalaryRangeCurrency = AppState.RecruitmentRequest.HourlySalaryRangeCurrency;
                    #endregion
                }

                BeginLoadComboboxTask();
            }

            // Initialize slider state
            UpdateSliderStates(_recruitmentRequest.SalaryRangeType!);
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

                _ = SaveRequisitionAsync(async () =>
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
        private async Task LoadComboboxAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

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

            // Get UDC dataset
            var result = await EmployeeService.GetUserDefinedCodeAllAsync();
            if (result.Success)
            {
                var udcData = result.Value;
                if (udcData!.Any() && udcGroupList!.Any())
                {
                    #region Get Employment Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.EMPLOYTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Employement Type group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _employmentTypeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_employmentTypeList != null)
                            _employmentTypeArray = _employmentTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Qualification Modes
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.QUALIFACTIONMODE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Qualification Mode group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _qualificationModeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_qualificationModeList != null)
                            _qualificationModeArray = _qualificationModeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Qualification Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.QUALIFACTIONTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Qualification Types group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _qualificationTypeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                    }
                    #endregion

                    #region Get Stream Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.STREAMTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Stream Types group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _streamTypeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                    }
                    #endregion

                    #region Get Specialization Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.SPECIALIZATION.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Specialization Types group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _specializationList = udcData!.Where(a => a.GroupID == groupID).ToList();
                    }
                    #endregion

                    #region Get Position Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.POSITIONTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Position Type group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _positionTypeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_positionTypeList != null)
                            _positionTypeArray = _positionTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Interview Processes
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.INTERVIEWWF.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Interview Process group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _interviewProcessList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_interviewProcessList != null)
                            _interviewProcessArray = _interviewProcessList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Departments
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

                    if (_departmentList != null && _departmentList.Count > 0)
                        _departmentArray = _departmentList.Select(d => d.DepartmentName).OrderBy(d => d).ToArray();
                    #endregion

                    #region Get Countries
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.COUNTRY.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Countries group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _countryList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_countryList != null)
                            _countryArray = _countryList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Education Levels
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.EDUCLEVEL.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Education Levels group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _educationLevelList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_educationLevelList != null)
                            _educationLevelArray = _educationLevelList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Employee Classes
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.EMPCLASS.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Employee Classes group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _employeeClassList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_employeeClassList != null)
                            _employeeClassArray = _employeeClassList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Job Titles
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.JOBTITLE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Job Titles group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _jobTitleList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_jobTitleList != null)
                            _jobTitleArray = _jobTitleList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Pay Grades
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.PAYGRADE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Pay Grades group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _payGradeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_payGradeList != null)
                            _payGradeArray = _payGradeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Ethnicity Types
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.ETHNICTYPE.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Ethnicity Types group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _ethnicityTypeList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_ethnicityTypeList != null)
                            _ethnicityTypeArray = _ethnicityTypeList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion

                    #region Get Customer List
                    try
                    {
                        groupID = udcGroupList!.Where(a => a.UDCGCode == UDCKeys.CUSTOMERLIST.ToString()).FirstOrDefault()!.UDCGroupId;
                    }
                    catch (Exception ex)
                    {
                        _errorMessage.Append($"Error getting Customers group ID: {ex.Message}");
                    }

                    if (groupID > 0)
                    {
                        _customerList = udcData!.Where(a => a.GroupID == groupID).ToList();
                        if (_customerList != null)
                            _customerArray = _customerList.Select(s => s.UDCDesc1).OrderBy(s => s).ToArray();
                    }
                    #endregion
                }
            }
            else
                _errorMessage.Append(result.Error);

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task AddQualificationAsync()
        {
            try
            {
                var parameters = new DialogParameters
                {
                    ["RecruitmentQualification"] = new JobQualificationDTO(),
                    ["QualificationTypeList"] = _qualificationTypeList,
                    ["StreamTypeList"] = _streamTypeList,
                    ["SpecializationList"] = _specializationList,
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
                var dialog = await DialogService.ShowAsync<RecruitmentQualDialog>("Add Qualification", parameters, options);
                var result = await dialog.Result;

                if (result != null && !result.Canceled)
                {
                    UserDefinedCodeDTO? udc = null;
                    var newQualification = (JobQualificationDTO)result.Data!;
                    newQualification.AutoId = 0;

                    #region Get the selected Qualification
                    if (!string.IsNullOrEmpty(newQualification.Qualification))
                    {
                        udc = _qualificationTypeList.Where(d => d.UDCDesc1 == newQualification.Qualification).FirstOrDefault();
                        if (udc != null)
                            newQualification.QualificationCode = udc.UDCCode;
                    }
                    #endregion

                    #region Get the selected Stream
                    if (!string.IsNullOrEmpty(newQualification.Stream))
                    {
                        udc = _streamTypeList.Where(d => d.UDCDesc1 == newQualification.Stream).FirstOrDefault();
                        if (udc != null)
                            newQualification.StreamCode = udc.UDCCode;
                    }
                    #endregion

                    #region Get the selected Specialization
                    if (!string.IsNullOrEmpty(newQualification.Specialization))
                    {
                        udc = _specializationList.Where(d => d.UDCDesc1 == newQualification.Specialization).FirstOrDefault();
                        if (udc != null)
                            newQualification.SpecializationCode = udc.UDCCode;
                    }
                    #endregion

                    #region Check for duplicate entries
                    var duplicateRecord = _recruitmentRequest.QualificationList.FirstOrDefault(e => e.QualificationCode.Trim().ToUpper() == newQualification.QualificationCode.Trim().ToUpper() 
                        && e.StreamCode.Trim() == newQualification.StreamCode.Trim()
                        && (!string.IsNullOrEmpty(newQualification.SpecializationCode) && e.SpecializationCode!.Trim() == newQualification.SpecializationCode!.Trim()));
                    if (duplicateRecord != null)
                    {
                        // Show error
                        await ShowErrorMessage(MessageBoxTypes.Error, "Error", "The specified qualification already exists. Please enter a different value for Qualification, Stream, and Specialization fields then try saving again.");
                        return;
                    }
                    #endregion

                    // Set flag to display the loading panel
                    _isRunning = true;

                    // Set the overlay message
                    overlayMessage = "Adding qualification, please wait...";

                    _ = SaveQualificationAsync(async () =>
                    {
                        _isRunning = false;

                        // Shows the spinner overlay
                        await InvokeAsync(StateHasChanged);

                    }, newQualification);
                }
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
                    Specialization = qualification.Specialization,
                    Remarks = qualification.Remarks 
                };

                var parameters = new DialogParameters
                {
                    ["RecruitmentQualification"] = editableCopy,
                    ["QualificationTypeList"] = _qualificationTypeList,
                    ["StreamTypeList"] = _streamTypeList,
                    ["SpecializationList"] = _specializationList,
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

                var dialog = await DialogService.ShowAsync<RecruitmentQualDialog>("Edit Qualification", parameters, options);
                var result = await dialog.Result;

                if (result != null && !result.Canceled)
                {
                    UserDefinedCodeDTO? udc = null;
                    var updated = (JobQualificationDTO)result.Data!;

                    #region Get the selected Qualification
                    if (!string.IsNullOrEmpty(updated.Qualification))
                    {
                        udc = _qualificationTypeList.Where(d => d.UDCDesc1 == updated.Qualification).FirstOrDefault();
                        if (udc != null)
                            updated.QualificationCode = udc.UDCCode;
                    }
                    #endregion

                    #region Get the selected Stream
                    if (!string.IsNullOrEmpty(updated.Stream))
                    {
                        udc = _streamTypeList.Where(d => d.UDCDesc1 == updated.Stream).FirstOrDefault();
                        if (udc != null)
                            updated.StreamCode = udc.UDCCode;
                    }
                    #endregion

                    #region Get the selected Specialization
                    if (!string.IsNullOrEmpty(updated.Specialization))
                    {
                        udc = _specializationList.Where(d => d.UDCDesc1 == updated.Specialization).FirstOrDefault();
                        if (udc != null)
                            updated.SpecializationCode = udc.UDCCode;
                    }
                    #endregion

                    // Update in-memory grid item
                    var index = _recruitmentRequest.QualificationList.FindIndex(x => x.AutoId == updated.AutoId);
                    if (index >= 0)
                    {
                        _recruitmentRequest.QualificationList[index] = updated;
                        await InvokeAsync(StateHasChanged);
                    }

                    #region Persist changes to DB
                    // Set flag to display the loading panel
                    _isRunning = true;

                    // Set the overlay message
                    overlayMessage = "Saving qualification, please wait...";

                    _ = SaveQualificationAsync(async () =>
                    {
                        _isRunning = false;

                        // Shows the spinner overlay
                        await InvokeAsync(StateHasChanged);
                    }, updated);
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
                // Remove locally from the list so UI updates immediately
                _recruitmentRequest.QualificationList.Remove(qualification);
            }
        }

        private async Task SaveQualificationAsync(Func<Task> callback, JobQualificationDTO qualification)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(300);

            // Reset error messages
            _errorMessage.Clear();

            if (qualification.AutoId == 0)
            {
                // Get the new identity seed
                if (_recruitmentRequest.QualificationList.Any())
                    qualification.AutoId = _recruitmentRequest.QualificationList.Max(d => d.AutoId) + 1;
                else
                    qualification.AutoId = 1;

                // Add locally to the list so UI updates immediately
                _recruitmentRequest.QualificationList.Add(qualification);

                StateHasChanged();
            }

            // Show notification
            ShowNotification("Qualification has been saved successfully.", NotificationType.Success);

            if (callback != null)
            {
                // Hide the spinner overlay
                await callback.Invoke();
            }
        }

        private async Task SaveRequisitionAsync(Func<Task> callback)
        {
            // Wait for 1 second then gives control back to the runtime
            await Task.Delay(500);

            // Reset error messages
            _errorMessage.Clear();

            bool isNewRequition = _recruitmentRequest.RequisitionId == 0;

            #region Get the combobox selected values
            UserDefinedCodeDTO? udc = null;

            if (!string.IsNullOrEmpty(_recruitmentRequest.DepartmentName))
            {
                DepartmentDTO? selectedDepartment = _departmentList.Where(a => a.DepartmentName == _recruitmentRequest.DepartmentName).FirstOrDefault();
                if (selectedDepartment != null)
                    _recruitmentRequest.DepartmentCode = selectedDepartment.DepartmentCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.EmploymentType))
            {
                udc = _employmentTypeList.Where(a => a.UDCDesc1 == _recruitmentRequest.EmploymentType).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.EmploymentTypeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.QualificationMode))
            {
                udc = _qualificationModeList.Where(a => a.UDCDesc1 == _recruitmentRequest.QualificationMode).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.QualificationModeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.PositionType))
            {
                udc = _positionTypeList.Where(a => a.UDCDesc1 == _recruitmentRequest.PositionType).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.PositionTypeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.InterviewProcess))
            {
                udc = _interviewProcessList.Where(a => a.UDCDesc1 == _recruitmentRequest.InterviewProcess).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.InterviewProcessCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.Company))
            {
                udc = _customerList.Where(a => a.UDCDesc1 == _recruitmentRequest.Company).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.CompanyCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.Country))
            {
                udc = _countryList.Where(a => a.UDCDesc1 == _recruitmentRequest.Country).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.CountryCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.Education))
            {
                udc = _educationLevelList.Where(a => a.UDCDesc1 == _recruitmentRequest.Education).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.EducationCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.EmploymentType))
            {
                udc = _employmentTypeList.Where(a => a.UDCDesc1 == _recruitmentRequest.EmploymentType).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.EmploymentTypeCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.EmployeeClass))
            {
                udc = _employeeClassList.Where(a => a.UDCDesc1 == _recruitmentRequest.EmployeeClass).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.EmployeeClassCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.Ethnicity))
            {
                udc = _ethnicityTypeList.Where(a => a.UDCDesc1 == _recruitmentRequest.Ethnicity).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.EthnicityCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.JobTitle))
            {
                udc = _jobTitleList.Where(a => a.UDCDesc1 == _recruitmentRequest.JobTitle).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.JobTitleCode = udc.UDCCode;
            }

            if (!string.IsNullOrEmpty(_recruitmentRequest.PayGradeDesc))
            {
                udc = _payGradeList.Where(a => a.UDCDesc1 == _recruitmentRequest.PayGradeDesc).FirstOrDefault();
                if (udc != null)
                    _recruitmentRequest.PayGradeCode = udc.UDCCode;
            }
            #endregion

            #region Initialize Gender List
            if (_recruitmentRequest.GenderList!.Any())
            {
                StringBuilder sbGender = new StringBuilder(); 
                foreach (string item in _recruitmentRequest.GenderList!)
                {
                    if (sbGender.Length == 0)
                        sbGender.Append(item);
                    else
                        sbGender.Append($";{item}");
                }

                _recruitmentRequest.RequiredGender = sbGender.ToString();   
            }
            #endregion

            #region Initialize Assets List
            if (_recruitmentRequest.AssetList!.Any())
            {
                StringBuilder sbAsset = new StringBuilder();
                foreach (string item in _recruitmentRequest.AssetList!)
                {
                    if (sbAsset.Length == 0)
                        sbAsset.Append(item);
                    else
                        sbAsset.Append($";{item}");
                }

                _recruitmentRequest.RequiredAsset = sbAsset.ToString();
            }
            #endregion

            // Initialize the cancellation token
            _cts = new CancellationTokenSource();

            bool isSuccess = false;
            string errorMsg = string.Empty;

            if (isNewRequition)
            {
                var addResult = await RecruitmentService.AddRecruitmentRequestAsync(_recruitmentRequest, _cts.Token);
                isSuccess = addResult.Success;
                if (!isSuccess)
                    errorMsg = addResult.Error!;
                else
                {
                    // Set flag to enable reload of _recruitmentRequests when navigating back to the Employe Search page
                    _forceLoad = true;
                }
            }
            else
            {
                var saveResult = await RecruitmentService.UpdateRecruitmentRequestAsync(_recruitmentRequest, _cts.Token);
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
                if (isNewRequition)
                    ShowNotification("Recruitment requisition has been created successfully!", NotificationType.Success);
                else
                    ShowNotification("Changes has been saved successfully!", NotificationType.Success);
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
        #endregion

        #region Private Methods
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

                // Shows the spinner overlay
                await InvokeAsync(StateHasChanged);
            });
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

        private void OnSalaryRangeTypeChanged(string value)
        {
            _recruitmentRequest.SalaryRangeType = value;
            UpdateSliderStates(value);
        }

        private void UpdateSliderStates(string type)
        {
            switch(type)
            {
                case "Yearly":
                    _recruitmentRequest.MonthlySalaryRange = 0;
                    _recruitmentRequest.DailySalaryRange = 0;
                    _recruitmentRequest.HourlySalaryRange = 0;
                    break;

                case "Monthly":
                    _recruitmentRequest.YearlySalaryRange = 0;
                    _recruitmentRequest.DailySalaryRange = 0;
                    _recruitmentRequest.HourlySalaryRange = 0;
                    break;

                case "Daily":
                    _recruitmentRequest.YearlySalaryRange = 0;
                    _recruitmentRequest.MonthlySalaryRange = 0;                    
                    _recruitmentRequest.HourlySalaryRange = 0;
                    break;

                case "Hourly":
                    _recruitmentRequest.YearlySalaryRange = 0;
                    _recruitmentRequest.MonthlySalaryRange = 0;
                    _recruitmentRequest.DailySalaryRange = 0;
                    break;
            }
        }

        private void OnChipClosed(MudChip<string> chip) => _skillChips.Remove(chip.Value);
        private void AddChip() 
        {
            if (string.IsNullOrEmpty(_skillName))
            {
                //await ShowErrorMessage(MessageBoxTypes.Error, "Error", "Please specify the skill to add in the list.");
                ShowNotification("Please specify the skill to be added in the list.", NotificationType.Warning);
                return;
            }

            // Case-insensitive duplicate validation
            bool exists = _skillChips.Any(s =>
                string.Equals(s.Trim(), _skillName, StringComparison.OrdinalIgnoreCase));

            if (exists)
            {
                ShowNotification("Validation Error: The specified skill already exist.", NotificationType.Error);
                return;
            }

            _skillChips.Add(_skillName);
            _skillName = string.Empty;
        }

        private void ClearChips()
        {
            _skillChips.Clear();
            _skillName = string.Empty;
        }

        private string FormatExperienceLabel(double value) => $"{value} yrs";
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

        private async Task<IEnumerable<string>> SearchQualificationMode(string value, CancellationToken token)
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

        private async Task<IEnumerable<string>> SearchPositionType(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _positionTypeArray!;
            }

            return _positionTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchInterviewProcess(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _interviewProcessArray!;
            }

            return _interviewProcessArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
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

        private async Task<IEnumerable<string>> SearchEthnicity(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _ethnicityTypeArray!;
            }

            return _ethnicityTypeArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }

        private async Task<IEnumerable<string>> SearchCustomer(string value, CancellationToken token)
        {
            // In real life use an asynchronous function for fetching data from an api.
            await Task.Delay(5, token);

            // if text is null or empty, show complete list
            if (string.IsNullOrEmpty(value))
            {
                return _customerArray!;
            }

            return _customerArray!.Where(x => x.Contains(value, StringComparison.InvariantCultureIgnoreCase));
        }
        #endregion
    }
}
