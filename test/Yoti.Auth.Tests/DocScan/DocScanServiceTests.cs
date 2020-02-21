using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DocScan;
using Yoti.Auth.DocScan.Session.Create;
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

        [TestInitialize]
        public void Startup()
        {
            _keyPair = Tests.Common.KeyPair.Get();
            _docScanService = new DocScanService(new HttpClient(), apiUri: null);
        }

        [DataTestMethod]
        [DataRow("")]
        [DataRow(null)]
        public void ApiUriDefaultIsUsedForNullOrEmpty(string envVar)
        {
            Environment.SetEnvironmentVariable("YOTI_API_URL", envVar);
            DocScanService service = new DocScanService(new HttpClient(), null);

            Uri expectedDefaultUri = Constants.Api.DefaultYotiDocsUrl;

            Assert.AreEqual(expectedDefaultUri, service.ApiUri);
        }

        [TestMethod]
        public void ApiUriConstructorOverEnvVariable()
        {
            Uri overriddenApiUri = new Uri("https://overridden.com");
            Environment.SetEnvironmentVariable("YOTI_API_URL", "https://envapiuri.com");
            DocScanService service = new DocScanService(new HttpClient(), overriddenApiUri);

            Assert.AreEqual(overriddenApiUri, service.ApiUri);
        }

        [TestMethod]
        public void ApiUriEnvVariableIsUsed()
        {
            Environment.SetEnvironmentVariable("YOTI_API_URL", "https://envapiuri.com");
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
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _docScanService.DeleteSession(null, _keyPair, _someSessionId);
            });

            Assert.IsTrue(exception.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void DeleteSessionShouldThrowExceptionForMissingKeyPair()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _docScanService.DeleteSession(_sdkId, null, _someSessionId);
            });

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void DeleteSessionShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _docScanService.DeleteSession(_sdkId, _keyPair, null);
            });

            Assert.IsTrue(exception.Message.Contains("sessionId"));
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
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                _docScanService.DeleteMediaContent(null, _keyPair, _someSessionId, _someMediaId);
            });

            Assert.IsTrue(exception.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingKeyPair()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _docScanService.DeleteMediaContent(_sdkId, null, _someSessionId, _someMediaId);
            });

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _docScanService.DeleteMediaContent(_sdkId, _keyPair, null, _someMediaId);
            });

            Assert.IsTrue(exception.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void DeleteMediaContentShouldThrowExceptionForMissingMediaId()
        {
            var exception = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _docScanService.DeleteMediaContent(_sdkId, _keyPair, _someSessionId, null);
            });

            Assert.IsTrue(exception.Message.Contains("mediaId"));
        }

        [TestMethod]
        public void GetSignedRequestBuilderShouldReturnNewInstance()
        {
            RequestBuilder requestBuilder1 = DocScanService.GetSignedRequestBuilder();
            RequestBuilder requestBuilder2 = DocScanService.GetSignedRequestBuilder();

            Assert.AreNotSame(requestBuilder1, requestBuilder2);
        }
    }
}