using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class FaceMapResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}