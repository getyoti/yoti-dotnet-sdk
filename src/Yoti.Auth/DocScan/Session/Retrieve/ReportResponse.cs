using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// ReportResponse represents a report for a given check
    /// </summary>
    public class ReportResponse
    {
        [JsonProperty(PropertyName = "recommendation")]
        public RecommendationResponse Recommendation { get; private set; }

        [JsonProperty(PropertyName = "breakdown")]
        public List<BreakdownResponse> Breakdown { get; private set; }
    }
}