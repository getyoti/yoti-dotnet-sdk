using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class UserProfileTests
    {
        [TestMethod]
        public void UserProfile_GetAttribute_Datetime()
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
        public void UserProfile_AddAttribute()
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
    }
}