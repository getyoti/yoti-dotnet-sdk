using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.Constants;
using Yoti.Auth.DigitalIdentity.Policy;
using Yoti.Auth.Tests.TestData;

namespace Yoti.Auth.Tests.DigitalIdentity.Policy
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

            Auth.DigitalIdentity.Policy.Policy result = new PolicyBuilder()
                .WithWantedAttribute(wantedAttribute)
                .WithWantedAttribute(wantedAttribute)
                .Build();

            Assert.AreEqual(1, result.WantedAttributes.Count);
            Assert.IsTrue(result.WantedAttributes.Contains(wantedAttribute));
        }

        [TestMethod]
        public void ShouldContainAllAddedAttributes()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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

            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithAuthTypeEnabledThenDisabled()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithAuthType(24, enabled: true)
                .WithAuthType(24, enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithAuthTypeDisabledThenEnabled()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithSelfieAuthentication(enabled: true)
                .WithSelfieAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithSelfieAuthenticationDisabledThenEnabled()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithSelfieAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldFilterSelfieAuthenticationDuplicates()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithSelfieAuthentication(enabled: true)
                .WithAuthType(Auth.DigitalIdentity.Policy.Policy.SelfieAuthType, enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { _expectedSelfieAuthValue }));
        }

        [TestMethod]
        public void ShouldFilterPinAuthenticationDuplicates()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithPinAuthentication(enabled: true)
                .WithAuthType(Auth.DigitalIdentity.Policy.Policy.PinAuthType, enabled: true)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(1, result.Count);
            Assert.IsTrue(result.SetEquals(new HashSet<int> { _expectedPinAuthValue }));
        }

        [TestMethod]
        public void ShouldBuildWithPinAuthenticationEnabledThenDisabled()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithPinAuthentication(enabled: true)
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithPinAuthenticationDisabledThenEnabled()
        {
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
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
            Auth.DigitalIdentity.Policy.Policy dynamicPolicy = new PolicyBuilder()
                .WithPinAuthentication(enabled: false)
                .Build();

            HashSet<int> result = dynamicPolicy.WantedAuthTypes;

            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void ShouldBuildWithRememberMeFlag()
        {
            Auth.DigitalIdentity.Policy.Policy result = new PolicyBuilder()
                .WithRememberMeId(true)
                .Build();

            Assert.IsTrue(result.WantedRememberMeId);
        }

        [TestMethod]
        public void ShouldBuildWithIdentityProfileRequirements()
        {
            object identityProfileRequirements = IdentityProfiles.CreateStandardIdentityProfileRequirements();

            Auth.DigitalIdentity.Policy.Policy result = new PolicyBuilder()
                    .WithIdentityProfileRequirements(identityProfileRequirements)
                    .Build();

            Assert.AreEqual(identityProfileRequirements, result.IdentityProfileRequirements);
        }
        
        [TestMethod]
        public void ShouldBuildWithAdvancedIdentityProfileRequirements()
        {
            var advancedIdentityProfileRequirements = IdentityProfiles.CreateAdvancedIdentityProfileRequirements();
           
            Auth.DigitalIdentity.Policy.Policy result = new PolicyBuilder()
                    .WithAdvancedIdentityProfileRequirements(advancedIdentityProfileRequirements)
                    .Build();

            Assert.AreEqual(advancedIdentityProfileRequirements, result.AdvancedIdentityProfileRequirements);
        }
    }

    
<<<<<<< HEAD
}
=======
}
>>>>>>> c2735ea (SDK:2238 refatoring tests)
