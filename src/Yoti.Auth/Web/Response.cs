using System;
using System.Net;
using System.Net.Http;
using Yoti.Auth.Exceptions;

namespace Yoti.Auth.Web
{
    internal class Response
    {
        public bool Success { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; }
        public string ReasonPhrase { get; set; }

        public static void CreateYotiExceptionFromStatusCode<E>(HttpResponseMessage response) where E : YotiException
        {
            YotiException exception;

            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    exception = Activator.CreateInstance(typeof(E),
                        $"Failed validation - " +
                        $"Status Code: '{(int)response.StatusCode}' ({response.StatusCode}). " +
                        $"Content: '{response.Content.ReadAsStringAsync().Result}'") as E;
                    break;

                case HttpStatusCode.Unauthorized:
                    exception = Activator.CreateInstance(typeof(E),
                        $"Failed authorization with the given key - " +
                        $"Status Code: '{(int)response.StatusCode}' ({response.StatusCode}). " +
                        $"Content: '{response.Content.ReadAsStringAsync().Result}'") as E;
                    break;

                case HttpStatusCode.InternalServerError:
                    exception = Activator.CreateInstance(typeof(E),
                        "An unexpected error occurred on the server - " +
                        $"Status Code: '{(int)response.StatusCode}' ({response.StatusCode}). " +
                        $"Content: '{response.Content.ReadAsStringAsync().Result}'") as E;
                    break;

                default:
                    exception = Activator.CreateInstance(typeof(E),
                        $"Unexpected error - " +
                        $"Status Code: '{(int)response.StatusCode}' ({response.StatusCode}). " +
                        $"Content: '{response.Content.ReadAsStringAsync().Result}'") as E;
                    break;
            }

            exception.HttpResponseMessage = response;

            throw exception;
        }
    }
}