using System;

namespace Yoti.Auth.Exceptions
{
    public class AmlException : YotiException
    {
        public AmlException()
            : base()
        {
        }

        public AmlException(string message)
            : base(message)
        {
        }

        public AmlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}