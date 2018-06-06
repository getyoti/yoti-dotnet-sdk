using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiAttributeValue_Tests
    {
        [TestMethod]
        public void YotiAttributeValue_ToDateValidDate_CorrectResult()
        {
            string dateString = "1985-01-13";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Date, dateString);
            DateTime? outputValue = yotiAttribueValue.ToDate();

            Assert.AreEqual(
                new DateTime(1985, 01, 13),
                outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateInvalidFormat_ReturnsNull()
        {
            string dateString = "1985/01/01";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Date, dateString);
            DateTime? outputValue = yotiAttribueValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateInvalidDate_ReturnsNull()
        {
            string dateString = "1985-13-01";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Date, dateString);
            DateTime? outputValue = yotiAttribueValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateOnPng_ReturnsNull()
        {
            string pngString = "PngImageData";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Png, pngString);
            DateTime? outputValue = yotiAttribueValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateOnJpeg_ReturnsNull()
        {
            string jpegString = "JpegData";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Jpeg, jpegString);
            DateTime? outputValue = yotiAttribueValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_CheckAccessors_NoExceptions()
        {
            var inputBytes = new byte[8];
            var inputType = TypeEnum.Jpeg;
            var yotiAttribueValue = new YotiAttributeValue(inputType, inputBytes);

            TypeEnum outputType = yotiAttribueValue.Type;
            byte[] outputBytes = yotiAttribueValue.ToBytes();

            var outputValue = yotiAttribueValue.ToDate();

            Assert.AreEqual(inputType, outputType);
            Assert.AreEqual(inputBytes, outputBytes);
        }
    }
}