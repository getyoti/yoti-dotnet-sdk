using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
	[JsonConverter(typeof(JsonSubtypes), "type")]
	[JsonSubtypes.KnownSubType(typeof(RequestedTypeListSources), Constants.DocScanConstants.TypeList)]
	[JsonSubtypes.KnownSubType(typeof(RequestedSearchProfileSources), Constants.DocScanConstants.Profile)]
	public abstract class RequestedCaSources
	{
		[JsonProperty(PropertyName = "type")]
		public abstract string Type { get; }
	}
}