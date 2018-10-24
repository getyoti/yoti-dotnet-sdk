using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Tests.TestTools;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ProfileTests
    {
        [TestMethod]
        public void ProfileGetAttribute()
        {
            string value = "value";

            ApplicationProfile profile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(
                Encoding.UTF8.GetBytes(value),
                YotiConstants.PhoneNumberAttribute,
                TypeEnum.Text);

            YotiAttribute<string> phoneNumberAttribute = profile.GetAttributeByName<string>(YotiConstants.PhoneNumberAttribute);

            Assert.AreEqual(YotiConstants.PhoneNumberAttribute, phoneNumberAttribute.GetName());
            Assert.AreEqual(value, phoneNumberAttribute.GetValue());
        }

        [TestMethod]
        public void ProfileGetAttribute_Datetime()
        {
            string value = "1980-01-13";
            ApplicationProfile profile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(
                Encoding.UTF8.GetBytes(value),
                YotiConstants.DateOfBirthAttribute,
                TypeEnum.Date);

            YotiAttribute<DateTime> dobAttribute = profile.GetAttributeByName<DateTime>(YotiConstants.DateOfBirthAttribute);

            Assert.AreEqual(YotiConstants.DateOfBirthAttribute, dobAttribute.GetName());

            DateTime.TryParseExact(value, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dob);
            Assert.AreEqual(dob, dobAttribute.GetValue());
        }

        [TestMethod]
        public void ProfileGetAttribute_Datetime_InvalidFormat()
        {
            string value = "1980/01/13";

            ApplicationProfile profile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(
               Encoding.UTF8.GetBytes(value),
               YotiConstants.DateOfBirthAttribute,
               TypeEnum.Date);

            YotiAttribute<DateTime> dobAttribute = profile.GetAttributeByName<DateTime>(YotiConstants.DateOfBirthAttribute);

            Assert.AreEqual(YotiConstants.DateOfBirthAttribute, dobAttribute.GetName());

            Assert.ThrowsException<InvalidCastException>(() =>
            {
                dobAttribute.GetValue();
            });
        }

        [TestMethod]
        public void ProfileGetAttribute_Image()
        {
            string value = "selfie0123456789";

            var yotiAttributeValue = new YotiAttributeValue(
                TypeEnum.Jpeg,
                 Encoding.UTF8.GetBytes(value));
            var yotiAttribute = new YotiAttribute<object>(YotiConstants.SelfieAttribute, yotiAttributeValue);

            var attributes = new Dictionary<string, YotiAttribute<object>>
            {
                { YotiConstants.SelfieAttribute, yotiAttribute }
            };

            var profile = new ApplicationProfile(attributes);

            YotiAttribute<Image> selfieAttribute = profile.GetAttributeByName<Image>(YotiConstants.SelfieAttribute);

            Assert.AreEqual(YotiConstants.SelfieAttribute, selfieAttribute.GetName());
            Assert.IsTrue(new ImageComparer().Equals(yotiAttributeValue.ToImage(), selfieAttribute.GetValue()));
        }

        [TestMethod]
        public void ProfileGetAttributeNotPresent()
        {
            var applicationProfile = new ApplicationProfile();

            YotiAttribute<string> notPresentAttribute = applicationProfile.GetAttributeByName<string>("notPresent");

            Assert.IsNull(notPresentAttribute);
        }
    }
}