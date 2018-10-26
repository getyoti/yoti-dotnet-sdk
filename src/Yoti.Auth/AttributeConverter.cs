using System;
using System.Collections.Generic;
using System.Linq;
using AttrpubapiV1;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    internal class AttributeConverter
    {
        [Obsolete("Will be using ConvertToBaseAttribute instead")]
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
                        ParseAnchors(attribute));
        }

        public static BaseAttribute ConvertToBaseAttribute(AttrpubapiV1.Attribute attribute)
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

            return new BaseAttribute(
                        attribute.Name,
                        value,
                        ParseAnchors(attribute));
        }

        private static List<Yoti.Auth.Anchors.Anchor> ParseAnchors(AttrpubapiV1.Attribute attribute)
        {
            var yotiAnchors = new HashSet<Yoti.Auth.Anchors.Anchor>();

            foreach (AttrpubapiV1.Anchor protoBufAnchor in attribute.Anchors)
            {
                yotiAnchors.Add(new Yoti.Auth.Anchors.Anchor(protoBufAnchor));
            }

            return yotiAnchors.ToList();
        }
    }
}