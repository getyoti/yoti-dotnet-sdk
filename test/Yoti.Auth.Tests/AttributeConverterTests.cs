using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class AttributeConverterTests
    {
        private const string StringValue = "{}";
        private readonly ByteString _byteValue = ByteString.CopyFromUtf8(StringValue);

        [TestMethod]
        public void FailureInAttributeParsing_ShouldNotStopOtherAttributes()
        {
            var attribute1 = CreateProtobufJsonAttribute(Constants.UserProfile.StructuredPostalAddressAttribute, _byteValue);
            var invalidAttribute = CreateProtobufJsonAttribute(Constants.UserProfile.PostalAddressAttribute, ByteString.CopyFromUtf8("invalid"));
            var attribute2 = CreateProtobufJsonAttribute(Constants.UserProfile.PhoneNumberAttribute, _byteValue);

            var attributeList = new ProtoBuf.Attribute.AttributeList
            {
                Attributes = { attribute1, invalidAttribute, attribute2 }
            };

            var result = AttributeConverter.ConvertToBaseAttributes(attributeList);

            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.ContainsKey(Constants.UserProfile.StructuredPostalAddressAttribute));
            Assert.IsTrue(result.ContainsKey(Constants.UserProfile.PhoneNumberAttribute));
        }

        private ProtoBuf.Attribute.Attribute CreateProtobufJsonAttribute(string name, ByteString value)
        {
            return new ProtoBuf.Attribute.Attribute
            {
                Name = name,
                ContentType = ProtoBuf.Attribute.ContentType.Json,
                Value = value
            };
        }
    }
}