using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class Notification
    {
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; } // Required if 'notification' is defined
        [JsonProperty(PropertyName = "method")]
        public string Method { get; set; } = "POST"; // Optional, defaults to 'POST'
        [JsonProperty(PropertyName = "headers")]
        public Dictionary<string, string> Headers { get; set; } // Optional
        [JsonProperty(PropertyName = "verifyTls")]
        public bool VerifyTls { get; set; } = true; // Optional, defaults to 'true' if URL is HTTPS
    }
}
