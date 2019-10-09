using System.Collections.Generic;

namespace Yoti.Auth.Sandbox.Profile.Request.Attribute
{
    public class SandboxAttributeBuilder
    {
        private string _name;
        private string _value;
        private string _derivation;
        private bool _optional;
        private List<SandboxAnchor> _anchors;

        internal SandboxAttributeBuilder()
        {
        }

        public SandboxAttributeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public SandboxAttributeBuilder WithValue(string value)
        {
            _value = value;
            return this;
        }

        public SandboxAttributeBuilder WithDerivation(string derivation)
        {
            _derivation = derivation;
            return this;
        }

        public SandboxAttributeBuilder WithOptional(bool optional)
        {
            _optional = optional;
            return this;
        }

        public SandboxAttributeBuilder WithAnchors(List<SandboxAnchor> anchors)
        {
            _anchors = anchors;
            return this;
        }

        public SandboxAttribute Build()
        {
            return new SandboxAttribute(_name, _value, _derivation, _optional, _anchors);
        }
    }
}