using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiAttributeValue_Tests
    {
        [TestMethod]
        public void YotiAttributeValue_JpegData_DoesntThrowException()
        {
            string jpegData = "jpegData";

            YotiAttributeValue yotiAttributeValue = new YotiAttributeValue(
                TypeEnum.Jpeg,
                jpegData);
        }

        [TestMethod]
        public void YotiAttributeValue_PngData_DoesntThrowException()
        {
            string pngData = "pngImageData";

            YotiAttributeValue yotiAttributeValue = new YotiAttributeValue(
                TypeEnum.Png,
                pngData);
        }

        [TestMethod]
        public void YotiAttributeValue_DateData_DoesntThrowException()
        {
            string dateString = "01/01/1985";

            YotiAttributeValue yotiAttributeValue = new YotiAttributeValue(
                TypeEnum.Date,
                dateString);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateValidDate_DoesntThrowException()
        {
            string dateString = "01/01/1985";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Text, dateString);

            var outputValue = yotiAttribueValue.ToDate();

            Assert.IsInstanceOfType(outputValue, typeof(DateTime));
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateInvalidDate_ReturnsNull()
        {
            string dateString = "01/13/1985";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Date, dateString);
            var outputValue = yotiAttribueValue.ToDate();

            Assert.IsNull(outputValue);
        }

        [TestMethod]
        public void YotiAttributeValue_ToDateOnPng_ReturnsNull()
        {
            string dateString = "PngImageData";
            var yotiAttribueValue = new YotiAttributeValue(TypeEnum.Png, dateString);
            var outputValue = yotiAttribueValue.ToDate();

            Assert.IsNull(outputValue);
        }
    }
}