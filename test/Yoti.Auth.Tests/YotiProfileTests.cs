using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiProfileTests
    {
        private readonly string _value = "value";

        [TestMethod]
        public void YotiProfile_SelfieAttribute()
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Jpeg, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<Image>(Constants.UserProfile.SelfieAttribute, attributeValue);

            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<Image> selfieAttribute = yotiProfile.Selfie;

            Assert.AreSame(initialAttribute, selfieAttribute);
        }

        [TestMethod]
        public void YotiProfile_FullNameAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.FullNameAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> fullNameAttribute = yotiProfile.FullName;

            Assert.AreSame(initialAttribute, fullNameAttribute);
        }

        [TestMethod]
        public void YotiProfile_GivenNamesAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.GivenNamesAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> givenNamesAttribute = yotiProfile.GivenNames;

            Assert.AreSame(initialAttribute, givenNamesAttribute);
        }

        [TestMethod]
        public void YotiProfile_FamilyNameAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.FamilyNameAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> familyNameAttribute = yotiProfile.FamilyName;

            Assert.AreSame(initialAttribute, familyNameAttribute);
        }

        [TestMethod]
        public void YotiProfile_PhoneNumberAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.PhoneNumberAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> mobileNumberAttribute = yotiProfile.MobileNumber;

            Assert.AreSame(initialAttribute, mobileNumberAttribute);
        }

        [TestMethod]
        public void YotiProfile_EmailAddressAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.EmailAddressAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> emailAddressAttribute = yotiProfile.EmailAddress;

            Assert.AreSame(initialAttribute, emailAddressAttribute);
        }

        [TestMethod]
        public void YotiProfile_DateOfBirthAttribute()
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Date, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<DateTime?>(Constants.UserProfile.DateOfBirthAttribute, attributeValue);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<DateTime?> dateOfBirthAttribute = yotiProfile.DateOfBirth;

            Assert.AreSame(initialAttribute, dateOfBirthAttribute);
        }

        [TestMethod]
        public void YotiProfile_AddressAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.PostalAddressAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> addressAttribute = yotiProfile.Address;

            Assert.AreSame(initialAttribute, addressAttribute);
        }

        [TestMethod]
        public void YotiProfile_StructuredPostalAddressAttribute()
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Text, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<IEnumerable<Dictionary<string, JToken>>>(Constants.UserProfile.StructuredPostalAddressAttribute, attributeValue);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<IEnumerable<Dictionary<string, JToken>>> structuredPostalAddressAttribute = yotiProfile.StructuredPostalAddress;

            Assert.AreSame(initialAttribute, structuredPostalAddressAttribute);
        }

        [TestMethod]
        public void YotiProfile_GenderAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.GenderAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> genderAttribute = yotiProfile.Gender;

            Assert.AreSame(initialAttribute, genderAttribute);
        }

        [TestMethod]
        public void YotiProfile_NationalityAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.NationalityAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> nationalityAttribute = yotiProfile.Nationality;

            Assert.AreSame(initialAttribute, nationalityAttribute);
        }

        private YotiAttribute<string> CreateStringAttribute(string name)
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Text, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<string>(name, attributeValue);
            return initialAttribute;
        }
    }
}