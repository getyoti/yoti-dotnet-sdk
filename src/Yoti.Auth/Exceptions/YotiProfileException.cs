using System;

namespace Yoti.Auth.Exceptions
{
    public class YotiProfileException : YotiException
    {
        public YotiProfileException()
            : base()
        {
        }

        public YotiProfileException(string message)
            : base(message)
        {
        }

        public YotiProfileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}