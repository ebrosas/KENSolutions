using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Validations
{
    public class CheckIfZeroValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (Convert.ToInt32(value) == 0)
                return new ValidationResult("Value cannot be zero.");

            return ValidationResult.Success;
        }
    }
}
