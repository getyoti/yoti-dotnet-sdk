using System;

namespace Yoti.Auth.Exceptions
{
    public class ExtraDataException : YotiException
    {
        public ExtraDataException()
            : base()
        {
        }

        public ExtraDataException(string message)
            : base(message)
        {
        }

        public ExtraDataException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}