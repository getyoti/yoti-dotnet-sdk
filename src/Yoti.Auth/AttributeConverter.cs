using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Document;
using Yoti.Auth.Images;
using Yoti.Auth.ProtoBuf.Attribute;

namespace Yoti.Auth
{
    internal static class AttributeConverter
    {
        public static Dictionary<string, BaseAttribute> ConvertToBaseAttributes(ProtoBuf.Attribute.AttributeList attributeList)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            var parsedAttributes = new Dictionary<string, BaseAttribute>();

            foreach (ProtoBuf.Attribute.Attribute attribute in attributeList.Attributes)
            {
                try
                {
                    parsedAttributes.Add(attribute.Name, ConvertToBaseAttribute(attribute));
                }
                catch (Exception ex)
                {
                    logger.Warn($"Failed to parse attribute '{attribute.Name}' due to '{ex.Message}'");
                }
            }

            return parsedAttributes;
        }

        public static BaseAttribute ConvertToBaseAttribute(ProtoBuf.Attribute.Attribute attribute)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

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

                default:
                    logger.Warn($"Unknown content type {attribute.ContentType}, attempting to parse it as a string");
                    return new YotiAttribute<string>(
                        attribute.Name,
                        Conversion.BytesToUtf8(attribute.Value.ToByteArray()),
                        ParseAnchors(attribute));
            }
        }

        private static List<Yoti.Auth.Anchors.Anchor> ParseAnchors(ProtoBuf.Attribute.Attribute attribute)
        {
            var yotiAnchors = new HashSet<Yoti.Auth.Anchors.Anchor>();

            foreach (Anchor protoBufAnchor in attribute.Anchors)
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