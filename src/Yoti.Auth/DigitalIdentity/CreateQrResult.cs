using Newtonsoft.Json;


namespace Yoti.Auth.DigitalIdentity
{
    public class CreateQrResult
    {
#pragma warning disable 0649
        // These fields are assigned to by JSON deserialization
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("uri")]
        public string Uri { get; set; }
#pragma warning restore 0649

    }
}
