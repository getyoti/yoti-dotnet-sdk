using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Represents a required share code for session creation
    /// </summary>
    public class RequiredShareCode
    {
        internal RequiredShareCode(string issuer, string scheme)
        {
            Issuer = issuer;
            Scheme = scheme;
        }

        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; }

        [JsonProperty(PropertyName = "scheme")]
        public string Scheme { get; }
    }
}
