using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// Requests creation of a Watchlist Screening Check
    /// </summary>
    public class RequestedWatchlistAdvancedCaCheck : RequestedCheck<RequestedWatchlistAdvancedCaConfig>
    {
        public RequestedWatchlistAdvancedCaCheck(RequestedWatchlistAdvancedCaConfig config)
        {
            Config = config;
        }

        public override RequestedWatchlistAdvancedCaConfig Config { get; }

        public override string Type => DocScanConstants.WatchlistAdvancedCa;
    }
}