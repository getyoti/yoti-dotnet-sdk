using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class ZoomLivenessResourceResponse : LivenessResourceResponse
    {
        [JsonProperty(PropertyName = "facemap")]
        public FaceMapResponse FaceMap { get; internal set; }

        [JsonProperty(PropertyName = "frames")]
        public List<FrameResponse> Frames { get; private set; }
    }
}