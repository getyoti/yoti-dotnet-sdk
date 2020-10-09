namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedDocumentAuthenticityCheckBuilder
    {
        private string _manualCheck;

        /// <summary>
        /// Requires that a manual follow-up check is always performed
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedDocumentAuthenticityCheckBuilder WithManualCheckAlways()
        {
            _manualCheck = Constants.DocScanConstants.Always;
            return this;
        }

        /// <summary>
        /// Requires that a manual follow-up check is performed only on failed Checks, and those with a low level of confidence
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedDocumentAuthenticityCheckBuilder WithManualCheckFallback()
        {
            _manualCheck = Constants.DocScanConstants.Fallback;
            return this;
        }

        /// <summary>
        /// Requires that only an automated Check is performed.  No manual follow-up Check will ever be initiated
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedDocumentAuthenticityCheckBuilder WithManualCheckNever()
        {
            _manualCheck = Constants.DocScanConstants.Never;
            return this;
        }

        public RequestedDocumentAuthenticityCheck Build()
        {
            return new RequestedDocumentAuthenticityCheck(new RequestedDocumentAuthenticityConfig(_manualCheck));
        }
    }
}