using System;
using System.Globalization;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Constants;
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

        [DataTestMethod]
        [DataRow("2019-01-02T03:04:05.678Z")]
        [DataRow("2019-01-02T04:04:05.678+01:00")]
        [DataRow("2019-01-02T15:04:05.678+12:00")]
        [DataRow("2019-01-02T02:04:05.678-01:00")]
        [DataRow("2019-01-01T15:04:05.678-12:00")]
        public void ShouldParseDifferentTimezonesDate(string expiryDate)
        {
            byte[] byteValue = CreateSerializedThirdPartyAttribute("token", expiryDate);
            AttributeIssuanceDetails result = ThirdPartyAttributeConverter.ParseThirdPartyAttribute(byteValue);

            Assert.AreEqual(new DateTime(2019, 1, 2, 3, 4, 5, 678, DateTimeKind.Utc), result.ExpiryDate);
        }

        [DataTestMethod]
        [DataRow("2006-01-02", "2006-01-02T00:00:00.000000Z")]
        [DataRow("2006-01-02T22:04:05Z", "2006-01-02T22:04:05.000000Z")]
        [DataRow("2006-01-02T22:04:05.1Z", "2006-01-02T22:04:05.100000Z")]
        [DataRow("2006-01-02T22:04:05.12Z", "2006-01-02T22:04:05.120000Z")]
        [DataRow("2006-01-02T22:04:05.123Z", "2006-01-02T22:04:05.123000Z")]
        [DataRow("2006-01-02T22:04:05.1234Z", "2006-01-02T22:04:05.123400Z")]
        [DataRow("2006-01-02T22:04:05.999999Z", "2006-01-02T22:04:05.999999Z")]
        [DataRow("2006-01-02T22:04:05.123456Z", "2006-01-02T22:04:05.123456Z")]
        [DataRow("2002-10-02T10:00:00.1-05:00", "2002-10-02T15:00:00.100000Z")]
        [DataRow("2002-10-02T10:00:00.12345+11:00", "2002-10-01T23:00:00.123450Z")]
        public void ShouldParseAllValidRFC3339Dates(string expiryDate, string expectedValue)
        {
            byte[] byteValue = CreateSerializedThirdPartyAttribute("token", expiryDate);
            AttributeIssuanceDetails result = ThirdPartyAttributeConverter.ParseThirdPartyAttribute(byteValue);
            DateTime nonNullableExpiryDate = (DateTime)result.ExpiryDate;

            string actualString = nonNullableExpiryDate.ToString(Format.RFC3339PatternMilli, DateTimeFormatInfo.InvariantInfo);
            Assert.AreEqual(expectedValue, actualString);
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