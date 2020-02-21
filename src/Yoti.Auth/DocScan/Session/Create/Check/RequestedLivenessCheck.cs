using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedLivenessCheck : RequestedCheck<RequestedLivenessConfig>
    {
        public RequestedLivenessCheck(RequestedLivenessConfig config)
        {
            Config = config;
        }

        public override RequestedLivenessConfig Config { get; }

        public override string Type => DocScanConstants.Liveness;
    }
}