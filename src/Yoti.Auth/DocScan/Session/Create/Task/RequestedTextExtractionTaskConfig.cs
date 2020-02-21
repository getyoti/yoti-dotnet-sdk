using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedTextExtractionTaskConfig : RequestedTaskConfig
    {
        public RequestedTextExtractionTaskConfig(string manualCheck)
        {
            ManualCheck = manualCheck;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }
    }
}