using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Images;
using Yoti.Auth.Profile;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class UserProfileTests
    {
        [TestMethod]
        public void GetAttributeByName_Datetime()
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
        public void GetAttributeByName_String()
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
        public void GetAttributeByName_Bool()
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
        public void GetAttributeByName_Image()
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
    }
}