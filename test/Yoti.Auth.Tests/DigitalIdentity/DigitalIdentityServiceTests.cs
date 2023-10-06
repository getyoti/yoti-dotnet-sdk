using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Tests.Common;

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

        [TestInitialize]
        public void Startup()
        {
            _someHeaders.Add("Key", "Value");
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
    }
}