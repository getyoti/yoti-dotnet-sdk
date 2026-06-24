using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.FaceCapture;
using Yoti.Auth.DocScan.Session.Retrieve;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration;
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
            ApiUri = apiUri ?? GetApiUri();
        }

        public async Task<CreateSessionResult> CreateSession(IAuthStrategy authStrategy, SessionSpecification sessionSpec)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(sessionSpec, nameof(sessionSpec));

            string serializedSessionSpec = JsonConvert.SerializeObject(sessionSpec, YotiDefaultJsonSettings);
            byte[] body = Encoding.UTF8.GetBytes(serializedSessionSpec);

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Post)
                .WithBaseUri(ApiUri)
                .WithEndpoint("/sessions")
                .WithContent(body)
                .WithContentHeader(Api.ContentTypeHeader, Api.ContentTypeJson);

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CreateSessionResult>(responseObject));
            }
        }

        public async Task<GetSessionResult> GetSession(IAuthStrategy authStrategy, string sessionId)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(sessionId, nameof(sessionId));

            string sessionEndpoint = SessionEndpoint(sessionId);
            _logger.Info($"Fetching session from '{sessionEndpoint}'");

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(ApiUri)
                .WithEndpoint(sessionEndpoint);

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<GetSessionResult>(responseObject));
            }
        }

        public async Task DeleteSession(IAuthStrategy authStrategy, string sessionId)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(sessionId, nameof(sessionId));

            string sessionEndpoint = SessionEndpoint(sessionId);
            _logger.Info($"Deleting session at '{sessionEndpoint}'");

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Delete)
                .WithBaseUri(ApiUri)
                .WithEndpoint(sessionEndpoint);

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
            }
        }

        public async Task<MediaValue> GetMediaContent(IAuthStrategy authStrategy, string sessionId, string mediaId)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(sessionId, nameof(sessionId));
            Validation.NotNull(mediaId, nameof(mediaId));

            string mediaContentPath = MediaEndpoint(sessionId, mediaId);
            _logger.Info($"Fetching media from '{mediaContentPath}'");

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(ApiUri)
                .WithEndpoint(mediaContentPath);

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);

                if (response.Content?.Headers?.ContentType == null)
                    return null;

                string contentType = response.Content.Headers.ContentType.MediaType;
                var responseObject = await response.Content.ReadAsByteArrayAsync();
                return await Task.Factory.StartNew(() => new MediaValue(contentType, responseObject));
            }
        }

        public async Task DeleteMediaContent(IAuthStrategy authStrategy, string sessionId, string mediaId)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNull(sessionId, nameof(sessionId));
            Validation.NotNull(mediaId, nameof(mediaId));

            string mediaContentPath = MediaEndpoint(sessionId, mediaId);
            _logger.Info($"Deleting media at '{mediaContentPath}'");

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Delete)
                .WithBaseUri(ApiUri)
                .WithEndpoint(mediaContentPath);

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
            }
        }

        public async Task<SupportedDocumentsResponse> GetSupportedDocuments(IAuthStrategy authStrategy, bool isStrictlyLatin)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            _logger.Info("Retrieving supported documents");

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(ApiUri)
                .WithEndpoint("/supported-documents")
                .WithQueryParam("includeNonLatin", isStrictlyLatin ? "1" : "0");

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<SupportedDocumentsResponse>(responseObject));
            }
        }

        public async Task<CreateFaceCaptureResourceResponse> CreateFaceCaptureResource(IAuthStrategy authStrategy, string sessionId, CreateFaceCaptureResourcePayload createFaceCaptureResourcePayload)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNullOrWhiteSpace(sessionId, nameof(sessionId));
            Validation.NotNull(createFaceCaptureResourcePayload, nameof(createFaceCaptureResourcePayload));

            _logger.Info("Creating new Face Capture resource");

            string serializedPayload = JsonConvert.SerializeObject(createFaceCaptureResourcePayload, YotiDefaultJsonSettings);
            byte[] body = Encoding.UTF8.GetBytes(serializedPayload);

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Post)
                .WithBaseUri(ApiUri)
                .WithEndpoint($"/sessions/{sessionId}/resources/face-capture")
                .WithContent(body)
                .WithContentHeader(Api.ContentTypeHeader, Api.ContentTypeJson);

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<CreateFaceCaptureResourceResponse>(responseObject));
            }
        }

        public async Task UploadFaceCaptureImage(IAuthStrategy authStrategy, string sessionId, string resourceId, UploadFaceCaptureImagePayload uploadFaceCaptureImagePayload)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNullOrWhiteSpace(sessionId, nameof(sessionId));
            Validation.NotNullOrWhiteSpace(resourceId, nameof(resourceId));
            Validation.NotNull(uploadFaceCaptureImagePayload, nameof(uploadFaceCaptureImagePayload));

            _logger.Info("Uploading image to Face Capture resource");

            var builder = GetRequestBuilder()
                .WithMultipartBoundary(DocScanConstants.MultiPartBoundary)
                .WithMultipartBinaryContent(
                    DocScanConstants.UploadFaceCaptureImageBinaryContentName,
                    uploadFaceCaptureImagePayload.ImageContents,
                    uploadFaceCaptureImagePayload.ImageContentType,
                    DocScanConstants.UploadFaceCaptureImageFileName)
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Put)
                .WithBaseUri(ApiUri)
                .WithEndpoint($"/sessions/{sessionId}/resources/face-capture/{resourceId}/image");

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);
            }
        }

        public async Task<SessionConfigurationResponse> GetSessionConfiguration(IAuthStrategy authStrategy, string sessionId)
        {
            Validation.NotNull(authStrategy, nameof(authStrategy));
            Validation.NotNullOrWhiteSpace(sessionId, nameof(sessionId));

            _logger.Info("Getting Session Configuration");

            var builder = GetRequestBuilder()
                .WithAuthStrategy(authStrategy)
                .WithHttpMethod(HttpMethod.Get)
                .WithBaseUri(ApiUri)
                .WithEndpoint($"/sessions/{sessionId}/configuration");

            if (authStrategy.SdkId != null)
                builder = builder.WithQueryParam("sdkId", authStrategy.SdkId);

            using (HttpResponseMessage response = await builder.Build().Execute(_httpClient).ConfigureAwait(false))
            {
                if (!response.IsSuccessStatusCode)
                    Response.CreateYotiExceptionFromStatusCode<DocScanException>(response);

                var responseObject = await response.Content.ReadAsStringAsync();
                return await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<SessionConfigurationResponse>(responseObject));
            }
        }

        private static Uri GetApiUri()
        {
            string envUrl = Environment.GetEnvironmentVariable("YOTI_DOC_SCAN_API_URL");
            return !string.IsNullOrEmpty(envUrl) ? new Uri(envUrl) : Api.DefaultYotiDocsUrl;
        }

        private static RequestBuilder GetRequestBuilder() => new RequestBuilder();

        private static string SessionEndpoint(string sessionId) => $"sessions/{sessionId}";

        private static string MediaEndpoint(string sessionId, string mediaId) => $"sessions/{sessionId}/media/{mediaId}/content";

        private static JsonSerializerSettings YotiDefaultJsonSettings => new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
    }
}
