using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// Requests creation of a FaceMatch Check
    /// </summary>
    public class RequestedThirdPartyIdentityCheck : RequestedCheck<RequestedThirdPartyIdentityCheckConfig>
    {
        public RequestedThirdPartyIdentityCheck(RequestedThirdPartyIdentityCheckConfig config)
        {
            Config = config;
        }

        public override RequestedThirdPartyIdentityCheckConfig Config { get; }

        public override string Type => DocScanConstants.ThirdPartyIdentity;
    }
}