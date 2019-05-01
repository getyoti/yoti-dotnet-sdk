using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth
{
    internal static class HeadersFactory
    {
        internal static Dictionary<string, string> Create(AsymmetricCipherKeyPair keyPair, HttpMethod httpMethod, string endpoint, byte[] httpContent)
        {
            string authKey = CryptoEngine.GetAuthKey(keyPair);
            string authDigest = SignedMessageFactory.SignMessage(httpMethod, endpoint, keyPair, httpContent);

            return PutHeaders(authDigest, authKey);
        }

        internal static Dictionary<string, string> PutHeaders(string authDigest, string authKey)
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