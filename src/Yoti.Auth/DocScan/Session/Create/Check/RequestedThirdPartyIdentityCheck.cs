using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// Requests creation of a Third Party Identity Check
    /// </summary>
    public class RequestedThirdPartyIdentityCheck : RequestedCheck<RequestedThirdPartyIdentityConfig>
    {
        public RequestedThirdPartyIdentityCheck(RequestedThirdPartyIdentityConfig config)
        {
            Config = config;
        }

        public override RequestedThirdPartyIdentityConfig Config { get; }

        public override string Type => DocScanConstants.ThirdPartyIdentity;
    }
}