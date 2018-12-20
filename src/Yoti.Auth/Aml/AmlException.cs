using System;

namespace Yoti.Auth.Aml
{
    public class AmlException : Exception
    {
        internal AmlException(string message) : base(message)
        {
        }

        internal AmlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}