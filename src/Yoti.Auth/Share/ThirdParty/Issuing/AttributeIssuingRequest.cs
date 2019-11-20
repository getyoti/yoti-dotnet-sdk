using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.Share.ThirdParty.Issuing
{
    public class AttributeIssuingRequest
    {
        [JsonProperty(PropertyName = "issuance_token")]
        public string IssuanceToken { get; private set; }

        [JsonProperty(PropertyName = "attributes")]
        public List<IssuingAttribute> IssuingAttributes { get; private set; }

        public AttributeIssuingRequest(string issuanceToken, List<IssuingAttribute> issuingAttributes)
        {
            Validation.NotNullOrEmpty(issuanceToken, nameof(issuanceToken));

            IssuanceToken = issuanceToken;
            IssuingAttributes = issuingAttributes;
        }
    }
}