using System.Collections.Generic;
using System.Linq;
using Yoti.Auth.DigitalIdentity.Policy;

namespace Yoti.Auth.Tests.DigitalIdentity.Policy
{
    internal class WantedAttributeMatcher
    {
        private readonly ICollection<WantedAttribute> _attributes;

        public WantedAttributeMatcher(ICollection<WantedAttribute> attributes)
        {
            _attributes = attributes;
        }

        public bool ContainsAttribute(string name, string derivation = null, List<Constraint> constraints = null)
        {
            var expectedAttribute = new WantedAttribute(name, derivation, constraints);

            foreach (var attribute in _attributes)
            {
                if (attribute.Name == expectedAttribute.Name
                    && attribute.Derivation == expectedAttribute.Derivation
                    && ConstraintsMatch(expectedAttribute.Constraints, attribute.Constraints))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool ConstraintsMatch(List<Constraint> expectedConstraints, List<Constraint> attributeConstraint)
        {
            if (expectedConstraints == null && attributeConstraint == null)
                return true;

            return Enumerable.SequenceEqual(expectedConstraints, attributeConstraint);
        }
    }
}