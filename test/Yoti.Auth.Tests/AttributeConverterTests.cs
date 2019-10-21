using System.Collections.Generic;
using System.Linq;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Attribute;
using Yoti.Auth.Images;
using Yoti.Auth.ProtoBuf.Attribute;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class AttributeConverterTests
    {
        private readonly ByteString _emptyByteStringValue = ByteString.CopyFromUtf8("");

        [TestMethod]
        public void FailureInAttributeParsing_ShouldNotStopOtherAttributes()
        {
            string jsonValue = "{}";
            ByteString byteJsonValue = ByteString.CopyFromUtf8(jsonValue);
            string attribute1Name = "attribute1";
            string attribute2Name = "attribute2";

            ProtoBuf.Attribute.Attribute attribute1 = CreateProtobufAttribute(attribute1Name, byteJsonValue, ContentType.Json);
            ProtoBuf.Attribute.Attribute invalidAttribute = CreateProtobufAttribute("invalidAttribute", ByteString.CopyFromUtf8("invalid"), ContentType.Json);
            ProtoBuf.Attribute.Attribute attribute2 = CreateProtobufAttribute(attribute2Name, byteJsonValue, ContentType.Json);

            var attributeList = new ProtoBuf.Attribute.AttributeList
            {
                Attributes = { attribute1, invalidAttribute, attribute2 }
            };

            var result = Attribute.AttributeConverter.ConvertToBaseAttributes(attributeList);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey(attribute1Name));
            Assert.IsTrue(result.ContainsKey(attribute2Name));
        }

        [TestMethod]
        public void StringAttributeWithEmptyValueIsAdded()
        {
            AttributeList attributeList = CreateAttributeListWithSingleAttribute(
                Constants.UserProfile.FullNameAttribute,
                ContentType.String,
                _emptyByteStringValue);

            var convertedAttributes = Attribute.AttributeConverter.ConvertToBaseAttributes(attributeList);
            var result = (Attribute.YotiAttribute<string>)convertedAttributes.Values.First();
            Assert.AreEqual("", result.GetValue());
        }

        [DataTestMethod]
        [DataRow(ContentType.Date)]
        [DataRow(ContentType.Jpeg)]
        [DataRow(ContentType.Json)]
        [DataRow(ContentType.Png)]
        [DataRow(ContentType.Undefined)]
        [DataRow(ContentType.MultiValue)]
        public void OtherAttributesWithEmptyValueAreNotAdded(ContentType contentType)
        {
            AttributeList attributeList = CreateAttributeListWithSingleAttribute(
                Constants.UserProfile.FullNameAttribute,
               contentType,
                _emptyByteStringValue);

            var convertedAttributes = Attribute.AttributeConverter.ConvertToBaseAttributes(attributeList);
            Assert.AreEqual(0, convertedAttributes.Count);
        }

        [DataTestMethod]
        [DataRow(ContentType.Date)]
        [DataRow(ContentType.Jpeg)]
        [DataRow(ContentType.Json)]
        [DataRow(ContentType.Png)]
        [DataRow(ContentType.Undefined)]
        [DataRow(ContentType.MultiValue)]
        public void OtherAttributesWithEmptyValueThrowsException(ContentType contentType)
        {
            ProtoBuf.Attribute.Attribute attribute = CreateProtobufAttribute("attributeName", _emptyByteStringValue, contentType);

            Assert.ThrowsException<System.InvalidOperationException>(() =>
            {
                Attribute.AttributeConverter.ConvertToBaseAttribute(attribute);
            });
        }

        [TestMethod]
        public void ShouldParseMultiValueAttribute()
        {
            var multiValueProtobufAttribute = TestTools.Attributes.CreateProtobufAttributeFromRawAnchor(TestData.TestAttributes.MultiValueAttribute);

            var imageList = (YotiAttribute<List<Image>>)AttributeConverter.ConvertToBaseAttribute(multiValueProtobufAttribute);

            List<Image> imageValues = imageList.GetValue();
            Assert.AreEqual(2, imageValues.Count);

            AssertImages.ContainsExpectedImage(imageValues, "image/jpeg", "38TVEH/9k=");
            AssertImages.ContainsExpectedImage(imageValues, "image/jpeg", "vWgD//2Q==");
        }

        [TestMethod]
        public void ShouldParseMixedDocumentImageTypes()
        {
            var multiValue1 = new MultiValue.Types.Value
            {
                ContentType = ContentType.Png,
                Data = ByteString.FromBase64("iVBORw0KGgoAAAANSUhEUgAAAAEAAAABAQMAAAAl21bKAAAABlBMVEUAAAD///+l2Z/dAAAACXBIWXMAAA7EAAAOxAGVKw4bAAAACklEQVQImWNgAAAAAgAB9HFkpgAAAABJRU5ErkJggg==")
            };

            var multiValue2 = new MultiValue.Types.Value
            {
                ContentType = ContentType.Jpeg,
                Data = ByteString.FromBase64("/9j/4AAQSkZJRgABAQEAYABgAAD//gA+Q1JFQVRPUjogZ2QtanBlZyB2MS4wICh1c2luZyBJSkcgSlBFRyB2ODApLCBkZWZhdWx0IHF1YWxpdHkK/9sAQwAIBgYHBgUIBwcHCQkICgwUDQwLCwwZEhMPFB0aHx4dGhwcICQuJyAiLCMcHCg3KSwwMTQ0NB8nOT04MjwuMzQy/9sAQwEJCQkMCwwYDQ0YMiEcITIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIy/8AAEQgAAQABAwEiAAIRAQMRAf/EAB8AAAEFAQEBAQEBAAAAAAAAAAABAgMEBQYHCAkKC//EALUQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+v/EAB8BAAMBAQEBAQEBAQEAAAAAAAABAgMEBQYHCAkKC//EALURAAIBAgQEAwQHBQQEAAECdwABAgMRBAUhMQYSQVEHYXETIjKBCBRCkaGxwQkjM1LwFWJy0QoWJDThJfEXGBkaJicoKSo1Njc4OTpDREVGR0hJSlNUVVZXWFlaY2RlZmdoaWpzdHV2d3h5eoKDhIWGh4iJipKTlJWWl5iZmqKjpKWmp6ipqrKztLW2t7i5usLDxMXGx8jJytLT1NXW19jZ2uLj5OXm5+jp6vLz9PX29/j5+v/aAAwDAQACEQMRAD8A+f6KKKAP/9k=")
            };

            var multiValue = new MultiValue();
            multiValue.Values.Add(multiValue1);
            multiValue.Values.Add(multiValue2);

            AttributeList attributeList = CreateAttributeListWithSingleAttribute(
               Constants.UserProfile.DocumentImagesAttribute,
               ContentType.MultiValue,
               multiValue.ToByteString());

            var convertedAttributes = Attribute.AttributeConverter.ConvertToBaseAttributes(attributeList);
            var attribute = (Attribute.YotiAttribute<List<Image>>)convertedAttributes.Values.First();

            var attributeValues = attribute.GetValue();

            Assert.IsInstanceOfType(attributeValues[0], typeof(PngImage));
            Assert.IsInstanceOfType(attributeValues[1], typeof(JpegImage));
        }

        [TestMethod]
        public void ShouldFilterNonImageTypeInDocumentImages()
        {
            var multiValue1 = new MultiValue.Types.Value
            {
                ContentType = ContentType.String,
                Data = ByteString.CopyFromUtf8("stringValue")
            };

            var multiValue = new MultiValue();
            multiValue.Values.Add(multiValue1);

            AttributeList attributeList = CreateAttributeListWithSingleAttribute(
               Constants.UserProfile.DocumentImagesAttribute,
               ContentType.MultiValue,
               multiValue.ToByteString());

            var convertedAttributes = Attribute.AttributeConverter.ConvertToBaseAttributes(attributeList);
            Assert.AreEqual(0, convertedAttributes.Count);
        }

        [TestMethod]
        public void ShouldParseNonDocumentImageMultiValue()
        {
            string stringValue = "stringValue";
            string multiValueAttributeName = "multiValueAttributeName";
            var multiValue1 = new MultiValue.Types.Value
            {
                ContentType = ContentType.String,
                Data = ByteString.CopyFromUtf8(stringValue)
            };

            var multiValue = new MultiValue();
            multiValue.Values.Add(multiValue1);

            AttributeList attributeList = CreateAttributeListWithSingleAttribute(
               multiValueAttributeName,
               ContentType.MultiValue,
               multiValue.ToByteString());

            var convertedAttributes = Attribute.AttributeConverter.ConvertToBaseAttributes(attributeList);
            var attribute = (Attribute.YotiAttribute<List<MultiValueItem>>)convertedAttributes.Values.First();

            var attributeValue = attribute.GetValue().First();

            Assert.AreEqual(ContentType.String, attributeValue.ContentType);
            Assert.AreEqual(stringValue, attributeValue.Value);
        }

        private static AttributeList CreateAttributeListWithSingleAttribute(string name, ContentType contentType, ByteString byteStringValue)
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = byteStringValue
            };

            return new AttributeList
            {
                Attributes = { attribute }
            };
        }

        private ProtoBuf.Attribute.Attribute CreateProtobufAttribute(string name, ByteString value, ContentType contentType)
        {
            return new ProtoBuf.Attribute.Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = value
            };
        }
    }
}