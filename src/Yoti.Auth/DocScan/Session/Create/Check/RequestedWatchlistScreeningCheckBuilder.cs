using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create.Check
{

    /// <summary>
    /// Builds the <see cref="RequestedWatchlistScreeningCheck"/> using the <see cref="RequestedWatchlistScreeningConfig"/> configured using the methods here.
    /// </summary>
    /// <remarks>
    /// A watchlist screening check is a type of AML (Anti Money Laundering) check against watchlists.<br/>
    /// Note: To request a WatchlistScreeningCheck you must request ID_DOCUMENT_TEXT_DATA_EXTRACTION as a minimum<br/>
    /// (e.g. using <see cref="SessionSpecificationBuilder.WithRequestedTask"/> and <seealso cref="Yoti.Auth.DocScan.Session.Create.Task.RequestedTextExtractionTaskBuilder"/>)
    /// </remarks>
    public class RequestedWatchlistScreeningCheckBuilder
    {
        private readonly List<string> _categories = new List<string>();
        private int? _handledCheckLimit;

        /// <summary>
        /// Adds SANCTIONS to the list of categories to check in the watchlist screening check
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedWatchlistScreeningCheckBuilder ForSanctions()
        {
            return WithCategory(Constants.DocScanConstants.Sanctions);
        }

        /// <summary>
        /// Adds ADVERSE_MEDIA to the list of categories to check in the watchlist screening check
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedWatchlistScreeningCheckBuilder ForAdverseMedia()
        {
            return WithCategory(Constants.DocScanConstants.AdverseMedia);
        }

        /// <summary>
        /// Adds a category to the list of categories for the watchlist screening check
        /// </summary>
        /// <param name="category">the category to add</param>
        /// <returns>The builder</returns>
        public RequestedWatchlistScreeningCheckBuilder WithCategory(string category)
        {
            Validation.NotNullOrWhiteSpace(category, nameof(category));
            if (!_categories.Contains(category))
                _categories.Add(category);
            return this;
        }

        /// <summary>
        /// Sets the number of times a check response should return a handled result before switching to success (for sandbox testing)
        /// </summary>
        /// <param name="handledCheckLimit">The number of handled check responses before success</param>
        /// <returns>The <see cref="RequestedWatchlistScreeningCheckBuilder"/></returns>
        public RequestedWatchlistScreeningCheckBuilder WithHandledCheckLimit(int handledCheckLimit)
        {
            _handledCheckLimit = handledCheckLimit;
            return this;
        }

        public RequestedWatchlistScreeningCheck Build()
        {
            var config = new RequestedWatchlistScreeningConfig(_categories, _handledCheckLimit);

            return new RequestedWatchlistScreeningCheck(config);
        }
    }
}