using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.FaceCapture;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Liveness;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture
{
    public class CaptureResponse
    {
        /// <summary>
        /// The state of biometric consent: determines if biometric consent needs to be captured
        /// </summary>
        [JsonProperty(PropertyName = "biometric_consent")]
        public string BiometricConsent { get; private set; }

        /// <summary>
        /// Required resources to satisfy the sessions requirements
        /// </summary>
        [JsonProperty(PropertyName = "required_resources")]
        public List<RequiredResourceResponse> RequiredResources { get; private set; }

        /// <summary>
        /// Document resource requirements (including Id and Supplementary Documents)
        /// </summary>
        public List<RequiredDocumentResourceResponse> GetDocumentResourceRequirements()
        {
            return FilterRequiredResources<RequiredDocumentResourceResponse>();
        }

        /// <summary>
        /// Id Document resource requirements
        /// </summary>
        public List<RequiredIdDocumentResourceResponse> GetIdDocumentResourceRequirements()
        {
            return FilterRequiredResources<RequiredIdDocumentResourceResponse>();
        }

        /// <summary>
        /// Supplementary Document resource requirements
        /// </summary>
        public List<RequiredSupplementaryDocumentResourceResponse> GetSupplementaryResourceRequirements()
        {
            return FilterRequiredResources<RequiredSupplementaryDocumentResourceResponse>();
        }

        /// <summary>
        /// Liveness resource requirements
        /// </summary>
        public List<RequiredLivenessResourceResponse> GetLivenessResourceRequirements()
        {
            return FilterRequiredResources<RequiredLivenessResourceResponse>();
        }

        /// <summary>
        /// Zoom Liveness resource requirements
        /// </summary>
        public List<RequiredZoomLivenessResourceResponse> GetZoomLivenessResourceRequirements()
        {
            return FilterRequiredResources<RequiredZoomLivenessResourceResponse>();
        }

        /// <summary>
        /// Face Capture resource requirements
        /// </summary>
        public List<RequiredFaceCaptureResourceResponse> GetFaceCaptureResourceRequirements()
        {
            return FilterRequiredResources<RequiredFaceCaptureResourceResponse>();
        }

        private List<TResourceType> FilterRequiredResources<TResourceType>()
        {
            if (RequiredResources == null)
                return new List<TResourceType>();

            return RequiredResources.OfType<TResourceType>().ToList();
        }
    }
}