using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Constants;
using Yoti.Auth.ShareUrl.Policy;

namespace Yoti.Auth.Tests.ShareUrl.Policy
{
    [TestClass]
    public class DynamicPolicyBuilderTests
    {
        private readonly int _expectedSelfieAuthValue = 1;
        private readonly int _expectedPinAuthValue = 2;

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

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.FamilyNameAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.GivenNamesAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.FullNameAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.GenderAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.PostalAddressAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.StructuredPostalAddressAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.NationalityAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.PhoneNumberAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.SelfieAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.EmailAddressAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{Constants.UserProfile.AgeOverAttribute}:55"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{Constants.UserProfile.AgeUnderAttribute}:18"));
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

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{UserProfile.AgeOverAttribute}:{18}"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{UserProfile.AgeUnderAttribute}:{30}"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{UserProfile.AgeUnderAttribute}:{40}"));
        }

        [TestMethod]
        public void ShouldOverwriteIdenticalAgeVerificationToEnsureItOnlyExistsOnce()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithAgeUnder(30)
                .WithAgeUnder(30)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(1, result.Count);

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{UserProfile.AgeUnderAttribute}:{30}"));
        }

        [TestMethod]
        public void BuildsWithAuthTypesTrue()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: true)
                .WithPinAuthentication(enabled: true)
                .WithAuthType(authType: 99, enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(3, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { _expectedSelfieAuthValue, _expectedPinAuthValue, 99 }));
        }

        [TestMethod]
        public void BuildsWithAuthTypesFalse()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildsWithAuthTypeEnabledThenDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithAuthType(24, enabled: true)
                .WithAuthType(24, enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildsWithAuthTypeDisabledThenEnabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithAuthType(23, enabled: false)
                .WithAuthType(23, enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { 23 }));
        }

        [TestMethod]
        public void BuildsWithSelfieAuthenticationEnabledThenDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: true)
                .WithSelfieAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildsWithSelfieAuthenticationDisabledThenEnabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .WithSelfieAuthentication(enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { _expectedSelfieAuthValue }));
        }

        [TestMethod]
        public void BuildsWithSelfieAuthenticationDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void FiltersSelfieAuthenticationDuplicates()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: true)
                .WithAuthType(DynamicPolicy.SelfieAuthType, enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { _expectedSelfieAuthValue }));
        }

        [TestMethod]
        public void FiltersPinAuthenticationDuplicates()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithPinAuthentication(enabled: true)
                .WithAuthType(DynamicPolicy.PinAuthType, enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { _expectedPinAuthValue }));
        }

        [TestMethod]
        public void BuildsWithPinAuthenticationEnabledThenDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithPinAuthentication(enabled: true)
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildsWithPinAuthenticationDisabledThenEnabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithPinAuthentication(enabled: false)
                .WithPinAuthentication(enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { _expectedPinAuthValue }));
        }

        [TestMethod]
        public void BuildsWithPinAuthenticationDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void BuildsWithRememberMeFlag()
        {
            DynamicPolicy result = new DynamicPolicyBuilder()
                .WithRememberMeId(true)
                .Build();

            Assert.IsTrue(result.WantedRememberMeId);
        }
    }
}