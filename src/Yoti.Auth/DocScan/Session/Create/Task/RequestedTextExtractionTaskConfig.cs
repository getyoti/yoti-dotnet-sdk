using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedTextExtractionTaskConfig : RequestedTaskConfig
    {
        public RequestedTextExtractionTaskConfig(string manualCheck, string chipData = null)
        {
            ManualCheck = manualCheck;
            ChipData = chipData;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }

        [JsonProperty(PropertyName = "chip_data")]
        public string ChipData { get; }
    }
}