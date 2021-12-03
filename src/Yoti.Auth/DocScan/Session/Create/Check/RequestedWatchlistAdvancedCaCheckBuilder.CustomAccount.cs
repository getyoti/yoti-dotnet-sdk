using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create.Check.Advanced;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedWatchlistAdvancedCaCheckBuilderCustomAccount : RequestedWatchlistAdvancedCaCheckBuilder
    {
        private string _apiKey;
        private bool _monitoring;
        private Dictionary<string, string> _tags;
        private string _clientRef;

        public RequestedWatchlistAdvancedCaCheckBuilderCustomAccount WithApiKey(string apiKey)
        {
            _apiKey = apiKey;
            return this;
        }

        public RequestedWatchlistAdvancedCaCheckBuilderCustomAccount WithMonitoring(bool monitoring)
        {
            _monitoring = monitoring;
            return this;
        }

        public RequestedWatchlistAdvancedCaCheckBuilderCustomAccount WithTags(Dictionary<string, string> tags)
        {
            _tags = tags;
            return this;
        }

        public RequestedWatchlistAdvancedCaCheckBuilderCustomAccount WithClientRef(string clientRef)
        {
            _clientRef = clientRef;
            return this;
        }

        public override RequestedWatchlistAdvancedCaCheck Build()
        {
            var config = new RequestedWatchlistAdvancedCaConfigCustomAccount(_removeDeceased, _shareUrl, _sources, _matchingStrategy, _apiKey, _monitoring, _tags, _clientRef);
            
            return new RequestedWatchlistAdvancedCaCheck(config);
        }
    }
}
