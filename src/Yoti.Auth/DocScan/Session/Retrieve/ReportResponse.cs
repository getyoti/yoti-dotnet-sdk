using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class ReportResponse
    {
        [JsonProperty(PropertyName = "recommendation")]
        public RecommendationResponse Recommendation { get; private set; }

        [JsonProperty(PropertyName = "breakdown")]
        public List<BreakdownResponse> Breakdown { get; private set; }
    }
}