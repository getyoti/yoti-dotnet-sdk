using System.Collections.Generic;
using System.Net.Http;

namespace Yoti.Auth.Web
{
    public interface IAuthStrategy
    {
        /// <summary>
        /// Returns auth-specific headers (e.g. Authorization: Bearer, or X-Yoti-Auth-Digest).
        /// </summary>
        IDictionary<string, string> CreateAuthHeaders(HttpMethod httpMethod, string endpoint, byte[] body);

        /// <summary>
        /// Returns auth-specific query params (e.g. nonce + timestamp for signed requests; empty for bearer).
        /// </summary>
        IDictionary<string, string> CreateQueryParams();

        /// <summary>
        /// The SDK ID associated with this strategy, or null when not applicable (e.g. bearer token auth).
        /// Services use this to decide whether to include sdkId in headers and query parameters.
        /// </summary>
        string SdkId { get; }
    }
}
