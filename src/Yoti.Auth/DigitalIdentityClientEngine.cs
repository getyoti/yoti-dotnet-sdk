using System;

#pragma warning disable S1128

using System.Net;

#pragma warning restore S1128

using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;
using Yoti.Auth.Exceptions;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Web;

namespace Yoti.Auth
{
    internal class DigitalIdentityClientEngine
    {
        private readonly HttpClient _httpClient;

        public DigitalIdentityClientEngine(HttpClient httpClient)
        {
            _httpClient = httpClient;

#if NET452 || NET462 || NET472 || NET48
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
#endif
        }

        
        public async Task<ShareSessionResult> CreateShareSessionAsync(string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl, ShareSessionRequest shareSessionRequest)
        {
            ShareSessionResult result = await Task.Run(async () => await DigitalIdentityService.CreateShareSession(
                _httpClient, apiUrl, sdkId, keyPair, shareSessionRequest).ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }
    }
}