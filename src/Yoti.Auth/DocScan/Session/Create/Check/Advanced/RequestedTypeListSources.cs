using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
	public class RequestedTypeListSources : RequestedCaSources
	{
		public override string Type => DocScanConstants.TypeList;

		[JsonProperty(PropertyName = "types")]
		public List<string> Types { get; }

        	public RequestedTypeListSources(List<string> types)
		{
			Types = types;
		}
	}
}