using JsonSubTypes;
using Newtonsoft.Json;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Task
{
    [JsonConverter(typeof(JsonSubtypes), "type")]
    [JsonSubtypes.KnownSubType(typeof(RequestedIdDocTaskResponse), DocScanConstants.IdDocumentTextDataExtraction)]
    [JsonSubtypes.KnownSubType(typeof(RequestedSupplementaryDocTaskResponse), DocScanConstants.SupplementaryDocumentTextDataExtraction)]
    [JsonSubtypes.FallBackSubType(typeof(UnknownRequestedTaskResponse))]
    public abstract class RequestedTaskResponse
    {
        /// <summary>
        /// The type of the <see cref="RequestedTaskResponse"/>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }

        /// <summary>
        /// The current state of the <see cref="RequestedTaskResponse"/>
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string State { get; private set; }
    }
}