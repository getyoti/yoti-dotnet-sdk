using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.Sandbox.Profile.Request.Attribute
{
    public class SandboxAttribute
    {
        internal SandboxAttribute(string name,
                string value,
                string derivation,
                bool? optional,
                List<SandboxAnchor> anchors)
        {
            Name = name;
            Value = value;
            Derivation = derivation;
            Optional = optional.ToString();
            Anchors = anchors ?? new List<SandboxAnchor>();
        }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; }

        [JsonProperty(PropertyName = "value")]
        public string Value { get; }

        [JsonProperty(PropertyName = "derivation")]
        public string Derivation { get; }

        [JsonProperty(PropertyName = "optional")]
        public string Optional { get; }

        [JsonProperty(PropertyName = "anchors")]
        public List<SandboxAnchor> Anchors { get; }

        public static SandboxAttributeBuilder Builder()
        {
            return new SandboxAttributeBuilder();
        }
    }
}