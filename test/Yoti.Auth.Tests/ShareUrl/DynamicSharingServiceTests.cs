using System;
using System.Collections.Generic;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.ShareUrl
{
    [TestClass]
    public class DynamicSharingServiceTests
    {
        private const string _sdkID = "sdkID";
        private readonly Uri _apiURL = new Uri("https://apiurl.com");
        private readonly Dictionary<string, string> _someHeaders = new Dictionary<string, string>();
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
        private DynamicScenario _someDynamicScenario;

        [TestInitialize]
        public void Startup()
        {
            _someHeaders.Add("Key", "Value");
            _someDynamicScenario = TestTools.ShareUrl.CreateStandardDynamicScenario();
        }

        [TestMethod]
        public void ShouldFailWithNullHttpClient()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(null, _apiURL, _sdkID, _keyPair, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("httpClient"));
        }

        [TestMethod]
        public void ShouldFailWithNullApiUrl()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, null, _sdkID, _keyPair, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("apiUrl"));
        }

        [TestMethod]
        public void ShouldFailWithNullSdkId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, _apiURL, null, _keyPair, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("sdkId"));
        }

        [TestMethod]
        public void ShouldFailWithNullKeyPair()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, _apiURL, _sdkID, null, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("keyPair"));
        }

        [TestMethod]
        public void ShouldFailWithNullDynamicScenario()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, _apiURL, _sdkID, _keyPair, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("dynamicScenario"));
        }
    }
}