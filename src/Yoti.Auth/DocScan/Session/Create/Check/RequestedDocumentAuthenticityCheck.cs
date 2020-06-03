using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedDocumentAuthenticityCheck : RequestedCheck<RequestedDocumentAuthenticityConfig>
    {
        public RequestedDocumentAuthenticityCheck(RequestedDocumentAuthenticityConfig config)
        {
            Config = config;
        }

        public override RequestedDocumentAuthenticityConfig Config { get; }

        public override string Type => DocScanConstants.IdDocumentAuthenticity;
    }
}