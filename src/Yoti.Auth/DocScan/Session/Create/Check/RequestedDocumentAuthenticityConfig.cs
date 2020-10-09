using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedDocumentAuthenticityConfig : RequestedCheckConfig
    {
        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }

        public RequestedDocumentAuthenticityConfig(string manualCheck)
        {
            ManualCheck = manualCheck;
        }
    }
}