using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// FaceMapResponse represents a FaceMap response object
    /// </summary>
    public class FaceMapResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}