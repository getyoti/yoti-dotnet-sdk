using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Anchors;
using Yoti.Auth.Tests.TestTools;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiAttributeTests
    {
        [TestMethod]
        public void YotiAttribute_GetImageValue()
        {
            byte[] imageBytes = Conversion.UtfToBytes("ImageValue");

            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Jpeg, imageBytes);
            var yotiAttribute = new YotiImageAttribute<Image>("selfie", yotiAttributeValue);

            var expectedImage = new Image
            {
                Data = imageBytes,
                Type = TypeEnum.Jpeg
            };

            string expectedBase64URI = "data:image/jpeg;base64," + Conversion.BytesToBase64(imageBytes);
            Image actualImage = yotiAttribute.GetValue();

            Assert.IsTrue(new ImageComparer().Equals(expectedImage, actualImage));
            Assert.AreEqual(expectedBase64URI, actualImage.Base64URI);
        }

        [TestMethod]
        public void YotiAttribute_GetValueOrDefault_ReturnsDefaultWhenNull()
        {
            var yotiAttribute = new YotiAttribute<string>(
                name: "given_names",
                value: null,
                anchors: new List<Anchor>());

            const string defaultValue = "default";

            Assert.AreEqual(defaultValue, yotiAttribute.GetValueOrDefault(defaultValue));
        }

        [TestMethod]
        public void YotiAttribute_GetValueOrDefault_ReturnsValueWhenNotNull()
        {
            const string expectedValue = "expected";
            var expectedAttributeValue = new YotiAttributeValue(TypeEnum.Text, Conversion.UtfToBytes(expectedValue));

            var yotiAttribute = new YotiAttribute<string>(
                name: "given_names",
                value: expectedAttributeValue,
                anchors: new List<Anchor>());

            const string defaultValue = "default";

            Assert.AreEqual(expectedValue, yotiAttribute.GetValueOrDefault(defaultValue));
        }

        [TestMethod]
        public void YotiAttributeValue_JpegBase64Uri()
        {
            byte[] jpegBytes = Conversion.UtfToBytes("jpegData");
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Jpeg, jpegBytes);
            var yotiAttribute = new YotiImageAttribute<Image>("selfie", yotiAttributeValue);

            string expectedString = string.Format("data:image/jpeg;base64,{0}", Conversion.BytesToBase64(jpegBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetBase64URI());
        }

        [TestMethod]
        public void YotiAttributeValue_PngBase64Uri()
        {
            byte[] pngBytes = Conversion.UtfToBytes("PngData");
            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Png, pngBytes);
            var yotiAttribute = new YotiImageAttribute<Image>("selfie", yotiAttributeValue);

            string expectedString = string.Format("data:image/png;base64,{0}", Conversion.BytesToBase64(pngBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetBase64URI());
        }
    }
}