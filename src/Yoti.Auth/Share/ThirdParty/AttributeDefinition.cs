using Newtonsoft.Json;

namespace Yoti.Auth.Share.ThirdParty
{
    public class AttributeDefinition
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        public AttributeDefinition(string name)
        {
            Name = name;
        }
    }
}