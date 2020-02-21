using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedTextExtractionTaskBuilder
    {
        private string _manualCheck;

        public RequestedTextExtractionTaskBuilder WithManualCheckAlways()
        {
            _manualCheck = DocScanConstants.Always;
            return this;
        }

        public RequestedTextExtractionTaskBuilder WithManualCheckFallback()
        {
            _manualCheck = DocScanConstants.Fallback;
            return this;
        }

        public RequestedTextExtractionTaskBuilder WithManualCheckNever()
        {
            _manualCheck = DocScanConstants.Never;
            return this;
        }

        public RequestedTextExtractionTask Build()
        {
            Validation.NotNullOrEmpty(_manualCheck, nameof(_manualCheck));

            RequestedTextExtractionTaskConfig config = new RequestedTextExtractionTaskConfig(_manualCheck);

            return new RequestedTextExtractionTask(config);
        }
    }
}