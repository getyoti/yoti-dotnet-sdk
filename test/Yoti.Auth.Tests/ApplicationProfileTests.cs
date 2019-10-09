using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Attribute;
using Yoti.Auth.Images;
using Yoti.Auth.Profile;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ApplicationProfileTests
    {
        private readonly string _value = "value";

        [TestMethod]
        public void ApplicationProfile_NameAttribute()
        {
            var initialAttribute = new YotiAttribute<string>(
              name: Constants.ApplicationProfile.ApplicationNameAttribute,
              value: _value,
              anchors: null);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> applicationNameAttribute = applicationProfile.Name;

            Assert.AreSame(initialAttribute, applicationNameAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_URLAttribute()
        {
            var initialAttribute = new YotiAttribute<string>(
                name: Constants.ApplicationProfile.ApplicationURLAttribute,
                value: _value,
                anchors: null);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> applicationURLAttribute = applicationProfile.URL;

            Assert.AreSame(initialAttribute, applicationURLAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_LogoAttribute()
        {
            var initialAttribute = new YotiAttribute<Image>(
              name: Constants.ApplicationProfile.ApplicationLogoAttribute,
              value: new JpegImage(Encoding.UTF8.GetBytes(_value)),
              anchors: null);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<Image> applicationLogoAttribute = applicationProfile.Logo;

            Assert.AreSame(initialAttribute, applicationLogoAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_ReceiptBgColorAttribute()
        {
            var initialAttribute = new YotiAttribute<string>(
                name: Constants.ApplicationProfile.ApplicationReceiptBgColorAttribute,
                value: _value,
                anchors: null);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> applicationReceiptBgColorAttribute = applicationProfile.ReceiptBackgroundColor;

            Assert.AreSame(initialAttribute, applicationReceiptBgColorAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_GetAttribute_String()
        {
            var initialAttribute = new YotiAttribute<string>(
                name: Constants.ApplicationProfile.ApplicationNameAttribute,
                value: _value,
                anchors: null);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<string> applicationNameAttribute = applicationProfile.GetAttributeByName<string>(Constants.ApplicationProfile.ApplicationNameAttribute);

            Assert.AreSame(initialAttribute, applicationNameAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_GetAttribute_Image()
        {
            var initialAttribute = new YotiAttribute<Image>(
                name: Constants.ApplicationProfile.ApplicationLogoAttribute,
                value: new JpegImage(Encoding.UTF8.GetBytes(_value)),
                anchors: null);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            YotiAttribute<Image> logoAttribute = applicationProfile.GetAttributeByName<Image>(Constants.ApplicationProfile.ApplicationLogoAttribute);

            Assert.AreSame(initialAttribute, logoAttribute);
        }

        [TestMethod]
        public void ApplicationProfile_GetAttribute_WithWrongType()
        {
            var initialAttribute = new YotiAttribute<string>(
                name: Constants.ApplicationProfile.ApplicationNameAttribute,
                value: _value,
                anchors: null);

            ApplicationProfile applicationProfile = TestTools.Profile.CreateApplicationProfileWithSingleAttribute(initialAttribute);

            Assert.ThrowsException<InvalidCastException>(() =>
            {
                applicationProfile.GetAttributeByName<Image>(Constants.ApplicationProfile.ApplicationNameAttribute);
            });
        }

        [TestMethod]
        public void ApplicationProfile_GetAttributeNotPresent()
        {
            var applicationProfile = new ApplicationProfile();

            YotiAttribute<string> notPresentAttribute = applicationProfile.GetAttributeByName<string>("notPresent");

            Assert.IsNull(notPresentAttribute);
        }
    }
}