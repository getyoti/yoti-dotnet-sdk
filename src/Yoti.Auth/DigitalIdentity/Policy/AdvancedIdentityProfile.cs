using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class AdvancedIdentityProfile
    {
        [JsonProperty(PropertyName = "profiles")]
        public List<Profile> Profiles { get; set; }
    }

    public class Profile
    {
        [JsonProperty(PropertyName = "trust_framework")]
        public string TrustFramework { get; set; }
        [JsonProperty(PropertyName = "schemes")]
        public List<Scheme> Schemes { get; set; }
    }

    public class Scheme
    {
        [JsonProperty(PropertyName = "label")]
        public string Label { get; set; }
        [JsonProperty(PropertyName = "objective")]
        public string Objective { get; set; }
        [JsonProperty(PropertyName = "type")]
        public string Type { get; set; }
    }
    
}
