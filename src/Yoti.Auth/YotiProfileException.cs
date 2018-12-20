using System;

namespace Yoti.Auth
{
    public class YotiProfileException : Exception
    {
        internal YotiProfileException(string message) : base(message)
        {
        }

        internal YotiProfileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}