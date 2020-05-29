namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedDocumentAuthenticityCheckBuilder
    {
        public RequestedDocumentAuthenticityCheck Build()
        {
            return new RequestedDocumentAuthenticityCheck(new RequestedDocumentAuthenticityConfig());
        }
    }
}