using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedLivenessCheckBuilder
    {
        private string _livenessType;
        private int _maxRetries = 1;
        private string _manualCheck = "NEVER";
        private int? _handledCheckLimit;

        /// <summary>
        /// ForZoomLiveness sets the liveness type to "ZOOM"
        /// </summary>
        /// <returns></returns>
        public RequestedLivenessCheckBuilder ForZoomLiveness()
        {
            return ForLivenessType(DocScanConstants.Zoom);
        }

        /// <summary>
        /// ForStaticLiveness sets the liveness type to "STATIC"
        /// </summary>
        /// <returns></returns>
        public RequestedLivenessCheckBuilder ForStaticLiveness()
        {
            return ForLivenessType(DocScanConstants.Static);
        }

        public RequestedLivenessCheckBuilder WithManualCheck(string manualCheck)
        {
            _manualCheck = manualCheck;
            return this;
        }

        public RequestedLivenessCheckBuilder ForLivenessType(string livenessType)
        {
            _livenessType = livenessType;
            return this;
        }

        public RequestedLivenessCheckBuilder WithMaxRetries(int maxRetries)
        {
            _maxRetries = maxRetries;
            return this;
        }

        /// <summary>
        /// Sets the number of times a check response should return a handled result before switching to success (for sandbox testing)
        /// </summary>
        /// <param name="handledCheckLimit">The number of handled check responses before success</param>
        /// <returns>The <see cref="RequestedLivenessCheckBuilder"/></returns>
        public RequestedLivenessCheckBuilder WithHandledCheckLimit(int handledCheckLimit)
        {
            Validation.NotLessThan(handledCheckLimit, 0, nameof(handledCheckLimit));
            _handledCheckLimit = handledCheckLimit;
            return this;
        }

        public RequestedLivenessCheck Build()
        {
            Validation.NotNullOrEmpty(_livenessType, nameof(_livenessType));
            RequestedLivenessConfig config = new RequestedLivenessConfig(_maxRetries, _livenessType, _manualCheck, _handledCheckLimit);

            return new RequestedLivenessCheck(config);
        }
    }
}