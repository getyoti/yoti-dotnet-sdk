using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Anchors;
using Yoti.Auth.Images;
using Yoti.Auth.Profile;
using Yoti.Auth.Tests.TestData;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class AnchorTests
    {
        private YotiProfile _yotiProfile;

        private const string YotiAdminVerifierType = "YOTI_ADMIN";
        private const string PassportSourceType = "PASSPORT";
        private const string DrivingLicenseSourceType = "DRIVING_LICENCE";

        private const string StringValue = "Value";
        private const string DateOfBirthString = "1980-01-13";

        [TestMethod]
        public void AnchorGetters()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
               Constants.UserProfile.GivenNamesAttribute,
               StringValue,
               Yoti.Auth.ProtoBuf.Attribute.ContentType.String,
               TestAnchors.DrivingLicenseAnchor);

            ProtoBuf.Attribute.Anchor protobufAnchor = attribute.Anchors.Single();

            var yotiAnchor = new Anchor(protobufAnchor);

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

        [TestMethod]
        public void GetSourcesIncludesDrivingLicense()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.StructuredPostalAddressAttribute,
                "{ \"properties\": { \"name\": { \"type\": \"string\"} } }",
                ProtoBuf.Attribute.ContentType.Json,
                TestAnchors.DrivingLicenseAnchor);

            _yotiProfile = TestTools.Profile.AddAttributeToProfile<Dictionary<string, JToken>>(new YotiProfile(), attribute);

            IEnumerable<Anchor> sources = _yotiProfile.StructuredPostalAddress.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue() == DrivingLicenseSourceType));
        }

        [TestMethod]
        public void GetSourcesIncludesPassport()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.DateOfBirthAttribute,
                DateOfBirthString,
                ProtoBuf.Attribute.ContentType.Date,
                TestAnchors.PassportAnchor);

            _yotiProfile = TestTools.Profile.AddAttributeToProfile<DateTime>(new YotiProfile(), attribute);

            IEnumerable<Anchor> sources = _yotiProfile.DateOfBirth.GetSources();
            Assert.IsTrue(
                sources.Any(
                    s => s.GetValue() == PassportSourceType));
        }

        [TestMethod]
        public void GetSourcesIncludesYotiAdmin()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.SelfieAttribute,
                StringValue,
                ProtoBuf.Attribute.ContentType.Jpeg,
                TestAnchors.YotiAdminAnchor);

            _yotiProfile = TestTools.Profile.AddAttributeToProfile<Image>(new YotiProfile(), attribute);

            IEnumerable<Anchor> verifiers = _yotiProfile.Selfie.GetVerifiers();
            Assert.IsTrue(
                verifiers.Any(
                    s => s.GetValue() == YotiAdminVerifierType));
        }

        [TestMethod]
        public void RetrievingAnUnknownAnchor()
        {
            ProtoBuf.Attribute.Attribute attribute = TestTools.Anchors.BuildAnchoredAttribute(
                Constants.UserProfile.NationalityAttribute,
                "LND",
                ProtoBuf.Attribute.ContentType.String,
                TestAnchors.UnknownAnchor);

            ProtoBuf.Attribute.Anchor protobufAnchor = attribute.Anchors.Single();

            var yotiAnchor = new Anchor(protobufAnchor);

            Assert.AreEqual(AnchorType.UNKNOWN, yotiAnchor.GetAnchorType());
            Assert.AreEqual("", yotiAnchor.GetValue());
            Assert.AreEqual("TEST UNKNOWN SUB TYPE", yotiAnchor.GetSubType());
            Assert.AreEqual(636873795118400370, yotiAnchor.GetSignedTimeStamp().GetTimestamp().Ticks);
            Assert.AreEqual("00ABA6DD34D84D2696171C6E856E952C81", yotiAnchor.GetOriginServerCerts().First().SerialNumber);
        }
    }
}