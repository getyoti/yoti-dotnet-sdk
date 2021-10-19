using Yoti.Auth.DocScan.Session.Create.Check.Advanced;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
	public class RequestedWatchlistAdvancedCaCheckBuilderYotiAccount : RequestedWatchlistAdvancedCaCheckBuilder
	{
		public override RequestedWatchlistAdvancedCaCheck Build()
		{
			var config = new RequestedWatchlistAdvancedCaConfigYotiAccount(_removeDeceased, _shareUrl, _sources, _matchingStrategy);
			return new RequestedWatchlistAdvancedCaCheck(config);
		}
	}
}