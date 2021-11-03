using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    public abstract class AllowedSourceResponse
    {
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }
    }
}