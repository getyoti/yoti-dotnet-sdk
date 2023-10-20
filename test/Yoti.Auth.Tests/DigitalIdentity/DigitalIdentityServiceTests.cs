using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Tests.Common;
using static System.Net.Mime.MediaTypeNames;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class DigitalIdentityServiceTests
    {
        private const string _sdkID = "sdkID";
        private readonly Uri _apiURL = new Uri("https://apiurl.com");
        private readonly Dictionary<string, string> _someHeaders = new Dictionary<string, string>();
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
        private ShareSessionRequest _someShareSessionRequest;
        private const string _sessionID = "someSessionID";
        private QrRequest _someCreateQrRequest;

        [TestInitialize]
        public void Startup()
        {
            _someHeaders.Add("Key", "Value");
            _someCreateQrRequest = TestTools.CreateQr.CreateQrStandard();
            _someShareSessionRequest = TestTools.ShareSession.CreateStandardShareSessionRequest();
        }

        [TestMethod]
        public void ShouldFailWithNullHttpClient()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(null, _apiURL, _sdkID, _keyPair, _someShareSessionRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("httpClient"));
        }

        [TestMethod]
        public void ShouldFailWithNullApiUrl()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(_httpClient, null, _sdkID, _keyPair, _someShareSessionRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("apiUrl"));
        }

        [TestMethod]
        public void ShouldFailWithNullSdkId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(_httpClient, _apiURL, null, _keyPair, _someShareSessionRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void ShouldFailWithNullKeyPair()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(_httpClient, _apiURL, _sdkID, null, _someShareSessionRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void ShouldFailWithNullDynamicScenario()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(_httpClient, _apiURL, _sdkID, _keyPair, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("shareSessionRequest"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingSdkId()
        {
            var exception = Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await DigitalIdentityService.GetSession(_httpClient, _apiURL, null, _keyPair, _sessionID);
            });

            Assert.IsTrue(exception.Exception.InnerException.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingKeyPair()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await DigitalIdentityService.GetSession(_httpClient, _apiURL, _sdkID, null, _sessionID);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await DigitalIdentityService.GetSession(_httpClient, _apiURL, _sdkID, _keyPair, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void CreateQrCodeShouldFailWithNullHttpClient()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateQrCode(null, _apiURL, _sdkID, _keyPair, _someCreateQrRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("httpClient"));
        }

        [TestMethod]
        public void CreateQrCodeShouldFailWithNullApiUrl()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateQrCode(_httpClient, null, _sdkID, _keyPair, _someCreateQrRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("apiUrl"));
        }

        [TestMethod]
        public void CreateQrCodeShouldFailWithNullSdkId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateQrCode(_httpClient, _apiURL, null, _keyPair, _someCreateQrRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void CreateQrCodeShouldFailWithNullKeyPair()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DigitalIdentityService.CreateQrCode(_httpClient, _apiURL, _sdkID, null, _someCreateQrRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void RetrieveQrShouldThrowExceptionForMissingSdkId()
        {
            var exception = Assert.ThrowsExceptionAsync<InvalidOperationException>(async () =>
            {
                await DigitalIdentityService.GetQrCode(_httpClient, _apiURL, null, _keyPair, _sessionID);
            });

            Assert.IsTrue(exception.Exception.InnerException.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void RetrieveQrCodeShouldThrowExceptionForMissingKeyPair()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await DigitalIdentityService.GetQrCode(_httpClient, _apiURL, _sdkID, null, _sessionID);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void RetrieveQrCodeShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                await DigitalIdentityService.GetQrCode(_httpClient, _apiURL, _sdkID, _keyPair, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("qrCodeId"));
        }


    }
}