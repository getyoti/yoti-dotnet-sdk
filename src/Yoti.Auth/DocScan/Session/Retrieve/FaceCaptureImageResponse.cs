using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// Face Capture Image Response wrapping a <see cref="MediaResponse"/>
    /// </summary>
    public class FaceCaptureImageResponse : IResponseWithMediaProperty
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}