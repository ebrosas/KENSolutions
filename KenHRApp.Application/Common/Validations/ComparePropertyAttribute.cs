using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Validations
{
    public class ComparePropertyAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public ComparePropertyAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (property == null)
                return new ValidationResult($"Unknown property: {_comparisonProperty}");

            var comparisonValue = property.GetValue(validationContext.ObjectInstance);

            if (!Equals(value, comparisonValue))
                return new ValidationResult(ErrorMessage ?? "Password and Retype Password do not match.");

            return ValidationResult.Success;
        }
    }
}
