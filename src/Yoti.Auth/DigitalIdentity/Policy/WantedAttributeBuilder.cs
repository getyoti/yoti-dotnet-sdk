using System.Collections.Generic;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class WantedAttributeBuilder
    {
        private string _name;
        private string _derivation;
        private List<Constraint> _constraints = new List<Constraint>();
        private bool? _acceptSelfAsserted;
        private bool? _optional;

        public WantedAttributeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public WantedAttributeBuilder WithOptional(bool optional)
        {
            _optional = optional;
            return this;
        }
        
        public WantedAttributeBuilder WithDerivation(string derivation)
        {
            _derivation = derivation;
            return this;
        }

        /// <summary>
        /// Adds a constraint to the wanted attribute.
        /// </summary>
        /// <param name="constraint"></param>
        public WantedAttributeBuilder WithConstraint(Constraint constraint)
        {
            _constraints.Add(constraint);
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

        /// <summary>
        /// Allow or deny the acceptance of self asserted attributes
        /// </summary>
        /// <param name="acceptSelfAsserted"></param>
        public WantedAttributeBuilder WithAcceptSelfAsserted(bool acceptSelfAsserted)
        {
            _acceptSelfAsserted = acceptSelfAsserted;
            return this;
        }

        public WantedAttribute Build()
        {
            Validation.NotNullOrEmpty(_name, nameof(_name));

            return new WantedAttribute(_name, _derivation, _constraints, _acceptSelfAsserted, _optional);
        }
    }
}
