using System.Collections.Generic;
using AttrpubapiV1;
using Yoti.Auth.Anchors;
using static Yoti.Auth.Anchors.AnchorCertificateParser;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    public class AttributeConverter
    {
        public static YotiAttribute<object> ConvertAttribute(AttrpubapiV1.Attribute attribute)
        {
            YotiAttributeValue value;

            switch (attribute.ContentType)
            {
                case ContentType.String:
                    value = new YotiAttributeValue(TypeEnum.Text, attribute.Value.ToByteArray());
                    break;

                case ContentType.Date:
                    value = new YotiAttributeValue(TypeEnum.Date, attribute.Value.ToByteArray());
                    break;

                case ContentType.Jpeg:
                    value = new YotiAttributeValue(TypeEnum.Jpeg, attribute.Value.ToByteArray());
                    break;

                case ContentType.Png:
                    value = new YotiAttributeValue(TypeEnum.Png, attribute.Value.ToByteArray());
                    break;

                case ContentType.Json:
                    value = new YotiAttributeValue(TypeEnum.Json, attribute.Value.ToByteArray());
                    break;

                case ContentType.Undefined:
                    // do not return attributes with undefined content types
                    return null;

                default:
                    return null;
            }

            return new YotiAttribute<object>(
                        attribute.Name,
                        value,
                        ExtractMetadata(attribute, AnchorType.Source),
                        ExtractMetadata(attribute, AnchorType.Verifier));
        }

        private static HashSet<string> ExtractMetadata(AttrpubapiV1.Attribute attribute, AnchorType anchorType)
        {
            var entries = new HashSet<string>();
            foreach (Anchor anchor in attribute.Anchors)
            {
                AnchorVerifierSourceData anchorData = AnchorCertificateParser.GetTypesFromAnchor(anchor);
                if (anchorData.GetAnchorType() == anchorType)
                {
                    entries.UnionWith(anchorData.GetEntries());
                }
            }

            return entries;
        }
    }
}