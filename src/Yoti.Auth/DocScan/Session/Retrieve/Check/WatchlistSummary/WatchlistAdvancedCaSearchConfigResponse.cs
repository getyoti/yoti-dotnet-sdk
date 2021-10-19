using JsonSubTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	[JsonConverter(typeof(JsonSubtypes), "type")]
	[JsonSubtypes.KnownSubType(typeof(WatchlistAdvancedCaSearchConfigResponseYotiAccount), Yoti.Auth.Constants.DocScanConstants.WithYotiAccount)]
	[JsonSubtypes.KnownSubType(typeof(WatchlistAdvancedCaSearchConfigResponseCustomAccount), Yoti.Auth.Constants.DocScanConstants.WithCustomAccount)]
	public abstract class WatchlistAdvancedCaSearchConfigResponse : ISearchConfig
	{
		[JsonProperty(PropertyName = "type")]
		public string Type { get; internal set; }

		[JsonProperty(PropertyName = "remove_deceased")]
		public bool RemoveDeceased { get; internal set; }

		[JsonProperty(PropertyName = "share_url")]
		public bool ShareUrl { get; internal set; }

		[JsonProperty(PropertyName = "sources")]
		public CaSourcesResponse Sources { get; internal set; }

		[JsonProperty(PropertyName = "matching_strategy")]
		public CaMatchingStrategyResponse MatchingStrategy { get; internal set; }
	}
}
