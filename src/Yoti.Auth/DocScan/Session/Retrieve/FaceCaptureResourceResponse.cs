using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// Face Capture Resource Response wrapping a <see cref="FaceCaptureImageResponse"></see>
    /// </summary>
    public class FaceCaptureResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "image")]
        public FaceCaptureImageResponse Image { get; private set; }
    }
}