﻿using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class WantedAttribute
    {
        [JsonRequired]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; private set; }

        [JsonProperty(PropertyName = "derivation")]
        public string Derivation { get; private set; }

        [JsonRequired]
        [JsonProperty(PropertyName = "optional")]
        public bool Optional { get; private set; }

        [JsonProperty(PropertyName = "accept_self_asserted")]
        public bool? AcceptSelfAsserted { get; private set; }

        [JsonProperty(PropertyName = "constraints")]
        public List<Constraint> Constraints { get; private set; }

        public WantedAttribute(string name, string derivation, List<Constraint> constraints, bool? acceptSelfAsserted = null, bool optional = false)
        {
            Name = name;
            Derivation = derivation;
            Optional = optional;
            AcceptSelfAsserted = acceptSelfAsserted;
            Constraints = constraints;
        }
    }
}
