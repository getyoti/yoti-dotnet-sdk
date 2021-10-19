using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
	public class RequestedWatchlistAdvancedCaConfigCustomAccount : RequestedWatchlistAdvancedCaConfig
	{
		public override string Type => DocScanConstants.WithCustomAccount;

		public RequestedWatchlistAdvancedCaConfigCustomAccount(bool removeDeceased, bool shareUrl, RequestedCaSources sources, RequestedCaMatchingStrategy matchingStrategy,
			string apiKey, bool monitoring, Dictionary<string, string> tags, string clientRef)
			: base(removeDeceased, shareUrl, sources, matchingStrategy)
		{
			ApiKey = apiKey;
			Monitoring = monitoring;
			Tags = tags;
			ClientRef = clientRef;
		}

		[JsonProperty(PropertyName = "api_key")]
		public string ApiKey { get; }

		[JsonProperty(PropertyName = "monitoring")]
		public bool Monitoring { get; }

		[JsonProperty(PropertyName = "tags")]
		public Dictionary<string, string> Tags { get; }

		[JsonProperty(PropertyName = "client_ref")]
		public string ClientRef { get; }
	}
}