using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class FrameResponse
    {
        [JsonProperty(PropertyName = "Media")]
        public MediaResponse Media { get; private set; }
    }
}