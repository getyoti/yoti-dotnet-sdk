using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoti.Auth
{
    internal class HttpRequester : IHttpRequester
    {
        public async Task<Response> DoRequest(HttpClient httpClient, HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers, byte[] byteContent)
        {
            if (headers.Count < 1)
                throw new ArgumentNullException(nameof(headers));

            if (uri == null)
                throw new ArgumentNullException(nameof(uri));

            var result = new Response
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

                    if (byteContent != null && byteContent.Length > 0)
                    {
                        var byteArrayContent = new ByteArrayContent(byteContent);
                        request.Content = byteArrayContent;
                    }

                    foreach (string headerName in headers.Keys)
                    {
                        request.Headers.Add(headerName, headers[headerName]);
                    }

                    using (HttpResponseMessage response = await client.SendAsync(request))
                    {
                        result.StatusCode = (int)response.StatusCode;
                        result.Content = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            result.Success = true;
                        }
                        else
                        {
                            result.ReasonPhrase = response.ReasonPhrase;
                        }
                    }
                }
            }

            return result;
        }
    }
}