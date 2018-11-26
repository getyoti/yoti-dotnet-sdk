using System;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class UserProfileTests
    {
        [TestMethod]
        public void UserProfile_GetAttribute_Datetime()
        {
            string value = "1980-01-13";
            var initialAttribute = new YotiAttribute<DateTime>(
                Constants.UserProfile.DateOfBirthAttribute,
                AttrpubapiV1.ContentType.Date,
                Encoding.UTF8.GetBytes(value));

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
            var initialAttribute = new YotiAttribute<DateTime>(
                Constants.UserProfile.DateOfBirthAttribute,
                AttrpubapiV1.ContentType.Date,
                Encoding.UTF8.GetBytes(value));

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
            var initialAttribute = new YotiAttribute<string>(
                Constants.UserProfile.NationalityAttribute,
                AttrpubapiV1.ContentType.String,
                Encoding.UTF8.GetBytes("Nation"));

            YotiProfile userProfile = new YotiProfile();
            userProfile.Add(initialAttribute);

            YotiAttribute<string> nationalityAttribute = userProfile.GetAttributeByName<string>(Constants.UserProfile.NationalityAttribute);

            Assert.AreSame(initialAttribute, nationalityAttribute);
        }
    }
}