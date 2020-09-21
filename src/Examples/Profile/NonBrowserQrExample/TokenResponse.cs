using Newtonsoft.Json;

namespace NonBrowserQrExample
{
    public class TokenResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("subscription")]
        public string Subscription { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }
    }
}