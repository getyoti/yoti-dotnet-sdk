using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.CreateFaceCaptureResourceResponse
{
    /// <summary>
    /// Create Face Capture Resource Response with Id and Frames.
    /// </summary>
    public class CreateFaceCaptureResourceResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "frames")]
        public int Frames { get; private set; }
    }
}