using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// RequestedCheck requests creation of a Check to be performed on a document
    /// </summary>
    /// <typeparam name="T">The config of the requested check</typeparam>
    public abstract class RequestedCheck<T> : BaseRequestedCheck where T : RequestedCheckConfig
    {
        [JsonProperty(PropertyName = "config")]
        public abstract T Config { get; }
    }
}