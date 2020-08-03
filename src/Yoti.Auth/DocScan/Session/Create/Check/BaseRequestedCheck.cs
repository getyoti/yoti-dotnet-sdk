using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// Abstract RequestedCheck, neccesary so we can store a list of mixed <see cref="RequestedCheck{T}"/> types (i.e. without a generic type)
    /// </summary>
    public abstract class BaseRequestedCheck
    {
        [JsonProperty(PropertyName = "type")]
        public abstract string Type { get; }
    }
}