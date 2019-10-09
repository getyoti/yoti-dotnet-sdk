using System;
using System.Net;
using System.Net.Http;

namespace Yoti.Auth.Web
{
    internal class Response
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; }
        public string ReasonPhrase { get; set; }

        public static void CreateExceptionFromStatusCode<E>(HttpResponseMessage response) where E : Exception
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw Activator.CreateInstance(typeof(E),
                        $"Failed validation:{Environment.NewLine}{response.Content.ReadAsStringAsync().Result}") as E;

                case HttpStatusCode.Unauthorized:
                    throw Activator.CreateInstance(typeof(E),
                        $"Failed authorization with the given key:{Environment.NewLine}{response.Content.ReadAsStringAsync().Result}") as E;

                case HttpStatusCode.InternalServerError:
                    throw Activator.CreateInstance(typeof(E),
                        $"An unexpected error occurred on the server:{Environment.NewLine}{response.Content.ReadAsStringAsync().Result}") as E;

                default:
                    throw Activator.CreateInstance(typeof(E),
                        $"Unexpected error:" +
                        $"{Environment.NewLine} Status Code: '{response.StatusCode}'" +
                        $"{Environment.NewLine} Content: '{response.Content.ReadAsStringAsync().Result}'") as E;
            }
        }
    }
}