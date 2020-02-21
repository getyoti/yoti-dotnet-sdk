using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class PageResponse
    {
        [JsonProperty(PropertyName = "capture_method")]
        public string CaptureMethod { get; private set; }

        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}