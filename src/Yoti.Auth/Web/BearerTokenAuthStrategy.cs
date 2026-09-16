using System;
using System.Collections.Generic;
using System.Net.Http;
using Yoti.Auth.Constants;

namespace Yoti.Auth.Web
{
    public class BearerTokenAuthStrategy : IAuthStrategy
    {
        private readonly string _token;

        public string SdkId { get; }

        public BearerTokenAuthStrategy(string token, string sdkId = null)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Authentication token must not be null, empty, or whitespace.", nameof(token));

            if (sdkId != null && string.IsNullOrWhiteSpace(sdkId))
                throw new ArgumentException("SDK ID must not be empty or whitespace.", nameof(sdkId));

            _token = token;
            SdkId = sdkId;
        }

        public IDictionary<string, string> CreateAuthHeaders(HttpMethod httpMethod, string endpoint, byte[] body)
        {
            return new Dictionary<string, string>
            {
                [Api.AuthorizationHeader] = "Bearer " + _token
            };
        }

        public IDictionary<string, string> CreateQueryParams()
        {
            return new Dictionary<string, string>();
        }
    }
}
