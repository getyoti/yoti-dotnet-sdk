using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	[JsonConverter(typeof(JsonSubtypes), "type")]
	[JsonSubtypes.KnownSubType(typeof(ExactMatchingStrategyResponse), Constants.DocScanConstants.Exact)]
	[JsonSubtypes.KnownSubType(typeof(FuzzyMatchingStrategyResponse), Constants.DocScanConstants.Fuzzy)]
	public abstract class CaMatchingStrategyResponse
	{
		[JsonProperty(PropertyName = "type")]
		public string Type { get; internal set; }
	}
}