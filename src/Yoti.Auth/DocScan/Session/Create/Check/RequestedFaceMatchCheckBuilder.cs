using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceMatchCheckBuilder
    {
        private string _manualCheck;

        public RequestedFaceMatchCheckBuilder WithManualCheckAlways()
        {
            _manualCheck = DocScanConstants.Always;
            return this;
        }

        public RequestedFaceMatchCheckBuilder WithManualCheckFallback()
        {
            _manualCheck = DocScanConstants.Fallback;
            return this;
        }

        public RequestedFaceMatchCheckBuilder WithManualCheckNever()
        {
            _manualCheck = DocScanConstants.Never;
            return this;
        }

        public RequestedFaceMatchCheck Build()
        {
            Validation.NotNullOrEmpty(_manualCheck, nameof(_manualCheck));
            RequestedFaceMatchConfig config = new RequestedFaceMatchConfig(_manualCheck);

            return new RequestedFaceMatchCheck(config);
        }
    }
}