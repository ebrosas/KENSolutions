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
    public class CheckIfRequiredAttribute : ValidationAttribute
    {
        #region Fields
        private readonly string _comparisonProperty;
        #endregion

        #region Constructor
        public CheckIfRequiredAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }
        #endregion

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
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

            if (comparisonPropertyInfo.Name == "PlannedLeave")
            {
                bool isPlannedLeave = comparisonValue switch
                {
                    'Y' => true,
                    'N' => false,
                    _ => false
                };
                long leavePlannedNo = value != null ? Convert.ToInt64(value) : 0;   

                // Validation rules
                if (isPlannedLeave && leavePlannedNo <= 0)
                {
                    return new ValidationResult("Planned leave requisition is required if Planned Leave is set to 'Yes'.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
