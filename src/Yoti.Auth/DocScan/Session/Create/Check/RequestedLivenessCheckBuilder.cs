using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedLivenessCheckBuilder
    {
        private string _livenessType;
        private int _maxRetries = 1;
        private string _manualCheck = "NEVER";

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

        public RequestedLivenessCheck Build()
        {
            Validation.NotNullOrEmpty(_livenessType, nameof(_livenessType));
            RequestedLivenessConfig config = new RequestedLivenessConfig(_maxRetries, _livenessType, _manualCheck);

            return new RequestedLivenessCheck(config);
        }
    }
}