using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedTextExtractionTaskConfig : RequestedTaskConfig
    {
        public RequestedTextExtractionTaskConfig(string manualCheck, string chipData = null, bool? createExpandedDocumentFields = false)
        {
            ManualCheck = manualCheck;
            ChipData = chipData;
            CreateExpandedDocumentFields = createExpandedDocumentFields;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }

        [JsonProperty(PropertyName = "chip_data")]
        public string ChipData { get; }

        [JsonProperty(PropertyName = "create_expanded_document_fields")]
        public bool? CreateExpandedDocumentFields { get; }
    }
}