using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace Yoti.Auth.Web
{
    /// <summary>
    /// Represents a response from the Yoti API containing both the response data and HTTP headers.
    /// </summary>
    /// <typeparam name="T">The type of the response data</typeparam>
    public class YotiHttpResponse<T>
    {
        /// <summary>
        /// The response data from the API
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// The HTTP response headers from the API
        /// </summary>
        public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; }

        /// <summary>
        /// Gets the X-Request-ID header value if present
        /// </summary>
        public string RequestId => GetHeaderValue("X-Request-ID") ?? GetHeaderValue("X-Request-Id");

        /// <summary>
        /// Creates a new YotiHttpResponse
        /// </summary>
        /// <param name="data">The response data</param>
        /// <param name="headers">The HTTP headers</param>
        internal YotiHttpResponse(T data, IReadOnlyDictionary<string, IEnumerable<string>> headers)
        {
            Data = data;
            Headers = headers;
        }

        /// <summary>
        /// Creates a YotiHttpResponse from an HttpResponseMessage and response data
        /// </summary>
        /// <param name="data">The response data</param>
        /// <param name="httpResponse">The HTTP response message</param>
        /// <returns>A new YotiHttpResponse</returns>
        internal static YotiHttpResponse<T> FromHttpResponse(T data, HttpResponseMessage httpResponse)
        {
            var headers = new Dictionary<string, IEnumerable<string>>();

            // Add response headers
            foreach (var header in httpResponse.Headers)
            {
                headers[header.Key] = header.Value;
            }

            // Add content headers if present
            if (httpResponse.Content?.Headers != null)
            {
                foreach (var header in httpResponse.Content.Headers)
                {
                    headers[header.Key] = header.Value;
                }
            }

            return new YotiHttpResponse<T>(data, headers);
        }

        /// <summary>
        /// Creates a YotiHttpResponse with new data but copying headers from another YotiHttpResponse
        /// </summary>
        /// <typeparam name="TSource">The type of the source response data</typeparam>
        /// <param name="newData">The new response data</param>
        /// <param name="sourceResponse">The source response to copy headers from</param>
        /// <returns>A new YotiHttpResponse with the new data and copied headers</returns>
        internal static YotiHttpResponse<T> FromHttpResponse<TSource>(T newData, YotiHttpResponse<TSource> sourceResponse)
        {
            return new YotiHttpResponse<T>(newData, sourceResponse.Headers);
        }

        /// <summary>
        /// Gets the first value of a header with the specified name (case-insensitive)
        /// </summary>
        /// <param name="headerName">The name of the header</param>
        /// <returns>The first header value, or null if not found</returns>
        public string GetHeaderValue(string headerName)
        {
            var header = Headers.FirstOrDefault(h => 
                string.Equals(h.Key, headerName, System.StringComparison.OrdinalIgnoreCase));
            
            return header.Value?.FirstOrDefault();
        }

        /// <summary>
        /// Gets all values of a header with the specified name (case-insensitive)
        /// </summary>
        /// <param name="headerName">The name of the header</param>
        /// <returns>All header values, or an empty enumerable if not found</returns>
        public IEnumerable<string> GetHeaderValues(string headerName)
        {
            var header = Headers.FirstOrDefault(h => 
                string.Equals(h.Key, headerName, System.StringComparison.OrdinalIgnoreCase));
            
            return header.Value ?? Enumerable.Empty<string>();
        }

        /// <summary>
        /// Implicitly converts to the underlying data type for backward compatibility
        /// </summary>
        /// <param name="response">The YotiHttpResponse to convert</param>
        public static implicit operator T(YotiHttpResponse<T> response)
        {
            return response.Data;
        }
    }
}
