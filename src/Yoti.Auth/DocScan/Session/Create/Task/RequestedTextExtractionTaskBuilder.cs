using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Task
{
    /// <summary>
    /// Builds a <see cref="RequestedTextExtractionTask"/>
    /// </summary>
    public class RequestedTextExtractionTaskBuilder
    {
        private string _manualCheck;
        private string _chipData;

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

        public RequestedTextExtractionTaskBuilder WithChipDataDesired()
        {
            _chipData = DocScanConstants.Desired;
            return this;
        }

        public RequestedTextExtractionTaskBuilder WithChipDataIgnore()
        {
            _chipData = DocScanConstants.Ignore;
            return this;
        }

        public RequestedTextExtractionTask Build()
        {
            Validation.NotNullOrEmpty(_manualCheck, nameof(_manualCheck));

            RequestedTextExtractionTaskConfig config = new RequestedTextExtractionTaskConfig(_manualCheck, _chipData);

            return new RequestedTextExtractionTask(config);
        }
    }
}