using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;

namespace Yoti.Auth
{
    internal class YotiClientEngine
    {
        private readonly IHttpRequester _httpRequester;
        private Activity _activity;
        private RemoteAmlService _remoteAmlService = null;

        public YotiClientEngine(IHttpRequester httpRequester)
        {
            _httpRequester = httpRequester;
            _activity = new Activity();

#if !NETSTANDARD1_6
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
        }

        /// <summary>
        /// Request a <see cref="ActivityDetails"/>  using the encrypted token provided by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>. </returns>
        public ActivityDetails GetActivityDetails(string encryptedToken, string sdkId, AsymmetricCipherKeyPair keyPair, string apiUrl)
        {
            Task<ActivityDetails> task = Task.Run<ActivityDetails>(async () => await GetActivityDetailsAsync(encryptedToken, sdkId, keyPair, apiUrl));

            return task.Result;
        }

        /// <summary>
        /// Asynchronously request a <see cref="ActivityDetails"/>  using the encrypted token provided by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>. </returns>
        public async Task<ActivityDetails> GetActivityDetailsAsync(string encryptedConnectToken, string sdkId, AsymmetricCipherKeyPair keyPair, string apiUrl)
        {
            string token = CryptoEngine.DecryptToken(encryptedConnectToken, keyPair);
            string path = "profile";
            byte[] httpContent = null;
            HttpMethod httpMethod = HttpMethod.Get;

            string endpoint = EndpointFactory.CreateProfileEndpoint(httpMethod, path, token, sdkId);

            Dictionary<string, string> headers = CreateHeaders(keyPair, httpMethod, endpoint, httpContent, contentType: YotiConstants.ContentTypeJson);

            Response response = await _httpRequester.DoRequest(
                new HttpClient(),
                HttpMethod.Get,
                new Uri(
                    apiUrl + endpoint),
                headers,
                httpContent);

            if (response.Success)
            {
                return _activity.HandleSuccessfulResponse(keyPair, response);
            }
            else
            {
                ActivityOutcome outcome = ActivityOutcome.Failure;
                switch (response.StatusCode)
                {
                    case (int)HttpStatusCode.NotFound:
                        {
                            outcome = ActivityOutcome.ProfileNotFound;
                        }
                        break;
                }

                return new ActivityDetails
                {
                    Outcome = outcome
                };
            }
        }

        /// <summary>
        /// Request a <see cref="ActivityDetails"/>  using the encrypted token provided by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>. </returns>
        public AmlResult PerformAmlCheck(string appId, AsymmetricCipherKeyPair keyPair, string apiUrl, AmlProfile amlProfile)
        {
            Task<AmlResult> task = Task.Run(async () => await PerformAmlCheckAsync(appId, keyPair, apiUrl, amlProfile));

            return task.Result;
        }

        /// <summary>
        /// Asynchronously request a <see cref="ActivityDetails"/>  using the encrypted token provided by yoti during the login process.
        /// </summary>
        /// <param name="encryptedToken">The encrypted returned by Yoti after successfully authenticating.</param>
        /// <returns>The account details of the logged in user as a <see cref="ActivityDetails"/>. </returns>
        public async Task<AmlResult> PerformAmlCheckAsync(string appId, AsymmetricCipherKeyPair keyPair, string apiUrl, AmlProfile amlProfile)
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

            Dictionary<string, string> headers = CreateHeaders(keyPair, httpMethod, endpoint, httpContent, contentType: YotiConstants.ContentTypeJson);

            AmlResult result = await Task.Run(async () => await new RemoteAmlService()
                .PerformCheck(amlProfile, headers, apiUrl, endpoint, httpContent));

            return result;
        }

        private static Dictionary<string, string> CreateHeaders(AsymmetricCipherKeyPair keyPair, HttpMethod httpMethod, string endpoint, byte[] httpContent, string contentType)
        {
            string authKey = CryptoEngine.GetAuthKey(keyPair);
            string authDigest = SignedMessageFactory.SignMessage(httpMethod, endpoint, keyPair, httpContent);

            if (string.IsNullOrEmpty(authDigest))
                throw new InvalidOperationException("Could not sign request");

            Dictionary<string, string> headers = new Dictionary<string, string>
            {
                { YotiConstants.AuthKeyHeader, authKey },
                { YotiConstants.DigestHeader, authDigest },
                { YotiConstants.YotiSdkHeader, YotiConstants.SdkIdentifier }
            };

            return headers;
        }
    }
}