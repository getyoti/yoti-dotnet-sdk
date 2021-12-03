using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class FuzzyMatchingStrategyResponse : CaMatchingStrategyResponse
	{
		[JsonProperty(PropertyName = "fuzziness")]
		public double Fuzziness { get; internal set; }
	}
}