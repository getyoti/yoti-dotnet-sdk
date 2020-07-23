using System;
using System.Collections.Generic;
using System.Globalization;
using Google.Protobuf;
using Google.Protobuf.Collections;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Properties;
using Yoti.Auth.ProtoBuf.Share;

namespace Yoti.Auth.Share.ThirdParty
{
    internal static class ThirdPartyAttributeConverter
    {
        internal static AttributeIssuanceDetails ParseThirdPartyAttribute(byte[] value)
        {
            var thirdPartyAttribute = ThirdPartyAttribute.Parser.ParseFrom(value);

            IssuingAttributes issuingAttributes = ParseIssuingAttributes(thirdPartyAttribute.IssuingAttributes);

            ByteString token = thirdPartyAttribute.IssuanceToken;
            string base64Token = Conversion.BytesToUrlSafeBase64(token.ToByteArray(), false);

            DateTime? expiryDate = issuingAttributes.ExpiryDate;
            List<AttributeDefinition> attributeDefinitions = issuingAttributes.AttributeDefinitions;

            if (string.IsNullOrEmpty(base64Token))
                throw new ExtraDataException(Resources.EmptyValueInvalid);

            return new AttributeIssuanceDetails(base64Token, expiryDate, attributeDefinitions);
        }

        private static IssuingAttributes ParseIssuingAttributes(ProtoBuf.Share.IssuingAttributes issuingAttributes)
        {
            string expiryDateString = issuingAttributes.ExpiryDate;
            DateTime? expiryDate = ParseExpiryDateTime(expiryDateString);

            List<AttributeDefinition> attributeDefinitions = ParseDefinitions(issuingAttributes.Definitions);

            return new IssuingAttributes(expiryDate, attributeDefinitions);
        }

        private static List<AttributeDefinition> ParseDefinitions(RepeatedField<Definition> definitions)
        {
            List<AttributeDefinition> parsedAttributes = new List<AttributeDefinition>();

            foreach (var definition in definitions)
            {
                parsedAttributes.Add(new AttributeDefinition(definition.Name));
            }

            return parsedAttributes;
        }

        private static DateTime? ParseExpiryDateTime(string dateTimeStringValue)
        {
            NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

            if (string.IsNullOrEmpty(dateTimeStringValue))
                return null;

            if (DateTime.TryParse(
                 dateTimeStringValue,
                 provider: CultureInfo.InvariantCulture,
                 styles: DateTimeStyles.None,
                 result: out DateTime date))
            {
                return date.ToUniversalTime();
            }
            else
                logger.Warn($"Failed to parse date: {dateTimeStringValue}");
            return null;
        }
    }
}