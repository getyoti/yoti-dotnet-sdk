using System;

namespace Yoti.Auth.Exceptions
{
    public class DynamicShareException : Exception
    {
        public DynamicShareException(string message) : base(message)
        {
        }

        public DynamicShareException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}