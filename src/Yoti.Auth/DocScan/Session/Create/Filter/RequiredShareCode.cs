using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class RequiredShareCode : RequiredDocument
    {
        [JsonProperty(PropertyName = "issuer")]
        public string Issuer { get; }

        [JsonProperty(PropertyName = "scheme")]
        public string Scheme { get; }

        public RequiredShareCode(string issuer, string scheme)
        {
            Issuer = issuer;
            Scheme = scheme;
        }

        public override string Type => Constants.DocScanConstants.ShareCode;
    }
}
