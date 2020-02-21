using System;

namespace Yoti.Auth.Exceptions
{
    public class DocScanException : YotiException
    {
        public DocScanException()
        {
        }

        public DocScanException(string message)
            : base(message)
        {
        }

        public DocScanException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}