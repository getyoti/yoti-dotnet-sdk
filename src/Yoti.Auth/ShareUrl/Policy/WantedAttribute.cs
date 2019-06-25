using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class WantedAttribute
    {
        [JsonProperty(PropertyName = "name")]
        private readonly string _name;

        [JsonProperty(PropertyName = "derivation")]
        private readonly string _derivation;

#pragma warning disable 0414 //"Value never used" warning: the JsonProperty is used when creating the DynamicPolicy JSON

        [JsonProperty(PropertyName = "optional")]
        private readonly bool _optional;

#pragma warning restore 0414

        public WantedAttribute(string name, string derivation)
        {
            _name = name;
            _derivation = derivation;
            _optional = false;
        }

        [JsonIgnore]
        public string Name
        {
            get
            {
                return _name;
            }
        }

        [JsonIgnore]
        public string Derivation
        {
            get
            {
                return _derivation;
            }
        }
    }
}