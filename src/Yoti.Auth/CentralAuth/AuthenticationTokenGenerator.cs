using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace Yoti.Auth.CentralAuth
{
    public class AuthenticationTokenGenerator
    {
        private readonly string _sdkId;
        private readonly AsymmetricCipherKeyPair _keyPair;
        private readonly IList<string> _scopes;
        private readonly string _authApiUrl;

        internal AuthenticationTokenGenerator(string sdkId, AsymmetricCipherKeyPair keyPair, IList<string> scopes, string authApiUrl)
        {
            _sdkId = sdkId;
            _keyPair = keyPair;
            _scopes = scopes;
            _authApiUrl = authApiUrl;
        }

        public async Task<AuthenticationTokenResponse> GetToken(HttpClient httpClient)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            string jwt = BuildJwt();
            string scope = string.Join(" ", _scopes);

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_assertion_type", "urn:ietf:params:oauth:client-assertion-type:jwt-bearer"),
                new KeyValuePair<string, string>("client_assertion", jwt),
                new KeyValuePair<string, string>("scope", scope)
            });

            using (var response = await httpClient.PostAsync(_authApiUrl, formContent).ConfigureAwait(false))
            {
                response.EnsureSuccessStatusCode();
                string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var tokenResponse = JsonConvert.DeserializeObject<AuthenticationTokenResponse>(body);
                if (tokenResponse == null)
                    throw new InvalidOperationException("The authentication server returned an unexpected empty response.");
                return tokenResponse;
            }
        }

        private string BuildJwt()
        {
            var header = new { alg = "PS384", typ = "JWT" };
            long now = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
            string sdkIdentity = "sdk:" + _sdkId;
            var payload = new
            {
                iss = sdkIdentity,
                sub = sdkIdentity,
                aud = _authApiUrl,
                iat = now,
                exp = now + 300,
                jti = Guid.NewGuid().ToString()
            };

            string encodedHeader = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(header)));
            string encodedPayload = Base64UrlEncode(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload)));
            string signingInput = $"{encodedHeader}.{encodedPayload}";

            byte[] signingBytes = Encoding.ASCII.GetBytes(signingInput);
            byte[] signature = SignPs384(signingBytes);

            return $"{signingInput}.{Base64UrlEncode(signature)}";
        }

        private byte[] SignPs384(byte[] data)
        {
            ISigner signer = SignerUtilities.GetSigner("SHA-384withRSAandMGF1");
            signer.Init(true, _keyPair.Private);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input)
                .Replace('+', '-')
                .Replace('/', '_')
                .TrimEnd('=');
        }
    }
}
