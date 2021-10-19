using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class SearchProfileSourcesResponse : CaSourcesResponse
	{
		[JsonProperty(PropertyName = "search_profile")]
		public string SearchProfile { get; internal set; }
	}
}