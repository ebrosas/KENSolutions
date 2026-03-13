using KenHRApp.Application.Interfaces;
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
        #endregion

        #region Constructor
        public LeaveDateValidationAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }
        #endregion

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success; // no need to validate if ActiveCount is null

            // Get the property to compare (BudgetHeadCount)
            var comparisonPropertyInfo = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (comparisonPropertyInfo == null)
            {
                return new ValidationResult($"Unknown property: {_comparisonProperty}");
            }

            // Get values
            var comparisonValue = comparisonPropertyInfo.GetValue(validationContext.ObjectInstance);

            //if (comparisonValue == null)
            //    return ValidationResult.Success;

            // Convert both to datetime
            DateTime? leaveStarDate = Convert.ToDateTime(comparisonValue);   
            DateTime? leaveResumeDate = value != null ? Convert.ToDateTime(value) : null;
            

            // Validation rules
            if (leaveStarDate > leaveResumeDate)
                return new ValidationResult("Start Date cannot be greater than the Resume Date.");
                        
            return ValidationResult.Success;
        }
    }
}
