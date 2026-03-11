using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.AdvancedIdentityProfile
{
    public class FailureReasonResponse
    {
        [JsonProperty(PropertyName = "reason_code")]
        public  string ReasonCode { get; private set; }
    }
}
