using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceComparisonCheckBuilder
    {
        private string _manualCheck;
        private int? _handledCheckLimit;
            
        /// <summary>
        /// Requires that only an automated Check is performed.  No manual follow-up Check will ever be initiated
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedFaceComparisonCheckBuilder WithManualCheckNever()
        {
            _manualCheck = DocScanConstants.Never;
            return this;
        }

        /// <summary>
        /// Sets the number of times a check response should return a handled result before switching to success (for sandbox testing)
        /// </summary>
        /// <param name="handledCheckLimit">The number of handled check responses before success</param>
        /// <returns>The <see cref="RequestedFaceComparisonCheckBuilder"/></returns>
        public RequestedFaceComparisonCheckBuilder WithHandledCheckLimit(int handledCheckLimit)
        {
            _handledCheckLimit = handledCheckLimit;
            return this;
        }

        public RequestedFaceComparisonCheck Build()
        {
            Validation.NotNullOrEmpty(_manualCheck, nameof(_manualCheck));
            RequestedFaceComparisonConfig config = new RequestedFaceComparisonConfig(_manualCheck, _handledCheckLimit);

            return new RequestedFaceComparisonCheck(config);
        }
    }
}