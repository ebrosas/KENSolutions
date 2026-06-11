using KenHRApp.Application.DTOs;
using System.ComponentModel.DataAnnotations;

namespace KenHRApp.Application.Common.Validations
{
    /// <summary>
    /// Validates that ActiveCount is not greater than the specified BudgetHeadCount property.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class RegularizationTimeValidationAttribute : ValidationAttribute
    {
        #region Fields
        private readonly string _comparisonProperty;
        #endregion

        #region Constructor
        public RegularizationTimeValidationAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }
        #endregion

        protected override ValidationResult? IsValid(
            object? value,
            ValidationContext validationContext)
        {
            if (value is not TimeSpan currentTime)
                return ValidationResult.Success;

            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
            {
                return new ValidationResult(
                    $"Unknown property '{_comparisonProperty}'.");
            }

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue is not TimeSpan comparisonTime)
                return ValidationResult.Success;

            bool isStartTimeValidation =
                validationContext.MemberName == nameof(RegularRequestDTO.RegularizedTimeIn);

            if (isStartTimeValidation)
            {
                if (currentTime > comparisonTime)
                {
                    return new ValidationResult(
                        "Regularized In Time cannot be greater than Out Time.");
                }
            }
            else
            {
                if (currentTime < comparisonTime)
                {
                    return new ValidationResult(
                        "Regularized Out Time cannot be less than In Time.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
