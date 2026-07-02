namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedThirdPartyIdentityCheckConfig : RequestedCheckConfig
    {
        public RequestedThirdPartyIdentityCheckConfig(int? handledCheckLimit = null)
        {
            HandledCheckLimit = handledCheckLimit;
        }
    }
}