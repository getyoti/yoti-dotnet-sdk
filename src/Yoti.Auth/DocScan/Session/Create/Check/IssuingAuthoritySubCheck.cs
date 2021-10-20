using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create.Filter;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class IssuingAuthoritySubCheck
    {
        [JsonProperty(PropertyName = "requested")]
        public bool Requested { get; }

        [JsonProperty(PropertyName = "filter")]
        public DocumentFilter Filter { get; }

        public IssuingAuthoritySubCheck(bool requested, DocumentFilter filter = null)
        {
            Requested = requested;
            Filter = filter;
        }
    }
}