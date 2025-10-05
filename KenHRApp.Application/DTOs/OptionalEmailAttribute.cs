using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KenHRApp.Application.DTOs
{
    public class OptionalEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var str = value as string;

            if (string.IsNullOrWhiteSpace(str))
                return ValidationResult.Success; // allow empty

            var emailAttr = new EmailAddressAttribute();
            return emailAttr.IsValid(str)
                ? ValidationResult.Success
                : new ValidationResult($"{validationContext.DisplayName} is not a valid email address");
        }
    }
}
