using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Yoti.Auth.DocScan.Session.Create.Check.Advanced;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(RequestedWatchlistAdvancedCaConfigYotiAccount), Constants.DocScanConstants.WithYotiAccount)]
    [JsonSubtypes.KnownSubType(typeof(RequestedWatchlistAdvancedCaConfigCustomAccount), Constants.DocScanConstants.WithCustomAccount)]
    public abstract class RequestedWatchlistAdvancedCaConfig : RequestedCheckConfig
    {
        [JsonProperty(PropertyName = "type")]
        public abstract string Type { get; }

        public RequestedWatchlistAdvancedCaConfig(bool removeDeceased, bool shareUrl, RequestedCaSources sources, RequestedCaMatchingStrategy matchingStrategy)
        {
            RemoveDeceased = removeDeceased;
            ShareUrl = shareUrl;
            Sources = sources;
            MatchingStrategy = matchingStrategy;
        }

        [JsonProperty(PropertyName = "remove_deceased")]
        public bool RemoveDeceased { get; }

        [JsonProperty(PropertyName = "share_url")]
        public bool ShareUrl { get; }

        [JsonProperty(PropertyName = "sources")]
        public RequestedCaSources Sources { get; }

        [JsonProperty(PropertyName = "matching_strategy")]
        public RequestedCaMatchingStrategy MatchingStrategy { get; }
    }
}
