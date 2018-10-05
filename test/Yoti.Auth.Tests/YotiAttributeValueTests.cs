using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiAttributeValueTests
    {
        [TestMethod]
        public void YotiAttributeValue_ToDateValidDate_CorrectResult()
        {
            const string dateString = "1985-01-13";
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Date, dateString);
            DateTime? outputValue = yotiAttributeValue.ToDate();

            Assert.AreEqual(
                new DateTime(1985, 01, 13),
                outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateInvalidFormat_ReturnsNull()
        {
            const string dateString = "1985/01/01";
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Date, dateString);
            DateTime? outputValue = yotiAttributeValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateInvalidDate_ReturnsNull()
        {
            const string dateString = "1985-13-01";
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Date, dateString);
            DateTime? outputValue = yotiAttributeValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateOnPng_ReturnsNull()
        {
            const string pngString = "PngImageData";
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Png, pngString);
            DateTime? outputValue = yotiAttributeValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateOnJpeg_ReturnsNull()
        {
            const string jpegString = "JpegData";
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Jpeg, jpegString);
            DateTime? outputValue = yotiAttributeValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_CheckAccessors_NoExceptions()
        {
            var inputBytes = new byte[8];
            const TypeEnum inputType = TypeEnum.Jpeg;
            var yotiAttributeValue = new YotiAttributeValue(inputType, inputBytes);

            TypeEnum outputType = yotiAttributeValue.Type;
            byte[] outputBytes = yotiAttributeValue.ToBytes();

            DateTime? outputValue = yotiAttributeValue.ToDate();

            Assert.AreEqual(inputType, outputType);
            Assert.AreEqual(inputBytes, outputBytes);
        }
    }
}