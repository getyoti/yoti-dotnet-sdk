using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class WantedAttribute
    {
        [JsonProperty(PropertyName = "name")]
        private readonly string _name;

        [JsonProperty(PropertyName = "derivation")]
        private readonly string _derivation;

        [JsonProperty(PropertyName = "optional")]
        private readonly bool _optional;

        public WantedAttribute(string name, string derivation, bool optional)
        {
            _name = name;
            _derivation = derivation;
            _optional = optional;
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

        [JsonIgnore]
        public bool IsOptional
        {
            get
            {
                return _optional;
            }
        }
    }
}