using System;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.Web
{
    [TestClass]
    public class BearerTokenAuthStrategyTests
    {
        [TestMethod]
        public void NullTokenShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(() => new BearerTokenAuthStrategy(null));
        }

        [TestMethod]
        public void EmptyTokenShouldThrow()
        {
            Assert.ThrowsException<ArgumentException>(() => new BearerTokenAuthStrategy(string.Empty));
        }

        [TestMethod]
        public void SdkIdShouldBeNull()
        {
            var strategy = new BearerTokenAuthStrategy("some-token");
            Assert.IsNull(strategy.SdkId);
        }

        [TestMethod]
        public void CreateAuthHeadersShouldReturnBearerHeader()
        {
            const string token = "my-bearer-token";
            var strategy = new BearerTokenAuthStrategy(token);

            var headers = strategy.CreateAuthHeaders(HttpMethod.Get, "/test", null);

            Assert.IsTrue(headers.ContainsKey(Constants.Api.AuthorizationHeader));
            Assert.AreEqual("Bearer " + token, headers[Constants.Api.AuthorizationHeader]);
        }

        [TestMethod]
        public void CreateQueryParamsShouldReturnEmpty()
        {
            var strategy = new BearerTokenAuthStrategy("token");
            var queryParams = strategy.CreateQueryParams();
            Assert.AreEqual(0, queryParams.Count);
        }
    }
}
