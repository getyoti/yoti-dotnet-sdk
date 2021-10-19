using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check.WatchlistSummary
{
	[JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(SearchProfileSourcesResponse), Constants.DocScanConstants.Profile)]
    [JsonSubtypes.KnownSubType(typeof(TypeListSourcesResponse), Constants.DocScanConstants.TypeList)]
    public abstract class CaSourcesResponse
	{
		[JsonProperty(PropertyName = "type")]
		public string Type { get; internal set; }
	}
}