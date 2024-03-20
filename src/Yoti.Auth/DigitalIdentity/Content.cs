using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity
{
    public class Content
    {
        [JsonProperty("profile")]
        public string Profile { get; set; }

        [JsonProperty("extraData")]
        public string ExtraData { get; set; }
    }
}
