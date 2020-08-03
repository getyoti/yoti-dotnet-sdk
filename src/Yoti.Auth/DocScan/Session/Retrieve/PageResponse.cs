using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// PageResponse represents information about an uploaded document Page
    /// </summary>
    public class PageResponse
    {
        [JsonProperty(PropertyName = "capture_method")]
        public string CaptureMethod { get; private set; }

        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}