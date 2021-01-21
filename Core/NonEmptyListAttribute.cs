using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core
{
    public class NonEmptyListAttribute : ValidationAttribute
    {
        private readonly Type _t;
        public NonEmptyListAttribute(Type t)
        {
            _t = t;
        }
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("List must not be null");
            }
            var list = value as List<_t>
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
