using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.IdentityProfile
{
    public class FailureReasonResponse
    {
        [JsonProperty(PropertyName = "reason_code")]
        public string ReasonCode { get; private set; }
        
        [JsonProperty(PropertyName = "requirements_not_met_details")]
        public RequirementNotMetDetails RequirementNotMetDetails { get; private set; }
    }
}
