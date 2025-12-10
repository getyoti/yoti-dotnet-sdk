using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    /// <summary>
    /// Represents an ID photo with media information
    /// </summary>
    public class IdPhotoResponse : IResponseWithMediaProperty
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; internal set; }
    }
}
