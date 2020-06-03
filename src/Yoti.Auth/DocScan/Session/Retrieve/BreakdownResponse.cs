using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class BreakdownResponse
    {
        [JsonProperty(PropertyName = "sub_check")]
        public string SubCheck { get; private set; }

        [JsonProperty(PropertyName = "result")]
        public string Result { get; private set; }

        [JsonProperty(PropertyName = "details")]
        public List<DetailsResponse> Details { get; private set; }
    }
}