using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedFaceMatchCheckBuilder
    {
        private string _manualCheck;

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

        public RequestedFaceMatchCheck Build()
        {
            Validation.NotNullOrEmpty(_manualCheck, nameof(_manualCheck));
            RequestedFaceMatchConfig config = new RequestedFaceMatchConfig(_manualCheck);

            return new RequestedFaceMatchCheck(config);
        }
    }
}