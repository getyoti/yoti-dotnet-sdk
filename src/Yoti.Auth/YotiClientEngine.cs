using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;

namespace Yoti.Auth
{
    internal class YotiClientEngine
    {
        private readonly IHttpRequester _httpRequester;

        public YotiClientEngine(IHttpRequester httpRequester)
        {
            _httpRequester = httpRequester;

#if !NETSTANDARD1_6
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
        }

        public ActivityDetails GetActivityDetails(string encryptedToken, string sdkId, AsymmetricCipherKeyPair keyPair, string apiUrl)
        {
            Task<ActivityDetails> task = Task.Run<ActivityDetails>(async () => await GetActivityDetailsAsync(encryptedToken, sdkId, keyPair, apiUrl));

            return task.Result;
        }

        public async Task<ActivityDetails> GetActivityDetailsAsync(string encryptedConnectToken, string sdkId, AsymmetricCipherKeyPair keyPair, string apiUrl)
        {
            string token = CryptoEngine.DecryptToken(encryptedConnectToken, keyPair);
            const string path = "profile";
            byte[] httpContent = null;
            HttpMethod httpMethod = HttpMethod.Get;

            string endpoint = EndpointFactory.CreateProfileEndpoint(httpMethod, path, token, sdkId);

            Dictionary<string, string> headers = CreateHeaders(keyPair, httpMethod, endpoint, httpContent: null);

            Response response = await _httpRequester.DoRequest(
                new HttpClient(),
                HttpMethod.Get,
                new Uri(
                    apiUrl + endpoint),
                headers,
                httpContent);

            return ProfileParser.HandleResponse(keyPair, response);
        }

        public AmlResult PerformAmlCheck(string appId, AsymmetricCipherKeyPair keyPair, string apiUrl, IAmlProfile amlProfile)
        {
            Task<AmlResult> task = Task.Run(async () => await PerformAmlCheckAsync(appId, keyPair, apiUrl, amlProfile));

            return task.Result;
        }

        public async Task<AmlResult> PerformAmlCheckAsync(string appId, AsymmetricCipherKeyPair keyPair, string apiUrl, IAmlProfile amlProfile)
        {
            if (apiUrl == null)
            {
                throw new ArgumentNullException(nameof(apiUrl));
            }

            if (amlProfile == null)
            {
                throw new ArgumentNullException(nameof(amlProfile));
            }

            string serializedProfile = Newtonsoft.Json.JsonConvert.SerializeObject(amlProfile);
            byte[] httpContent = System.Text.Encoding.UTF8.GetBytes(serializedProfile);

            HttpMethod httpMethod = HttpMethod.Post;

            string endpoint = EndpointFactory.CreateAmlEndpoint(httpMethod, appId);

            Dictionary<string, string> headers = CreateHeaders(keyPair, httpMethod, endpoint, httpContent);

            AmlResult result = await Task.Run(async () => await new RemoteAmlService()
                .PerformCheck(_httpRequester, amlProfile, headers, apiUrl, endpoint, httpContent));

            return result;
        }

        private static Dictionary<string, string> CreateHeaders(AsymmetricCipherKeyPair keyPair, HttpMethod httpMethod, string endpoint, byte[] httpContent)
        {
            string authKey = CryptoEngine.GetAuthKey(keyPair);
            string authDigest = SignedMessageFactory.SignMessage(httpMethod, endpoint, keyPair, httpContent);

            if (string.IsNullOrEmpty(authDigest))
                throw new InvalidOperationException("Could not sign request");

            string SDKVersion = typeof(YotiClientEngine).GetTypeInfo().Assembly.GetName().Version.ToString();

            var headers = new Dictionary<string, string>
            {
                { Constants.Web.AuthKeyHeader, authKey },
                { Constants.Web.DigestHeader, authDigest },
                { Constants.Web.YotiSdkHeader, Constants.Web.SdkIdentifier },
                { Constants.Web.YotiSdkVersionHeader,
                    $"{Constants.Web.SdkIdentifier}-{SDKVersion}" }
            };

            return headers;
        }
    }
}