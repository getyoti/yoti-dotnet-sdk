using System;

namespace Yoti.Auth.Exceptions
{
    public class DigitalIdentityException : YotiException
    {
        public DigitalIdentityException()
            : base()
        {
        }

        public DigitalIdentityException(string message)
            : base(message)
        {
        }

        public DigitalIdentityException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}