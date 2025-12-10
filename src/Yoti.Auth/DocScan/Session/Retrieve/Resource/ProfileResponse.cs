using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    /// <summary>
    /// Represents a profile with media information
    /// </summary>
    public class ProfileResponse : IResponseWithMediaProperty
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; internal set; }
    }
}
