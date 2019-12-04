using System;

namespace Yoti.Auth.Exceptions
{
    public class ExtraDataException : Exception
    {
        public ExtraDataException()
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