using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// Requests creation of a Watchlist Screening Check
    /// </summary>
    /// <remarks>
    ///     Note: To request a <see cref="RequestedWatchlistScreeningCheck"/> you must request ID_DOCUMENT_TEXT_DATA_EXTRACTION as a minimum<br/>
    ///     (e.g. using <see cref="SessionSpecificationBuilder.WithRequestedTask"/> and <seealso cref="Yoti.Auth.DocScan.Session.Create.Task.RequestedTextExtractionTaskBuilder"/>)
    /// </remarks>
    public class RequestedWatchlistScreeningCheck : RequestedCheck<RequestedWatchlistScreeningConfig>
    {
        public RequestedWatchlistScreeningCheck(RequestedWatchlistScreeningConfig config)
        {
            Config = config;
        }

        public override RequestedWatchlistScreeningConfig Config { get; }

        public override string Type => DocScanConstants.WatchlistScreening;
    }
}