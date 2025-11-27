using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public abstract class RequestedCheckConfig
    {
        [JsonProperty(PropertyName = "handled_check_limit", NullValueHandling = NullValueHandling.Ignore)]
        public int? HandledCheckLimit { get; protected set; }
    }
}