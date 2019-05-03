using System.Linq;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.ProtoBuf.Attribute;

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

            var attribute1 = CreateProtobufJsonAttribute(attribute1Name, byteJsonValue);
            var invalidAttribute = CreateProtobufJsonAttribute("invalidAttribute", ByteString.CopyFromUtf8("invalid"));
            var attribute2 = CreateProtobufJsonAttribute(attribute2Name, byteJsonValue);

            var attributeList = new ProtoBuf.Attribute.AttributeList
            {
                Attributes = { attribute1, invalidAttribute, attribute2 }
            };

            var result = AttributeConverter.ConvertToBaseAttributes(attributeList);

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

            var convertedAttributes = AttributeConverter.ConvertToBaseAttributes(attributeList);
            var result = (YotiAttribute<string>)convertedAttributes.Values.First();
            Assert.AreEqual("", result.GetValue());
        }

        [DataTestMethod]
        [DataRow(ContentType.Date)]
        [DataRow(ContentType.Jpeg)]
        [DataRow(ContentType.Json)]
        [DataRow(ContentType.Png)]
        [DataRow(ContentType.Undefined)]
        public void OtherAttributesWithEmptyValueAreNotAdded(ContentType contentType)
        {
            AttributeList attributeList = CreateAttributeListWithSingleAttribute(
                Constants.UserProfile.FullNameAttribute,
               contentType,
                _emptyByteStringValue);

            var convertedAttributes = AttributeConverter.ConvertToBaseAttributes(attributeList);
            Assert.AreEqual(0, convertedAttributes.Count);
        }

        private static AttributeList CreateAttributeListWithSingleAttribute(string name, ContentType contentType, ByteString byteStringValue)
        {
            var attribute = new Attribute
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

        private Attribute CreateProtobufJsonAttribute(string name, ByteString value)
        {
            return new Attribute
            {
                Name = name,
                ContentType = ContentType.Json,
                Value = value
            };
        }
    }
}