using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Document;
using Yoti.Auth.Images;
using Yoti.Auth.Properties;
using Yoti.Auth.ProtoBuf.Attribute;

namespace Yoti.Auth.Attribute
{
    internal static class AttributeConverter
    {
        public static Dictionary<string, BaseAttribute> ConvertToBaseAttributes(AttributeList attributeList)
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

            if (attribute.ContentType != ContentType.String
                    && attribute.Value.Length == 0)
                throw new InvalidOperationException(Properties.Resources.EmptyValueInvalid);

            byte[] byteAttributeValue = attribute.Value.ToByteArray();

            return CreateYotiAttribute(attribute, logger, byteAttributeValue);
        }

        private static BaseAttribute CreateYotiAttribute(ProtoBuf.Attribute.Attribute attribute, NLog.Logger logger, byte[] byteAttributeValue)
        {
            object value = ParseAttributeValue(attribute.ContentType, logger, byteAttributeValue);

            switch (attribute.ContentType)
            {
                case ContentType.String:
                    if (attribute.Name == Constants.UserProfile.DocumentDetailsAttribute)
                    {
                        DocumentDetails documementDetails = DocumentDetailsAttributeParser.ParseFrom((string)value);

                        return new YotiAttribute<DocumentDetails>(
                          attribute.Name,
                          documementDetails,
                          ParseAnchors(attribute));
                    }

                    return new YotiAttribute<string>(
                      attribute.Name,
                      (string)value,
                      ParseAnchors(attribute));

                case ContentType.Date:
                    return new YotiAttribute<DateTime>(
                      attribute.Name,
                      (DateTime)value,
                      ParseAnchors(attribute));

                case ContentType.Jpeg:
                    return new YotiAttribute<Image>(
                      attribute.Name,
                      (JpegImage)value,
                      ParseAnchors(attribute));

                case ContentType.Png:
                    return new YotiAttribute<Image>(
                      attribute.Name,
                      (PngImage)value,
                      ParseAnchors(attribute));

                case ContentType.Json:
                    return new YotiAttribute<Dictionary<string, JToken>>(
                      attribute.Name,
                      (Dictionary<string, JToken>)value,
                      ParseAnchors(attribute));

                case ContentType.MultiValue:
                    var multiValueList = (List<MultiValueItem>)value;
                    if (attribute.Name == Constants.UserProfile.DocumentImagesAttribute)
                    {
                        return new YotiAttribute<List<Image>>(
                            attribute.Name,
                            value: CreateImageListFromMultiValue(multiValueList),
                            ParseAnchors(attribute));
                    }

                    return new YotiAttribute<List<MultiValueItem>>(
                        attribute.Name,
                        multiValueList,
                        ParseAnchors(attribute));

                default:
                    logger.Warn($"Unknown content type {attribute.ContentType}, attempting to parse it as a string");
                    return new YotiAttribute<string>(
                        attribute.Name,
                        Conversion.BytesToUtf8(byteAttributeValue),
                        ParseAnchors(attribute));
            }
        }

        private static object ParseAttributeValue(ContentType contentType, NLog.Logger logger, byte[] byteAttributeValue)
        {
            switch (contentType)
            {
                case ContentType.String:
                    return Conversion.BytesToUtf8(byteAttributeValue);

                case ContentType.Date:
                    return GetDateValue(byteAttributeValue);

                case ContentType.Jpeg:
                    return new JpegImage(byteAttributeValue);

                case ContentType.Png:
                    return new PngImage(byteAttributeValue);

                case ContentType.Json:
                    return GetJsonValue(byteAttributeValue);

                case ContentType.MultiValue:
                    return ConvertMultiValue(byteAttributeValue);

                default:
                    logger.Warn($"Unknown content type {contentType}, attempting to parse it as a string");
                    return Conversion.BytesToUtf8(byteAttributeValue);
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

        private static List<MultiValueItem> ConvertMultiValue(byte[] byteAttributeValue)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
            List<MultiValueItem> multiValueList = new List<MultiValueItem>();

            var protobufultiValue = ProtoBuf.Attribute.MultiValue.Parser.ParseFrom(byteAttributeValue);
            foreach (MultiValue.Types.Value value in protobufultiValue.Values)
            {
                object attributeValue = ParseAttributeValue(value.ContentType, logger, value.Data.ToByteArray());

                multiValueList.Add(new MultiValueItem(attributeValue, value.ContentType));
            }

            return multiValueList;
        }

        private static List<Image> CreateImageListFromMultiValue(List<MultiValueItem> multiValueItemList)
        {
            var imageList = new List<Image>();

            foreach (var multiValueItem in multiValueItemList)
            {
                if (multiValueItem.ContentType == ContentType.Jpeg)
                    imageList.Add((JpegImage)multiValueItem.Value);
                else if (multiValueItem.ContentType == ContentType.Png)
                    imageList.Add((PngImage)multiValueItem.Value);
                else
                    throw new InvalidOperationException(Resources.UnsupportedImageType);
            }

            return imageList;
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
            else throw new InvalidCastException(Properties.Resources.InvalidCastDateTime);
        }
    }
}