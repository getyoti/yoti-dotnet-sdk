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

            Attribute attribute1 = CreateProtobufAttribute(attribute1Name, byteJsonValue, ContentType.Json);
            Attribute invalidAttribute = CreateProtobufAttribute("invalidAttribute", ByteString.CopyFromUtf8("invalid"), ContentType.Json);
            Attribute attribute2 = CreateProtobufAttribute(attribute2Name, byteJsonValue, ContentType.Json);

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

        [DataTestMethod]
        [DataRow(ContentType.Date)]
        [DataRow(ContentType.Jpeg)]
        [DataRow(ContentType.Json)]
        [DataRow(ContentType.Png)]
        [DataRow(ContentType.Undefined)]
        public void OtherAttributesWithEmptyValueThrowsException(ContentType contentType)
        {
            Attribute attribute = CreateProtobufAttribute("attributeName", _emptyByteStringValue, contentType);

            Assert.ThrowsException<System.InvalidOperationException>(() =>
            {
                AttributeConverter.ConvertToBaseAttribute(attribute);
            });
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

        private Attribute CreateProtobufAttribute(string name, ByteString value, ContentType contentType)
        {
            return new Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = value
            };
        }
    }
}