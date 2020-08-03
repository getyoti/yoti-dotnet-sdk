using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    /// <summary>
    /// Abstract RequestedTask, neccesary so we can store a list of mixed <see cref="RequestedTask{T}"/> types (i.e. without a generic type)

    /// </summary>
    public abstract class BaseRequestedTask
    {
        [JsonProperty(PropertyName = "type")]
        public abstract string Type { get; }
    }
}