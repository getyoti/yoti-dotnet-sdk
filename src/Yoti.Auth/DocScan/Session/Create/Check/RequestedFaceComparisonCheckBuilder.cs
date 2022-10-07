using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceComparisonCheckBuilder
    {
        private string _manualCheck;
            
        /// <summary>
        /// Requires that only an automated Check is performed.  No manual follow-up Check will ever be initiated
        /// </summary>
        /// <returns>The builder</returns>
        public RequestedFaceComparisonCheckBuilder WithManualCheckNever()
        {
            _manualCheck = DocScanConstants.Never;
            return this;
        }

        public RequestedFaceComparisonCheck Build()
        {
            Validation.NotNullOrEmpty(_manualCheck, nameof(_manualCheck));
            RequestedFaceComparisonConfig config = new RequestedFaceComparisonConfig(_manualCheck);

            return new RequestedFaceComparisonCheck(config);
        }
    }
}