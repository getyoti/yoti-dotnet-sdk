using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// RequestedDocumentAuthenticityCheck requests creation of a Document Authenticity Check
    /// </summary>
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