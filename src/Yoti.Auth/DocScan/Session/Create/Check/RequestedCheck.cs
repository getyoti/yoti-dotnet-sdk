using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public abstract class RequestedCheck<T> : BaseRequestedCheck where T : RequestedCheckConfig
    {
        [JsonProperty(PropertyName = "config")]
        public abstract T Config { get; }
    }
}