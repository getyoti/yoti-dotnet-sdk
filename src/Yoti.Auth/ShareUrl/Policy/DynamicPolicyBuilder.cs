using System.Collections.Generic;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class DynamicPolicyBuilder
    {
        private readonly Dictionary<string, WantedAttribute> _wantedAttributes = new Dictionary<string, WantedAttribute>();
        private readonly HashSet<int> _wantedAuthTypes = new HashSet<int>();
        private bool _wantedRememberMeId;

        public DynamicPolicyBuilder WithWantedAttribute(WantedAttribute wantedAttribute)
        {
            Validation.NotNull(wantedAttribute, "wantedAttribute");

            string key = wantedAttribute.Derivation ?? wantedAttribute.Name;
            _wantedAttributes[key] = wantedAttribute;
            return this;
        }

        public DynamicPolicyBuilder WithWantedAttribute(string name)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(name)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public DynamicPolicyBuilder WithFamilyName()
        {
            return WithWantedAttribute(Constants.UserProfile.FamilyNameAttribute);
        }

        public DynamicPolicyBuilder WithGivenNames()
        {
            return WithWantedAttribute(Constants.UserProfile.GivenNamesAttribute);
        }

        public DynamicPolicyBuilder WithFullName()
        {
            return WithWantedAttribute(Constants.UserProfile.FullNameAttribute);
        }

        public DynamicPolicyBuilder WithDateOfBirth()
        {
            return WithWantedAttribute(Constants.UserProfile.DateOfBirthAttribute);
        }

        public DynamicPolicyBuilder WithAgeOver(int age)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeOverAttribute}:{age}");
        }

        public DynamicPolicyBuilder WithAgeUnder(int age)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeUnderAttribute}:{age}");
        }

        private DynamicPolicyBuilder WithAgeDerivedAttribute(string derivation)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(Constants.UserProfile.DateOfBirthAttribute)
                    .WithDerivation(derivation)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public DynamicPolicyBuilder WithGender()
        {
            return WithWantedAttribute(Constants.UserProfile.GenderAttribute);
        }

        public DynamicPolicyBuilder WithPostalAddress()
        {
            return WithWantedAttribute(Constants.UserProfile.PostalAddressAttribute);
        }

        public DynamicPolicyBuilder WithStructuredPostalAddress()
        {
            return WithWantedAttribute(Constants.UserProfile.StructuredPostalAddressAttribute);
        }

        public DynamicPolicyBuilder WithNationality()
        {
            return WithWantedAttribute(Constants.UserProfile.NationalityAttribute);
        }

        public DynamicPolicyBuilder WithPhoneNumber()
        {
            return WithWantedAttribute(Constants.UserProfile.PhoneNumberAttribute);
        }

        public DynamicPolicyBuilder WithSelfie()
        {
            return WithWantedAttribute(Constants.UserProfile.SelfieAttribute);
        }

        public DynamicPolicyBuilder WithEmail()
        {
            return WithWantedAttribute(Constants.UserProfile.EmailAddressAttribute);
        }

        public DynamicPolicyBuilder WithDocumentDetails()
        {
            return WithWantedAttribute(Constants.UserProfile.DocumentDetailsAttribute);
        }

        public DynamicPolicyBuilder WithSelfieAuthentication(bool enabled)
        {
            if (enabled)
            {
                return WithAuthType(DynamicPolicy.SelfieAuthType);
            }

            _wantedAuthTypes.Remove(DynamicPolicy.SelfieAuthType);
            return this;
        }

        public DynamicPolicyBuilder WithPinAuthentication(bool enabled)
        {
            if (enabled)
            {
                return WithAuthType(DynamicPolicy.PinAuthType);
            }

            _wantedAuthTypes.Remove(DynamicPolicy.PinAuthType);
            return this;
        }

        public DynamicPolicyBuilder WithAuthType(int authType)
        {
            _wantedAuthTypes.Add(authType);
            return this;
        }

        public DynamicPolicyBuilder WithRememberMeId(bool required)
        {
            _wantedRememberMeId = required;
            return this;
        }

        public DynamicPolicy Build()
        {
            return new DynamicPolicy(_wantedAttributes.Values, _wantedAuthTypes, _wantedRememberMeId);
        }
    }
}