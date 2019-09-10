using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Web;

namespace Yoti.Auth.Aml
{
    internal class RemoteAmlService : IRemoteAmlService
    {
        public async Task<AmlResult> PerformCheck(
            HttpClient httpClient,
            AsymmetricCipherKeyPair keyPair,
            Uri apiUrl,
            string sdkId,
            byte[] httpContent)
        {
            Request amlRequest = new RequestBuilder()
               .WithKeyPair(keyPair)
               .WithBaseUri(apiUrl)
               .WithEndpoint("/aml-check")
               .WithQueryParam("appId", sdkId)
               .WithHttpMethod(HttpMethod.Post)
               .WithContent(httpContent)
               .Build();

            using (HttpResponseMessage response = await amlRequest.Execute(httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateExceptionFromStatusCode<AmlException>(response);

                return JsonConvert.DeserializeObject<AmlResult>(
                    await response.Content.ReadAsStringAsync().ConfigureAwait(true));
            }
        }
    }
}