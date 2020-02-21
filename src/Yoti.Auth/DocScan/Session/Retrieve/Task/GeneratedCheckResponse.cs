using JsonSubTypes;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Task
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(GeneratedTextDataCheckResponse), Constants.DocScanConstants.IdDocumentTextDataCheck)]
    public class GeneratedCheckResponse
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; internal set; }

        [JsonProperty(PropertyName = "type")]
        public string Type { get; internal set; }
    }
}