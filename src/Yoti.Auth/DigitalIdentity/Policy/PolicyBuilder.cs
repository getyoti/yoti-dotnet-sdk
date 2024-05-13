using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    public class PolicyBuilder
    {
        private readonly Dictionary<string, WantedAttribute> _wantedAttributes = new Dictionary<string, WantedAttribute>();
        private readonly HashSet<int> _wantedAuthTypes = new HashSet<int>();
        private bool _wantedRememberMeId;
        private object _identityProfileRequirements;
        private AdvancedIdentityProfile _advancedIdentityProfileRequirements;

        public PolicyBuilder WithWantedAttribute(WantedAttribute wantedAttribute)
        {
            Validation.NotNull(wantedAttribute, nameof(wantedAttribute));

            string key = wantedAttribute.Derivation ?? wantedAttribute.Name;

            if (wantedAttribute.Constraints?.Count > 0)
            {
                key += "-" + wantedAttribute.Constraints.GetHashCode();
            }

            _wantedAttributes[key] = wantedAttribute;
            return this;
        }

        public PolicyBuilder WithWantedAttribute(string name, List<Constraint> constraints = null)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(name)
                    .WithConstraints(constraints)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public PolicyBuilder WithFamilyName(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.FamilyNameAttribute, constraints);
        }

        public PolicyBuilder WithGivenNames(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.GivenNamesAttribute, constraints);
        }

        public PolicyBuilder WithFullName(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.FullNameAttribute, constraints);
        }

        public PolicyBuilder WithDateOfBirth(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.DateOfBirthAttribute, constraints);
        }

        public PolicyBuilder WithAgeOver(int age, List<Constraint> constraints = null)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeOverAttribute}:{age}", constraints);
        }

        public PolicyBuilder WithAgeUnder(int age, List<Constraint> constraints = null)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeUnderAttribute}:{age}", constraints);
        }

        private PolicyBuilder WithAgeDerivedAttribute(string derivation, List<Constraint> constraints)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(Constants.UserProfile.DateOfBirthAttribute)
                    .WithDerivation(derivation)
                    .WithConstraints(constraints)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public PolicyBuilder WithGender(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.GenderAttribute, constraints);
        }

        public PolicyBuilder WithPostalAddress(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.PostalAddressAttribute, constraints);
        }

        public PolicyBuilder WithStructuredPostalAddress(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.StructuredPostalAddressAttribute, constraints);
        }

        public PolicyBuilder WithNationality(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.NationalityAttribute, constraints);
        }

        public PolicyBuilder WithPhoneNumber(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.PhoneNumberAttribute, constraints);
        }

        public PolicyBuilder WithSelfie(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.SelfieAttribute, constraints);
        }

        public PolicyBuilder WithEmail(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.EmailAddressAttribute, constraints);
        }

        public PolicyBuilder WithDocumentDetails(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.DocumentDetailsAttribute, constraints);
        }

        public PolicyBuilder WithDocumentImages(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.DocumentImagesAttribute, constraints);
        }

        public PolicyBuilder WithSelfieAuthentication(bool enabled)
        {
            return WithAuthType(Policy.SelfieAuthType, enabled);
        }

        public PolicyBuilder WithPinAuthentication(bool enabled)
        {
            return WithAuthType(Policy.PinAuthType, enabled);
        }

        public PolicyBuilder WithAuthType(int authType, bool enabled)
        {
            if (enabled)
            {
                _wantedAuthTypes.Add(authType);
                return this;
            }

            _wantedAuthTypes.Remove(authType);
            return this;
        }

        public PolicyBuilder WithRememberMeId(bool required)
        {
            _wantedRememberMeId = required;
            return this;
        }

        /// <summary>
        /// Use an Identity Profile Requirement object for the share
        /// </summary>
        /// <param name="identityProfileRequirements"> object describing the identity profile requirements to use</param>
        /// <returns><see cref="PolicyBuilder"/> with the identity profile requirements</returns>
        public PolicyBuilder WithIdentityProfileRequirements(object identityProfileRequirements)
        {
            _identityProfileRequirements = identityProfileRequirements;
            return this;
        }
        
        /// <summary>
        /// Use an Advanced Identity Profile Requirement object for the share
        /// </summary>
        /// <param name="advancedIdentityProfileRequirements"> object describing the advanced identity profile requirements to use</param>
        /// <returns><see cref="PolicyBuilder"/> with the advanced identity profile requirements</returns>
        public PolicyBuilder WithAdvancedIdentityProfileRequirements(AdvancedIdentityProfile advancedIdentityProfileRequirements)
        {
            _advancedIdentityProfileRequirements = advancedIdentityProfileRequirements;
            return this;
        }

        public Policy Build()
        {
            return new Policy(_wantedAttributes.Values, _wantedAuthTypes, _wantedRememberMeId, _identityProfileRequirements, _advancedIdentityProfileRequirements);
        }
    }
}
