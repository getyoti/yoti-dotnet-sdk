using Google.Protobuf;
using Google.Protobuf.Collections;
using Yoti.Auth.ProtoBuf.Attribute;

namespace Yoti.Auth.Tests.TestTools
{
    internal class Anchors
    {
        public static ProtoBuf.Attribute.Attribute BuildAnchoredAttribute(string name, string value, ContentType contentType, string rawAnchor)
        {
            var attribute = new ProtoBuf.Attribute.Attribute
            {
                Name = name,
                ContentType = contentType,
                Value = ByteString.CopyFromUtf8(value)
            };

            AddAnchorToAttribute(Conversion.Base64ToBytes(rawAnchor), attribute);

            return attribute;
        }

        public static void AddAnchorToAttribute(byte[] anchorBytes, ProtoBuf.Attribute.Attribute attribute)
        {
            attribute.Anchors.AddRange(
                new RepeatedField<Anchor>
                {
                    Anchor.Parser.ParseFrom(anchorBytes)
                });
        }
    }
}