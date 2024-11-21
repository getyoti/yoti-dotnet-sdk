using System;
#pragma warning disable S1128
using System.Net;
#pragma warning restore S1128
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DigitalIdentity;

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

        public async Task<SharedReceiptResponse> GetShareReceipt(string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl, string receiptId)
        {
            SharedReceiptResponse result = await Task.Run(async () => await DigitalIdentityService.GetShareReceipt(
                _httpClient, sdkId, apiUrl, keyPair, receiptId).ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }
        
        public async Task<CreateQrResult> CreateQrCodeAsync(string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl, string sessionid, QrRequest qRRequest)
        {
            CreateQrResult result = await Task.Run(async () => await DigitalIdentityService.CreateQrCode(
                    _httpClient, apiUrl, sdkId, keyPair, sessionid, qRRequest).ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }
        
        public async Task<GetQrCodeResult> GetQrCodeAsync(string sdkId, AsymmetricCipherKeyPair keyPair, Uri apiUrl, string qrcodeId)
        {
            GetQrCodeResult result = await Task.Run(async () => await DigitalIdentityService.GetQrCode(
                    _httpClient, apiUrl, sdkId, keyPair, qrcodeId).ConfigureAwait(false))
                .ConfigureAwait(false);

            return result;
        }
    }
}
