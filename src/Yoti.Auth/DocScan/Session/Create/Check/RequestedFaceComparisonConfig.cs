using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceComparisonConfig : RequestedCheckConfig
    {
        public RequestedFaceComparisonConfig(string manualCheck, int? handledCheckLimit = null)
        {
            ManualCheck = manualCheck;
            HandledCheckLimit = handledCheckLimit;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }
    }
}