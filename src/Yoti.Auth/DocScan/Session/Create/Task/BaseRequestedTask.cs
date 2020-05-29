using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public abstract class BaseRequestedTask
    {
        [JsonProperty(PropertyName = "type")]
        public abstract string Type { get; }
    }
}