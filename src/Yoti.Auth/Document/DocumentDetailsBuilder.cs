using System;

namespace Yoti.Auth.Document
{
    internal class DocumentDetailsBuilder
    {
        public static DocumentDetailsBuilder Builder()
        {
            return new DocumentDetailsBuilder();
        }

        private string _type;
        private string _issuingCountry;
        private string _number;
        private DateTime? _expirationDate;
        private string _authority;

        private DocumentDetailsBuilder()
        {
        }

        public DocumentDetailsBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public DocumentDetailsBuilder WithIssuingCountry(string issuingCountry)
        {
            _issuingCountry = issuingCountry;
            return this;
        }

        public DocumentDetailsBuilder WithNumber(string number)
        {
            _number = number;
            return this;
        }

        public DocumentDetailsBuilder WithDate(DateTime? expirationDate)
        {
            _expirationDate = expirationDate;
            return this;
        }

        public DocumentDetailsBuilder WithAuthority(string authority)
        {
            _authority = authority;
            return this;
        }

        public DocumentDetails Build()
        {
            CheckStringIsNotNullOrEmpty(_type);
            CheckStringIsNotNullOrEmpty(_issuingCountry);
            CheckStringIsNotNullOrEmpty(_number);

            return new DocumentDetails(_type, _issuingCountry, _number, _expirationDate, _authority);
        }

        private static void CheckStringIsNotNullOrEmpty(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new InvalidOperationException(
                    $"'{nameof(str)}' cannot be null or empty");
            }
        }
    }
}