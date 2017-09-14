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
            string pngData = "pngData";

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
    }
}