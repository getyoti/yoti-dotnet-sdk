using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedDocumentAuthenticityConfig : RequestedCheckConfig
    {
        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }

        [JsonProperty(PropertyName = "issuing_authority_sub_check")]
        public IssuingAuthoritySubCheck IssuingAuthoritySubCheck { get; }

        public RequestedDocumentAuthenticityConfig(string manualCheck, IssuingAuthoritySubCheck issuingAuthoritySubCheck = null)
        {
            ManualCheck = manualCheck;
            IssuingAuthoritySubCheck = issuingAuthoritySubCheck;
        }
    }
}