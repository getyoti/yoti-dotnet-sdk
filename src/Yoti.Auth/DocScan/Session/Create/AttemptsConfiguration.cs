using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create
{
    public class AttemptsConfiguration
    {
        [JsonProperty("ID_DOCUMENT_TEXT_DATA_EXTRACTION")]
        public Dictionary<string, int> IdDocumentTextDataExtraction { get; set; }
    }
}