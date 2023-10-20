using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class WantedAnchor
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "sub_type")]
        public string SubType { get; private set; }

        public WantedAnchor(string name, string subType)
        {
            Name = name;
            SubType = subType;
        }
    }
}