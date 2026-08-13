using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

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
        private IAuthStrategy _authStrategy;

        [TestInitialize]
        public void Startup()
        {
            _someHeaders.Add("Key", "Value");
            _someCreateQrRequest = TestTools.CreateQr.CreateQrStandard();
            _someShareSessionRequest = TestTools.ShareSession.CreateStandardShareSessionRequest();
            _authStrategy = new SignedRequestAuthStrategy(_keyPair, _sdkID);
        }

        [TestMethod]
        public void ShouldFailWithNullHttpClient()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(null, _apiURL, _authStrategy, _someShareSessionRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("httpClient"));
        }

        [TestMethod]
        public void ShouldFailWithNullApiUrl()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(_httpClient, null, _authStrategy, _someShareSessionRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("apiUrl"));
        }

        [TestMethod]
        public void ShouldFailWithNullAuthStrategy()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(_httpClient, _apiURL, null, _someShareSessionRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void ShouldFailWithNullDynamicScenario()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.CreateShareSession(_httpClient, _apiURL, _authStrategy, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("shareSessionRequest"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForNullAuthStrategy()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.GetSession(_httpClient, _apiURL, null, _sessionID).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void RetrieveSessionShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
            {
                await DigitalIdentityService.GetSession(_httpClient, _apiURL, _authStrategy, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("sessionId"));
        }

        [TestMethod]
        public void CreateQrCodeShouldFailWithNullHttpClient()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.CreateQrCode(null, _apiURL, _authStrategy, _sessionID, _someCreateQrRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("httpClient"));
        }

        [TestMethod]
        public void CreateQrCodeShouldFailWithNullApiUrl()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.CreateQrCode(_httpClient, null, _authStrategy, _sessionID, _someCreateQrRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("apiUrl"));
        }

        [TestMethod]
        public void CreateQrCodeShouldFailWithNullAuthStrategy()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.CreateQrCode(_httpClient, _apiURL, null, _sessionID, _someCreateQrRequest).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void RetrieveQrShouldThrowExceptionForNullAuthStrategy()
        {
            var aggregateException = Assert.ThrowsExactly<AggregateException>(() =>
            {
                DigitalIdentityService.GetQrCode(_httpClient, _apiURL, null, _sessionID).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("authStrategy"));
        }

        [TestMethod]
        public void RetrieveQrCodeShouldThrowExceptionForMissingSessionId()
        {
            var exception = Assert.ThrowsExactlyAsync<ArgumentNullException>(async () =>
            {
                await DigitalIdentityService.GetQrCode(_httpClient, _apiURL, _authStrategy, null);
            }).Result;

            Assert.IsTrue(exception.Message.Contains("qrCodeId"));
        }
    }
}
