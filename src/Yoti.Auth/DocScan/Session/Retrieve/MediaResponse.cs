using System;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// MediaResponse represents a media resource
    /// </summary>
    public class MediaResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; private set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; private set; }

        [JsonProperty(PropertyName = "last_updated")]
        public DateTime LastUpdated { get; private set; }
    }
}