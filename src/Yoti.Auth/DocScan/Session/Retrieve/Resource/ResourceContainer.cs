using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    public class ResourceContainer
    {
        [JsonProperty(PropertyName = "id_documents")]
        public List<IdDocumentResourceResponse> IdDocuments { get; internal set; }

        [JsonProperty(PropertyName = "supplementary_documents")]
        public List<SupplementaryDocResourceResponse> SupplementaryDocuments { get; internal set; }

        [JsonProperty(PropertyName = "liveness_capture")]
        public List<LivenessResourceResponse> LivenessCapture { get; internal set; }

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
    }
}