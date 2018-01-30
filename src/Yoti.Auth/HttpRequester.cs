using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Yoti.Auth
{
    internal class HttpRequester : IHttpRequester
    {
        public async Task<Response> DoRequest(HttpClient httpClient, HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers)
        {
            if (headers.Count < 1)
                throw new ArgumentNullException("headers");

            if (uri == null)
                throw new ArgumentNullException("uri");

            Response result = new Response
            {
                Success = false,
                StatusCode = 0,
                Content = null,
            };

            using (var client = httpClient)
            {
                using (var request = new HttpRequestMessage(httpMethod, uri))
                {
                    request.RequestUri = uri;

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