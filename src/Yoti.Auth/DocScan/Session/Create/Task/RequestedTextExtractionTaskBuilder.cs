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

        /// <summary>
        /// Requires that a manual follow-up check is always performed
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedTextExtractionTaskBuilder WithManualCheckAlways()
        {
            _manualCheck = DocScanConstants.Always;
            return this;
        }

        /// <summary>
        /// Requires that a manual follow-up check is performed only on failed Checks, and those with a low level of confidence
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedTextExtractionTaskBuilder WithManualCheckFallback()
        {
            _manualCheck = DocScanConstants.Fallback;
            return this;
        }

        /// <summary>
        /// Requires that only an automated Check is performed.  No manual follow-up Check will ever be initiated
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedTextExtractionTaskBuilder WithManualCheckNever()
        {
            _manualCheck = DocScanConstants.Never;
            return this;
        }

        /// <summary>
        /// Sets the value of chip data to "DESIRED"
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedTextExtractionTaskBuilder WithChipDataDesired()
        {
            _chipData = DocScanConstants.Desired;
            return this;
        }

        /// <summary>
        /// Sets the value of chip data to "IGNORE"
        /// </summary>
        /// <returns>The builder</returns>
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