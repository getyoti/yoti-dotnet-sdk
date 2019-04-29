using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class HeadersFactoryTests
    {
        private const string _someDigest = "someDigest";
        private const string _someKey = "someKey";

        [TestMethod]
        public void ShouldCreateHeadersWithDigestAndKey()
        {
            string expectedSDKVersionHeader = typeof(YotiClientEngine).GetTypeInfo().Assembly.GetName().Version.ToString();

            var result = HeadersFactory.PutHeaders(_someDigest, _someKey);

            Assert.AreEqual(_someDigest, result[Constants.Web.DigestHeader]);
            Assert.AreEqual(_someKey, result[Constants.Web.AuthKeyHeader]);
            Assert.AreEqual(Constants.Web.SdkIdentifier, result[Constants.Web.YotiSdkHeader]);
            Assert.AreEqual(
                $"{Constants.Web.SdkIdentifier}-{expectedSDKVersionHeader}",
                result[Constants.Web.YotiSdkVersionHeader]);
        }
    }
}