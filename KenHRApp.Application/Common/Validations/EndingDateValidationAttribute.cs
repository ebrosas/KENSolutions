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
    public class EndingDateValidationAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public EndingDateValidationAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

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
            DateTime effectiveDate = Convert.ToDateTime(comparisonValue);   
            DateTime? endingDate = value != null ? Convert.ToDateTime(value) : null;

            // Validation rules
            if (endingDate.HasValue && endingDate < effectiveDate)
            {
                return new ValidationResult("Ending Date should be greater than or equals to Effective Date.");
            }

            return ValidationResult.Success;
        }
    }
}
