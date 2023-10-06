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

        [JsonProperty("created")]
        public string Created { get; set; }

        [JsonProperty("updated")]
        public string Updated { get; set; }

        [JsonProperty("qrCode")]
        public qrCode QrCode { get; set; }

        [JsonProperty("receipt")]
        public receipt Receipt { get; set; }

#pragma warning restore 0649

    }

    public class qrCode
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class receipt
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}