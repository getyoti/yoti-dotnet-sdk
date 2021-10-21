using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class DocumentIdPhotoResponse : IResponseWithMediaProperty
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}