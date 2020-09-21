using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class RequiredIdDocument : RequiredDocument
    {
        [JsonProperty(PropertyName = "filter")]
        public DocumentFilter Filter { get; }

        public RequiredIdDocument(DocumentFilter documentFilter)
        {
            Filter = documentFilter;
        }

        public override string Type => Constants.DocScanConstants.IdDocument;
    }
}