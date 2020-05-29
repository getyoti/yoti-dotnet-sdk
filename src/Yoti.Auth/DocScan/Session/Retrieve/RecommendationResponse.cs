using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class RecommendationResponse
    {
        [JsonProperty(PropertyName = "value")]
        public string Value { get; private set; }

        [JsonProperty(PropertyName = "reason")]
        public string Reason { get; private set; }

        [JsonProperty(PropertyName = "recovery_suggestion")]
        public string RecoverySuggestion { get; private set; }
    }
}