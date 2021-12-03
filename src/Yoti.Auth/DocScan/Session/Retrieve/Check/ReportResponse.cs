using System.Collections.Generic;
using JsonSubTypes;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// ReportResponse represents a report for a given check
    /// </summary>
    [JsonConverter(typeof(JsonSubtypes))]
    [JsonSubtypes.KnownSubTypeWithProperty(typeof(ReportResponseWithSummary), "watchlist_summary")]
    public class ReportResponse
    {
        [JsonProperty(PropertyName = "recommendation")]
        public RecommendationResponse Recommendation { get; private set; }

        [JsonProperty(PropertyName = "breakdown")]
        public List<BreakdownResponse> Breakdown { get; private set; }
    }
}