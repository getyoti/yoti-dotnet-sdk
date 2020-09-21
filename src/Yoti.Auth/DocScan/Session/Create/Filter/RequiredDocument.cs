using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public abstract class RequiredDocument
    {
        [JsonProperty(PropertyName = "type")]
        public abstract string Type { get; }
    }
}