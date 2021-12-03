using JsonSubTypes;
using Newtonsoft.Json;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Liveness
{
    [JsonConverter(typeof(JsonSubtypes), "liveness_type")]
    [JsonSubtypes.KnownSubType(typeof(RequiredZoomLivenessResourceResponse), DocScanConstants.Zoom)]
    [JsonSubtypes.FallBackSubType(typeof(UnknownRequiredLivenessResourceResponse))]
    public abstract class RequiredLivenessResourceResponse : RequiredResourceResponse
    {
        [JsonProperty(PropertyName = "liveness_type")]
        public string LivenessType { get; private set; }
    }
}