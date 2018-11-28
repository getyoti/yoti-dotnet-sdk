using System;
using System.Collections.Generic;
using System.Linq;
using AttrpubapiV1;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Images;

namespace Yoti.Auth
{
    internal class AttributeConverter
    {
        public static BaseAttribute ConvertToBaseAttribute(AttrpubapiV1.Attribute attribute)
        {
            switch (attribute.ContentType)
            {
                case ContentType.String:
                    return new YotiAttribute<string>(
                      attribute.Name,
                      attribute.ContentType,
                      attribute.Value.ToByteArray(),
                      ParseAnchors(attribute));

                case ContentType.Date:
                    return new YotiAttribute<DateTime>(
                        attribute.Name,
                      attribute.ContentType,
                      attribute.Value.ToByteArray(),
                      ParseAnchors(attribute));

                case ContentType.Jpeg:
                    return new YotiAttribute<Image>(
                        attribute.Name,
                      attribute.ContentType,
                      attribute.Value.ToByteArray(),
                      ParseAnchors(attribute));

                case ContentType.Png:
                    return new YotiAttribute<Image>(
                       attribute.Name,
                      attribute.ContentType,
                      attribute.Value.ToByteArray(),
                      ParseAnchors(attribute));

                case ContentType.Json:
                    return new YotiAttribute<IEnumerable<Dictionary<string, JToken>>>(
                        attribute.Name,
                      attribute.ContentType,
                      attribute.Value.ToByteArray(),
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