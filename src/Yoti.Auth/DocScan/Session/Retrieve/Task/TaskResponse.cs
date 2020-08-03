using System.Collections.Generic;
using System.Linq;
using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Task
{
    /// <summary>
    /// TaskResponse represents the attributes of a task, for any given session
    /// </summary>
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(TextExtractionTaskResponse), Constants.DocScanConstants.IdDocumentTextDataExtraction)]
    public class TaskResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }

        [JsonProperty(PropertyName = "state")]
        public string State { get; internal set; }

        [JsonProperty(PropertyName = "created")]
        public string Created { get; internal set; }

        [JsonProperty(PropertyName = "last_updated")]
        public string LastUpdated { get; internal set; }

        [JsonProperty(PropertyName = "generated_checks")]
        public List<GeneratedCheckResponse> GeneratedChecks { get; internal set; }

        [JsonProperty(PropertyName = "generated_media")]
        public List<GeneratedMedia> GeneratedMedia { get; internal set; }

        public List<GeneratedTextDataCheckResponse> GetGeneratedTextDataChecks()
        {
            return GeneratedChecks?.OfType<GeneratedTextDataCheckResponse>()?.ToList();
        }
    }
}