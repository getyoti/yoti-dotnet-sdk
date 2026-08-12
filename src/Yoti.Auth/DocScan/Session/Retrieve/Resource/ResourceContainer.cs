using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Check;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    public class ResourceContainer
    {
        [JsonProperty(PropertyName = "id_documents")]
        public List<IdDocumentResourceResponse> IdDocuments { get; internal set; }

        [JsonProperty(PropertyName = "supplementary_documents")]
        public List<SupplementaryDocResourceResponse> SupplementaryDocuments { get; internal set; } = new List<SupplementaryDocResourceResponse>();

        [JsonProperty(PropertyName = "liveness_capture")]
        public List<LivenessResourceResponse> LivenessCapture { get; internal set; }

        [JsonProperty(PropertyName = "face_capture")]
        public List<FaceCaptureResourceResponse> FaceCapture { get; internal set; }

        [JsonProperty(PropertyName = "share_codes")]
        public List<ShareCodeResourceResponse> ShareCodes { get; internal set; }

        [JsonProperty(PropertyName = "applicant_profiles")]
        public List<ApplicantProfileResourceResponse> ApplicantProfiles { get; internal set; }

        public List<ZoomLivenessResourceResponse> ZoomLivenessResources
        {
            get
            {
                if (LivenessCapture == null)
                    return new List<ZoomLivenessResourceResponse>();

                List<ZoomLivenessResourceResponse> zoomResources = new List<ZoomLivenessResourceResponse>();

                foreach (var resource in LivenessCapture)
                {
                    if (resource is ZoomLivenessResourceResponse zoomLivenessResource)
                    {
                        zoomResources.Add(zoomLivenessResource);
                    }
                }

                return zoomResources;
            }
        }

        public List<StaticLivenessResourceResponse> StaticLivenessResources
        {
            get
            {
                if (LivenessCapture == null)
                    return new List<StaticLivenessResourceResponse>();

                List<StaticLivenessResourceResponse> staticResources = new List<StaticLivenessResourceResponse>();

                foreach (var resource in LivenessCapture)
                {
                    if (resource is StaticLivenessResourceResponse staticLivenessResource)
                    {
                        staticResources.Add(staticLivenessResource);
                    }
                }

                return staticResources;
            }
        }

        /// <summary>
        /// Returns a new <see cref="ResourceContainer"/> containing only the resources referenced by the given check's ResourcesUsed
        /// </summary>
        /// <param name="checkResponse">The check to filter resources for</param>
        /// <returns>A ResourceContainer containing only the resources used by the check</returns>
        internal ResourceContainer FilterForCheck(CheckResponse checkResponse)
        {
            if (checkResponse == null)
                throw new ArgumentNullException(nameof(checkResponse));

            HashSet<string> resourceIds = new HashSet<string>(checkResponse.ResourcesUsed ?? new List<string>());

            return new ResourceContainer
            {
                IdDocuments = FilterResources(IdDocuments, resourceIds),
                SupplementaryDocuments = FilterResources(SupplementaryDocuments, resourceIds),
                LivenessCapture = FilterResources(LivenessCapture, resourceIds),
                FaceCapture = FilterResources(FaceCapture, resourceIds),
                ShareCodes = FilterResources(ShareCodes, resourceIds),
                ApplicantProfiles = FilterResources(ApplicantProfiles, resourceIds)
            };
        }

        private static List<T> FilterResources<T>(List<T> resources, HashSet<string> resourceIds) where T : ResourceResponse
        {
            if (resources == null)
                return new List<T>();

            return resources.Where(resource => resourceIds.Contains(resource.Id)).ToList();
        }
    }
}