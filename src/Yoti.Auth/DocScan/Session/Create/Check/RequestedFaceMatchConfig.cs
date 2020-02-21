using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceMatchConfig : RequestedCheckConfig
    {
        public RequestedFaceMatchConfig(string manualCheck)
        {
            ManualCheck = manualCheck;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }
    }
}