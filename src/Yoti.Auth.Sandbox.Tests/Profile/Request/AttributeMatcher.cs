using System;
using System.Collections.Generic;
using System.Linq;
using Xunit.Sdk;
using Yoti.Auth.Sandbox.Profile.Request.Attribute;

namespace Yoti.Auth.Sandbox.Tests.Profile.Request
{
    internal static class AttributeMatcher
    {
        public static void AssertContainsAttribute(ICollection<SandboxAttribute> attributes, string name, string value, bool optional, string derivation = null, List<SandboxAnchor> anchors = null)
        {
            var expectedAttribute = new SandboxAttribute(name, value, derivation, optional, anchors);

            foreach (var attribute in attributes)
            {
                if (expectedAttribute.Name == attribute.Name
                && expectedAttribute.Value == attribute.Value
                && expectedAttribute.Derivation == attribute.Derivation
                && expectedAttribute.Optional == attribute.Optional
                && expectedAttribute.Anchors.SequenceEqual(attribute.Anchors))
                {
                    return;
                }
            }

            throw new XunitException(
                    $"Expected attribute with: " +
                    $"Name='{expectedAttribute.Name}'," +
                    $"Value='{expectedAttribute.Value}'," +
                    $"Derivation='{expectedAttribute.Derivation}', " +
                    $"Optional='{expectedAttribute.Optional}'," +
                    $"Anchors='{expectedAttribute.Anchors.ToString()}', but it was not found");
        }

        public static void AssertContainsAttribute(ICollection<SandboxAttribute> attributes, string name, string value)
        {
            AssertContainsAttribute(attributes, name, value, optional: false, derivation: null, anchors: new List<SandboxAnchor>());
        }

        public static void AssertContainsAttribute(ICollection<SandboxAttribute> attributes, string name, string value, List<SandboxAnchor> anchors)
        {
            AssertContainsAttribute(attributes, name, value, optional: false, derivation: null, anchors: anchors);
        }

        public static void AssertContainsDerivedAttribute(ICollection<SandboxAttribute> attributes, string name, string value, string derivation)
        {
            AssertContainsAttribute(attributes, name, value, optional: false, derivation: derivation);
        }

        public static void AssertContainsOptionalAttribute(ICollection<SandboxAttribute> attributes, string name, string value)
        {
            AssertContainsAttribute(attributes, name, value, optional: true, derivation: null, anchors: new List<SandboxAnchor>());
        }

        public static void AssertContainsOptionalAttribute(ICollection<SandboxAttribute> attributes, string name, string value, List<SandboxAnchor> anchors)
        {
            AssertContainsAttribute(attributes, name, value, optional: true, derivation: null, anchors: anchors);
        }
    }
}