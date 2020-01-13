using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Attribute;
using Yoti.Auth.Images;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiAttributeTests
    {
        [TestMethod]
        public void GetValueShouldRetrieveImage()
        {
            byte[] imageBytes = Conversion.UtfToBytes("ImageValue");

            var yotiAttribute = new YotiAttribute<Image>("selfie", new JpegImage(imageBytes), null);

            var expectedImage = new JpegImage(imageBytes);

            string expectedBase64URI = string.Format("data:image/jpeg;base64,{0}", Conversion.BytesToBase64(imageBytes));
            Image actualImage = yotiAttribute.GetValue();

            Assert.IsTrue(new ImageComparer().Equals(expectedImage, actualImage));
            Assert.AreEqual(expectedBase64URI, actualImage.GetBase64URI());
        }

        [TestMethod]
        public void JpegBase64UriShouldRetrieveCorrectValue()
        {
            byte[] jpegBytes = Conversion.UtfToBytes("jpegData");
            var yotiAttribute = new YotiAttribute<Image>(
                name: "selfie",
                value: new JpegImage(jpegBytes),
                anchors: null);

            string expectedString = string.Format("data:image/jpeg;base64,{0}", Conversion.BytesToBase64(jpegBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetValue().GetBase64URI());
        }

        [TestMethod]
        public void PngBase64UriShouldRetrieveCorrectValue()
        {
            byte[] pngBytes = Conversion.UtfToBytes("PngData");
            var yotiAttribute = new YotiAttribute<Image>(
                name: "selfie",
                value: new PngImage(pngBytes),
                anchors: null);

            string expectedString = string.Format("data:image/png;base64,{0}", Conversion.BytesToBase64(pngBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetValue().GetBase64URI());
        }
    }
}