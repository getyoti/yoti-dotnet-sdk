using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class DocumentRestriction
    {
        public DocumentRestriction(List<string> countryCodes, List<string> documentTypes)
        {
            CountryCodes = countryCodes;
            DocumentTypes = documentTypes;
        }

        [JsonProperty(PropertyName = "country_codes")]
        public List<string> CountryCodes { get; }

        [JsonProperty(PropertyName = "document_types")]
        public List<string> DocumentTypes { get; }
    }
}