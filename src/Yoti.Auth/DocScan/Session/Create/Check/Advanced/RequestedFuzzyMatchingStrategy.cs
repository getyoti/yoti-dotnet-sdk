using Newtonsoft.Json;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
	public class RequestedFuzzyMatchingStrategy : RequestedCaMatchingStrategy
	{
		public override string Type => DocScanConstants.Fuzzy;

		[JsonProperty(PropertyName = "fuzziness")]
		public double Fuzziness { get; }

		public RequestedFuzzyMatchingStrategy(double fuzziness)
		{
			Fuzziness = fuzziness;
		}
	}
}