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
    public class NewIndentValidationAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public NewIndentValidationAttribute(string comparisonProperty)
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

            if (comparisonValue == null)
                return ValidationResult.Success;

            // Convert both to integers
            int positionCount = Convert.ToInt32(value);
            int netGapCount = Convert.ToInt32(comparisonValue);

            // Validation rule
            if (positionCount > netGapCount)
            {
                return new ValidationResult($"The New Indent (number of positions) should be the same or less than Net Gap value of ({netGapCount}).");
            }

            return ValidationResult.Success;
        }
    }
}
