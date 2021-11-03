using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.FaceCapture;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.CreateFaceCaptureResourceResponse;
using Yoti.Auth.DocScan.Support;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Web;

namespace Yoti.Auth.DocScan
{
    internal class DocScanService
    {
        public Uri ApiUri { get; private set; }

        private readonly HttpClient _httpClient;
        private readonly NLog.Logger _logger;

        public DocScanService(HttpClient httpClient, Uri apiUri)
        {
            _logger = NLog.LogManager.GetCurrentClassLogger();
            _httpClient = httpClient;

            if (apiUri == null)
            {
                apiUri = GetApiUri();
            }

            ApiUri = apiUri;
        }

        public async Task<CreateSessionResult> CreateSession(string sdkId, AsymmetricCipherKeyPair keyPair, SessionSpecification sessionSpec)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(sessionSpec, nameof(sessionSpec));

            string serializedSessionSpec = JsonConvert.SerializeObject(sessionSpec, DefaultJsonSettings);
            byte[] body = Encoding.UTF8.GetBytes(serializedSessionSpec);

            Request createSessionRequest = GetSignedRequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Post)
                .WithBaseUri(ApiUri)
                .WithEndpoint("/sessions")
                .WithQueryParam("sdkId", sdkId)
                .WithContent(body)
                .WithContentHeader(Api.ContentTypeHeader, Api.ContentTypeJson)
                .Build();

            using (HttpResponseMessage response = await createSessionRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }

