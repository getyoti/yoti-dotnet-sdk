using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class GeneratedMedia
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }
    }
}