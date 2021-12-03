using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create.Check.Advanced;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
	public class RequestedWatchlistAdvancedCaConfigYotiAccount : RequestedWatchlistAdvancedCaConfig
	{
		public override string Type => DocScanConstants.WithYotiAccount;
		public RequestedWatchlistAdvancedCaConfigYotiAccount(bool removeDeceased, bool shareUrl, RequestedCaSources sources, RequestedCaMatchingStrategy matchingStrategy)
			: base(removeDeceased, shareUrl, sources, matchingStrategy)
		{
		}
	}
}