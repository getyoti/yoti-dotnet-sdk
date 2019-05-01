using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Yoti.Auth.Exceptions;

namespace Yoti.Auth.Aml
{
    internal class RemoteAmlService : IRemoteAmlService
    {
        public async Task<AmlResult> PerformCheck(HttpClient httpClient, IHttpRequester httpRequester, Dictionary<string, string> headers, string apiUrl, string endpoint, byte[] httpContent)
        {
            try
            {
                HttpMethod httpMethod = HttpMethod.Post;

                Response response = await httpRequester.DoRequest(
                    httpClient,
                    httpMethod,
                    new Uri(apiUrl + endpoint),
                    headers,
                    httpContent).ConfigureAwait(false);

                if (!response.Success)
                {
                    Response.CreateExceptionFromStatusCode<AmlException>(response);
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
    }
}