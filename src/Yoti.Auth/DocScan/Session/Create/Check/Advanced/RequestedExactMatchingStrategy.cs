using Newtonsoft.Json;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check.Advanced
{
    public class RequestedExactMatchingStrategy : RequestedCaMatchingStrategy
    {
        public override string Type => DocScanConstants.Exact;

        [JsonProperty(PropertyName = "exact_match")]
        public bool ExactMatch => true;
    }
}