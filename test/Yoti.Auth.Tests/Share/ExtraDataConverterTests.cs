using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Constants;
using Yoti.Auth.Share;

namespace Yoti.Auth.Tests.Share
{
    [TestClass]
    public class ExtraDataConverterTests
    {
        [TestMethod]
        public void ShouldParseExtraDataBytes()
        {
            string base64ExtraData = TestData.TestActivityDetails.ExtraData;
            byte[] extraDataBytes = Conversion.Base64ToBytes(base64ExtraData);

            ExtraData result = ExtraDataConverter.ParseExtraDataProto(extraDataBytes);

            string expectedDateTime = "2019-10-15T22:04:05.123Z";
            DateTime nonNullableExpiryDate = (DateTime)result.AttributeIssuanceDetails.ExpiryDate;

            string actualDateTime = nonNullableExpiryDate.ToString(Format.RFC3339PatternMilli, DateTimeFormatInfo.InvariantInfo);

            Assert.AreEqual(expectedDateTime, actualDateTime);
            Assert.AreEqual("c29tZUlzc3VhbmNlVG9rZW4=", result.AttributeIssuanceDetails.Token);
            Assert.AreEqual(2, result.AttributeIssuanceDetails.IssuingAttributes.Count);
            Assert.AreEqual("com.thirdparty.id", result.AttributeIssuanceDetails.IssuingAttributes[0].Name);
            Assert.AreEqual("com.thirdparty.other_id", result.AttributeIssuanceDetails.IssuingAttributes[1].Name);
        }
    }
}