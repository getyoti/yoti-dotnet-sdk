using System;
using System.Net;

namespace Yoti.Auth
{
    internal class Response
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; }
        public string ReasonPhrase { get; set; }

        public static void CreateExceptionFromStatusCode<E>(Response response) where E : Exception
        {
            switch ((HttpStatusCode)response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw Activator.CreateInstance(typeof(E),
                        new object[] { $"Failed validation:{Environment.NewLine}{response.Content}" }) as E;

                case HttpStatusCode.Unauthorized:
                    throw Activator.CreateInstance(typeof(E),
                       $"Failed authorization with the given key:{Environment.NewLine}{response.Content}") as E;

                case HttpStatusCode.InternalServerError:
                    throw Activator.CreateInstance(typeof(E),
                       $"An unexpected error occurred on the server:{Environment.NewLine}{response.Content}") as E;

                default:
                    throw Activator.CreateInstance(typeof(E),
                        $"Unexpected error:{Environment.NewLine}{response.Content}") as E;
            }
        }
    }
}