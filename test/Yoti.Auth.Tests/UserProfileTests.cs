using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Images;

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
        public void GetAttributeByName_WithoutSpecifyingType_String()
        {
            var initialAttribute = new YotiAttribute<object>(
                name: Constants.UserProfile.NationalityAttribute,
                value: "Nation",
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<object> nationalityAttribute = userProfile.GetAttributeByName(Constants.UserProfile.NationalityAttribute);

            Assert.IsInstanceOfType(nationalityAttribute.GetValue(), typeof(object));
            Assert.AreSame(initialAttribute.GetValue(), nationalityAttribute.GetValue().ToString());
        }

        [TestMethod]
        public void GetAttributeByName_WithoutSpecifyingType_DateTime()
        {
            var initialAttribute = new YotiAttribute<object>(
                name: Constants.UserProfile.DateOfBirthAttribute,
                value: new DateTime(2000, 2, 1),
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<object> dateOfBirthAttribute = userProfile.GetAttributeByName(Constants.UserProfile.DateOfBirthAttribute);

            DateTime.TryParse(dateOfBirthAttribute.GetValue().ToString(), out DateTime result);

            Assert.AreEqual(initialAttribute.GetValue(), result);
        }

        [TestMethod]
        public void GetAttributeByName_WithoutSpecifyingType_Bool()
        {
            bool boolValue = true;
            string attributeName = "Bool";

            var initialAttribute = new YotiAttribute<object>(
                name: attributeName,
                value: boolValue,
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<object> boolAttribute = userProfile.GetAttributeByName(attributeName);

            bool.TryParse(boolAttribute.GetValue().ToString(), out bool result);

            Assert.AreEqual(boolValue, result);
        }

        [TestMethod]
        public void GetAttributeByName_WithoutSpecifyingType_Image()
        {
            Image imageValue = new PngImage(Encoding.UTF8.GetBytes("Value"));

            var initialAttribute = new YotiAttribute<object>(
                name: Constants.UserProfile.SelfieAttribute,
                value: imageValue,
                anchors: null);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<object> boolAttribute = userProfile.GetAttributeByName(Constants.UserProfile.SelfieAttribute);

            Assert.AreEqual(imageValue, (Image)boolAttribute.GetValue());
        }
    }
}