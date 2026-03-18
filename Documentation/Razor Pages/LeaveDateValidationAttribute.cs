using KenHRApp.Application.Interfaces;
using KenHRApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Validations
{
    /// <summary>
    /// Validates that ActiveCount is not greater than the specified BudgetHeadCount property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class LeaveDateValidationAttribute : ValidationAttribute
    {
        #region Fields
        private readonly string _comparisonProperty;
        private ILeaveRequestService _leaveService;
        #endregion

        #region Constructor
        public LeaveDateValidationAttribute(string comparisonProperty, ILeaveRequestService leaveService)
        {
            _comparisonProperty = comparisonProperty;
            _leaveService = leaveService;
        }
        #endregion

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // no need to validate if ActiveCount is null

            // Get the property to compare 
            var comparisonPropertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (comparisonPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            // Get values
            var comparisonValue = comparisonPropertyInfo.GetValue(validationContext.ObjectInstance);

            if (comparisonValue == null)
                return ValidationResult.Success;

            // Convert both to datetime
            DateTime? leaveStarDate = null; 
            DateTime? leaveResumeDate = null;
            bool isPublicHoliday = false;

            if (comparisonPropertyInfo.Name == "LeaveResumeDate")
            {
                leaveStarDate = value != null ? Convert.ToDateTime(value) : null;
                leaveResumeDate = comparisonValue != null ? Convert.ToDateTime(comparisonValue) : null;                                
            }
            else
            {
                leaveStarDate = comparisonValue != null ? Convert.ToDateTime(comparisonValue) : null;
                leaveResumeDate = value != null ? Convert.ToDateTime(value) : null;
            }

            #region Validation rules
            // Check if leave period is valid
            if (leaveStarDate.HasValue && leaveResumeDate.HasValue && leaveStarDate > leaveResumeDate)
            {
                if (comparisonPropertyInfo.Name == "LeaveResumeDate")
                    return new ValidationResult("Start Date cannot be greater than the Resume Date.");
                else
                    return new ValidationResult("Resume Date cannot be less than the Start Date.");
            }

            if (comparisonPropertyInfo.Name == "LeaveResumeDate")
            {
                // Check if Leave Start Date is a public holiday
                if (leaveStarDate.HasValue)
                {
                    isPublicHoliday = _leaveService.CheckIfLeaveDateIsHolidayAsync(leaveStarDate!.Value).Result;
                    if (isPublicHoliday)
                        return new ValidationResult("Start Date cannot be a public holiday.");
                }
            }
            else
            {
                // Check if Leave Resume Date is a public holiday
                if (leaveResumeDate.HasValue)
                {
                    isPublicHoliday = _leaveService.CheckIfLeaveDateIsHolidayAsync(leaveResumeDate!.Value).Result;
                    if (isPublicHoliday)
                        return new ValidationResult("Resume Date cannot be a public holiday.");
                }
            }
            #endregion

            return ValidationResult.Success;
        }
    }
}
