using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    /// <summary>
    /// Requests creation of a FaceMatch Check
    /// </summary>
    public class RequestedFaceMatchCheck : RequestedCheck<RequestedFaceMatchConfig>
    {
        public RequestedFaceMatchCheck(RequestedFaceMatchConfig config)
        {
            Config = config;
        }

        public override RequestedFaceMatchConfig Config { get; }

        public override string Type => DocScanConstants.IdDocumentFaceMatch;
    }
}