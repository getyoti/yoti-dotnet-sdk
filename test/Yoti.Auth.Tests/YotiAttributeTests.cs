using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Anchors;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiAttributeTests
    {
        private class ImageComparer : IEqualityComparer<Image>
        {
            public bool Equals(Image x, Image y)
            {
                if (x == null && y == null)
                {
                    return true;
                }

                if (x == null || y == null)
                {
                    return false;
                }

                return (x.Base64URI == y.Base64URI)
                    && (x.Data == y.Data)
                    && (x.Type == y.Type);
            }

            public int GetHashCode(Image obj)
            {
                throw new System.NotImplementedException();
            }
        }

        [TestMethod]
        public void YotiAttribute_GetImageValue()
        {
            byte[] imageBytes = Conversion.UtfToBytes("ImageValue");

            var yotiAttributeValue = new YotiAttributeValue(TypeEnum.Jpeg, imageBytes);
            var yotiAttribute = new YotiImageAttribute<Image>("selfie", yotiAttributeValue);

            var expectedImage = new Image
            {
                Base64URI = "data:image/jpeg;base64," + Conversion.BytesToBase64(imageBytes),
                Data = imageBytes,
                Type = TypeEnum.Jpeg
            };

            Image actualImage = yotiAttribute.GetImage();

            Assert.IsTrue(new ImageComparer().Equals(expectedImage, actualImage));
        }

        [TestMethod]
        public void YotiAttribute_GetValueOrDefault_ReturnsDefaultWhenNull()
        {
            var yotiAttribute = new YotiAttribute<string>(
                name: "given_names",
                value: null,
                anchors: new List<Anchor>());

            string defaultValue = "default";

            Assert.AreEqual(defaultValue, yotiAttribute.GetValueOrDefault(defaultValue));
        }

        [TestMethod]
        public void YotiAttribute_GetValueOrDefault_ReturnsValueWhenNotNull()
        {
            string expectedValue = "expected";
            var expectedAttributeValue = new YotiAttributeValue(TypeEnum.Text, Conversion.UtfToBytes(expectedValue));

            var yotiAttribute = new YotiAttribute<string>(
                name: "given_names",
                value: expectedAttributeValue,
                anchors: new List<Anchor>());

            string defaultValue = "default";

            Assert.AreEqual(expectedValue, yotiAttribute.GetValueOrDefault(defaultValue));
        }

        [TestMethod]
        public void YotiAttributeValue_JpegBase64Uri()
        {
            byte[] jpegBytes = Conversion.UtfToBytes("jpegData");
            var yotiAttributeValue = new YotiAttributeValue(
                TypeEnum.Jpeg,
                jpegBytes);
            var yotiAttribute = new YotiImageAttribute<Image>("selfie", yotiAttributeValue);

            string expectedString = String.Format("data:image/jpeg;base64,{0}", Conversion.BytesToBase64(jpegBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetBase64URI());
        }

        [TestMethod]
        public void YotiAttributeValue_PngBase64Uri()
        {
            byte[] pngBytes = Conversion.UtfToBytes("PngData");
            var yotiAttributeValue = new YotiAttributeValue(
                TypeEnum.Png,
                pngBytes);
            var yotiAttribute = new YotiImageAttribute<Image>("selfie", yotiAttributeValue);

            string expectedString = String.Format("data:image/png;base64,{0}", Conversion.BytesToBase64(pngBytes));

            Assert.AreEqual(expectedString, yotiAttribute.GetBase64URI());
        }
    }
}