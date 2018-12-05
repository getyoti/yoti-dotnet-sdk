using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AttrpubapiV1;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Document;
using Yoti.Auth.Images;

namespace Yoti.Auth
{
    internal class AttributeConverter
    {
        public static BaseAttribute ConvertToBaseAttribute(AttrpubapiV1.Attribute attribute)
        {
            byte[] byteAttributeValue = attribute.Value.ToByteArray();

            switch (attribute.ContentType)
            {
                case ContentType.String:
                    string stringAttributeValue = Conversion.BytesToUtf8(byteAttributeValue);

                    if (attribute.Name == Constants.UserProfile.DocumentDetailsAttribute)
                    {
                        DocumentDetails documementDetails = DocumentDetailsAttributeParser.ParseFrom(stringAttributeValue);

                        return new YotiAttribute<DocumentDetails>(
                          attribute.Name,
                          documementDetails,
                          ParseAnchors(attribute));
                    }

                    return new YotiAttribute<string>(
                      attribute.Name,
                      Conversion.BytesToUtf8(byteAttributeValue),
                      ParseAnchors(attribute));

                case ContentType.Date:
                    return new YotiAttribute<DateTime>(
                      attribute.Name,
                      GetDateValue(byteAttributeValue),
                      ParseAnchors(attribute));

                case ContentType.Jpeg:
                    return new YotiAttribute<Image>(
                      attribute.Name,
                      new JpegImage(byteAttributeValue),
                      ParseAnchors(attribute));

                case ContentType.Png:
                    return new YotiAttribute<Image>(
                      attribute.Name,
                      new PngImage(byteAttributeValue),
                      ParseAnchors(attribute));

                case ContentType.Json:
                    return new YotiAttribute<Dictionary<string, JToken>>(
                      attribute.Name,
                      value: GetJsonValue(byteAttributeValue),
                      ParseAnchors(attribute));

                case ContentType.Undefined:
                    // do not return attributes with undefined content types
                    return null;

                default:
                    return new YotiAttribute<object>(
                        attribute.Name,
                        Conversion.BytesToUtf8(attribute.Value.ToByteArray()),
                        ParseAnchors(attribute));
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

        private static Dictionary<string, JToken> GetJsonValue(byte[] bytes)
        {
            string utf8JSON = Conversion.BytesToUtf8(bytes);
            Dictionary<string, JToken> deserializedJson = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, JToken>>(utf8JSON);
            return deserializedJson;
        }

        private static DateTime GetDateValue(byte[] bytes)
        {
            if (DateTime.TryParseExact(
                s: Conversion.BytesToUtf8(bytes),
                format: "yyyy-MM-dd",
                provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None,
                result: out DateTime date))
            {
                return date;
            }
            else throw new InvalidCastException("Unable to cast to DateTime");
        }
    }
}