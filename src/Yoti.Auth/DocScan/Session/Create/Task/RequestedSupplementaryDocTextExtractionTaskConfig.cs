using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedSupplementaryDocTextExtractionTaskConfig : RequestedTaskConfig
    {
        public RequestedSupplementaryDocTextExtractionTaskConfig(string manualCheck)
        {
            ManualCheck = manualCheck;
        }

        [JsonProperty(PropertyName = "manual_check")]
        public string ManualCheck { get; }
    }
}