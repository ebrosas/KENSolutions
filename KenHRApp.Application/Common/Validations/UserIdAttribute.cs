using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KenHRApp.Application.Common.Validations
{
    public class UserIdAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return new ValidationResult("User ID is required.");

            var userId = value.ToString();

            if (string.IsNullOrWhiteSpace(userId))
                return new ValidationResult("User ID is required.");

            // Minimum length check
            if (userId.Length < 5)
                return new ValidationResult("User ID must be at least 5 characters long.");

            // Must start with letter or number
            if (!Regex.IsMatch(userId, @"^[A-Za-z0-9]"))
                return new ValidationResult("User ID must start with a letter or number.");

            // Disallowed characters check
            if (userId.Contains("'"))
                return new ValidationResult("User ID cannot contain single quote (').");

            if (userId.Contains(";"))
                return new ValidationResult("User ID cannot contain semicolon (;).");

            if (userId.Contains("--"))
                return new ValidationResult("User ID cannot contain double dash (--).");

            if (userId.Contains("/"))
                return new ValidationResult("User ID cannot contain forward slash (/).");

            if (userId.Contains("\\"))
                return new ValidationResult("User ID cannot contain backslash (\\).");

            if (userId.Contains("%"))
                return new ValidationResult("User ID cannot contain percent (%).");

            return ValidationResult.Success;
        }
    }
}
