using System;

namespace Yoti.Auth
{
    internal class YotiProfileException : Exception
    {
        public YotiProfileException()
        {
        }

        public YotiProfileException(string message) : base(message)
        {
        }

        public YotiProfileException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}