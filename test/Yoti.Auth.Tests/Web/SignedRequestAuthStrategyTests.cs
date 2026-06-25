using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.Web
{
    [TestClass]
    public class SignedRequestAuthStrategyTests
    {
        private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
        private const string _sdkId = "test-sdk-id";

        [TestMethod]
        public void NullKeyPairShouldThrow()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
                new SignedRequestAuthStrategy(null, _sdkId));
        }

        [TestMethod]
        public void NullSdkIdShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new SignedRequestAuthStrategy(_keyPair, null));
        }

        [TestMethod]
        public void EmptySdkIdShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(() =>
                new SignedRequestAuthStrategy(_keyPair, string.Empty));
        }

        [TestMethod]
        public void SdkIdShouldBeSet()
        {
            var strategy = new SignedRequestAuthStrategy(_keyPair, _sdkId);
            Assert.AreEqual(_sdkId, strategy.SdkId);
        }

        [TestMethod]
        public void CreateAuthHeadersShouldReturnDigestHeader()
        {
            var strategy = new SignedRequestAuthStrategy(_keyPair, _sdkId);
            var headers = strategy.CreateAuthHeaders(HttpMethod.Get, "/sessions?nonce=abc&timestamp=123", null);

            Assert.IsTrue(headers.ContainsKey(Constants.Api.DigestHeader));
            Assert.IsFalse(string.IsNullOrEmpty(headers[Constants.Api.DigestHeader]));
        }

        [TestMethod]
        public void CreateQueryParamsShouldContainNonceAndTimestamp()
        {
            var strategy = new SignedRequestAuthStrategy(_keyPair, _sdkId);
            var queryParams = strategy.CreateQueryParams();

            Assert.IsTrue(queryParams.ContainsKey("nonce"));
            Assert.IsTrue(queryParams.ContainsKey("timestamp"));
            Assert.IsFalse(string.IsNullOrEmpty(queryParams["nonce"]));
            Assert.IsFalse(string.IsNullOrEmpty(queryParams["timestamp"]));
        }
    }
}
