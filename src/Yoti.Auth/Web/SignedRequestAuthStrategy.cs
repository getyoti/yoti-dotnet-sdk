using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Constants;

namespace Yoti.Auth.Web
{
    public class SignedRequestAuthStrategy : IAuthStrategy
    {
        public AsymmetricCipherKeyPair KeyPair { get; }
        public string SdkId { get; }

        public SignedRequestAuthStrategy(AsymmetricCipherKeyPair keyPair, string sdkId)
        {
            KeyPair = keyPair ?? throw new ArgumentNullException(nameof(keyPair));
            if (string.IsNullOrEmpty(sdkId))
                throw new ArgumentException("SDK ID must not be null or empty.", nameof(sdkId));
            SdkId = sdkId;
        }

        public IDictionary<string, string> CreateAuthHeaders(HttpMethod httpMethod, string endpoint, byte[] body)
        {
            string digest = SignedMessageFactory.SignMessage(httpMethod, endpoint, KeyPair, body);
            return new Dictionary<string, string>
            {
                [Api.DigestHeader] = digest
            };
        }

        public IDictionary<string, string> CreateQueryParams()
        {
            return new Dictionary<string, string>
            {
                ["nonce"] = CryptoEngine.GenerateNonce(),
                ["timestamp"] = GetTimestamp()
            };
        }

        private static string GetTimestamp()
        {
            long ms = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;
            return ms.ToString(CultureInfo.InvariantCulture);
        }
    }
}
