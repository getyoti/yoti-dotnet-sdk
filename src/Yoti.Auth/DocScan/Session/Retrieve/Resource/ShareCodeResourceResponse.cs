using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Task;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
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
        public MediaResponse LookupProfile { get; internal set; }

        [JsonProperty(PropertyName = "returned_profile")]
        public MediaResponse ReturnedProfile { get; internal set; }

        [JsonProperty(PropertyName = "id_photo")]
        public MediaResponse IdPhoto { get; internal set; }

        [JsonProperty(PropertyName = "file")]
        public MediaResponse File { get; internal set; }
    }
}
