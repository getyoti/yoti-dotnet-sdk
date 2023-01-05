using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedLivenessConfig : RequestedCheckConfig
    {
        public RequestedLivenessConfig(int maxRetries, string livenessType, string manualCheck)
        {
            MaxRetries = maxRetries;
            LivenessType = livenessType;
            ManualCheck = manualCheck;
        }

        [JsonProperty(PropertyName = "liveness_type")]
        public string LivenessType { get; }

        [JsonProperty(PropertyName = "max_retries")]
        public int MaxRetries { get; }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }
    }
}