using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedSupplementaryDocTextExtractionTask : RequestedTask<RequestedSupplementaryDocTextExtractionTaskConfig>
    {
        public RequestedSupplementaryDocTextExtractionTask(RequestedSupplementaryDocTextExtractionTaskConfig config)
        {
            Config = config;
        }

        public override RequestedSupplementaryDocTextExtractionTaskConfig Config { get; }

        public override string Type => DocScanConstants.SupplementaryDocumentTextDataExtraction;
    }
}