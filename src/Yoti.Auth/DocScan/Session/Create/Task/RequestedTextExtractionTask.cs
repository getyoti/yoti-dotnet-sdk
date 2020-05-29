using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedTextExtractionTask : RequestedTask<RequestedTextExtractionTaskConfig>
    {
        public RequestedTextExtractionTask(RequestedTextExtractionTaskConfig config)
        {
            Config = config;
        }

        public override RequestedTextExtractionTaskConfig Config { get; }

        public override string Type => DocScanConstants.IdDocumentTextDataExtraction;
    }
}