using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceComparisonConfig : RequestedCheckConfig
    {
        public RequestedFaceComparisonConfig(string manualCheck)
        {
            ManualCheck = manualCheck;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }
    }
}