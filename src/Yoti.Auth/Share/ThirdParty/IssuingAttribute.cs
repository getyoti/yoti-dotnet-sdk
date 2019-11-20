using Newtonsoft.Json;

namespace Yoti.Auth.Share.ThirdParty
{
    public class IssuingAttribute
    {
        private readonly AttributeDefinition _definition;

        [JsonProperty(PropertyName = "value")]
        public string Value { get; private set; }

        [JsonProperty(PropertyName = "name")]
        public string Name => _definition.Name;

        public IssuingAttribute(AttributeDefinition definition, string value)
        {
            _definition = definition;
            Value = value;
        }
    }
}