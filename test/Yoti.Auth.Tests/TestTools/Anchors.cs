using Google.Protobuf;
using Google.Protobuf.Collections;

namespace Yoti.Auth.Tests.TestTools
{
    internal class Anchors
    {
        public static AttrpubapiV1.Attribute BuildAnchoredAttribute(string name, string value, AttrpubapiV1.ContentType contentType, string rawAnchor)
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAnchorToAttribute(Conversion.Base64ToBytes(rawAnchor), attribute);

            return attribute;
        }

        public static AttrpubapiV1.Attribute BuildDoubleAnchoredAttribute(string name, string value, AttrpubapiV1.ContentType contentType, string rawAnchor)
        {
            var attribute = new AttrpubapiV1.Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAnchorToAttribute(Conversion.Base64ToBytes(rawAnchor), attribute);
            AddAnchorToAttribute(Conversion.Base64ToBytes(rawAnchor), attribute);

            return attribute;
        }

        public static void AddAnchorToAttribute(byte[] anchorBytes, AttrpubapiV1.Attribute attribute)
        {
            attribute.Anchors.AddRange(
                new RepeatedField<AttrpubapiV1.Anchor>
                {
                    AttrpubapiV1.Anchor.Parser.ParseFrom(anchorBytes)
                });
        }
    }
}