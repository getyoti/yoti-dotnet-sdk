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

        [JsonProperty(PropertyName = "is_strictly_latin")]
        public string IsStrictlyLatin { get; private set; }

    }
}