using System.Collections.Generic;
using System.Net.Http;

namespace Yoti.Auth.Web
{
    public class NoAuthStrategy : IAuthStrategy
    {
        public string SdkId => null;

        public IDictionary<string, string> CreateAuthHeaders(HttpMethod httpMethod, string endpoint, byte[] body)
        {
            return new Dictionary<string, string>();
        }

        public IDictionary<string, string> CreateQueryParams()
        {
            return new Dictionary<string, string>();
        }
    }
}
