using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.Web
{
    [TestClass]
    public class NoAuthStrategyTests
    {
        [TestMethod]
        public void SdkIdShouldBeNull()
        {
            Assert.IsNull(new NoAuthStrategy().SdkId);
        }

        [TestMethod]
        public void CreateAuthHeadersShouldReturnEmpty()
        {
            var headers = new NoAuthStrategy().CreateAuthHeaders(HttpMethod.Get, "/test", null);
            Assert.AreEqual(0, headers.Count);
        }

        [TestMethod]
        public void CreateQueryParamsShouldReturnEmpty()
        {
            var queryParams = new NoAuthStrategy().CreateQueryParams();
            Assert.AreEqual(0, queryParams.Count);
        }
    }
}
