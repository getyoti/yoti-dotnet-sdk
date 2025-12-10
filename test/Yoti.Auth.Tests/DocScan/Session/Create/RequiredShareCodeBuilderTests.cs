using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class RequiredShareCodeBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithIssuerAndScheme()
        {
            string issuer = "test_issuer";
            string scheme = "test_scheme";

            RequiredShareCode requiredShareCode = new RequiredShareCodeBuilder()
                .WithIssuer(issuer)
                .WithScheme(scheme)
                .Build();

            Assert.AreEqual(issuer, requiredShareCode.Issuer);
            Assert.AreEqual(scheme, requiredShareCode.Scheme);
        }

        [TestMethod]
        public void ShouldSerializeCorrectly()
        {
            string issuer = "test_issuer";
            string scheme = "test_scheme";

            RequiredShareCode requiredShareCode = new RequiredShareCodeBuilder()
                .WithIssuer(issuer)
                .WithScheme(scheme)
                .Build();

            string json = JsonConvert.SerializeObject(requiredShareCode);

            Assert.IsTrue(json.Contains("\"issuer\":\"test_issuer\""));
            Assert.IsTrue(json.Contains("\"scheme\":\"test_scheme\""));
        }
    }
}
