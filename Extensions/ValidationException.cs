using System;

namespace Extensions
{
    /// <summary>
    ///   Classed used to distinguish validation exceptions from standard argument or application exceptions
    /// </summary>
    public class ValidationException : ArgumentException
    {
        public ValidationException()
        {
        }

        public ValidationException(string message)
            : base(message)
        {
        }

        public ValidationException(string message, string paramName)
            : base(message, paramName)
        {
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ValidationException(string message, string paramName, Exception innerException)
            : base(message, paramName, innerException)
        {
        }
    }
}