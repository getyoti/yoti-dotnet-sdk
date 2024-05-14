using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity
{
    public class Content
    {
        [JsonProperty("profile")]
        public byte[] Profile { get; set; }

        [JsonProperty("extraData")]
        public byte[] ExtraData { get; set; }
    }

    public class ReceiptResponse
    {
        [JsonProperty("id")]
        public string ID { get; set; }

        [JsonProperty("sessionId")]
        public string SessionID { get; set; }

        [JsonProperty("timestamp")]
        public string Timestamp { get; set; }

        [JsonProperty("rememberMeId")]
        public string RememberMeID { get; set; }

        [JsonProperty("parentRememberMeId")]
        public string ParentRememberMeID { get; set; }

        [JsonProperty("content")]
        public Content Content { get; set; }

        [JsonProperty("otherPartyContent")]
        public Content OtherPartyContent { get; set; }

        [JsonProperty("wrappedItemKeyId")]
        public string WrappedItemKeyId { get; set; }

        [JsonProperty("wrappedKey")]
        public byte[] WrappedKey { get; set; }

        [JsonProperty("error")]
        public string Error { get; set; }
    }
}
