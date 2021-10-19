namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedThirdPartyIdentityCheckBuilder
    {
        public RequestedThirdPartyIdentityCheck Build()
        {
            RequestedThirdPartyIdentityConfig config = new RequestedThirdPartyIdentityConfig();

            return new RequestedThirdPartyIdentityCheck(config);
        }
    }
}