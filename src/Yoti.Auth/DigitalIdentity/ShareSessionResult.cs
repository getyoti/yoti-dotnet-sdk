using Newtonsoft.Json;


namespace Yoti.Auth.DigitalIdentity
{
    public class ShareSessionResult
    {
#pragma warning disable 0649
        // These fields are assigned to by JSON deserialization
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("expiry")]
        public string Expiry { get; set; }

#pragma warning restore 0649

    }

    
}