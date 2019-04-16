using System.Collections.Generic;
using System.Reflection;

namespace Yoti.Auth
{
    internal static class HeadersFactory
    {
        public static Dictionary<string, string> Create(string authDigest, string authKey)
        {
            string SDKVersion = typeof(YotiClientEngine).GetTypeInfo().Assembly.GetName().Version.ToString();

            var headers = new Dictionary<string, string>
            {
                { Constants.Web.AuthKeyHeader, authKey },
                { Constants.Web.DigestHeader, authDigest },
                { Constants.Web.YotiSdkHeader, Constants.Web.SdkIdentifier },
                { Constants.Web.YotiSdkVersionHeader, $"{Constants.Web.SdkIdentifier}-{SDKVersion}" }
            };

            return headers;
        }
    }
}