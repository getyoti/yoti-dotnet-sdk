using System;
using System.Collections.Generic;

namespace Yoti.Auth.Share.ThirdParty
{
    public class IssuingAttributes
    {
        public DateTime? ExpiryDate { get; private set; }
        public List<AttributeDefinition> AttributeDefinitions { get; private set; }

        public IssuingAttributes(DateTime? expiryDate, List<AttributeDefinition> attributeDefinitions)
        {
            ExpiryDate = expiryDate;
            AttributeDefinitions = attributeDefinitions;
        }
    }
}