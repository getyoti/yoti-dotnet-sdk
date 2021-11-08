using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document
{
    public class SupportedDocumentResponse
    {
        /// <summary>
        /// The type of document that is supported
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }
    }
}