using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class WantedAttribute
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "derivation")]
        public string Derivation { get; private set; }

#pragma warning disable 0414 //"Value never used" warning: the JsonProperty is used when creating the DynamicPolicy JSON

        [JsonRequired]
        [JsonProperty(PropertyName = "optional")]
        public bool Optional { get; private set; }

#pragma warning restore 0414

        [JsonProperty(PropertyName = "constraints")]
        public List<Constraint> Constraints { get; private set; }

        public WantedAttribute(string name, string derivation, List<Constraint> constraints)
        {
            Name = name;
            Derivation = derivation;
            Optional = false;
            Constraints = constraints;
        }
    }
}