using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    /// <summary>
    /// Represents a required share code configuration
    /// </summary>
    public class RequiredShareCode
    {
        /// <summary>
        /// The issuer of the share code
        /// </summary>
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; set; }

        /// <summary>
        /// The scheme of the share code
        /// </summary>
        [JsonProperty(PropertyName = "scheme")]
        public string Scheme { get; set; }

        public RequiredShareCode(string issuer, string scheme)
        {
            Issuer = issuer;
            Scheme = scheme;
        }
    }
}
