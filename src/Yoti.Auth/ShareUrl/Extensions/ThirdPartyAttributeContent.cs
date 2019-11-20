using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Yoti.Auth.Share.ThirdParty;

namespace Yoti.Auth.ShareUrl.Extensions
{
    public class ThirdPartyAttributeContent
    {
        private readonly DateTime _expiryDate;

        public ThirdPartyAttributeContent(DateTime expiryDate, List<AttributeDefinition> definitions)
        {
            _expiryDate = expiryDate;
            Definitions = definitions;
        }

        [JsonProperty(PropertyName = "definitions")]
        public List<AttributeDefinition> Definitions { get; private set; }

        [JsonProperty(PropertyName = "expiry_date")]
        public string ExpiryDate
        {
            get
            {
                return _expiryDate.ToString(Constants.Format.RFC3339PatternMilli, DateTimeFormatInfo.InvariantInfo);
            }
        }
    }
}