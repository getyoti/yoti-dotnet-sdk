using Yoti.Auth.DocScan.Session.Create.Check.Advanced;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public abstract class RequestedWatchlistAdvancedCaCheckBuilder
    {
        protected bool _removeDeceased;
        protected bool _shareUrl;
        protected RequestedCaSources _sources;
        protected RequestedCaMatchingStrategy _matchingStrategy;
        protected int? _handledCheckLimit;

        public RequestedWatchlistAdvancedCaCheckBuilder WithRemoveDeceased(bool removeDeceased)
        {
            _removeDeceased = removeDeceased;
            return this;
        }

        public RequestedWatchlistAdvancedCaCheckBuilder WithShareUrl(bool shareUrl)
        {
            _shareUrl = shareUrl;
            return this;
        }

        public RequestedWatchlistAdvancedCaCheckBuilder WithSources(RequestedCaSources sources)
        {
            _sources = sources;
            return this;
        }

        public RequestedWatchlistAdvancedCaCheckBuilder WithMatchingStrategy(RequestedCaMatchingStrategy matchingStrategy)
        {
            _matchingStrategy = matchingStrategy;
            return this;
        }

        /// <summary>
        /// Sets the number of times a check response should return a handled result before switching to success (for sandbox testing)
        /// </summary>
        /// <param name="handledCheckLimit">The number of handled check responses before success</param>
        /// <returns>The builder</returns>
        public RequestedWatchlistAdvancedCaCheckBuilder WithHandledCheckLimit(int handledCheckLimit)
        {
            _handledCheckLimit = handledCheckLimit;
            return this;
        }

        public abstract RequestedWatchlistAdvancedCaCheck Build();
    }
}