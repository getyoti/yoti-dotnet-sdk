using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity
{
    public class ReceiptItemKeyResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("iv")]
        public byte[] Iv { get; set; }

        [JsonProperty("value")]
        public byte[] Value { get; set; }
    }
}