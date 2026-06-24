using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Web;

namespace Yoti.Auth
{
    public class DigitalIdentityClient
    {
        private readonly IAuthStrategy _authStrategy;
        private readonly DigitalIdentityClientEngine _yotiDigitalClientEngine;
        internal Uri ApiUri { get; private set; }

        /// <summary>
        /// Create a <see cref="DigitalIdentityClient"/> using signed-request authentication.
        /// </summary>
        public DigitalIdentityClient(string sdkId, StreamReader privateKeyStream)
            : this(new HttpClient(), sdkId, CryptoEngine.LoadRsaKey(privateKeyStream))
        {
        }

        /// <summary>
        /// Create a <see cref="DigitalIdentityClient"/> using signed-request authentication with a specified <see cref="HttpClient"/>.
        /// </summary>
        public DigitalIdentityClient(HttpClient httpClient, string sdkId, StreamReader privateKeyStream)
            : this(httpClient, sdkId, CryptoEngine.LoadRsaKey(privateKeyStream))
        {
        }

        /// <summary>
        /// Create a <see cref="DigitalIdentityClient"/> using signed-request authentication with a specified <see cref="HttpClient"/>.
        /// </summary>
        public DigitalIdentityClient(HttpClient httpClient, string sdkId, AsymmetricCipherKeyPair keyPair)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));

            _authStrategy = new SignedRequestAuthStrategy(keyPair, sdkId);
            SetYotiApiUri();
            _yotiDigitalClientEngine = new DigitalIdentityClientEngine(httpClient);
        }

        /// <summary>
        /// Creates a <see cref="DigitalIdentityClient"/> using central auth bearer token authentication.
        /// Use this factory method instead of a constructor to avoid ambiguity with the sdkId overloads.
        /// </summary>
        /// <param name="authToken">The bearer token supplied by the relying business.</param>
        public static DigitalIdentityClient FromBearerToken(string authToken)
            => FromBearerToken(new HttpClient(), authToken);

        /// <summary>
        /// Creates a <see cref="DigitalIdentityClient"/> using central auth bearer token authentication with a specified <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="httpClient">The <see cref="HttpClient"/> to use.</param>
        /// <param name="authToken">The bearer token supplied by the relying business.</param>
        public static DigitalIdentityClient FromBearerToken(HttpClient httpClient, string authToken)
        {
            Validation.NotNullOrEmpty(authToken, nameof(authToken));
            return new DigitalIdentityClient(new BearerTokenAuthStrategy(authToken), httpClient);
        }

        private DigitalIdentityClient(IAuthStrategy authStrategy, HttpClient httpClient)
        {
            _authStrategy = authStrategy;
            SetYotiApiUri();
            _yotiDigitalClientEngine = new DigitalIdentityClientEngine(httpClient);
        }

        public ShareSessionResult CreateShareSession(ShareSessionRequest shareSessionRequest)
        {
            Task<ShareSessionResult> task = Task.Run(async () => await CreateShareSessionAsync(shareSessionRequest).ConfigureAwait(false));
            return task.Result;
        }

        public async Task<ShareSessionResult> CreateShareSessionAsync(ShareSessionRequest shareSessionRequest)
        {
            return await _yotiDigitalClientEngine.CreateShareSessionAsync(_authStrategy, ApiUri, shareSessionRequest).ConfigureAwait(false);
        }

        public SharedReceiptResponse GetShareReceipt(string receiptId)
        {
            Task<SharedReceiptResponse> task = Task.Run(async () => await _yotiDigitalClientEngine.GetShareReceipt(_authStrategy, ApiUri, receiptId).ConfigureAwait(false));
            return task.Result;
        }

        public async Task<CreateQrResult> CreateQrCode(string sessionId, QrRequest qrRequest)
        {
            return await _yotiDigitalClientEngine.CreateQrCodeAsync(_authStrategy, ApiUri, sessionId, qrRequest).ConfigureAwait(false);
        }

        public async Task<GetQrCodeResult> GetQrCode(string qrCodeId)
        {
            return await _yotiDigitalClientEngine.GetQrCodeAsync(_authStrategy, ApiUri, qrCodeId).ConfigureAwait(false);
        }

        public async Task<GetSessionResult> GetSession(string sessionId)
        {
            return await _yotiDigitalClientEngine.GetSession(_authStrategy, ApiUri, sessionId).ConfigureAwait(false);
        }

        internal void SetYotiApiUri()
        {
            string envUrl = Environment.GetEnvironmentVariable("YOTI_API_URL");
            ApiUri = !string.IsNullOrEmpty(envUrl)
                ? new Uri(envUrl)
                : new Uri(Constants.Api.DefaultYotiShareApiUrl);
        }

        public DigitalIdentityClient OverrideApiUri(Uri apiUri)
        {
            ApiUri = apiUri;
            return this;
        }
    }
}
