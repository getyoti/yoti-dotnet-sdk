using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture.Document
{
    public class ObjectiveResponse
    {
        /// <summary>
        /// The type of the <see cref="ObjectiveResponse"/>
        /// </summary>
        [JsonProperty(PropertyName = "type")]
        public string Type { get; private set; }
    }
}