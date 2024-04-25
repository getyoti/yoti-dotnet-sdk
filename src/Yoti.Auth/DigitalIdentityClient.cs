using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DigitalIdentity;

namespace Yoti.Auth
{
    public class DigitalIdentityClient
    {
        private readonly string _sdkId;
        private readonly AsymmetricCipherKeyPair _keyPair;
        private readonly DigitalIdentityClientEngine _yotiDigitalClientEngine;
        internal Uri ApiUri { get; private set; }

        /// <summary>
        /// Create a <see cref="YotiClient"/>
        /// </summary>
        /// <param name="sdkId">The client SDK ID provided on the Yoti Hub.</param>
        /// <param name="privateKeyStream">
        /// The private key file provided on the Yoti Hub as a <see cref="StreamReader"/>.
        /// </param>
        public DigitalIdentityClient(string sdkId, StreamReader privateKeyStream)
            : this(new HttpClient(), sdkId, CryptoEngine.LoadRsaKey(privateKeyStream))
        {
        }

        /// <summary>
        /// Create a <see cref="YotiClient"/> with a specified <see cref="HttpClient"/>
        /// </summary>
        /// <param name="httpClient">Allows the specification of a HttpClient</param>
        /// <param name="sdkId">The client SDK ID provided on the Yoti Hub.</param>
        /// <param name="privateKeyStream">
        /// The private key file provided on the Yoti Hub as a <see cref="StreamReader"/>.
        /// </param>
        public DigitalIdentityClient(HttpClient httpClient, string sdkId, StreamReader privateKeyStream)
            : this(httpClient, sdkId, CryptoEngine.LoadRsaKey(privateKeyStream))
        {
        }

        /// <summary>
        /// Create a <see cref="YotiClient"/> with a specified <see cref="HttpClient"/>
        /// </summary>
        /// <param name="httpClient">Allows the specification of a HttpClient</param>
        /// <param name="sdkId">The client SDK ID provided on the Yoti Hub.</param>
        /// <param name="keyPair">The key pair from the Yoti Hub.</param>
        public DigitalIdentityClient(HttpClient httpClient, string sdkId, AsymmetricCipherKeyPair keyPair)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));

            _sdkId = sdkId;
            _keyPair = keyPair;

            SetYotiApiUri();

            _yotiDigitalClientEngine = new DigitalIdentityClientEngine(httpClient);
        }
        
        /// <summary>
        /// Initiate a sharing process based on a <see cref="ShareSessionRequest"/>.
        /// </summary>
        /// <param name="shareSessionRequest">
        /// Details of the device's callback endpoint, <see
        /// cref="Yoti.Auth.DigitalIdentity.Policy"/> and extensions for the application
        /// </param>
        /// <returns><see cref="ShareSessionResult"/></returns>
        public ShareSessionResult CreateShareSession(ShareSessionRequest shareSessionRequest)
        {
            Task<ShareSessionResult> task = Task.Run(async () => await CreateShareSessionAsync(shareSessionRequest).ConfigureAwait(false));

            return task.Result;
        }

        /// <summary>
        /// Asynchronously initiate a sharing process based on a <see cref="ShareSessionRequest"/>.
        /// </summary>
        /// <param name="shareSessionRequest">
        /// Details of the device's callback endpoint, <see
        /// cref="Yoti.Auth.DigitalIdentity.Policy"/> and extensions for the application
        /// </param>
        /// <returns><see cref="ShareSessionResult"/></returns>
        public async Task<ShareSessionResult> CreateShareSessionAsync(ShareSessionRequest shareSessionRequest)
        {
            return await _yotiDigitalClientEngine.CreateShareSessionAsync(_sdkId, _keyPair, ApiUri, shareSessionRequest).ConfigureAwait(false);
        }

        public SharedReceiptResponse GetShareReceipt(string receiptId)
        {
            Task<SharedReceiptResponse> task = Task.Run(async () => await _yotiDigitalClientEngine.GetShareReceipt(_sdkId, _keyPair, ApiUri, receiptId).ConfigureAwait(false));
            return task.Result;
        }

        internal void SetYotiApiUri()
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("YOTI_API_URL")))
            {
                ApiUri = new Uri(Environment.GetEnvironmentVariable("YOTI_API_URL"));
            }
            else
            {
                ApiUri = new Uri(Constants.Api.DefaultYotiShareApiUrl);
            }
        }

        public DigitalIdentityClient OverrideApiUri(Uri apiUri)
        {
            ApiUri = apiUri;

            return this;
        }
    }
}
