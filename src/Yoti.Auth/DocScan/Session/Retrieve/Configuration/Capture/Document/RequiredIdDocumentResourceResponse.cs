using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document
{
    public class RequiredIdDocumentResourceResponse : RequiredDocumentResourceResponse
    {
        /// <summary>
        /// List of supported country codes that can be used to satisfy the requirement
        /// </summary>
        /// <remarks>
        /// Each supported country will contain a list of document types that can be used
        /// </remarks>
        [JsonProperty(PropertyName = "supported_countries")]
        public List<SupportedCountryResponse> SupportedCountries { get; private set; }

        /// <summary>
        /// The allowed capture methods as a string 
        /// </summary>
        [JsonProperty(PropertyName = "allowed_capture_methods")]
        public string AllowedCaptureMethods { get; private set; }

        /// <summary>
        /// Used to track how many attempts are remaining when performing text-extraction
        /// </summary>
        [JsonProperty(PropertyName = "attempts_remaining")]
        public Dictionary<string, int> AttemptsRemaining { get; private set; }
    }
}