using System;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Yoti.Auth.Document
{
    internal class DocumentDetailsAttributeParser
    {
        private const string _minimumAcceptable = "([A-Za-z_]*) ([A-Za-z]{3}) ([A-Za-z0-9]{1}).*";
        private const int TYPE_INDEX = 0;
        private const int COUNTRY_INDEX = 1;
        private const int NUMBER_INDEX = 2;
        private const int EXPIRATION_INDEX = 3;
        private const int AUTHORITY_INDEX = 4;

        public static DocumentDetails ParseFrom(string attributeValue)
        {
            if (attributeValue == null || !Regex.IsMatch(attributeValue, _minimumAcceptable))
            {
                throw new InvalidOperationException("Unable to parse attribute value to a DocumentDetails");
            }

            string[] attributes = Regex.Split(attributeValue, @"\s+").Where(s => !string.IsNullOrEmpty(s)).ToArray();

            return DocumentDetailsBuilder.Builder()
                    .WithType(attributes[TYPE_INDEX])
                    .WithIssuingCountry(attributes[COUNTRY_INDEX])
                    .WithNumber(attributes[NUMBER_INDEX])
                    .WithDate(GetDateSafely(attributes, EXPIRATION_INDEX))
                    .WithAuthority(GetSafely(attributes, AUTHORITY_INDEX))
                    .Build();
        }

        private static DateTime? GetDateSafely(string[] attributes, int index)
        {
            string expirationDate = GetSafely(attributes, index);

            if (expirationDate == null)
            {
                return null;
            }

            bool success = DateTime.TryParseExact(
                    s: expirationDate,
                    format: "yyyy-MM-dd",
                    provider: CultureInfo.InvariantCulture,
                    style: DateTimeStyles.None,
                    result: out DateTime parsedDate);

            if (success)
                return parsedDate;

            throw new FormatException(
                $"Unable to parse {nameof(expirationDate)} value of '{expirationDate}'");
        }

        private static string GetSafely(string[] attributes, int index)
        {
            string value = attributes.Length > index ? attributes[index] : null;
            return "-".Equals(value, System.StringComparison.Ordinal) ? null : value;
        }
    }
}