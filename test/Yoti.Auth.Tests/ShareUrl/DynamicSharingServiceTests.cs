using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Exceptions;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests.ShareUrl
{
    [TestClass]
    public class DynamicSharingServiceTests
    {
        private const string _sdkID = "sdkID";
        private const string _apiURL = @"https://apiurl.com";
        private readonly Dictionary<string, string> _someHeaders = new Dictionary<string, string>();
        private readonly HttpClient _httpClient = new HttpClient();
        private readonly HttpRequester _httpRequester = new HttpRequester();
        private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
        private DynamicScenario _someDynamicScenario;

        [TestInitialize]
        public void Startup()
        {
            _someHeaders.Add("Key", "Value");
            _someDynamicScenario = TestTools.ShareUrl.CreateStandardDynamicScenario();
        }

        [TestMethod]
        public void ShouldFaillWithNullHttpClient()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(null, _httpRequester, _apiURL, _sdkID, _keyPair, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("HTTP Client"));
        }

        [TestMethod]
        public void ShouldFaillWithNullHttpRequester()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, null, _apiURL, _sdkID, _keyPair, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("HTTP Requester"));
        }

        [TestMethod]
        public void ShouldFaillWithNullapiUrl()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, _httpRequester, null, _sdkID, _keyPair, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("API URL"));
        }

        [TestMethod]
        public void ShouldFaillWithNullSdkId()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, _httpRequester, _apiURL, null, _keyPair, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("Client SDK ID"));
        }

        [TestMethod]
        public void ShouldFaillWithNullKeyPair()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, _httpRequester, _apiURL, _sdkID, null, _someDynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("Application Key Pair"));
        }

        [TestMethod]
        public void ShouldFaillWithNullDynamicScenario()
        {
            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                DynamicSharingService.CreateShareURL(_httpClient, _httpRequester, _apiURL, _sdkID, _keyPair, null).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<ArgumentNullException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.Contains("Dynamic Scenario"));
        }
    }
}