using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.Constants;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class SdkConfigBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithAllowsCamera()
        {
            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithAllowsCamera()
              .Build();

            Assert.AreEqual("CAMERA", sdkConfig.AllowedCaptureMethods);
        }

        [TestMethod]
        public void ShouldBuildWithAllowsCameraAndUpload()
        {
            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithAllowsCameraAndUpload()
              .Build();

            Assert.AreEqual("CAMERA_AND_UPLOAD", sdkConfig.AllowedCaptureMethods);
        }

        [TestMethod]
        public void ShouldRetainLatestAllowedMethod()
        {
            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithAllowsCamera()
              .WithAllowsCameraAndUpload()
              .Build();

            Assert.AreEqual("CAMERA_AND_UPLOAD", sdkConfig.AllowedCaptureMethods);
        }

        [TestMethod]
        public void ShouldBuildWithPrimaryColour()
        {
            string colour = "#ffffff";

            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithPrimaryColour(colour)
              .Build();

            Assert.AreEqual(colour, sdkConfig.PrimaryColour);
        }

        [TestMethod]
        public void ShouldBuildWithSecondaryColour()
        {
            string colour = "#000000";

            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithSecondaryColour(colour)
              .Build();

            Assert.AreEqual(colour, sdkConfig.SecondaryColour);
        }

        [TestMethod]
        public void ShouldBuildWithFontColour()
        {
            string fontColour = "#2d9fff";

            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithFontColour(fontColour)
              .Build();

            Assert.AreEqual(fontColour, sdkConfig.FontColour);
        }

        [TestMethod]
        public void ShouldBuildWithLocale()
        {
            string locale = "en";

            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithLocale(locale)
              .Build();

            Assert.AreEqual(locale, sdkConfig.Locale);
        }

        [TestMethod]
        public void ShouldBuildWithPresetIssuingCountry()
        {
            string country = "USA";

            SdkConfig sdkConfig =
              new SdkConfigBuilder()
              .WithPresetIssuingCountry(country)
              .Build();

            Assert.AreEqual(country, sdkConfig.PresetIssuingCountry);
        }

        [TestMethod]
        public void ShouldBuildWithSuccessUrl()
        {
            string success = "https://yourdomain.com/some/success/endpoint";

            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .WithSuccessUrl(success)
             .Build();

            Assert.AreEqual(success, sdkConfig.SuccessUrl);
        }

        [TestMethod]
        public void ShouldBuildWithPrivacyPolicyUrl()
        {
            string privacyPolicyUrl = "https://yourdomain.com/some/privacy-policy";

            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .WithPrivacyPolicyUrl(privacyPolicyUrl)
             .Build();

            Assert.AreEqual(privacyPolicyUrl, sdkConfig.PrivacyPolicyUrl);
        }

        [TestMethod]
        public void ShouldBuildWithErrorUrl()
        {
            string error = "https://yourdomain.com/some/failure/endpoint";

            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .WithErrorUrl(error)
             .Build();

            Assert.AreEqual(error, sdkConfig.ErrorUrl);
        }

        [TestMethod]
        public void ShouldBuildWithMobileHandoff()
        {
            bool mobileHandoff = true;

            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .WithAllowHandoff(mobileHandoff)
             .Build();

            Assert.AreEqual(mobileHandoff, sdkConfig.AllowHandoff);
        }

        [TestMethod]
        public void MobileHandoffShouldBeNullIfNotSet()
        {
            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .Build();

            Assert.IsNull(sdkConfig.AllowHandoff);
        }

        [TestMethod]
        public void ShouldBuildWithIdDocumentTextExtractionCategoryAttempts()
        {
            string category = "someCategory";
            int attempts = 2;
            var kvp = new KeyValuePair<string, int>(category, attempts);

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithIdDocumentTextExtractionCategoryAttempts(category, attempts)
                .Build();

            CollectionAssert.Contains(sdkConfig.AttemptsConfiguration.IdDocumentTextDataExtraction, kvp);
        }

        [TestMethod]
        public void AttemptsConfigurationShouldBeNullIfNotSet()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .Build();

            Assert.IsNull(sdkConfig.AttemptsConfiguration);
        }

        [TestMethod]
        public void AttemptsConfigurationShouldResetSameValueWithRepeatedCalls()
        {
            var kvp = new KeyValuePair<string, int>(DocScanConstants.Reclassification, 4);

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithIdDocumentTextExtractionReclassificationAttempts(2)
                .WithIdDocumentTextExtractionReclassificationAttempts(3)
                .WithIdDocumentTextExtractionReclassificationAttempts(4)
                .Build();

            Assert.AreEqual(1, sdkConfig.AttemptsConfiguration.IdDocumentTextDataExtraction.Count);
            CollectionAssert.Contains(sdkConfig.AttemptsConfiguration.IdDocumentTextDataExtraction, kvp);
        }

        [TestMethod]
        public void ShouldBuildWithSuppressedScreens()
        {
            var screens = new List<string>
            {
                DocScanConstants.IdDocumentEducation,
                DocScanConstants.FlowCompletion
            };

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(screens)
                .Build();

            CollectionAssert.AreEqual(screens, sdkConfig.SuppressedScreens);
        }

        [TestMethod]
        public void ShouldBuildWithSuppressedScreenSingle()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreen(DocScanConstants.ZoomLivenessEducation)
                .WithSuppressedScreen(DocScanConstants.FaceCaptureEducation)
                .Build();

            Assert.AreEqual(2, sdkConfig.SuppressedScreens.Count);
            CollectionAssert.Contains(sdkConfig.SuppressedScreens, DocScanConstants.ZoomLivenessEducation);
            CollectionAssert.Contains(sdkConfig.SuppressedScreens, DocScanConstants.FaceCaptureEducation);
        }

        [TestMethod]
        public void SuppressedScreensShouldBeNullIfNotSet()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .Build();

            Assert.IsNull(sdkConfig.SuppressedScreens);
        }

        [TestMethod]
        public void SuppressedScreensShouldBeOmittedFromJsonWhenEmpty()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(new List<string>())
                .Build();

            string json = JsonConvert.SerializeObject(sdkConfig);

            Assert.IsFalse(json.Contains("suppressed_screens"));
        }

        [TestMethod]
        public void SuppressedScreensShouldBeSerializedWhenSet()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreen(DocScanConstants.IdDocumentRequirements)
                .Build();

            string json = JsonConvert.SerializeObject(sdkConfig);

            Assert.IsTrue(json.Contains("\"suppressed_screens\":[\"ID_DOCUMENT_REQUIREMENTS\"]"));
        }

        [TestMethod]
        public void SuppressedScreensShouldBeOmittedFromJsonWhenNotSet()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .Build();

            string json = JsonConvert.SerializeObject(sdkConfig);

            Assert.IsFalse(json.Contains("suppressed_screens"));
        }

        [TestMethod]
        public void SuppressedScreensShouldDeserializeFromJson()
        {
            string json = "{\"suppressed_screens\":[\"ID_DOCUMENT_EDUCATION\",\"FLOW_COMPLETION\"]}";

            SdkConfig sdkConfig = JsonConvert.DeserializeObject<SdkConfig>(json);

            Assert.IsNotNull(sdkConfig.SuppressedScreens);
            Assert.AreEqual(2, sdkConfig.SuppressedScreens.Count);
            CollectionAssert.Contains(sdkConfig.SuppressedScreens, DocScanConstants.IdDocumentEducation);
            CollectionAssert.Contains(sdkConfig.SuppressedScreens, DocScanConstants.FlowCompletion);
        }

        [TestMethod]
        public void SuppressedScreensShouldBeNullWhenMissingFromJson()
        {
            string json = "{}";

            SdkConfig sdkConfig = JsonConvert.DeserializeObject<SdkConfig>(json);

            Assert.IsNull(sdkConfig.SuppressedScreens);
        }

        [TestMethod]
        public void SuppressedScreensShouldHandleEmptyArrayFromJson()
        {
            string json = "{\"suppressed_screens\":[]}";

            SdkConfig sdkConfig = JsonConvert.DeserializeObject<SdkConfig>(json);

            Assert.IsNotNull(sdkConfig.SuppressedScreens);
            Assert.AreEqual(0, sdkConfig.SuppressedScreens.Count);
        }

        [TestMethod]
        public void SuppressedScreensShouldHandleUnknownValueFromJson()
        {
            string json = "{\"suppressed_screens\":[\"ID_DOCUMENT_EDUCATION\",\"SOMETHING_NEW\"]}";

            SdkConfig sdkConfig = JsonConvert.DeserializeObject<SdkConfig>(json);

            Assert.IsNotNull(sdkConfig.SuppressedScreens);
            Assert.AreEqual(2, sdkConfig.SuppressedScreens.Count);
            CollectionAssert.Contains(sdkConfig.SuppressedScreens, "SOMETHING_NEW");
        }

        [TestMethod]
        public void ShouldReplaceSuppressedScreensWithNullWhenPassed()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreen(DocScanConstants.IdDocumentEducation)
                .WithSuppressedScreens(null)
                .Build();

            Assert.IsNull(sdkConfig.SuppressedScreens);
        }

        [TestMethod]
        public void AttemptsConfigurationShouldAllowMultipleCategories()
        {
            var kvpReclassificationAttempts = new KeyValuePair<string, int>(DocScanConstants.Reclassification, 1);
            string category = "someCategory";
            int attempts = 2;
            var kvpUsersChoiceOfCategory = new KeyValuePair<string, int>(category, attempts);
            var kvpGenericAttempts = new KeyValuePair<string, int>(DocScanConstants.Generic, 3);

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithIdDocumentTextExtractionReclassificationAttempts(1)
                .WithIdDocumentTextExtractionCategoryAttempts(category, attempts)
                .WithIdDocumentTextExtractionGenericAttempts(3)
                .Build();

            Assert.AreEqual(3, sdkConfig.AttemptsConfiguration.IdDocumentTextDataExtraction.Count);
            CollectionAssert.Contains(sdkConfig.AttemptsConfiguration.IdDocumentTextDataExtraction, kvpReclassificationAttempts);
            CollectionAssert.Contains(sdkConfig.AttemptsConfiguration.IdDocumentTextDataExtraction, kvpUsersChoiceOfCategory);
            CollectionAssert.Contains(sdkConfig.AttemptsConfiguration.IdDocumentTextDataExtraction, kvpGenericAttempts);
        }
    }
}