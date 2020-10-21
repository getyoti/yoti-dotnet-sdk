using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    /// <summary>
    /// Requests creation of a Task to be performed on each document
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RequestedTask<T> : BaseRequestedTask where T : RequestedTaskConfig
    {
        [JsonProperty(PropertyName = "config")]
        public abstract T Config { get; }
    }
}