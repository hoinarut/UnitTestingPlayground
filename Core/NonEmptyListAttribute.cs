using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class NonEmptyListAttribute : ValidationAttribute
    {
        public string GetErrorMessage() => $"List must not be null and contain at least one value";
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("List must not be null");
            }
            var list = (IList<object>)value;
            if (list.Count == 0)
            {
                return new ValidationResult("List must contain at least one value");
            }
            else
            {
                return ValidationResult.Success;
            }
        }
    }
}
