using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class ReportResponseWithSummary : ReportResponse
	{
		[JsonProperty(PropertyName = "watchlist_summary")]
		public WatchlistSummary WatchlistSummary { get; internal set; }
	}
}