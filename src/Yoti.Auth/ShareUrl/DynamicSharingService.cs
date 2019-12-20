using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Web;

namespace Yoti.Auth.ShareUrl
{
    public static class DynamicSharingService
    {
        internal static async Task<ShareUrlResult> CreateShareURL(HttpClient httpClient, Uri apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, DynamicScenario dynamicScenario)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(dynamicScenario, nameof(dynamicScenario));

            string serializedScenario = JsonConvert.SerializeObject(
                dynamicScenario,
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });
            byte[] body = Encoding.UTF8.GetBytes(serializedScenario);

            Request shareUrlRequest = new RequestBuilder()
                 .WithKeyPair(keyPair)
                 .WithBaseUri(apiUrl)
                 .WithEndpoint($"/qrcodes/apps/{sdkId}")
                 .WithHttpMethod(HttpMethod.Post)
                 .WithContent(body)
                 .Build();

            using (HttpResponseMessage response = await shareUrlRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateExceptionFromStatusCode<DynamicShareException>(response);
                }

                return JsonConvert.DeserializeObject<ShareUrlResult>(
                    response.Content.ReadAsStringAsync().Result);
            }
        }
    }
}