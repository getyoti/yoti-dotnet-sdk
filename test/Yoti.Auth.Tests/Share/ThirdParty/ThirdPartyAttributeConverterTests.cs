using System;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.Tests.Share.ThirdParty
{
    [TestClass]
    public class ThirdPartyAttributeConverterTests
    {
        private const string _someExpiryDate = "2019-01-02T03:04:05.678+00:00";

        [TestMethod]
        public void ShouldReturnNullForExpiryDateEmpty()
        {
            byte[] byteValue = CreateSerializedThirdPartyAttribute("token", expiryDate: "");
            AttributeIssuanceDetails result = ThirdPartyAttributeConverter.ParseThirdPartyAttribute(byteValue);

            Assert.IsNull(result.ExpiryDate);
        }

        [TestMethod]
        public void ShouldParseDate()
        {
            byte[] byteValue = CreateSerializedThirdPartyAttribute("token", _someExpiryDate);
            AttributeIssuanceDetails result = ThirdPartyAttributeConverter.ParseThirdPartyAttribute(byteValue);

            Assert.AreEqual(new DateTime(2019, 1, 2, 3, 4, 5, 678, DateTimeKind.Utc), result.ExpiryDate);
        }

        [TestMethod]
        public void ShouldThrowExtraDataExceptionForEmptyToken()
        {
            byte[] byteValue = CreateSerializedThirdPartyAttribute("", expiryDate: _someExpiryDate);

            Assert.ThrowsException<ExtraDataException>(() =>
            {
                ThirdPartyAttributeConverter.ParseThirdPartyAttribute(byteValue);
            });
        }

        private static byte[] CreateSerializedThirdPartyAttribute(string token, string expiryDate)
        {
            var thirdPartyAttribute = new ProtoBuf.Share.ThirdPartyAttribute
            {
                IssuanceToken = ByteString.CopyFromUtf8(token),
                IssuingAttributes = new ProtoBuf.Share.IssuingAttributes
                {
                    ExpiryDate = expiryDate
                }
            };

            byte[] byteValue = TestTools.Protobuf.SerializeProtobuf(thirdPartyAttribute);

            return byteValue;
        }
    }
}