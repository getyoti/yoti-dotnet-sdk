using System;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// Represents a Share Code resource for a given session
    /// </summary>
    public class ShareCodeResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; internal set; }

        [JsonProperty(PropertyName = "last_updated")]
        public string LastUpdated { get; internal set; }

        [JsonProperty(PropertyName = "lookup_profile")]
        public MediaContainerResponse LookupProfile { get; internal set; }

        [JsonProperty(PropertyName = "returned_profile")]
        public MediaContainerResponse ReturnedProfile { get; internal set; }

        [JsonProperty(PropertyName = "id_photo")]
        public MediaContainerResponse IdPhoto { get; internal set; }

        [JsonProperty(PropertyName = "file")]
        public MediaContainerResponse File { get; internal set; }
    }
}
