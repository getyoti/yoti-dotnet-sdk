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

            Request shareUrlRequest = new RequestBuilder()
                 .WithKeyPair(keyPair)
                 .WithBaseUri(apiUrl)
                 .WithEndpoint($"/sessions/v2")
                 .WithHttpMethod(HttpMethod.Post)
                 .WithContent(body)
                 .Build();

            using (HttpResponseMessage response = await shareUrlRequest.Execute(httpClient).ConfigureAwait(false))
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
    }
}