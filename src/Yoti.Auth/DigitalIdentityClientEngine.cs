using System;
#pragma warning disable S1128
using System.Net;
#pragma warning restore S1128
using System.Net.Http;
using System.Threading.Tasks;
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

        public async Task<ShareSessionResult> CreateShareSessionAsync(IAuthStrategy authStrategy, Uri apiUrl, ShareSessionRequest shareSessionRequest)
        {
            return await Task.Run(async () => await DigitalIdentityService.CreateShareSession(
                _httpClient, apiUrl, authStrategy, shareSessionRequest).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<SharedReceiptResponse> GetShareReceipt(IAuthStrategy authStrategy, Uri apiUrl, string receiptId)
        {
            return await Task.Run(async () => await DigitalIdentityService.GetShareReceipt(
                _httpClient, apiUrl, authStrategy, receiptId).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<CreateQrResult> CreateQrCodeAsync(IAuthStrategy authStrategy, Uri apiUrl, string sessionId, QrRequest qrRequest)
        {
            return await Task.Run(async () => await DigitalIdentityService.CreateQrCode(
                _httpClient, apiUrl, authStrategy, sessionId, qrRequest).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<GetQrCodeResult> GetQrCodeAsync(IAuthStrategy authStrategy, Uri apiUrl, string qrCodeId)
        {
            return await Task.Run(async () => await DigitalIdentityService.GetQrCode(
                _httpClient, apiUrl, authStrategy, qrCodeId).ConfigureAwait(false))
                .ConfigureAwait(false);
        }

        public async Task<GetSessionResult> GetSession(IAuthStrategy authStrategy, Uri apiUrl, string sessionId)
        {
            return await Task.Run(async () => await DigitalIdentityService.GetSession(
                _httpClient, apiUrl, authStrategy, sessionId).ConfigureAwait(false))
                .ConfigureAwait(false);
        }
    }
}
