using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// Requests creation of a FaceComparison Check
    /// </summary>
    public class RequestedFaceComparisonCheck : RequestedCheck<RequestedFaceComparisonConfig>
    {
        public RequestedFaceComparisonCheck(RequestedFaceComparisonConfig config)
        {
            Config = config;
        }

        public override RequestedFaceComparisonConfig Config { get; }

        public override string Type => DocScanConstants.FaceComparison;
    }
}