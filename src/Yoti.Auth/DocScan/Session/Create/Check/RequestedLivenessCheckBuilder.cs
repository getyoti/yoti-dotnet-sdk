using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class RequestedLivenessCheckBuilder
    {
        private string _livenessType;
        private int _maxRetries = 1;

        /// <summary>
        /// ForZoomLiveness sets the liveness type to "ZOOM"
        /// </summary>
        /// <returns></returns>
        public RequestedLivenessCheckBuilder ForZoomLiveness()
        {
            return ForLivenessType(DocScanConstants.Zoom);
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
            RequestedLivenessConfig config = new RequestedLivenessConfig(_maxRetries, _livenessType);

            return new RequestedLivenessCheck(config);
        }
    }
}