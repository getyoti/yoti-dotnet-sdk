using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceMatchConfig : RequestedCheckConfig
    {
        public RequestedFaceMatchConfig(string manualCheck)
            : this(manualCheck, null)
        {
        }

        public RequestedFaceMatchConfig(string manualCheck, int? handledCheckLimit = null)
        {
            ManualCheck = manualCheck;
            HandledCheckLimit = handledCheckLimit;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }
    }
}