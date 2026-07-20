using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan;
using Yoti.Auth.DocScan.Session.Create;
using Yoti.Auth.DocScan.Session.Create.FaceCapture;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.DocScan
{
    [TestClass]
    public class DocScanServiceTests
    {
        private const string _sdkId = "sdkId";

        private const string _someSessionId = "someSessionId";
        private const string _someMediaId = "someMediaId";

        private AsymmetricCipherKeyPair _keyPair;
        private IAuthStrategy _authStrategy;
        private DocScanService _docScanService;
        private CreateFaceCaptureResourcePayload _createFaceCaptureResourcePayload;
        private string _someResourceId = "someResourceId";
        private UploadFaceCaptureImagePayload _uploadFaceCaptureImagePayload;

        [TestInitialize]
        public void Startup()
        {
            _keyPair = Tests.Common.KeyPair.Get();
            _authStrategy = new SignedRequestAuthStrategy(_keyPair, _sdkId);
            _docScanService = new DocScanService(new HttpClient(), apiUri: null);
            _createFaceCaptureResourcePayload = new CreateFaceCaptureResourcePayload("someRequirementId");
            _uploadFaceCaptureImagePayload = new UploadFaceCaptureImagePayload(DocScanConstants.MimeTypePng, new byte[] { 0x00, 0x21, 0x60, 0x1F, 0xA1 });
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void ApiUriDefaultIsUsedForNullOrEmpty(string envVar)
        {
            Environment.SetEnvironmentVariable("YOTI_DOC_SCAN_API_URL", envVar);
            DocScanService service = new DocScanService(new HttpClient(), null);

            Uri expectedDefaultUri = Constants.Api.DefaultYotiDocsUrl;

            Assert.AreEqual(expectedDefaultUri, service.ApiUri);
        }

        [TestMethod]
        public void ApiUriConstructorOverEnvVariable()
        {
            Uri overriddenApiUri = new Uri("https://overridden.com");
            Environment.SetEnvironmentVariable("YOTI_DOC_SCAN_API_URL", "https://envapiuri.com");
            DocScanService service = new DocScanService(new HttpClient(), overriddenApiUri);

            Assert.AreEqual(overriddenApiUri, service.ApiUri);
        }

        [TestMethod]
        public void ApiUriEnvVariableIsUsed()
        {
            Environment.SetEnvironmentVariable("YOTI_DOC_SCAN_API_URL", "https://envapiuri.com");
            DocScanService service = new DocScanService(new HttpClient(), apiUri: null);

            Uri expectedApiUri = new Uri("https://envapiuri.com");
            Assert.AreEqual(expectedApiUri, service.ApiUri);
        }

        [TestMethod]
        public void CreateSessionShouldThrowExceptionForNullAuthStrategy()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.CreateSession(null, new SessionSpecificationBuilder().Build());
            }).Result;

            Assert.IsTrue(exception.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void CreateSessionShouldThrowExceptionForMissingSessionSpec()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.CreateSession(_authStrategy, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionSpec"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForNullAuthStrategy()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetSession(null, _someSessionId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetSession(_authStrategy, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void DeleteSessionShouldThrowExceptionForNullAuthStrategy()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteSession(null, _someSessionId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void DeleteSessionShouldThrowExceptionForMissingSessionId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteSession(_authStrategy, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void GetMediaContentShouldThrowExceptionForNullAuthStrategy()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetMediaContent(null, _someSessionId, _someMediaId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void GetMediaContentShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetMediaContent(_authStrategy, null, _someMediaId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void GetMediaContentShouldThrowExceptionForMissingMediaId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetMediaContent(_authStrategy, _someSessionId, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("mediaId"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForNullAuthStrategy()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteMediaContent(null, _someSessionId, _someMediaId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingSessionId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteMediaContent(_authStrategy, null, _someMediaId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingMediaId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteMediaContent(_authStrategy, _someSessionId, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("mediaId"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task CreateFaceCaptureResourceShouldThrowExceptionWhenSessionIdIsNullEmptyOrWhitespace(string sessionId)
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.CreateFaceCaptureResource(_authStrategy, sessionId, _createFaceCaptureResourcePayload);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sessionId)));
        }

        [TestMethod]
        public async Task CreateFaceCaptureResourceShouldThrowExceptionWhenCreateFaceCaptureResourcePayloadIsNull()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.CreateFaceCaptureResource(_authStrategy, _someSessionId, null);
            });

            Assert.IsTrue(exception.Message.Contains("createFaceCaptureResourcePayload"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task UploadFaceCaptureImageShouldThrowExceptionWhenSessionIdIsNullEmptyOrWhitespace(string sessionId)
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.UploadFaceCaptureImage(_authStrategy, sessionId, _someResourceId, _uploadFaceCaptureImagePayload);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sessionId)));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task UploadFaceCaptureImageShouldThrowExceptionWhenResourceIdIsNullEmptyOrWhitespace(string resourceId)
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.UploadFaceCaptureImage(_authStrategy, _someSessionId, resourceId, _uploadFaceCaptureImagePayload);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(resourceId)));
        }

        [TestMethod]
        public async Task UploadFaceCaptureImageShouldThrowExceptionWhenUploadFaceCaptureImagePayloadIsNull()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.UploadFaceCaptureImage(_authStrategy, _someSessionId, _someResourceId, null);
            });

            Assert.IsTrue(exception.Message.Contains("uploadFaceCaptureImagePayload"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task GetSessionConfigurationShouldThrowExceptionWhenSessionIdIsNullEmptyOrWhitespace(string sessionId)
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.GetSessionConfiguration(_authStrategy, sessionId);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sessionId)));
        }

        [TestMethod]
        public async Task CreateSessionShouldIncludeAuthIdHeaderAndQueryParamWhenAuthStrategyHasSdkId()
        {
            HttpRequestMessage capturedRequest = null;
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{}")
                });

            var bearerTokenAuthStrategy = new BearerTokenAuthStrategy("some-bearer-token", _sdkId);
            var service = new DocScanService(new HttpClient(handlerMock.Object), apiUri: null);

            await service.CreateSession(bearerTokenAuthStrategy, new SessionSpecificationBuilder().Build());

            Assert.IsNotNull(capturedRequest);
            Assert.IsTrue(capturedRequest.Headers.Contains(Api.AuthIdHeader));
            Assert.AreEqual(_sdkId, capturedRequest.Headers.GetValues(Api.AuthIdHeader).First());
            Assert.IsTrue(capturedRequest.RequestUri.Query.Contains($"sdkId={_sdkId}"));
        }
    }
}
