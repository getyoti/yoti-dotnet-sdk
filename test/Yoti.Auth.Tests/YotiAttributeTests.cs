using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Images;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiAttributeTests
    {
        [TestMethod]
        public void YotiAttribute_GetImageValue()
        {
            byte[] imageBytes = Conversion.UtfToBytes("ImageValue");

            var yotiAttribute = new YotiAttribute<Image>("selfie", AttrpubapiV1.ContentType.Jpeg, imageBytes);

            var expectedImage = new JpegImage(imageBytes);

            string expectedBase64URI = "data:image/jpeg;base64," + Conversion.BytesToBase64(imageBytes);
            Image actualImage = yotiAttribute.GetValue();

            Assert.IsTrue(new ImageComparer().Equals(expectedImage, actualImage));
            Assert.AreEqual(expectedBase64URI, actualImage.Base64URI());
        }

        [TestMethod]
        public void YotiAttribute_JpegBase64Uri()
        {
            byte[] jpegBytes = Conversion.UtfToBytes("jpegData");
            var yotiAttribute = new YotiAttribute<Image>("selfie", AttrpubapiV1.ContentType.Jpeg, jpegBytes);

            string expectedString = string.Format("data:image/jpeg;base64,{0}", Conversion.BytesToBase64(jpegBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetValue().Base64URI());
        }

        [TestMethod]
        public void YotiAttribute_PngBase64Uri()
        {
            byte[] pngBytes = Conversion.UtfToBytes("PngData");
            var yotiAttribute = new YotiAttribute<Image>("selfie", AttrpubapiV1.ContentType.Png, pngBytes);

            string expectedString = string.Format("data:image/png;base64,{0}", Conversion.BytesToBase64(pngBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetValue().Base64URI());
        }
    }
}