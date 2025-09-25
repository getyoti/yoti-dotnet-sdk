using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoti.Auth.Web
{
    public class Request
    {
        public HttpRequestMessage RequestMessage { get; set; }

        public Request(HttpRequestMessage message)
        {
            RequestMessage = message;
        }

        public async Task<HttpResponseMessage> Execute(HttpClient httpClient)
        {
            Validation.NotNull(httpClient, nameof(httpClient));

            return await httpClient.SendAsync(RequestMessage).ConfigureAwait(false);
        }

        /// <summary>
        /// Executes the request and returns the response with headers
        /// </summary>
        /// <typeparam name="T">The type to deserialize the response to</typeparam>
        /// <param name="httpClient">The HTTP client to use</param>
        /// <param name="dataExtractor">Function to extract data from the HTTP response</param>
        /// <returns>YotiHttpResponse containing both data and headers</returns>
        public async Task<YotiHttpResponse<T>> ExecuteWithHeaders<T>(HttpClient httpClient, Func<HttpResponseMessage, Task<T>> dataExtractor)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(dataExtractor, nameof(dataExtractor));

            using (HttpResponseMessage response = await Execute(httpClient).ConfigureAwait(false))
            {
                // Extract data using the provided function
                T data = await dataExtractor(response).ConfigureAwait(false);

                // Use the existing factory method that properly handles headers
                return YotiHttpResponse<T>.FromHttpResponse(data, response);
            }
        }
    }
}
