using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
	[JsonConverter(typeof(JsonSubtypes), "type")]
	[JsonSubtypes.KnownSubType(typeof(RequestedExactMatchingStrategy), Constants.DocScanConstants.Exact)]
	[JsonSubtypes.KnownSubType(typeof(RequestedFuzzyMatchingStrategy), Constants.DocScanConstants.Fuzzy)]
	public abstract class RequestedCaMatchingStrategy
	{
		[JsonProperty(PropertyName = "type")]
		public abstract string Type { get; }
	}
}