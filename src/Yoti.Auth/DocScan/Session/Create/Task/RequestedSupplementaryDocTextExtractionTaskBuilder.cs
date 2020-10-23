namespace Yoti.Auth.DocScan.Session.Create.Task
{
    public class RequestedSupplementaryDocTextExtractionTaskBuilder
    {
        private string _manualCheck;

        /// <summary>
        /// Requires that a manual follow-up check is always performed
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedSupplementaryDocTextExtractionTaskBuilder WithManualCheckAlways()
        {
            _manualCheck = Constants.DocScanConstants.Always;
            return this;
        }

        /// <summary>
        /// Requires that a manual follow-up check is performed only on failed Checks, and those with a low level of confidence
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedSupplementaryDocTextExtractionTaskBuilder WithManualCheckFallback()
        {
            _manualCheck = Constants.DocScanConstants.Fallback;
            return this;
        }

        /// <summary>
        /// Requires that only an automated Check is performed.  No manual follow-up Check will ever be initiated
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedSupplementaryDocTextExtractionTaskBuilder WithManualCheckNever()
        {
            _manualCheck = Constants.DocScanConstants.Never;
            return this;
        }

        public RequestedSupplementaryDocTextExtractionTask Build()
        {
            return new RequestedSupplementaryDocTextExtractionTask(new RequestedSupplementaryDocTextExtractionTaskConfig(_manualCheck));
        }
    }
}