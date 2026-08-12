using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// ExpandedDocumentFieldsResponse represents the document fields in a document
    /// </summary>
    public class ExpandedDocumentFieldsResponse : IResponseWithMediaProperty
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }
    }
}