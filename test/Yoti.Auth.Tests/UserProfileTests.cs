using System;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class UserProfileTests
    {
        [TestMethod]
        public void UserProfile_GetAttribute_Datetime()
        {
            string value = "1980-01-13";
            var attributeValue = new YotiAttributeValue(TypeEnum.Date, Encoding.UTF8.GetBytes(value));
            var initialAttribute = new YotiAttribute<DateTime>(Constants.UserProfile.DateOfBirthAttribute, attributeValue);

            YotiProfile userProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<DateTime> dobAttribute = userProfile.GetAttributeByName<DateTime>(Constants.UserProfile.DateOfBirthAttribute);

            Assert.AreSame(initialAttribute, dobAttribute);
            DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob);
            Assert.AreEqual(dob, dobAttribute.GetValue());
        }

        [TestMethod]
        public void UserProfile_GetAttribute_Datetime_InvalidFormat()
        {
            string value = "1980/01/13";

            var attributeValue = new YotiAttributeValue(TypeEnum.Date, Encoding.UTF8.GetBytes(value));
            var initialAttribute = new YotiAttribute<DateTime>(Constants.UserProfile.DateOfBirthAttribute, attributeValue);

            YotiProfile userProfile = TestTools.Profile.CreateUserProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<DateTime> dobAttribute = userProfile.GetAttributeByName<DateTime>(Constants.UserProfile.DateOfBirthAttribute);

            Assert.ThrowsException<InvalidCastException>(() =>
            {
                dobAttribute.GetValue();
            });
        }

        [TestMethod]
        public void UserProfile_AddAttribute()
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Jpeg, Encoding.UTF8.GetBytes("Nation"));
            var initialAttribute = new YotiAttribute<Image>(Constants.UserProfile.NationalityAttribute, attributeValue);

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<Image> nationalityAttribute = userProfile.GetAttributeByName<Image>(Constants.UserProfile.NationalityAttribute);

            Assert.AreSame(initialAttribute, nationalityAttribute);
        }
    }
}