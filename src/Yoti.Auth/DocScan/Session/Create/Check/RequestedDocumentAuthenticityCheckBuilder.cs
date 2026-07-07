using Yoti.Auth.DocScan.Session.Create.Filter;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedDocumentAuthenticityCheckBuilder
    {
        private string _manualCheck;
        private IssuingAuthoritySubCheck _issuingAuthoritySubCheck;
        private int? _handledCheckLimit;

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

        /// <summary>
        /// Adds an Issuing Authority Sub Check
        /// </summary>
        /// <returns>The <see cref="RequestedDocumentAuthenticityCheckBuilder"/></returns>
        public RequestedDocumentAuthenticityCheckBuilder WithIssuingAuthoritySubCheck()
        {
            _issuingAuthoritySubCheck = new IssuingAuthoritySubCheckBuilder()
                    .WithRequested(true)
                    .Build();
            return this;
        }

        /// <summary>
        /// Adds an Issuing Authority Sub Check with a <see cref="DocumentFilter">documentFilter</see> (used to determine which documents the sub check is performed on).
        /// </summary>
        /// <param name="documentFilter">The <see cref="DocumentFilter"/> used to determine which documents the sub check is performed on</param>
        /// <returns>The <see cref="RequestedDocumentAuthenticityCheckBuilder"/></returns>
        public RequestedDocumentAuthenticityCheckBuilder WithIssuingAuthoritySubCheck(DocumentFilter documentFilter)
        {
            _issuingAuthoritySubCheck = new IssuingAuthoritySubCheckBuilder()
                    .WithRequested(true)
                    .WithFilter(documentFilter)
                    .Build();
            return this;
        }

        /// <summary>
        /// Sets the number of times a check response should return a handled result before switching to success (for sandbox testing)
        /// </summary>
        /// <param name="handledCheckLimit">The number of handled check responses before success</param>
        /// <returns>The <see cref="RequestedDocumentAuthenticityCheckBuilder"/></returns>
        public RequestedDocumentAuthenticityCheckBuilder WithHandledCheckLimit(int handledCheckLimit)
        {
            Validation.NotLessThan(handledCheckLimit, 0, nameof(handledCheckLimit));
            _handledCheckLimit = handledCheckLimit;
            return this;
        }

        public RequestedDocumentAuthenticityCheck Build()
        {
            var config = new RequestedDocumentAuthenticityConfig(_manualCheck, _issuingAuthoritySubCheck, _handledCheckLimit);

            return new RequestedDocumentAuthenticityCheck(config);
        }
    }
}