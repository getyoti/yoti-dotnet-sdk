using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// RequestedDocumentAuthenticityCheck requests creation of a Identity Document Comparison Check
    /// </summary>
    public class RequestedIdDocumentComparisonCheck : RequestedCheck<RequestedIdDocumentComparisonConfig>
    {
        public RequestedIdDocumentComparisonCheck(RequestedIdDocumentComparisonConfig config)
        {
            Config = config;
        }

        public override RequestedIdDocumentComparisonConfig Config { get; }

        public override string Type => DocScanConstants.IdDocumentComparison;
    }
}