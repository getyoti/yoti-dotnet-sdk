namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedThirdPartyIdentityCheckBuilder
    {
        public RequestedThirdPartyIdentityCheck Build()
        {
            RequestedThirdPartyIdentityCheckConfig config = new RequestedThirdPartyIdentityCheckConfig();

            return new RequestedThirdPartyIdentityCheck(config);
        }
    }
}