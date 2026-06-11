using KenHRApp.Application.DTOs.TNA;
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
    public sealed class OTTimeValidationAttribute : ValidationAttribute
    {
        #region Fields
        private readonly string _comparisonProperty;
        #endregion

        #region Constructor
        public OTTimeValidationAttribute(string comparisonProperty)
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
                validationContext.MemberName == nameof(ExtraTimeRequestDTO.OTStartTime);

            if (isStartTimeValidation)
            {
                if (currentTime > comparisonTime)
                {
                    return new ValidationResult(
                        "OT Start Time cannot be greater than OT End Time.");
                }
            }
            else
            {
                if (currentTime < comparisonTime)
                {
                    return new ValidationResult(
                        "OT End Time cannot be less than OT Start Time.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