                return JsonConvert.DeserializeObject<CreateSessionResult>(
                   response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<GetSessionResult> GetSession(string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(sessionId, nameof(sessionId));

            string sessionEndpoint = SessionEndpoint(sessionId);
            _logger.Info($"Fetching session from '{sessionEndpoint}'");

            Request sessionRequest = GetSignedRequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(ApiUri)
                .WithEndpoint(sessionEndpoint)
                .WithQueryParam("sdkId", sdkId)  
                .Build();

            using (HttpResponseMessage response = await sessionRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }

                return JsonConvert.DeserializeObject<GetSessionResult>(
                    response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task DeleteSession(string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(sessionId, nameof(sessionId));

            string sessionEndpoint = SessionEndpoint(sessionId);
            _logger.Info($"Deleting session at '{sessionEndpoint}'");

            Request deleteSessionRequest = GetSignedRequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Delete)
                .WithBaseUri(ApiUri)
                .WithEndpoint(sessionEndpoint)
                .WithQueryParam("sdkId", sdkId)
                .Build();

            using (HttpResponseMessage response = await deleteSessionRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }
            }
        }

        public async Task<MediaValue> GetMediaContent(string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId, string mediaId)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(sessionId, nameof(sessionId));
            Validation.NotNull(mediaId, nameof(mediaId));

            string mediaContentPath = MediaEndpoint(sessionId, mediaId);
            _logger.Info($"Fetching media from '{mediaContentPath}'");

            Request getMediaContentRequest = GetSignedRequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(ApiUri)
                .WithEndpoint(mediaContentPath)
                .WithQueryParam("sdkId", sdkId)
                .Build();

            using (HttpResponseMessage response = await getMediaContentRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }

                if (response.Content == null)
                {
                    return null;
                }

                string contentType = response.Content.Headers.ContentType.MediaType;

                return new MediaValue(contentType, response.Content.ReadAsByteArrayAsync().Result);
            }
        }

        public async Task DeleteMediaContent(string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId, string mediaId)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNull(sessionId, nameof(sessionId));
            Validation.NotNull(mediaId, nameof(mediaId));

            string mediaContentPath = MediaEndpoint(sessionId, mediaId);
            _logger.Info($"Deleting media at '{mediaContentPath}'");

            Request deleteMediaContentRequest = GetSignedRequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Delete)
                .WithBaseUri(ApiUri)
                .WithEndpoint(mediaContentPath)
                .WithQueryParam("sdkId", sdkId)
                .Build();

            using (HttpResponseMessage response = await deleteMediaContentRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }
            }
        }

        public async Task<SupportedDocumentsResponse> GetSupportedDocuments(string sdkId, AsymmetricCipherKeyPair keyPair)
        {
            Validation.NotNullOrEmpty(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            _logger.Info($"Retrieving supported documents'");

            Request supportedDocumentsRequest = GetSignedRequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(ApiUri)
                .WithEndpoint("/supported-documents")
                .WithQueryParam("sdkId", sdkId)
                .Build();

            using (HttpResponseMessage response = await supportedDocumentsRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }

                return JsonConvert.DeserializeObject<SupportedDocumentsResponse>(
                    response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task<CreateFaceCaptureResourceResponse> CreateFaceCaptureResource(string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId, CreateFaceCaptureResourcePayload createFaceCaptureResourcePayload)
        {
            Validation.NotNullOrWhiteSpace(sdkId, nameof(sdkId));
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNullOrWhiteSpace(sessionId, nameof(sessionId));
            Validation.NotNull(createFaceCaptureResourcePayload, nameof(createFaceCaptureResourcePayload));

            _logger.Info($"Creating new Face Capture resource");

            string serializedFaceCaptureResourcePayload = JsonConvert.SerializeObject(createFaceCaptureResourcePayload, DefaultJsonSettings);
            byte[] body = Encoding.UTF8.GetBytes(serializedFaceCaptureResourcePayload);

            Request createFaceCaptureRequest = GetSignedRequestBuilder()
                .WithKeyPair(keyPair)
                .WithHttpMethod(HttpMethod.Post)
                .WithBaseUri(ApiUri)
                .WithEndpoint($"/sessions/{sessionId}/resources/face-capture")
                .WithQueryParam("sdkId", sdkId)
                .WithContent(body)
                .WithContentHeader(Api.ContentTypeHeader, Api.ContentTypeJson)
                .Build();

            using (HttpResponseMessage response = await createFaceCaptureRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }

                return JsonConvert.DeserializeObject<CreateFaceCaptureResourceResponse>(
                    response.Content.ReadAsStringAsync().Result);
            }
        }

        public async Task UploadFaceCaptureImage(string sdkId, AsymmetricCipherKeyPair keyPair, string sessionId, string resourceId, UploadFaceCaptureImagePayload uploadFaceCaptureImagePayload)
        {
            Validation.NotNullOrWhiteSpace(sdkId, nameof(sdkId)); 
            Validation.NotNull(keyPair, nameof(keyPair));
            Validation.NotNullOrWhiteSpace(sessionId, nameof(sessionId)); 
            Validation.NotNullOrWhiteSpace(resourceId, nameof(resourceId)); 
            Validation.NotNull(uploadFaceCaptureImagePayload, nameof(uploadFaceCaptureImagePayload));

            _logger.Info($"Uploading image to Face Capture resource");

            Request uploadFaceCaptureImageRequest = GetSignedRequestBuilder()
                     .WithMultipartBoundary(DocScanConstants.MultiPartBoundary)
                     .WithMultipartBinaryContent(DocScanConstants.UploadFaceCaptureImageBinaryContentName,
                                uploadFaceCaptureImagePayload.ImageContents,
                                uploadFaceCaptureImagePayload.ImageContentType,
                                DocScanConstants.UploadFaceCaptureImageFileName)
                     .WithKeyPair(keyPair)
                     .WithHttpMethod(HttpMethod.Put)
                     .WithBaseUri(ApiUri)
                     .WithEndpoint($"/sessions/{sessionId}/resources/face-capture/{resourceId}/image")
                     .WithQueryParam("sdkId", sdkId)
                     .Build();

            using (HttpResponseMessage response = await uploadFaceCaptureImageRequest.Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                {
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
                }
            }
        }

        private static Uri GetApiUri()
        {
            if (!string.IsNullOrEmpty(Environment.GetEnvironmentVariable("YOTI_DOC_SCAN_API_URL")))
            {
                return new Uri(Environment.GetEnvironmentVariable("YOTI_DOC_SCAN_API_URL"));
            }
            else
            {
                return Api.DefaultYotiDocsUrl;
            }
        }

        public static RequestBuilder GetSignedRequestBuilder()
        {
            return new RequestBuilder();
        }

        private static string SessionEndpoint(string sessionId)
        {
            return $"sessions/{sessionId}";
        }

        private static string MediaEndpoint(string sessionId, string mediaId)
        {
            return $"sessions/{sessionId}/media/{mediaId}/content";
        }

        private static JsonSerializerSettings DefaultJsonSettings => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }
}