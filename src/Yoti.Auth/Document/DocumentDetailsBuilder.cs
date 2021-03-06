﻿using System;

namespace Yoti.Auth.Document
{
    internal class DocumentDetailsBuilder
    {
        private string _type;
        private string _issuingCountry;
        private string _number;
        private DateTime? _expirationDate;
        private string _authority;

        public DocumentDetailsBuilder()
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
            Validation.NotNullOrEmpty(_type, nameof(_type));
            Validation.NotNullOrEmpty(_issuingCountry, nameof(_issuingCountry));
            Validation.NotNullOrEmpty(_number, nameof(_number));

            return new DocumentDetails(_type, _issuingCountry, _number, _expirationDate, _authority);
        }
    }
}