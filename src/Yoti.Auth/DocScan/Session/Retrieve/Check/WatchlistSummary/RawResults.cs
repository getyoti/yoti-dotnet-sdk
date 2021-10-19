using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class RawResults
	{
		[JsonProperty(PropertyName = "media")]
		public MediaResponse Media { get; private set; }
	}
}