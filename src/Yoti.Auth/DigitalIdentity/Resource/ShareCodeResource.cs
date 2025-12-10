using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;

namespace Yoti.Auth.DigitalIdentity.Resource
{
    /// <summary>
    /// Represents a share code resource in the session response
    /// </summary>
    public class ShareCodeResource
    {
        /// <summary>
        /// The unique identifier of the share code resource
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        /// <summary>
        /// The source of the share code
        /// </summary>
        [JsonProperty(PropertyName = "source")]
        public string Source { get; internal set; }

        /// <summary>
        /// The creation timestamp
        /// </summary>
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; internal set; }

        /// <summary>
        /// The last update timestamp
        /// </summary>
        [JsonProperty(PropertyName = "last_updated")]
        public string LastUpdated { get; internal set; }

        /// <summary>
        /// The lookup profile media
        /// </summary>
        [JsonProperty(PropertyName = "lookup_profile")]
        public MediaResponse LookupProfile { get; internal set; }

        /// <summary>
        /// The returned profile media
        /// </summary>
        [JsonProperty(PropertyName = "returned_profile")]
        public MediaResponse ReturnedProfile { get; internal set; }

        /// <summary>
        /// The ID photo media
        /// </summary>
        [JsonProperty(PropertyName = "id_photo")]
        public MediaResponse IdPhoto { get; internal set; }

        /// <summary>
        /// The file media
        /// </summary>
        [JsonProperty(PropertyName = "file")]
        public MediaResponse File { get; internal set; }

        /// <summary>
        /// List of tasks associated with this share code resource
        /// </summary>
        [JsonProperty(PropertyName = "tasks")]
        public List<ShareCodeTask> Tasks { get; internal set; }
    }
}
