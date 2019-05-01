using System.Collections.Generic;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.Tests.ShareUrl.Policy
{
    internal class WantedAttributeMatcher
    {
        private readonly ICollection<WantedAttribute> _attributes;

        public WantedAttributeMatcher(ICollection<WantedAttribute> attributes)
        {
            _attributes = attributes;
        }

        public bool ContainsAttribute(string name, bool optional, string derivation = null)
        {
            var expectedAttribute = new WantedAttribute(name, derivation, optional);

            foreach (var attribute in _attributes)
            {
                if (attribute.Name == expectedAttribute.Name
                    && attribute.Derivation == expectedAttribute.Derivation
                    && attribute.IsOptional == expectedAttribute.IsOptional)
                {
                    return true;
                }
            }

            return false;
        }
    }
}