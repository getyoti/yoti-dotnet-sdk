using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Web;

namespace Yoti.Auth.DigitalIdentity
{
    public static class DigitalIdentityService
    {
        internal static async Task<ShareSessionResult> CreateShareSession(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, ShareSessionRequest shareSessionRequest)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(shareSessionRequest, nameof(shareSessionRequest));

            string serializedScenario = JsonConvert.SerializeObject(
                shareSessionRequest,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            byte[] body = Encoding.UTF8.GetBytes(serializedScenario);

            Request shareSessionlRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint($"/v2/sessions")
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(body)
                .Build();

            using (HttpResponseMessage response = await shareSessionlRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DynamicShareException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<ShareSessionResult>(responseObject));

                return deserialized;

            }
        }

        internal static async Task<GetSessionResult> GetSession(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(sessionId, nameof(sessionId));           

            Request shareSessionlRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint(string.Format($"/v2/sessions/{0}", sessionId))
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Get)
                .Build();

            using (HttpResponseMessage response = await shareSessionlRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DynamicShareException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetSessionResult>(responseObject));

                return deserialized;

            }
        }

        internal static async Task<CreateQrResult> CreateQrCode(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId,QrRequest qrRequestPayload)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            
            string serializedQrCode = JsonConvert.SerializeObject(
                qrRequestPayload,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            byte[] body = Encoding.UTF8.GetBytes(serializedQrCode);

            Request qrRequest = new RequestBuilder()
                .WithKeyPair(keyPair)
                .WithBaseUri(apiUrl)
                .WithHeader("X-Yoti-Auth-Id", sdkId)
                .WithEndpoint(string.Format($"/v2/sessions/{0}/qr-codes", sessionId))
                .WithQueryParam("appId", sdkId)
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(body)
                .Build();

            using (HttpResponseMessage response = await qrRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DynamicShareException>(response);
                }

                var responseObject = await response.Content.ReadAsStringAsync();
                var deserialized = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CreateQrResult>(responseObject));

                return deserialized;

            }
        }

    }
}