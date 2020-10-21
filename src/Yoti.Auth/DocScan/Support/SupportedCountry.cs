using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Support
{
    public class SupportedCountry
    {
        public SupportedCountry(string code, List<SupportedDocument> supportedDocuments)
        {
            Code = code;
            SupportedDocuments = supportedDocuments;
        }

        [JsonProperty(PropertyName = "code")]
        public string Code { get; }

        [JsonProperty(PropertyName = "supported_documents")]
        public List<SupportedDocument> SupportedDocuments { get; }
    }
}