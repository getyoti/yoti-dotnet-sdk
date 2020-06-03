using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedLivenessConfig : RequestedCheckConfig
    {
        public RequestedLivenessConfig(int maxRetries, string livenessType)
        {
            MaxRetries = maxRetries;
            LivenessType = livenessType;
        }

        [JsonProperty(PropertyName = "liveness_type")]
        public string LivenessType { get; }

        [JsonProperty(PropertyName = "max_retries")]
        public int MaxRetries { get; }
    }
}