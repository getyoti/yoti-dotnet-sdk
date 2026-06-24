using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.FaceCapture;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration;
using Yoti.Auth.DocScan.Session.Retrieve.CreateFaceCaptureResourceResponse;
using Yoti.Auth.DocScan.Support;
using Yoti.Auth.Web;

namespace Yoti.Auth.DocScan
{
    /// <summary>
    /// Client used for communication with the Yoti Doc Scan service.
    /// </summary>
    public class DocScanClient
    {
        private readonly IAuthStrategy _authStrategy;
        private readonly DocScanService _docScanService;
        private readonly NLog.Logger _logger;

        public DocScanClient(string sdkId, StreamReader privateKeyStream, HttpClient httpClient = null, Uri apiUri = null)
            : this(sdkId, CryptoEngine.LoadRsaKey(privateKeyStream), httpClient, apiUri)
        {
        }

        public DocScanClient(string sdkId, AsymmetricCipherKeyPair keyPair, HttpClient httpClient = null, Uri apiUrl = null)
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();

            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));

            if (httpClient == null)
                httpClient = new HttpClient();

            _authStrategy = new SignedRequestAuthStrategy(keyPair, sdkId);
            _docScanService = new DocScanService(httpClient, apiUrl);
        }

        /// <summary>
        /// Creates a <see cref="DocScanClient"/> using central auth bearer token authentication.
        /// Use this factory method instead of a constructor to avoid ambiguity with the sdkId overloads.
        /// </summary>
        /// <param name="authToken">The bearer token supplied by the relying business.</param>
        /// <param name="httpClient">Optional <see cref="HttpClient"/> to use.</param>
        /// <param name="apiUri">Optional API URI override.</param>
        public static DocScanClient FromBearerToken(string authToken, HttpClient httpClient = null, Uri apiUri = null)
        {
            Validation.NotNullOrEmpty(authToken, nameof(authToken));
            return new DocScanClient(new BearerTokenAuthStrategy(authToken), httpClient ?? new HttpClient(), apiUri);
        }

        private DocScanClient(IAuthStrategy authStrategy, HttpClient httpClient, Uri apiUri)
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _authStrategy = authStrategy;
            _docScanService = new DocScanService(httpClient, apiUri);
        }

        /// <summary>
        /// Creates a Doc Scan session using the supplied session specification
        /// </summary>
        /// <param name="sessionSpec">the Doc Scan session specification</param>
        /// <returns>the session creation result</returns>
        public async Task<CreateSessionResult> CreateSessionAsync(SessionSpecification sessionSpec)
        {
            _logger.Debug("Creating a Yoti Doc Scan session...");

            return await _docScanService.CreateSession(_authStrategy, sessionSpec).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a Doc Scan session using the supplied session specification
        /// </summary>
        /// <param name="sessionSpec">the Doc Scan session specification</param>
        /// <returns>the session creation result</returns>
        public CreateSessionResult CreateSession(SessionSpecification sessionSpec)
        {
            return CreateSessionAsync(sessionSpec).Result;
        }

        /// <summary>
        /// Retrieves the state of a previously created Yoti Doc Scan session
        /// </summary>
        /// <param name="sessionId">The ID of the session</param>
        /// <returns>The session state</returns>
        public async Task<GetSessionResult> GetSessionAsync(string sessionId)
        {
            _logger.Debug($"Retrieving session '{sessionId}'");

            return await _docScanService.GetSession(_authStrategy, sessionId).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the state of a previously created Yoti Doc Scan session
        /// </summary>
        /// <param name="sessionId">The ID of the session</param>
        /// <returns>The session state</returns>
        public GetSessionResult GetSession(string sessionId)
        {
            return GetSessionAsync(sessionId).Result;
        }

        /// <summary>
        /// Deletes a previously created Yoti Doc Scan session and all of its related resources
        /// </summary>
        /// <param name="sessionId">The session ID</param>
        public async Task DeleteSessionAsync(string sessionId)
        {
            _logger.Debug($"Deleting session '{sessionId}'");

            await _docScanService.DeleteSession(_authStrategy, sessionId).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a previously created Yoti Doc Scan session and all of its related resources
        /// </summary>
        /// <param name="sessionId">The session ID</param>
        public void DeleteSession(string sessionId)
        {
            DeleteSessionAsync(sessionId).Wait();
        }

        /// <summary>
        /// Retrieves media related to a Yoti Doc Scan session based on the supplied media ID
        /// </summary>
        /// <param name="sessionId">The Session ID</param>
        /// <param name="mediaId">The Media ID</param>
        /// <returns>The Media</returns>
        public async Task<MediaValue> GetMediaContentAsync(string sessionId, string mediaId)
        {
            _logger.Debug($"Retrieving media content '{mediaId}' in session '{sessionId}'");

            return await _docScanService.GetMediaContent(_authStrategy, sessionId, mediaId).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves media related to a Yoti Doc Scan session based on the supplied media ID
        /// </summary>
        /// <param name="sessionId">The Session ID</param>
        /// <param name="mediaId">The Media ID</param>
        public MediaValue GetMediaContent(string sessionId, string mediaId)
        {
            return GetMediaContentAsync(sessionId, mediaId).Result;
        }

        /// <summary>
        /// Deletes media related to a Yoti Doc Scan session based on the supplied media ID
        /// </summary>
        /// <param name="sessionId">The Session ID</param>
        /// <param name="mediaId">The Media ID</param>
        public void DeleteMediaContent(string sessionId, string mediaId)
        {
            DeleteMediaContentAsync(sessionId, mediaId).Wait();
        }

        /// <summary>
        /// Deletes media related to a Yoti Doc Scan session based on the supplied media ID
        /// </summary>
        /// <param name="sessionId">The Session ID</param>
        /// <param name="mediaId">The Media ID</param>
        public async Task DeleteMediaContentAsync(string sessionId, string mediaId)
        {
            _logger.Debug($"Deleting media content '{mediaId}' in session '{sessionId}'");

            await _docScanService.DeleteMediaContent(_authStrategy, sessionId, mediaId).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets a list of supported documents.
        /// </summary>
        /// <returns>The supported documents</returns>
        public SupportedDocumentsResponse GetSupportedDocuments(bool isStrictlyLatin)
        {
            return GetSupportedDocumentsAsync(isStrictlyLatin).Result;
        }

        /// <summary>
        /// Gets a list of supported documents.
        /// </summary>
        /// <returns>The supported documents</returns>
        public async Task<SupportedDocumentsResponse> GetSupportedDocumentsAsync(bool isStrictlyLatin)
        {
            _logger.Debug("Retrieving supported documents");

            return await _docScanService.GetSupportedDocuments(_authStrategy, isStrictlyLatin).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a Face Capture resource, that will be linked using the supplied Requirement Id (a property of <see cref="CreateFaceCaptureResourcePayload"/>).
        /// </summary>
        /// <param name="sessionId">The Session Id</param>
        /// <param name="createFaceCaptureResourcePayload">Object containing Requirement Id</param>
        /// <returns>The <see cref="CreateFaceCaptureResourceResponse"/> Response</returns>
        public CreateFaceCaptureResourceResponse CreateFaceCaptureResource(string sessionId, CreateFaceCaptureResourcePayload createFaceCaptureResourcePayload)
        {
            return CreateFaceCaptureResourceAsync(sessionId, createFaceCaptureResourcePayload).Result;
        }

        /// <summary>
        /// Creates a Face Capture resource, that will be linked using the supplied Requirement Id (a property of <see cref="CreateFaceCaptureResourcePayload"/>).
        /// </summary>
        /// <param name="sessionId">The Session Id</param>
        /// <param name="createFaceCaptureResourcePayload">Object containing Requirement Id</param>
        /// <returns>The <see cref="CreateFaceCaptureResourceResponse"/> Response</returns>
        public async Task<CreateFaceCaptureResourceResponse> CreateFaceCaptureResourceAsync(string sessionId, CreateFaceCaptureResourcePayload createFaceCaptureResourcePayload)
        {
            _logger.Debug($"Creating Face Capture resource in session '{sessionId}' for requirement '{createFaceCaptureResourcePayload.RequirementId}'");

            return await _docScanService.CreateFaceCaptureResource(_authStrategy, sessionId, createFaceCaptureResourcePayload).ConfigureAwait(false);
        }

        /// <summary>
        /// Uploads an image to the specified Face Capture resource.
        /// </summary>
        /// <param name="sessionId">The Session Id</param>
        /// <param name="resourceId">The Resource Id</param>
        /// <param name="uploadFaceCaptureImagePayload">The Face Capture image payload as binary data and a content type (mime type)</param>
        public void UploadFaceCaptureImage(string sessionId, string resourceId, UploadFaceCaptureImagePayload uploadFaceCaptureImagePayload)
        {
            UploadFaceCaptureImageAsync(sessionId, resourceId, uploadFaceCaptureImagePayload).Wait();
        }

        /// <summary>
        /// Uploads an image to the specified Face Capture resource.
        /// </summary>
        /// <param name="sessionId">The Session Id</param>
        /// <param name="resourceId">The Resource Id</param>
        /// <param name="uploadFaceCaptureImagePayload">The Face Capture image payload as binary data and a content type (mime type)</param>
        public async Task UploadFaceCaptureImageAsync(string sessionId, string resourceId, UploadFaceCaptureImagePayload uploadFaceCaptureImagePayload)
        {
            _logger.Debug($"Uploading image to Face Capture resource '{resourceId}' for session '{sessionId}'");

            await _docScanService.UploadFaceCaptureImage(_authStrategy, sessionId, resourceId, uploadFaceCaptureImagePayload).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the configuration for a specific session.
        /// </summary>
        /// <param name="sessionId">The Session Id</param>
        /// <returns>The <see cref="SessionConfigurationResponse"/> Response</returns>
        public SessionConfigurationResponse GetSessionConfiguration(string sessionId)
        {
            return GetSessionConfigurationAsync(sessionId).Result;
        }

        /// <summary>
        /// Gets the configuration for a specific session.
        /// </summary>
        /// <param name="sessionId">The Session Id</param>
        /// <returns>The <see cref="SessionConfigurationResponse"/> Response</returns>
        public async Task<SessionConfigurationResponse> GetSessionConfigurationAsync(string sessionId)
        {
            _logger.Debug($"Getting configuration for session '{sessionId}'");

            return await _docScanService.GetSessionConfiguration(_authStrategy, sessionId).ConfigureAwait(false);
        }
    }
}