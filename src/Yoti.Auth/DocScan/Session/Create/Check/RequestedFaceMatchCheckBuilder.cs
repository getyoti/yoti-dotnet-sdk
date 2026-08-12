using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceMatchCheckBuilder
    {
        private string _manualCheck;
        private int? _handledCheckLimit;

        /// <summary>
        /// Requires that a manual follow-up check is always performed
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedFaceMatchCheckBuilder WithManualCheckAlways()
        {
            _manualCheck = DocScanConstants.Always;
            return this;
        }

        /// <summary>
        /// Requires that a manual follow-up check is performed only on failed Checks, and those with a low level of confidence
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedFaceMatchCheckBuilder WithManualCheckFallback()
        {
            _manualCheck = DocScanConstants.Fallback;
            return this;
        }

        /// <summary>
        /// Requires that only an automated Check is performed.  No manual follow-up Check will ever be initiated
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedFaceMatchCheckBuilder WithManualCheckNever()
        {
            _manualCheck = DocScanConstants.Never;
            return this;
        }

        /// <summary>
        /// Sets the number of times a check response should return a handled result before switching to success (for sandbox testing)
        /// </summary>
        /// <param name="handledCheckLimit">The number of handled check responses before success</param>
        /// <returns>The <see cref="RequestedFaceMatchCheckBuilder"/></returns>
        public RequestedFaceMatchCheckBuilder WithHandledCheckLimit(int handledCheckLimit)
        {
            Validation.NotLessThan(handledCheckLimit, 0, nameof(handledCheckLimit));
            _handledCheckLimit = handledCheckLimit;
            return this;
        }

        public RequestedFaceMatchCheck Build()
        {
            Validation.NotNullOrEmpty(_manualCheck, nameof(_manualCheck));
            RequestedFaceMatchConfig config = new RequestedFaceMatchConfig(_manualCheck, _handledCheckLimit);

            return new RequestedFaceMatchCheck(config);
        }
    }
}