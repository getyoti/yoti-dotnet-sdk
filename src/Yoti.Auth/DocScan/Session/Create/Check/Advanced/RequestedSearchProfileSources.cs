using Newtonsoft.Json;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
	public class RequestedSearchProfileSources : RequestedCaSources
	{
		public override string Type => DocScanConstants.Profile;

		[JsonProperty(PropertyName = "search_profile")]
		public string SearchProfile { get; }

		public RequestedSearchProfileSources(string searchProfile)
		{
			SearchProfile = searchProfile;
		}
	}
}