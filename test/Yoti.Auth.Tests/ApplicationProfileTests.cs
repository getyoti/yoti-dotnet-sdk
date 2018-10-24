using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Tests.TestTools;
using static Yoti.Auth.YotiAttributeValue;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ApplicationProfileTests
    {
        private string _value = "value";

        [TestMethod]
        public void ApplicationProfile_NameAttribute()
        {
            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(
                Encoding.UTF8.GetBytes(_value),
                YotiConstants.ApplicationNameAttribute,
                TypeEnum.Text);

            YotiAttribute<string> applicationNameAttribute = applicationProfile.Name;

            Assert.AreEqual(YotiConstants.ApplicationNameAttribute, applicationNameAttribute.GetName());
            Assert.AreEqual(_value, applicationNameAttribute.GetValue());
        }

        [TestMethod]
        public void ApplicationProfile_URLAttribute()
        {
            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(
                Encoding.UTF8.GetBytes(_value),
                YotiConstants.ApplicationURLAttribute,
                TypeEnum.Text);

            YotiAttribute<string> applicationURLAttribute = applicationProfile.URL;

            Assert.AreEqual(YotiConstants.ApplicationURLAttribute, applicationURLAttribute.GetName());
            Assert.AreEqual(_value, applicationURLAttribute.GetValue());
        }

        [TestMethod]
        public void ApplicationProfile_LogoAttribute()
        {
            var yotiAttributeValue = new YotiAttributeValue(
                TypeEnum.Png,
                 Encoding.UTF8.GetBytes(_value));
            var yotiAttribute = new YotiAttribute<object>(YotiConstants.ApplicationLogoAttribute, yotiAttributeValue);

            var attributes = new Dictionary<string, YotiAttribute<object>>
            {
                { YotiConstants.ApplicationLogoAttribute, yotiAttribute }
            };

            var applicationProfile = new ApplicationProfile(attributes);

            YotiAttribute<Image> applicationLogoAttribute = applicationProfile.Logo;

            Assert.AreEqual(YotiConstants.ApplicationLogoAttribute, applicationLogoAttribute.GetName());
            Assert.IsTrue(new ImageComparer().Equals(yotiAttributeValue.ToImage(), applicationLogoAttribute.GetValue()));
        }

        [TestMethod]
        public void ApplicationProfile_ReceiptBgColourAttribute()
        {
            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(
                Encoding.UTF8.GetBytes(_value),
                YotiConstants.ApplicationReceiptBgColourAttribute,
                TypeEnum.Text);

            YotiAttribute<string> applicationReceiptBgColourAttribute = applicationProfile.ReceiptBackgroundColour;

            Assert.AreEqual(YotiConstants.ApplicationReceiptBgColourAttribute, applicationReceiptBgColourAttribute.GetName());
            Assert.AreEqual(_value, applicationReceiptBgColourAttribute.GetValue());
        }
    }
}