using System;
using System.Collections.Generic;
using System.Net.Http;
using Yoti.Auth.Constants;

namespace Yoti.Auth.Web
{
    public class BearerTokenAuthStrategy : IAuthStrategy
    {
        private readonly string _token;

        public string SdkId => null;

        public BearerTokenAuthStrategy(string token)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("Authentication token must not be null or empty.", nameof(token));

            _token = token;
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
