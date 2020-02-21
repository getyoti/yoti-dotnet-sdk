using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public abstract class BaseRequestedCheck
    {
        [JsonProperty(PropertyName = "type")]
        public abstract string Type { get; }
    }
}