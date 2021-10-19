using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class WatchlistAdvancedCaSearchConfigResponseCustomAccount : WatchlistAdvancedCaSearchConfigResponse
	{
		[JsonProperty(PropertyName = "api_key")]
		public string ApiKey { get; internal set; }

		[JsonProperty(PropertyName = "monitoring")]
		public bool Monitoring { get; internal set; }

		[JsonProperty(PropertyName = "tags")]
		public Dictionary<string, string> Tags { get; internal set; }

		[JsonProperty(PropertyName = "client_ref")]
		public string ClientRef { get; internal set; }
	}
}