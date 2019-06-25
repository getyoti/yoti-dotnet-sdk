using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Anchors;
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
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
               "given_names",
               "givenNameValue",
               Yoti.Auth.ProtoBuf.Attribute.ContentType.String,
               TestAnchors.DrivingLicenseAnchor);

            ProtoBuf.Attribute.Anchor protobufAnchor = attribute.Anchors.Single();

            var yotiAnchor = new Yoti.Auth.Anchors.Anchor(protobufAnchor);

            Assert.AreEqual(AnchorType.SOURCE, yotiAnchor.GetAnchorType());
            Assert.IsFalse(yotiAnchor.GetSignature().IsDefault());
            Assert.AreEqual(636590455839235370, yotiAnchor.GetSignedTimeStamp().GetTimestamp().Ticks);
            Assert.AreEqual(1, yotiAnchor.GetSignedTimeStamp().GetVersion());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetMessageDigest().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetChainDigest().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetChainDigestSkip1().IsDefault());
            Assert.IsFalse(yotiAnchor.GetSignedTimeStamp().GetChainDigestSkip2().IsDefault());
            Assert.AreEqual("", yotiAnchor.GetSubType());
            Assert.AreEqual("DRIVING_LICENCE", yotiAnchor.GetValue());

            X509Certificate2 firstOriginServerCert = yotiAnchor.GetOriginServerCerts().First();
            Assert.AreEqual("CN=driving-licence-registration-server", firstOriginServerCert.Subject);
            Assert.AreEqual("CN=driving-licence-registration-server", firstOriginServerCert.Issuer);
            Assert.AreEqual("22B4AA0414D35D6C6019FE8EBD59B95C", firstOriginServerCert.SerialNumber);
            Assert.AreEqual(new DateTime(2018, 4, 5, 14, 27, 36, DateTimeKind.Utc), firstOriginServerCert.NotBefore.ToUniversalTime());
            Assert.AreEqual(new DateTime(2018, 4, 12, 14, 27, 36, DateTimeKind.Utc), firstOriginServerCert.NotAfter.ToUniversalTime());
            Assert.AreEqual("3C753FFD1D8A359EC89AD2BD679563F2E4F9B767", firstOriginServerCert.Thumbprint);
        }
    }
}