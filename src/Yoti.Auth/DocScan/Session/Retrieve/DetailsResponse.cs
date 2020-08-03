using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    /// <summary>
    /// DetailsResponse represents a specific detail for a breakdown
    /// </summary>
    public class DetailsResponse
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; private set; }
    }
}