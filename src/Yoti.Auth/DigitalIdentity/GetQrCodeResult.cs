using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DigitalIdentity.Extensions;

namespace Yoti.Auth.DigitalIdentity
{
    public class GetQrCodeResult
    {
#pragma warning disable 0649
        // These fields are assigned to by JSON deserialization
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("expiry")]
        public string Expiry { get; set; }

        [JsonProperty("policy")]
        public string Policy { get; set; }

        [JsonProperty("extensions")]
        private readonly List<BaseExtension> Extensions;

        [JsonProperty("session")]
        public ShareSessionResult Session { get; set; }

        [JsonProperty("redirectUri")]
        public string RedirectUri { get; set; }

#pragma warning restore 0649

    }
}