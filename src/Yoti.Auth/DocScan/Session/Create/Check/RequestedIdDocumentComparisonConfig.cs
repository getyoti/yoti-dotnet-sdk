namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedIdDocumentComparisonConfig
        : RequestedCheckConfig
    {
        public RequestedIdDocumentComparisonConfig(int? handledCheckLimit = null)
        {
            HandledCheckLimit = handledCheckLimit;
        }
    }
}