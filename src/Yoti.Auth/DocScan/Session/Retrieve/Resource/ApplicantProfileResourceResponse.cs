using System;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    public class ApplicantProfileResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; internal set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; internal set; }

        [JsonProperty(PropertyName = "last_updated")]
        public DateTime? LastUpdated { get; internal set; }
    }
}
