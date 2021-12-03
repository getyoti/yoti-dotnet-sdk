using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document
{
    public class RequiredSupplementaryDocumentResourceResponse : RequiredDocumentResourceResponse
    {
        /// <summary>
        /// List of document types that can be used to satisfy the requirement
        /// </summary>
        [JsonProperty(PropertyName = "document_types")]
        public List<string> DocumentTypes { get; private set; }

        /// <summary>
        /// List of country codes that can be used to satisfy the requirement
        /// </summary>
        [JsonProperty(PropertyName = "country_codes")]
        public List<string> CountryCodes { get; private set; }

        /// <summary>
        /// The objective that the <see cref="RequiredSupplementaryDocumentResourceResponse"/> will satisfy
        /// </summary>
        [JsonProperty(PropertyName = "objective")]
        public ObjectiveResponse Objective { get; private set; }
    }
}