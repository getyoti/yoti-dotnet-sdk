using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Constants;
using Yoti.Auth.ShareUrl.Policy;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests.ShareUrl.Policy
{
    [TestClass]
    public class DynamicPolicyBuilderTests
    {
        private readonly int _expectedSelfieAuthValue = 1;
        private readonly int _expectedPinAuthValue = 2;

        [TestMethod]
        public void AttributeShouldOnlyExistOnce()
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
        public void ShouldContainAllAddedAttributes()
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
                .WithDocumentDetails()
                .WithDocumentImages()
                .WithAgeOver(55)
                .WithAgeUnder(18)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(15, result.Count);

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
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DocumentImagesAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DocumentDetailsAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{Constants.UserProfile.AgeOverAttribute}:55"));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DateOfBirthAttribute, derivation: $"{Constants.UserProfile.AgeUnderAttribute}:18"));
        }

        [TestMethod]
        public void ShouldBuildWithMultipleAgeDerivedAttributes()
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
        public void ShouldAddMultipleAttributesWithSameNameAndDifferentConstraints()
        {
            var passportConstraint = new SourceConstraintBuilder()
                  .WithPassport()
                  .Build();

            var docImage1 = new WantedAttributeBuilder()
                .WithName(Yoti.Auth.Constants.UserProfile.DocumentImagesAttribute)
                .WithConstraint(passportConstraint)
                .Build();

            var drivingLicenseConstraint = new SourceConstraintBuilder()
               .WithDrivingLicense()
               .Build();

            var docImage2 = new WantedAttributeBuilder()
                .WithName(Yoti.Auth.Constants.UserProfile.DocumentImagesAttribute)
                .WithConstraints(new List<Constraint> { drivingLicenseConstraint })
                .Build();

            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithWantedAttribute(docImage1)
                .WithWantedAttribute(docImage2)
                .Build();

            ICollection<WantedAttribute> result = dynamicPolicy.WantedAttributes;
            var attributeMatcher = new WantedAttributeMatcher(result);

            Assert.AreEqual(2, result.Count);

            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DocumentImagesAttribute, null, new List<Constraint> { passportConstraint }));
            Assert.IsTrue(attributeMatcher.ContainsAttribute(UserProfile.DocumentImagesAttribute, null, new List<Constraint> { drivingLicenseConstraint }));
        }

        [TestMethod]
        public void ShouldBuildWithAuthTypesTrue()
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
        public void ShouoldBuildWithAuthTypesFalse()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithAuthTypeEnabledThenDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithAuthType(24, enabled: true)
                .WithAuthType(24, enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithAuthTypeDisabledThenEnabled()
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
        public void ShouldBuildWithSelfieAuthenticationEnabledThenDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: true)
                .WithSelfieAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithSelfieAuthenticationDisabledThenEnabled()
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
        public void ShouldBuildWithSelfieAuthenticationDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldFilterSelfieAuthenticationDuplicates()
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
        public void ShouldFilterPinAuthenticationDuplicates()
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
        public void ShouldBuildWithPinAuthenticationEnabledThenDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithPinAuthentication(enabled: true)
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithPinAuthenticationDisabledThenEnabled()
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
        public void ShouldBuildWithPinAuthenticationDisabled()
        {
            DynamicPolicy dynamicPolicy = new DynamicPolicyBuilder()
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithRememberMeFlag()
        {
            DynamicPolicy result = new DynamicPolicyBuilder()
                .WithRememberMeId(true)
                .Build();

            Assert.IsTrue(result.WantedRememberMeId);
        }

        [TestMethod]
        public void ShouldBuildWithIdentityProfileRequirements()
        {
            object identityProfileRequirements = IdentityProfiles.CreateStandardIdentityProfileRequirements();

            DynamicPolicy result = new DynamicPolicyBuilder()
                    .WithIdentityProfileRequirements(identityProfileRequirements)
                    .Build();

            Assert.AreEqual(identityProfileRequirements, result.IdentityProfileRequirements);
        }
    }
}