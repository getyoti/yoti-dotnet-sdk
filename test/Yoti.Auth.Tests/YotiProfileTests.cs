﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.Protobuf;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;
using Yoti.Auth.Attribute;
using Yoti.Auth.Images;
using Yoti.Auth.Profile;
using Yoti.Auth.ProtoBuf.Attribute;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiProfileTests
    {
        private readonly string _value = "value";

        [TestMethod]
        public void ShouldRetrieveSelfieAttribute()
        {
            var initialAttribute = new YotiAttribute<Image>(
               name: Constants.UserProfile.SelfieAttribute,
               value: new JpegImage(Encoding.UTF8.GetBytes(_value)),
               anchors: null);

            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<Image> selfieAttribute = yotiProfile.Selfie;

            Assert.AreSame(initialAttribute, selfieAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveFullNameAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.FullNameAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> fullNameAttribute = yotiProfile.FullName;

            Assert.AreSame(initialAttribute, fullNameAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveGivenNamesAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.GivenNamesAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> givenNamesAttribute = yotiProfile.GivenNames;

            Assert.AreSame(initialAttribute, givenNamesAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveFamilyNameAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.FamilyNameAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> familyNameAttribute = yotiProfile.FamilyName;

            Assert.AreSame(initialAttribute, familyNameAttribute);
        }

        [TestMethod]
        public void ShouldRetrievePhoneNumberAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.PhoneNumberAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> mobileNumberAttribute = yotiProfile.MobileNumber;

            Assert.AreSame(initialAttribute, mobileNumberAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveEmailAddressAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.EmailAddressAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> emailAddressAttribute = yotiProfile.EmailAddress;

            Assert.AreSame(initialAttribute, emailAddressAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveDateOfBirthAttribute()
        {
            var initialAttribute = new YotiAttribute<DateTime>(
                name: Constants.UserProfile.DateOfBirthAttribute,
                value: new DateTime(2000, 01, 13),
                anchors: null);

            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<DateTime> dateOfBirthAttribute = yotiProfile.DateOfBirth;

            Assert.AreSame(initialAttribute, dateOfBirthAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveAddressAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.PostalAddressAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> addressAttribute = yotiProfile.Address;

            Assert.AreSame(initialAttribute, addressAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveStructuredPostalAddressAttribute()
        {
            var jsonValue = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, JToken>>(
                    "{ \"properties\": { \"name\": { \"type\": \"string\" } } }");

            var initialAttribute = new YotiAttribute<Dictionary<string, JToken>>(
               name: Constants.UserProfile.StructuredPostalAddressAttribute,
               value: jsonValue,
               anchors: null);

            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<Dictionary<string, JToken>> structuredPostalAddressAttribute = yotiProfile.StructuredPostalAddress;

            Assert.AreSame(initialAttribute, structuredPostalAddressAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveGenderAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.GenderAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> genderAttribute = yotiProfile.Gender;

            Assert.AreSame(initialAttribute, genderAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveNationalityAttribute()
        {
            YotiAttribute<string> initialAttribute = CreateStringAttribute(Constants.UserProfile.NationalityAttribute);
            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> nationalityAttribute = yotiProfile.Nationality;

            Assert.AreSame(initialAttribute, nationalityAttribute);
        }

        [TestMethod]
        public void ShouldRetrieveIntAttribute()
        {
            string intAttributeName = "intAttributeName";
            int intValue = 92387;
            var initialAttribute = new YotiAttribute<int>(
                intAttributeName,
                intValue,
                anchors: null);

            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);
            YotiAttribute<int> intAttribute = yotiProfile.GetAttributeByName<int>(intAttributeName);
            YotiAttribute<int> intAttributeFromCollection = yotiProfile.GetAttributesByName<int>(intAttributeName).Single();

            Assert.AreEqual(intValue, intAttribute.GetValue());
            Assert.AreEqual(intValue, intAttributeFromCollection.GetValue());
        }

        [TestMethod]
        public void ShouldRetrieveAttributeByID()
        {
            string attributeId1 = "9e2b479a-7be9-4e88-b4ab-e47fc930af61";
            string attributeId2 = "a8960bbb-de13-47d1-9bd3-f6f32de8505a";

            var attribute1 = CreateImageAttribute(Constants.UserProfile.SelfieAttribute, attributeId1);
            var attribute2 = CreateImageAttribute(Constants.UserProfile.SelfieAttribute, attributeId2);

            var selfieAttributes = new Dictionary<string, List<BaseAttribute>>
            {
                { "selfie", new List<BaseAttribute> {attribute1, attribute2 } },
            };

            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithAttributes<Image>(selfieAttributes);
            var givenNamesattribute = new ProtoBuf.Attribute.Attribute
            {
                Name = Constants.UserProfile.GivenNamesAttribute,
                ContentType = ContentType.String,
                Value = ByteString.CopyFromUtf8("Alan")
            };

            TestTools.Profile.AddAttributeToProfile<string>(yotiProfile, givenNamesattribute);

            Assert.AreSame(attribute1, yotiProfile.GetAttributeById<Image>(attributeId1));
            Assert.AreSame(attribute2, yotiProfile.GetAttributeById<Image>(attributeId2));
        }

        [TestMethod]
        public void GetAttributeByIdReturnsNullWhenNotPresent()
        {
            var attribute = CreateImageAttribute(
                Constants.UserProfile.SelfieAttribute,
                "9e2b479a-7be9-4e88-b4ab-e47fc930af61");

            YotiProfile yotiProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(attribute);

            Assert.IsNull(yotiProfile.GetAttributeById<Image>("not-matching-id"));
        }

        private YotiAttribute<string> CreateStringAttribute(string name)
        {
            return new YotiAttribute<string>(
               name,
               _value,
               anchors: null);
        }

        private YotiAttribute<Image> CreateImageAttribute(string name, string id)
        {
            return new YotiAttribute<Image>(
               name,
               value: new JpegImage(Encoding.UTF8.GetBytes(_value)),
               anchors: null,
               id);
        }
    }
}