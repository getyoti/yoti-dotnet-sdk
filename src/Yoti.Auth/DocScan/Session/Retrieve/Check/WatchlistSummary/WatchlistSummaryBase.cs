using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
    public class WatchlistSummaryBase
    {
        [JsonProperty(PropertyName = "total_hits")]
        public int TotalHits { get; internal set; }

        [JsonProperty(PropertyName = "raw_results")]
        public RawResults RawResults { get; internal set; }

        [JsonProperty(PropertyName = "associated_country_codes")]
        public List<string> AssociatedCountryCodes { get; internal set; }

        [JsonProperty(PropertyName = "search_config")]
        public ISearchConfig SearchConfig { get; internal set; }
    }
}