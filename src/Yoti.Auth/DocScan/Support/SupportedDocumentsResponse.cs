using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Support
{
    public class SupportedDocumentsResponse
    {
        public SupportedDocumentsResponse(List<SupportedCountry> supportedCountries)
        {
            SupportedCountries = supportedCountries;
        }

        [JsonProperty(PropertyName = "supported_countries")]
        public List<SupportedCountry> SupportedCountries { get; }
    }
}