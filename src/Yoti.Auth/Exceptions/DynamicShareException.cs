using System;

namespace Yoti.Auth.Exceptions
{
    public class DynamicShareException : YotiException
    {
        public DynamicShareException()
            : base()
        {
        }

        public DynamicShareException(string message)
            : base(message)
        {
        }

        public DynamicShareException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}