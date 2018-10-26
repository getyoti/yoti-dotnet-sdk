using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var attributeValue = new YotiAttributeValue(TypeEnum.Text, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<string>(YotiConstants.ApplicationNameAttribute, attributeValue);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> applicationNameAttribute = applicationProfile.Name;

            Assert.AreSame(initialAttribute, applicationNameAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_URLAttribute()
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Text, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<string>(YotiConstants.ApplicationURLAttribute, attributeValue);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> applicationURLAttribute = applicationProfile.URL;

            Assert.AreSame(initialAttribute, applicationURLAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_LogoAttribute()
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Png, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<Image>(YotiConstants.ApplicationLogoAttribute, attributeValue);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<Image> applicationLogoAttribute = applicationProfile.Logo;

            Assert.AreSame(initialAttribute, applicationLogoAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_ReceiptBgColourAttribute()
        {
            var attributeValue = new YotiAttributeValue(TypeEnum.Text, Encoding.UTF8.GetBytes(_value));
            var initialAttribute = new YotiAttribute<string>(YotiConstants.ApplicationReceiptBgColourAttribute, attributeValue);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> applicationReceiptBgColourAttribute = applicationProfile.ReceiptBackgroundColour;

            Assert.AreSame(initialAttribute, applicationReceiptBgColourAttribute);
        }
    }
}