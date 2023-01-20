using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.ShareUrl.Policy
{
    public class DynamicPolicyBuilder
    {
        private readonly Dictionary<string, WantedAttribute> _wantedAttributes = new Dictionary<string, WantedAttribute>();
        private readonly HashSet<int> _wantedAuthTypes = new HashSet<int>();
        private bool _wantedRememberMeId;
        private object _identityProfileRequirements;

        public DynamicPolicyBuilder WithWantedAttribute(WantedAttribute wantedAttribute)
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

        public DynamicPolicyBuilder WithWantedAttribute(string name, List<Constraint> constraints = null)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(name)
                    .WithConstraints(constraints)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public DynamicPolicyBuilder WithFamilyName(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.FamilyNameAttribute, constraints);
        }

        public DynamicPolicyBuilder WithGivenNames(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.GivenNamesAttribute, constraints);
        }

        public DynamicPolicyBuilder WithFullName(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.FullNameAttribute, constraints);
        }

        public DynamicPolicyBuilder WithDateOfBirth(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.DateOfBirthAttribute, constraints);
        }

        public DynamicPolicyBuilder WithAgeOver(int age, List<Constraint> constraints = null)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeOverAttribute}:{age}", constraints);
        }

        public DynamicPolicyBuilder WithAgeUnder(int age, List<Constraint> constraints = null)
        {
            return WithAgeDerivedAttribute($"{Constants.UserProfile.AgeUnderAttribute}:{age}", constraints);
        }

        private DynamicPolicyBuilder WithAgeDerivedAttribute(string derivation, List<Constraint> constraints)
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                    .WithName(Constants.UserProfile.DateOfBirthAttribute)
                    .WithDerivation(derivation)
                    .WithConstraints(constraints)
                    .Build();
            return WithWantedAttribute(wantedAttribute);
        }

        public DynamicPolicyBuilder WithGender(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.GenderAttribute, constraints);
        }

        public DynamicPolicyBuilder WithPostalAddress(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.PostalAddressAttribute, constraints);
        }

        public DynamicPolicyBuilder WithStructuredPostalAddress(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.StructuredPostalAddressAttribute, constraints);
        }

        public DynamicPolicyBuilder WithNationality(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.NationalityAttribute, constraints);
        }

        public DynamicPolicyBuilder WithPhoneNumber(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.PhoneNumberAttribute, constraints);
        }

        public DynamicPolicyBuilder WithSelfie(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.SelfieAttribute, constraints);
        }

        public DynamicPolicyBuilder WithEmail(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.EmailAddressAttribute, constraints);
        }

        public DynamicPolicyBuilder WithDocumentDetails(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.DocumentDetailsAttribute, constraints);
        }

        public DynamicPolicyBuilder WithDocumentImages(List<Constraint> constraints = null)
        {
            return WithWantedAttribute(Constants.UserProfile.DocumentImagesAttribute, constraints);
        }

        public DynamicPolicyBuilder WithSelfieAuthentication(bool enabled)
        {
            return WithAuthType(DynamicPolicy.SelfieAuthType, enabled);
        }

        public DynamicPolicyBuilder WithPinAuthentication(bool enabled)
        {
            return WithAuthType(DynamicPolicy.PinAuthType, enabled);
        }

        public DynamicPolicyBuilder WithAuthType(int authType, bool enabled)
        {
            if (enabled)
            {
                _wantedAuthTypes.Add(authType);
                return this;
            }

            _wantedAuthTypes.Remove(authType);
            return this;
        }

        public DynamicPolicyBuilder WithRememberMeId(bool required)
        {
            _wantedRememberMeId = required;
            return this;
        }

        /// <summary>
        /// Use an Identity Profile Requirement object for the share
        /// </summary>
        /// <param name="identityProfileRequirements"> object describing the identity profile requirements to use</param>
        /// <returns><see cref="DynamicPolicyBuilder"/> with the identity profile requirements</returns>
        public DynamicPolicyBuilder WithIdentityProfileRequirements(object identityProfileRequirements)
        {
            _identityProfileRequirements = identityProfileRequirements;
            return this;
        }

        public DynamicPolicy Build()
        {
            return new DynamicPolicy(_wantedAttributes.Values, _wantedAuthTypes, _wantedRememberMeId, _identityProfileRequirements);
        }
    }
}