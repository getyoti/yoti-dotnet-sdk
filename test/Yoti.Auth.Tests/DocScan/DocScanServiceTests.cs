using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        private DocScanService _docScanService;
        private CreateFaceCaptureResourcePayload _createFaceCaptureResourcePayload;
        private string _someResourceId = "someResourceId";
        private UploadFaceCaptureImagePayload _uploadFaceCaptureImagePayload;

        [TestInitialize]
        public void Startup()
        {
            _keyPair = Tests.Common.KeyPair.Get();
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
        public void CreateSessionShouldThrowExceptionForMissingSdkId()
        {
            var exception = Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.CreateSession(null, _keyPair, new SessionSpecificationBuilder().Build());
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void CreateSessionShouldThrowExceptionForMissingKeyPair()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.CreateSession(_sdkId, null, new SessionSpecificationBuilder().Build());
            }).Result;

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void CreateSessionShouldThrowExceptionForMissingSessionSpec()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.CreateSession(_sdkId, _keyPair, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionSpec"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingSdkId()
        {
            var exception = Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.GetSession(null, _keyPair, _someSessionId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingKeyPair()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetSession(_sdkId, null, _someSessionId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetSession(_sdkId, _keyPair, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void DeleteSessionShouldThrowExceptionForMissingSdkId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteSession(null, _keyPair, _someSessionId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<InvalidOperationException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void DeleteSessionShouldThrowExceptionForMissingKeyPair()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteSession(_sdkId, null, _someSessionId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void DeleteSessionShouldThrowExceptionForMissingSessionId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteSession(_sdkId, _keyPair, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void GetMediaContentShouldThrowExceptionForMissingSdkId()
        {
            var exception = Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.GetMediaContent(null, _keyPair, _someSessionId, _someMediaId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void GetMediaContentShouldThrowExceptionForMissingKeyPair()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetMediaContent(_sdkId, null, _someSessionId, _someMediaId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void GetMediaContentShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetMediaContent(_sdkId, _keyPair, null, _someMediaId);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void GetMediaContentShouldThrowExceptionForMissingMediaId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetMediaContent(_sdkId, _keyPair, _someSessionId, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("mediaId"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingSdkId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteMediaContent(null, _keyPair, _someSessionId, _someMediaId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<InvalidOperationException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingKeyPair()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteMediaContent(_sdkId, null, _someSessionId, _someMediaId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingSessionId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteMediaContent(_sdkId, _keyPair, null, _someMediaId).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingMediaId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                _docScanService.DeleteMediaContent(_sdkId, _keyPair, _someSessionId, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("mediaId"));
        }

        [TestMethod]
        public void GetSignedRequestBuilderShouldReturnNewInstance()
        {
            RequestBuilder requestBuilder1 = DocScanService.GetSignedRequestBuilder();
            RequestBuilder requestBuilder2 = DocScanService.GetSignedRequestBuilder();

            Assert.AreNotSame(requestBuilder1, requestBuilder2);
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task CreateFaceCaptureResourceShouldThrowExceptionWhenSdkIdIsNullEmptyOrWhitespace(string sdkId)
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.CreateFaceCaptureResource(sdkId, _keyPair, _someSessionId, _createFaceCaptureResourcePayload);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sdkId)));
        }

        [TestMethod]
        public async Task CreateFaceCaptureResourceShouldThrowExceptionWhenKeyPairIsNull()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.CreateFaceCaptureResource(_sdkId, null, _someSessionId, _createFaceCaptureResourcePayload);
            });

            Assert.IsTrue(exception.Message.Contains("keyPair"));
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
                await _docScanService.CreateFaceCaptureResource(_sdkId, _keyPair, sessionId, _createFaceCaptureResourcePayload);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sessionId)));
        }

        [TestMethod]
        public async Task CreateFaceCaptureResourceShouldThrowExceptionWhenCreateFaceCaptureResourcePayloadIsNull()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.CreateFaceCaptureResource(_sdkId, _keyPair, _someSessionId, null);
            });

            Assert.IsTrue(exception.Message.Contains("createFaceCaptureResourcePayload"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task UploadFaceCaptureImageShouldThrowExceptionWhenSdkIdIsNullEmptyOrWhitespace(string sdkId)
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.UploadFaceCaptureImage(sdkId, _keyPair, _someSessionId, _someResourceId, _uploadFaceCaptureImagePayload);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sdkId)));
        }

        [TestMethod]
        public async Task UploadFaceCaptureImageShouldThrowExceptionWhenKeyPairIsNull()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.UploadFaceCaptureImage(_sdkId, null, _someSessionId, _someResourceId, _uploadFaceCaptureImagePayload);
            });

            Assert.IsTrue(exception.Message.Contains("keyPair"));
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
                await _docScanService.UploadFaceCaptureImage(_sdkId, _keyPair, sessionId, _someResourceId, _uploadFaceCaptureImagePayload);
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
                await _docScanService.UploadFaceCaptureImage(_sdkId, _keyPair, _someSessionId, resourceId, _uploadFaceCaptureImagePayload);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(resourceId)));
        }

        [TestMethod]
        public async Task UploadFaceCaptureImageShouldThrowExceptionWhenUploadFaceCaptureImagePayloadIsNull()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.UploadFaceCaptureImage(_sdkId, _keyPair, _someSessionId, _someResourceId, null);
            });

            Assert.IsTrue(exception.Message.Contains("uploadFaceCaptureImagePayload"));
        }

        [DataTestMethod]
        [DataRow(null)]
        [DataRow("")]
        [DataRow(" ")]
        [DataRow("  ")]
        public async Task GetSessionConfigurationShouldThrowExceptionWhenSdkIdIsNullEmptyOrWhitespace(string sdkId)
        {
            var exception = await Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await _docScanService.GetSessionConfiguration(sdkId, _keyPair, _someSessionId);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sdkId)));
        }

        [TestMethod]
        public async Task GetSessionConfigurationShouldThrowExceptionWhenKeyPairIsNull()
        {
            var exception = await Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await _docScanService.GetSessionConfiguration(_sdkId, null, _someSessionId);
            });

            Assert.IsTrue(exception.Message.Contains("keyPair"));
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
                await _docScanService.GetSessionConfiguration(_sdkId, _keyPair, sessionId);
            });

            Assert.IsTrue(exception.Message.Contains(nameof(sessionId)));
        }
    }
}