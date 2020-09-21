using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class DocumentRestrictionsFilter : DocumentFilter
    {
        public DocumentRestrictionsFilter(string inclusion, List<DocumentRestriction> documents)
        : base(Constants.DocScanConstants.DocumentRestrictions)
        {
            Inclusion = inclusion;
            Documents = documents;
        }

        [JsonProperty(PropertyName = "inclusion")]
        public string Inclusion { get; }

        [JsonProperty(PropertyName = "documents")]
        public List<DocumentRestriction> Documents { get; }
    }
}