using System.Collections.Generic;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class WantedAttributeBuilder
    {
        private string _name;
        private string _derivation;
        private List<Constraint> _constraints;

        public WantedAttributeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public WantedAttributeBuilder WithDerivation(string derivation)
        {
            _derivation = derivation;
            return this;
        }

        /// <summary>
        /// Add constraints to the wanted attribute.
        /// Calling this will override any previously set constraints for this attribute.
        /// </summary>
        /// <param name="constraints">Constraints</param>
        public WantedAttributeBuilder WithConstraints(List<Constraint> constraints)
        {
            _constraints = constraints;
            return this;
        }

        public WantedAttribute Build()
        {
            Validation.NotNullOrEmpty(_name, nameof(_name));

            return new WantedAttribute(_name, _derivation, _constraints);
        }
    }
}