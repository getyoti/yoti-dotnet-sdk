using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;
using Yoti.Auth.Exceptions;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.Web;

namespace Yoti.Auth
{
    internal class YotiClientEngine
    {
        private readonly HttpClient _httpClient;

        public YotiClientEngine(HttpClient httpClient)
        {
            _httpClient = httpClient;

#if NET452 || NET462 || NET472 || NET48
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
        }

        public async Task<ActivityDetails> GetActivityDetailsAsync(string encryptedConnectToken, string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl)
        {
            string token = CryptoEngine.DecryptToken(encryptedConnectToken, keyPair);
            string path = $"profile/{token}";

            Request profileRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(apiUrl)
                .WithEndpoint(path)
                .WithQueryParam("appId", sdkId)
                .WithHeader(Constants.Api.AuthKeyHeader, CryptoEngine.GetAuthKey(keyPair))
                .Build();

            using HttpResponseMessage response = await profileRequest.Execute(_httpClient).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
                Response.CreateExceptionFromStatusCode<YotiProfileException>(response);

            return ActivityDetailsParser.HandleResponse(
                keyPair,
                await response.Content.ReadAsStringAsync().ConfigureAwait(true));
        }

        public Task<AmlResult> PerformAmlCheckAsync(string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl, IAmlProfile amlProfile)
        {
            if (apiUrl == null)
            {
                throw new ArgumentNullException(nameof(apiUrl));
            }

            if (amlProfile == null)
            {
                throw new ArgumentNullException(nameof(amlProfile));
            }

            return PerformAmlCheckInternalAsync(sdkId, keyPair, apiUrl, amlProfile);
        }

        private async Task<AmlResult> PerformAmlCheckInternalAsync(string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl, IAmlProfile amlProfile)
        {
            string serializedProfile = Newtonsoft.Json.JsonConvert.SerializeObject(amlProfile);
            byte[] httpContent = System.Text.Encoding.UTF8.GetBytes(serializedProfile);

            AmlResult result = await Task.Run(async () => await new RemoteAmlService()
                .PerformCheck(
                _httpClient, keyPair, apiUrl, sdkId, httpContent).ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }

        public async Task<ShareUrlResult> CreateShareURLAsync(string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl, DynamicScenario dynamicScenario)
        {
            ShareUrlResult result = await Task.Run(async () => await DynamicSharingService.CreateShareURL(
                _httpClient, apiUrl, sdkId, keyPair, dynamicScenario).ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }
    }
}