using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Web;

namespace Yoti.Auth.ShareUrl
{
    public static class DynamicSharingService
    {
        internal static async Task<ShareUrlResult> CreateShareURL(HttpClient httpClient, IHttpRequester httpRequester, string apiUrl, string sdkId, AsymmetricCipherKeyPair keyPair, DynamicScenario dynamicScenario)
        {
            Validation.NotNull(httpClient, nameof(httpClient));
            Validation.NotNull(httpRequester, nameof(httpRequester));
            Validation.NotNull(apiUrl, nameof(apiUrl));
            Validation.NotNull(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(dynamicScenario, nameof(dynamicScenario));

            string endpoint = EndpointFactory.CreateDynamicSharingPath(sdkId);
            HttpMethod httpMethod = HttpMethod.Post;

            string serializedScenario = Newtonsoft.Json.JsonConvert.SerializeObject(dynamicScenario);
            byte[] body = Encoding.UTF8.GetBytes(serializedScenario);

            Dictionary<string, string> headers = HeadersFactory.Create(keyPair, httpMethod, endpoint, body);

            Response response = await httpRequester.DoRequest(
                httpClient,
                httpMethod,
                new Uri(apiUrl + endpoint),
                headers,
                body).ConfigureAwait(false);

            if (!response.Success)
            {
                Response.CreateExceptionFromStatusCode<DynamicShareException>(response);
            }

            var dynamicShareResult = Newtonsoft.Json.JsonConvert.DeserializeObject<ShareUrlResult>(response.Content);

            return dynamicShareResult;
        }
    }
}