using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Tests.TestData;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class AnchorTests
    {
        [TestMethod]
        public void Anchor_Getters()
        {
            AttrpubapiV1.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
               "given_names",
               "givenNameValue",
               AttrpubapiV1.ContentType.String,
               TestAnchors.DrivingLicenseAnchor);

            AttrpubapiV1.Anchor protobufAnchor = attribute.Anchors.Single();

            var yotiAnchor = new Yoti.Auth.Anchors.Anchor(protobufAnchor);

            Assert.IsFalse(yotiAnchor.GetAnchorType().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignature().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetTimestamp().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetVersion().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetMessageDigest().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetChainDigest().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetChainDigestSkip1().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetChainDigestSkip2().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSubType().IsDefault());
            Assert.IsFalse(yotiAnchor.GetOriginServerCerts().IsDefault());
            Assert.IsFalse(yotiAnchor.GetValue().IsDefault());
        }
    }
}