using System;
using System.Collections.Generic;

namespace Yoti.Auth.Share.ThirdParty
{
    public class AttributeIssuanceDetails
    {
        public DateTime? ExpiryDate { get; private set; }

        public string Token { get; private set; }
        public List<AttributeDefinition> IssuingAttributes { get; private set; }

        public AttributeIssuanceDetails(string token, DateTime? expiryDate, List<AttributeDefinition> issuingAttributes)
        {
            if (token == null)
                Token = "";
            else
                Token = token;

            ExpiryDate = expiryDate;
            IssuingAttributes = issuingAttributes;
        }
    }
}