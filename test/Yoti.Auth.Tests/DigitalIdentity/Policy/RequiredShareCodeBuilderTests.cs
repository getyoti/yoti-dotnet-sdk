using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.Tests.DigitalIdentity.Policy
{
    [TestClass]
    public class RequiredShareCodeBuilderTests
    {
        [TestMethod]
        public void ShouldBuildRequiredShareCode()
        {
            const string issuer = "test-issuer";
            const string scheme = "test-scheme";

            RequiredShareCode result = new RequiredShareCodeBuilder()
                .WithIssuer(issuer)
                .WithScheme(scheme)
                .Build();

            Assert.AreEqual(issuer, result.Issuer);
            Assert.AreEqual(scheme, result.Scheme);
        }

        [TestMethod]
        public void ShouldSerializeCorrectly()
        {
            RequiredShareCode shareCode = new RequiredShareCodeBuilder()
                .WithIssuer("issuer-value")
                .WithScheme("scheme-value")
                .Build();

            string json = JsonConvert.SerializeObject(shareCode);

            Assert.IsTrue(json.Contains("\"issuer\":\"issuer-value\""));
            Assert.IsTrue(json.Contains("\"scheme\":\"scheme-value\""));
        }

        [TestMethod]
        public void ShouldDeserializeCorrectly()
        {
            const string json = "{\"issuer\":\"test-issuer\",\"scheme\":\"test-scheme\"}";

            RequiredShareCode result = JsonConvert.DeserializeObject<RequiredShareCode>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("test-issuer", result.Issuer);
            Assert.AreEqual("test-scheme", result.Scheme);
        }
    }
}
