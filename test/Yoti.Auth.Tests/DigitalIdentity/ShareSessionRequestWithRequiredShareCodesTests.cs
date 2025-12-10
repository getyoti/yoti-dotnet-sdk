using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class ShareSessionRequestWithRequiredShareCodesTests
    {
        [TestMethod]
        public void ShouldBuildWithRequiredShareCodes()
        {
            RequiredShareCode shareCode1 = new RequiredShareCodeBuilder()
                .WithIssuer("issuer1")
                .WithScheme("scheme1")
                .Build();

            RequiredShareCode shareCode2 = new RequiredShareCodeBuilder()
                .WithIssuer("issuer2")
                .WithScheme("scheme2")
                .Build();

            var policy = TestTools.ShareSession.CreateStandardPolicy();

            ShareSessionRequest result = new ShareSessionRequestBuilder()
                .WithRedirectUri("https://example.com")
                .WithPolicy(policy)
                .WithRequiredShareCode(shareCode1)
                .WithRequiredShareCode(shareCode2)
                .Build();

            Assert.IsNotNull(result.RequiredShareCodes);
            Assert.AreEqual(2, result.RequiredShareCodes.Count);
            Assert.AreEqual("issuer1", result.RequiredShareCodes[0].Issuer);
            Assert.AreEqual("scheme1", result.RequiredShareCodes[0].Scheme);
            Assert.AreEqual("issuer2", result.RequiredShareCodes[1].Issuer);
            Assert.AreEqual("scheme2", result.RequiredShareCodes[1].Scheme);
        }

        [TestMethod]
        public void ShouldSerializeWithRequiredShareCodes()
        {
            RequiredShareCode shareCode = new RequiredShareCodeBuilder()
                .WithIssuer("test-issuer")
                .WithScheme("test-scheme")
                .Build();

            var policy = TestTools.ShareSession.CreateStandardPolicy();

            ShareSessionRequest request = new ShareSessionRequestBuilder()
                .WithRedirectUri("https://example.com")
                .WithPolicy(policy)
                .WithRequiredShareCode(shareCode)
                .Build();

            string json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            Assert.IsTrue(json.Contains("\"required_share_codes\""));
            Assert.IsTrue(json.Contains("\"issuer\":\"test-issuer\""));
            Assert.IsTrue(json.Contains("\"scheme\":\"test-scheme\""));
        }

        [TestMethod]
        public void ShouldNotSerializeEmptyRequiredShareCodes()
        {
            var policy = TestTools.ShareSession.CreateStandardPolicy();

            ShareSessionRequest request = new ShareSessionRequestBuilder()
                .WithRedirectUri("https://example.com")
                .WithPolicy(policy)
                .Build();

            string json = JsonConvert.SerializeObject(request, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            // Empty required_share_codes should not be serialized
            Assert.IsFalse(json.Contains("\"required_share_codes\""));
        }

        [TestMethod]
        public void ShouldHaveNullRequiredShareCodesWhenNotAdded()
        {
            var policy = TestTools.ShareSession.CreateStandardPolicy();

            ShareSessionRequest request = new ShareSessionRequestBuilder()
                .WithRedirectUri("https://example.com")
                .WithPolicy(policy)
                .Build();

            Assert.IsNull(request.RequiredShareCodes);
        }
    }
}
