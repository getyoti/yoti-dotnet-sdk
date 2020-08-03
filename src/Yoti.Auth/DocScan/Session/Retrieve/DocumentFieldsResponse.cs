using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// DocumentFieldsResponse represents the document fields in a document
    /// </summary>
    public class DocumentFieldsResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}