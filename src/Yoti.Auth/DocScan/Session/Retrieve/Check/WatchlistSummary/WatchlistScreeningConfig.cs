using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	public class WatchlistScreeningConfig : ISearchConfig
	{
		[JsonProperty(PropertyName = "categories")]
		public List<string> Categories { get; private set; }
	}
}