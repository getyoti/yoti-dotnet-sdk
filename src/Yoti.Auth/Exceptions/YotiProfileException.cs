using System;

namespace Yoti.Auth.Exceptions
{
    public class YotiProfileException : Exception
    {
        public YotiProfileException(string message) : base(message)
        {
        }

        public YotiProfileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}