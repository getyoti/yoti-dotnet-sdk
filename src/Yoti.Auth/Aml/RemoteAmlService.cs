using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoti.Auth.Aml
{
    internal class RemoteAmlService : IRemoteAmlService
    {
        public async Task<AmlResult> PerformCheck(IHttpRequester httpRequester, Dictionary<string, string> headers, string apiUrl, string endpoint, byte[] httpContent)
        {
            try
            {
                HttpMethod httpMethod = HttpMethod.Post;

                Response response = await httpRequester.DoRequest(
                    new HttpClient(),
                    httpMethod,
                    new Uri(apiUrl + endpoint),
                    headers,
                    httpContent).ConfigureAwait(false);

                if (!response.Success)
                {
                    CreateExceptionFromStatusCode(response);
                }

                var amlResult = Newtonsoft.Json.JsonConvert.DeserializeObject<AmlResult>(response.Content);

                return amlResult;
            }
            catch (Exception ex)
            {
                if (ex is AmlException)
                    throw;

                throw new AmlException(
                    $"Inner exception:{Environment.NewLine}{ex.Message}",
                    ex);
            }
        }

        private static void CreateExceptionFromStatusCode(Response response)
        {
            switch ((HttpStatusCode)response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new AmlException(
                        $"Failed validation:{Environment.NewLine}{response.Content}");

                case HttpStatusCode.Unauthorized:
                    throw new AmlException(
                       $"Failed authorization with the given key:{Environment.NewLine}{response.Content}");

                case HttpStatusCode.InternalServerError:
                    throw new AmlException(
                       $"An unexpected error occurred on the server:{Environment.NewLine}{response.Content}");

                default:
                    throw new AmlException(
                       $"Unexpected error:{Environment.NewLine}{response.Content}");
            }
        }
    }
}