using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoti.Auth
{
    internal class HttpRequester : IHttpRequester
    {
        public async Task<Response> DoRequest(HttpClient httpClient, Uri uri, Dictionary<string, string> headers)
        {
            Response result = new Response
            {
                Success = false,
                StatusCode = 0,
                Content = null,
            };

            using (var client = httpClient)
            {
                using (var request = new HttpRequestMessage())
                {
                    request.RequestUri = uri;

                    request.Method = HttpMethod.Get;

                    foreach (string headerName in headers.Keys)
                    {
                        request.Headers.Add(headerName, headers[headerName]);
                    }

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        result.StatusCode = (int)response.StatusCode;
                        if (response.IsSuccessStatusCode)
                        {
                            result.Content = await response.Content.ReadAsStringAsync();
                            result.Success = true;
                        }
                    }
                }
            }

            return result;
        }
    }
}