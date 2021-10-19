using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check
{
	public class GeneratedProfileResponse
	{
		[JsonProperty(PropertyName = "media")]
		public MediaResponse Media { get; internal set; }
	}
}