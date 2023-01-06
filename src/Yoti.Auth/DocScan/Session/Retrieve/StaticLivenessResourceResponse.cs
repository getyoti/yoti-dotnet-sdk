using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class StaticLivenessResourceResponse : LivenessResourceResponse
    {
        [JsonProperty(PropertyName = "image")]
        public StaticLivenessImageResponse image { get; internal set; }
    }
}