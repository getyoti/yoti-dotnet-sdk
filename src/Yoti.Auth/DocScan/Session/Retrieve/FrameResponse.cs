using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// FrameResponse represents a frame of a resource
    /// </summary>
    public class FrameResponse
    {
        [JsonProperty(PropertyName = "Media")]
        public MediaResponse Media { get; private set; }
    }
}