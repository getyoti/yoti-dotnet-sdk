using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	[JsonConverter(typeof(JsonSubtypes))]
	[JsonSubtypes.KnownSubTypeWithProperty(typeof(WatchlistAdvancedCaSearchConfigResponse), "type")]
	[JsonSubtypes.KnownSubTypeWithProperty(typeof(WatchlistScreeningConfig), "categories")] 
	public interface ISearchConfig
	{ 
	}
}