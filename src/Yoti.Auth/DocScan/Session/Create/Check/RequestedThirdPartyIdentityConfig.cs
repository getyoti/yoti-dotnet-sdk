namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedThirdPartyIdentityCheckConfig : RequestedCheckConfig
    {
        public RequestedThirdPartyIdentityCheckConfig()
            : this(null)
        {
        }

        public RequestedThirdPartyIdentityCheckConfig(int? handledCheckLimit = null)
        {
            HandledCheckLimit = handledCheckLimit;
        }
    }
}