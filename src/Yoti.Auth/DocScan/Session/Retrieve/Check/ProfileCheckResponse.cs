using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Check
{
	/// <summary>
	/// Abstract base for API check responses that include a GeneratedProfile
	/// </summary>
	public abstract class ProfileCheckResponse : CheckResponse
	{
		[JsonProperty(PropertyName = "generated_profile")]
		public GeneratedProfileResponse GeneratedProfile { get; internal set; }
	}
}