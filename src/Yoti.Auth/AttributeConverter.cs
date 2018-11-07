using System;
using System.Collections.Generic;
using System.Linq;
using AttrpubapiV1;
using Newtonsoft.Json.Linq;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth
{
    internal class AttributeConverter
    {
        public static BaseAttribute ConvertToBaseAttribute(AttrpubapiV1.Attribute attribute)
        {
            YotiAttributeValue value;

            switch (attribute.ContentType)
            {
                case ContentType.String:
                    value = new YotiAttributeValue(TypeEnum.Text, attribute.Value.ToByteArray());
                    return new YotiAttribute<string>(
                      attribute.Name,
                      value,
                      ParseAnchors(attribute));

                case ContentType.Date:
                    value = new YotiAttributeValue(TypeEnum.Date, attribute.Value.ToByteArray());
                    return new YotiAttribute<DateTime?>(
                        attribute.Name,
                        value,
                        ParseAnchors(attribute));

                case ContentType.Jpeg:
                    value = new YotiAttributeValue(TypeEnum.Jpeg, attribute.Value.ToByteArray());
                    return new YotiAttribute<Image>(
                        attribute.Name,
                        value,
                        ParseAnchors(attribute));

                case ContentType.Png:
                    value = new YotiAttributeValue(TypeEnum.Png, attribute.Value.ToByteArray());
                    return new YotiAttribute<Image>(
                        attribute.Name,
                        value,
                        ParseAnchors(attribute));

                case ContentType.Json:
                    value = new YotiAttributeValue(TypeEnum.Json, attribute.Value.ToByteArray());
                    return new YotiAttribute<IEnumerable<Dictionary<string, JToken>>>(
                        attribute.Name,
                        value,
                        ParseAnchors(attribute));

                case ContentType.Undefined:
                    // do not return attributes with undefined content types
                    return null;

                default:
                    return null;
            }
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