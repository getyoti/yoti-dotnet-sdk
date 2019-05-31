namespace Yoti.Auth.ShareUrl.Policy
{
    public class WantedAttributeBuilder
    {
        private string _name;
        private string _derivation;

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

        public WantedAttribute Build()
        {
            return new WantedAttribute(_name, _derivation);
        }
    }
}