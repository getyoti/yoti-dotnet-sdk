namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedIdDocumentComparisonCheckBuilder
    {
        public RequestedIdDocumentComparisonCheck Build()
        {
            return new RequestedIdDocumentComparisonCheck(new RequestedIdDocumentComparisonConfig());
        }
    }
}