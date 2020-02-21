using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public abstract class RequestedTask<T> : BaseRequestedTask where T : RequestedTaskConfig
    {
        [JsonProperty(PropertyName = "config")]
        public abstract T Config { get; }
    }
}