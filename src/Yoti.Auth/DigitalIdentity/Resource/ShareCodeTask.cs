using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve;

namespace Yoti.Auth.DigitalIdentity.Resource
{
    /// <summary>
    /// Represents a task associated with a share code resource
    /// </summary>
    public class ShareCodeTask
    {
        /// <summary>
        /// The type of the task
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }

        /// <summary>
        /// The unique identifier of the task
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        /// <summary>
        /// The state of the task
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string State { get; internal set; }

        /// <summary>
        /// The creation timestamp
        /// </summary>
        [JsonProperty(PropertyName = "created")]
        public string Created { get; internal set; }

        /// <summary>
        /// The last update timestamp
        /// </summary>
        [JsonProperty(PropertyName = "last_updated")]
        public string LastUpdated { get; internal set; }

        /// <summary>
        /// List of generated media associated with this task
        /// </summary>
        [JsonProperty(PropertyName = "generated_media")]
        public List<GeneratedMedia> GeneratedMedia { get; internal set; }
    }
}
