using System;
using System.Collections.Generic;

namespace Core.Exceptions
{
    public class ValidationException : Exception
    {
        public Dictionary<string, string> ValidationErrors { get; protected set; }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message, Dictionary<string, string> validationErrors) : base(message)
        {
            ValidationErrors = validationErrors;
        }
    }
}
