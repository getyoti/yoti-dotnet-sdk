using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document
{
    public class SupportedCountryResponse
    {
        /// <summary>
        /// The ISO Country Code of the supported country
        /// </summary>
        [JsonProperty(PropertyName = "code")]
        public string Code { get; private set; }

        /// <summary>
        /// List of document types that are supported for the country code
        /// </summary>
        [JsonProperty(PropertyName = "supported_documents")]
        public List<SupportedDocumentResponse> SupportedDocuments { get; private set; }
    }
}