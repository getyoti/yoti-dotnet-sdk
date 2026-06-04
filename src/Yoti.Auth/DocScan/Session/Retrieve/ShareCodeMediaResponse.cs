using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// Represents a media wrapper used within share code resources
    /// </summary>
    public class ShareCodeMediaResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; internal set; }
    }
}
