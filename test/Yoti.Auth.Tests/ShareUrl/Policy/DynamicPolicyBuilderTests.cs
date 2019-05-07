using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Constants;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.Tests.ShareUrl.Policy
{
    [TestClass]
    public class DynamicPolicyBuilderTests
    {
        [TestMethod]
        public void EnsuresAnAttributeCanOnlyExistOnce()
        {
            WantedAttribute wantedAttribute = new WantedAttributeBuilder()
                .WithName("SomeAttributeName")
                .Build();

            DynamicPolicy result = new DynamicPolicyBuilder()
                .WithWantedAttribute(wantedAttribute)
                .WithWantedAttribute(wantedAttribute)
                .Build();

            Assert.AreEqual(1, result.WantedAttributes.Count);
            Assert.IsTrue(result.WantedAttributes.Contains(wantedAttribute));
        }

        [TestMethod]
        public void BuildWithAttributes()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithFamilyName()
                .WithGivenNames()
                .WithFullName()
                .WithDateOfBirth()
                .WithGender()
                .WithPostalAddress()
                .WithStructuredPostalAddress()
                .WithNationality()
                .WithPhoneNumber()
                .WithSelfie()
                .WithEmail()
                .WithAgeOver(55)
                .WithAgeUnder(18)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(13, result.Count);

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.FamilyNameAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.GivenNamesAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.FullNameAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.GenderAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.PostalAddressAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.StructuredPostalAddressAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.NationalityAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.PhoneNumberAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.SelfieAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.EmailAddressAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: false, derivation: $"{Constants.UserProfile.AgeOverAttribute}:55"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: false, derivation: $"{Constants.UserProfile.AgeUnderAttribute}:18"));
        }

        [TestMethod]
        public void BuildWithAttributesOptionalFlag()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithFamilyName(optional: true)
                .WithGivenNames(optional: true)
                .WithFullName(optional: true)
                .WithDateOfBirth(optional: true)
                .WithGender(optional: true)
                .WithPostalAddress(optional: true)
                .WithStructuredPostalAddress(optional: true)
                .WithNationality(optional: true)
                .WithPhoneNumber(optional: true)
                .WithSelfie(optional: true)
                .WithEmail(optional: true)
                .WithAgeOver(55, optional: true)
                .WithAgeUnder(18, optional: true)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(13, result.Count);

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.FamilyNameAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.GivenNamesAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.FullNameAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.GenderAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.PostalAddressAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.StructuredPostalAddressAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.NationalityAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.PhoneNumberAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.SelfieAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.EmailAddressAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: true, derivation: $"{Constants.UserProfile.AgeOverAttribute}:55"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: true, derivation: $"{Constants.UserProfile.AgeUnderAttribute}:18"));
        }

        [TestMethod]
        public void BuildWithMultipleAgeDerivedAttributes()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithDateOfBirth()
                .WithAgeOver(18)
                .WithAgeUnder(30)
                .WithAgeUnder(40)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(4, result.Count);

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: false));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, false, derivation: $"{UserProfile.AgeOverAttribute}:{18}"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, false, derivation: $"{UserProfile.AgeUnderAttribute}:{30}"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, false, derivation: $"{UserProfile.AgeUnderAttribute}:{40}"));
        }

        [TestMethod]
        public void BuildWithMultipleAgeDerivedAttributesOptionalFlag()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithDateOfBirth(optional: true)
                .WithAgeOver(18, optional: true)
                .WithAgeUnder(30, optional: true)
                .WithAgeUnder(40, optional: true)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(4, result.Count);

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, optional: true));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, true, derivation: $"{UserProfile.AgeOverAttribute}:{18}"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, true, derivation: $"{UserProfile.AgeUnderAttribute}:{30}"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, true, derivation: $"{UserProfile.AgeUnderAttribute}:{40}"));
        }

        [TestMethod]
        public void ShouldOverwriteIdenticalAgeVerificationToEnsureItOnlyExistsOnce()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithAgeUnder(30, optional: true)
                .WithAgeUnder(30, optional: false)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(1, result.Count);

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, false, derivation: $"{UserProfile.AgeUnderAttribute}:{30}"));
        }

        [TestMethod]
        public void BuildsWithAuthTypesTrue()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: true)
                .WithPinAuthentication(enabled: true)
                .WithAuthType(99)
                .Build();

            List<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(3, result.Count);
            CollectionAssert.AreEqual(result, new List<int> { 1, 2, 99 });
        }

        [TestMethod]
        public void BuildsWithAuthTypesFalse()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .WithPinAuthentication(enabled: false)
                .Build();

            List<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildsWithRememberMeFlags()
        {
            DynamicPolicy result = new DynamicPolicyBuilder()
                .WithRememberMeId(true)
                .WithRememberMeIdOptional(false)
                .Build();

            Assert.IsTrue(result.WantedRememberMeId);
            Assert.IsFalse(result.IsWantedRememberMeIdOptional);
        }
    }
}