using System.Collections.Generic;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class DynamicPolicyBuilder
    {
        private const int _selfieAuthType = 1;
        private const int _pinAuthType = 2;

        private readonly Dictionary<string, WantedAttribute> _wantedAttributes = new Dictionary<string, WantedAttribute>();
        private readonly List<int> _wantedAuthTypes = new List<int>();
        private bool _wantedRememberMeId;
        private bool _wantedRememberMeIdOptional;

        public DynamicPolicyBuilder WithWantedAttribute(WantedAttribute wantedAttribute)
        {
            string key = wantedAttribute.Derivation ?? wantedAttribute.Name;
            _wantedAttributes[key] = wantedAttribute;
            return this;
        }

        public DynamicPolicyBuilder WithWantedAttribute(string name, bool optional = false)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(name)
                    .WithOptional(optional)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public DynamicPolicyBuilder WithFamilyName(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.FamilyNameAttribute, optional);
        }

        public DynamicPolicyBuilder WithGivenNames(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.GivenNamesAttribute, optional);
        }

        public DynamicPolicyBuilder WithFullName(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.FullNameAttribute, optional);
        }

        public DynamicPolicyBuilder WithDateOfBirth(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.DateOfBirthAttribute, optional);
        }

        public DynamicPolicyBuilder WithAgeOver(int age, bool optional = false)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeOverAttribute}:{age}", optional);
        }

        public DynamicPolicyBuilder WithAgeUnder(int age, bool optional = false)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeUnderAttribute}:{age}", optional);
        }

        private DynamicPolicyBuilder WithAgeDerivedAttribute(string derivation, bool optional)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(Constants.UserProfile.DateOfBirthAttribute)
                    .WithDerivation(derivation)
                    .WithOptional(optional)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public DynamicPolicyBuilder WithGender(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.GenderAttribute, optional);
        }

        public DynamicPolicyBuilder WithPostalAddress(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.PostalAddressAttribute, optional);
        }

        public DynamicPolicyBuilder WithStructuredPostalAddress(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.StructuredPostalAddressAttribute, optional);
        }

        public DynamicPolicyBuilder WithNationality(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.NationalityAttribute, optional);
        }

        public DynamicPolicyBuilder WithPhoneNumber(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.PhoneNumberAttribute, optional);
        }

        public DynamicPolicyBuilder WithSelfie(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.SelfieAttribute, optional);
        }

        public DynamicPolicyBuilder WithEmail(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.EmailAddressAttribute, optional);
        }

        public DynamicPolicyBuilder WithDocumentDetails(bool optional = false)
        {
            return WithWantedAttribute(Constants.UserProfile.DocumentDetailsAttribute, optional);
        }

        public DynamicPolicyBuilder WithSelfieAuthorisation(bool enabled)
        {
            if (enabled)
            {
                return WithAuthType(_selfieAuthType);
            }
            return this;
        }

        public DynamicPolicyBuilder WithPinAuthorisation(bool enabled)
        {
            if (enabled)
            {
                return WithAuthType(_pinAuthType);
            }
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

        public DynamicPolicyBuilder WithRememberMeIdOptional(bool rememberMeIdOptional)
        {
            _wantedRememberMeIdOptional = rememberMeIdOptional;
            return this;
        }

        public DynamicPolicy Build()
        {
            return new DynamicPolicy(_wantedAttributes.Values, _wantedAuthTypes, _wantedRememberMeId, _wantedRememberMeIdOptional);
        }
    }
}