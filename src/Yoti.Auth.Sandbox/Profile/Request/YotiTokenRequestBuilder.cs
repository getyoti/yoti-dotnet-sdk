using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Yoti.Auth.Constants;
using Yoti.Auth.Sandbox.Profile.Request.Attribute;
using Yoti.Auth.Sandbox.Profile.Request.Attribute.Derivation;

namespace Yoti.Auth.Sandbox.Profile.Request
{
    public class YotiTokenRequestBuilder
    {
        private string _rememberMeId;
        private readonly Dictionary<string, SandboxAttribute> _attributes = new Dictionary<string, SandboxAttribute>();

        public YotiTokenRequestBuilder()
        {
        }

        public YotiTokenRequestBuilder WithRememberMeId(string value)
        {
            _rememberMeId = value;
            return this;
        }

        public YotiTokenRequestBuilder WithAttribute(SandboxAttribute sandboxAttribute)
        {
            Validation.NotNull(sandboxAttribute, nameof(sandboxAttribute));
            string key = sandboxAttribute.Derivation ?? sandboxAttribute.Name;
            _attributes[key] = sandboxAttribute;
            return this;
        }

        public YotiTokenRequestBuilder WithGivenNames(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.GivenNamesAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithGivenNames(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.GivenNamesAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithFamilyName(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.FamilyNameAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithFamilyName(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.FamilyNameAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithFullName(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.FullNameAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithFullName(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.FullNameAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithDateOfBirth(DateTime value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(
                UserProfile.DateOfBirthAttribute,
                value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithDateOfBirth(DateTime value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(
                UserProfile.DateOfBirthAttribute,
                value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithDateOfBirth(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.DateOfBirthAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithDateOfBirth(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.DateOfBirthAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithAgeVerification(SandboxAgeVerification sandboxAgeVerification)
        {
            Validation.NotNull(sandboxAgeVerification, nameof(sandboxAgeVerification));
            return WithAttribute(sandboxAgeVerification.ToAttribute());
        }

        public YotiTokenRequestBuilder WithGender(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.GenderAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithGender(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.GenderAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithPhoneNumber(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.PhoneNumberAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithPhoneNumber(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.PhoneNumberAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithNationality(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.NationalityAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithNationality(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.NationalityAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithPostalAddress(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.PostalAddressAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithPostalAddress(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.PostalAddressAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithStructuredPostalAddress(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.StructuredPostalAddressAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithStructuredPostalAddress(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.StructuredPostalAddressAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithSelfie(byte[] value)
        {
            string base64Selfie = Conversion.BytesToBase64(value);
            return WithBase64Selfie(base64Selfie);
        }

        public YotiTokenRequestBuilder WithSelfie(byte[] value, List<SandboxAnchor> anchors)
        {
            string base64Selfie = Conversion.BytesToBase64(value);
            return WithBase64Selfie(base64Selfie, anchors);
        }

        public YotiTokenRequestBuilder WithBase64Selfie(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.SelfieAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithBase64Selfie(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.SelfieAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithEmailAddress(string value)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.EmailAddressAttribute, value);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithEmailAddress(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = CreateAttribute(UserProfile.EmailAddressAttribute, value, anchors);
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequestBuilder WithDocumentDetails(Document.DocumentDetails value)
        {
            Validation.NotNull(value, nameof(value));
            return WithDocumentDetails(value.ToString());
        }

        public YotiTokenRequestBuilder WithDocumentDetails(Document.DocumentDetails value, List<SandboxAnchor> anchors)
        {
            Validation.NotNull(value, nameof(value));
            return WithDocumentDetails(value.ToString(), anchors);
        }

        public YotiTokenRequestBuilder WithDocumentDetails(string value)
        {
            return WithDocumentDetails(value, new List<SandboxAnchor>());
        }

        public YotiTokenRequestBuilder WithDocumentDetails(string value, List<SandboxAnchor> anchors)
        {
            SandboxAttribute sandboxAttribute = SandboxAttribute.Builder()
                    .WithName(UserProfile.DocumentDetailsAttribute)
                    .WithValue(value)
                    .WithOptional(true)
                    .WithAnchors(anchors)
                    .Build();
            return WithAttribute(sandboxAttribute);
        }

        public YotiTokenRequest Build()
        {
            return new YotiTokenRequest(_rememberMeId, new ReadOnlyCollection<SandboxAttribute>(_attributes.Values.ToList()));
        }

        private static SandboxAttribute CreateAttribute(string name, string value)
        {
            return CreateAttribute(name, value, new List<SandboxAnchor>());
        }

        private static SandboxAttribute CreateAttribute(string name, string value, List<SandboxAnchor> anchors)
        {
            return SandboxAttribute.Builder()
                    .WithName(name)
                    .WithValue(value)
                    .WithAnchors(anchors)
                    .Build();
        }
    }
}