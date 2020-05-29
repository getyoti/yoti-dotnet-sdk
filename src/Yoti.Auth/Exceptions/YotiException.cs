using System;
using System.Net.Http;

namespace Yoti.Auth.Exceptions
{
    public abstract class YotiException : Exception
    {
        protected YotiException()
        {
        }

        protected YotiException(string message)
            : base(message)
        {
        }

        protected YotiException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public HttpResponseMessage HttpResponseMessage { get; internal set; }
    }
}