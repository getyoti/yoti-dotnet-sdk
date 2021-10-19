using Yoti.Auth.DocScan.Session.Create.Check.Advanced;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public abstract class RequestedWatchlistAdvancedCaCheckBuilder
    {
        protected bool _removeDeceased;
        protected bool _shareUrl;
        protected RequestedCaSources _sources;
        protected RequestedCaMatchingStrategy _matchingStrategy;

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

        public abstract RequestedWatchlistAdvancedCaCheck Build();
    }
}