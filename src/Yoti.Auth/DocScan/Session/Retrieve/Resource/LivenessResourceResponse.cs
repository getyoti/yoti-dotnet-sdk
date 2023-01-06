using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    [JsonConverter(typeof(JsonSubtypes), "liveness_type")]
    [JsonSubtypes.KnownSubType(typeof(ZoomLivenessResourceResponse), Constants.DocScanConstants.Zoom)]
    [JsonSubtypes.KnownSubType(typeof(StaticLivenessResourceResponse), Constants.DocScanConstants.Static)]
    public class LivenessResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "liveness_type")]
        public string LivenessType { get; internal set; }
    }
}   