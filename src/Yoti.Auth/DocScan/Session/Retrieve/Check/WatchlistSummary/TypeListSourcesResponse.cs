using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class TypeListSourcesResponse : CaSourcesResponse
	{
		[JsonProperty(PropertyName = "types")]
		public List<string> Types { get; internal set; }
	}
}