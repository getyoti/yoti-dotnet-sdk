using System;

namespace Yoti.Auth.Aml
{
    public class AmlException : Exception
    {
        public AmlException(string message) : base(message)
        {
        }

        public AmlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}