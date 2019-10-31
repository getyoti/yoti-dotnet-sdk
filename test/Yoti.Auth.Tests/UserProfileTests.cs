using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Attribute;
using Yoti.Auth.Images;
using Yoti.Auth.Profile;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class UserProfileTests
    {
        [TestMethod]
        public void GetAttributeByNameShouldRetrieveDatetime()
        {
            DateTime value = new DateTime(1990, 1, 13);
            var initialAttribute = new YotiAttribute<DateTime>(
                name: Constants.UserProfile.DateOfBirthAttribute,
                value: value,
                anchors: null);

            YotiProfile userProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<DateTime> dobAttribute = userProfile.GetAttributeByName<DateTime>(Constants.UserProfile.DateOfBirthAttribute);

            Assert.AreSame(initialAttribute, dobAttribute);
            Assert.AreEqual(value, dobAttribute.GetValue());
        }

        [TestMethod]
        public void GetAttributeByNameShouldRetrieveString()
        {
            var initialAttribute = new YotiAttribute<string>(
                name: Constants.UserProfile.NationalityAttribute,
                value: "Nation",
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<string> nationalityAttribute = userProfile.GetAttributeByName<string>(Constants.UserProfile.NationalityAttribute);

            Assert.AreSame(initialAttribute, nationalityAttribute);
        }

        [TestMethod]
        public void GetAttributeByNameShouldRetrieveBool()
        {
            bool boolValue = true;
            string attributeName = "Bool";

            var initialAttribute = new YotiAttribute<bool>(
                name: attributeName,
                value: boolValue,
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<bool> boolAttribute = userProfile.GetAttributeByName<bool>(attributeName);

            Assert.AreEqual(boolValue, boolAttribute.GetValue());
        }

        [TestMethod]
        public void GetAttributeByNameShouldRetrieveImage()
        {
            Image imageValue = new PngImage(Encoding.UTF8.GetBytes("Value"));

            var initialAttribute = new YotiAttribute<Image>(
                name: Constants.UserProfile.SelfieAttribute,
                value: imageValue,
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<Image> imageAttribute = userProfile.GetAttributeByName<Image>(Constants.UserProfile.SelfieAttribute);

            Assert.AreEqual(imageValue, imageAttribute.GetValue());
        }

        [TestMethod]
        public void ShouldRetrieveAttributesThroughAttributeList()
        {
            string mobileNumberValue = "0127456689";
            var initialAttribute = new YotiAttribute<string>(
                 name: Constants.UserProfile.PhoneNumberAttribute,
                 value: mobileNumberValue,
                 anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

#pragma warning disable CS0618 // Type or member is obsolete
            var attribute1 = (YotiAttribute<string>)userProfile.Attributes.Values.First();
#pragma warning restore CS0618 // Type or member is obsolete
            Assert.AreEqual(mobileNumberValue, attribute1.GetValue());

            var attributeListItem1 = (YotiAttribute<string>)userProfile.AttributeCollection.First();
            Assert.AreEqual(mobileNumberValue, attributeListItem1.GetValue());
        }

        [TestMethod]
        public void GetAttributesByNameReturnsCorrectAttributes()
        {
            string commonAttributeName = "matchingName1";
            string attributeValue1 = "attributeValue1";
            string attributeValue2 = "attributeValue2";

            var matchingName1 = new YotiAttribute<string>(
                name: commonAttributeName,
                value: attributeValue1,
                anchors: null);

            var matchingName2 = new YotiAttribute<string>(
                name: commonAttributeName,
                value: attributeValue2,
                anchors: null);

            var nonMatchingName = new YotiAttribute<string>(
                name: "nonMatchingName",
                value: "attributeValue",
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(matchingName1);
            userProfile.Add(matchingName2);
            userProfile.Add(nonMatchingName);

            ReadOnlyCollection<YotiAttribute<string>> matchingAttributes = userProfile.GetAttributesByName<string>(commonAttributeName);

            Assert.AreEqual(2, matchingAttributes.Count);
            Assert.AreEqual(3, userProfile.AttributeCollection.Count);
#pragma warning disable CS0618 // Type or member is obsolete
            Assert.AreEqual(2, userProfile.Attributes.Count);
#pragma warning restore CS0618 // Type or member is obsolete

            Assert.AreEqual(attributeValue1, matchingAttributes[0].GetValue());
            Assert.AreEqual(attributeValue2, matchingAttributes[1].GetValue());
        }
    }
}