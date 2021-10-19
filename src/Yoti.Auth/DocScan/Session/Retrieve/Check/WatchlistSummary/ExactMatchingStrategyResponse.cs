using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class ExactMatchingStrategyResponse : CaMatchingStrategyResponse
	{
		[JsonProperty(PropertyName = "exact_match")]
		public bool ExactMatch { get; internal set; }
	}
}