using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public void ShouldBuildWithEnforceHandoffTrue()
        {
            SdkConfigBuilder builder = new SdkConfigBuilder();
            SdkConfigBuilder returned = builder.WithEnforceHandoff(true);

            SdkConfig sdkConfig = returned.Build();

            Assert.AreSame(builder, returned);
            Assert.IsTrue(sdkConfig.EnforceHandoff.HasValue);
            Assert.IsTrue(sdkConfig.EnforceHandoff.Value);

            string json = JsonConvert.SerializeObject(
                sdkConfig,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            StringAssert.Contains(json, "\"enforce_handoff\":true");
        }

        [TestMethod]
        public void ShouldBuildWithEnforceHandoffFalse()
        {
            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .WithEnforceHandoff(false)
             .Build();

            Assert.IsTrue(sdkConfig.EnforceHandoff.HasValue);
            Assert.IsFalse(sdkConfig.EnforceHandoff.Value);

            string json = JsonConvert.SerializeObject(
                sdkConfig,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            StringAssert.Contains(json, "\"enforce_handoff\":false");
        }

        [TestMethod]
        public void EnforceHandoffShouldBeNullIfNotSet()
        {
            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .Build();

            Assert.IsNull(sdkConfig.EnforceHandoff);
        }

        [TestMethod]
        public void EnforceHandoffShouldBeOmittedFromJsonWhenNotSet()
        {
            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .Build();

            string json = JsonConvert.SerializeObject(
                sdkConfig,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

            Assert.IsFalse(json.Contains("enforce_handoff"));
        }

        [TestMethod]
        public void ShouldBuildWithBothAllowHandoffAndEnforceHandoff()
        {
            SdkConfig sdkConfig =
             new SdkConfigBuilder()
             .WithAllowHandoff(true)
             .WithEnforceHandoff(true)
             .Build();

            Assert.IsTrue(sdkConfig.AllowHandoff.HasValue);
            Assert.IsTrue(sdkConfig.AllowHandoff.Value);
            Assert.IsTrue(sdkConfig.EnforceHandoff.HasValue);
            Assert.IsTrue(sdkConfig.EnforceHandoff.Value);

            string json = JsonConvert.SerializeObject(
                sdkConfig,
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            StringAssert.Contains(json, "\"allow_handoff\":true");
            StringAssert.Contains(json, "\"enforce_handoff\":true");
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
        public void ShouldBuildWithBrandId()
        {
            string brandid = "some_brand_id";

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                    .WithBrandId(brandid)
                    .Build();

            Assert.AreEqual(brandid, sdkConfig.BrandId);
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
        public void SuppressedScreensShouldBeNullIfNotSet()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .Build();

            Assert.IsNull(sdkConfig.SuppressedScreens);
        }

        [TestMethod]
        public void ShouldBuildWithSuppressedScreens()
        {
            var screens = new List<string>
            {
                SuppressedScreen.IdDocumentEducation,
                SuppressedScreen.FlowCompletion,
            };

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(screens)
                .Build();

            CollectionAssert.AreEqual(screens, sdkConfig.SuppressedScreens);
        }

        [TestMethod]
        public void ShouldBuildWithEmptySuppressedScreensList()
        {
            var screens = new List<string>();

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(screens)
                .Build();

            Assert.IsNotNull(sdkConfig.SuppressedScreens);
            Assert.AreEqual(0, sdkConfig.SuppressedScreens.Count);
        }

        [TestMethod]
        public void WithSuppressedScreensNullShouldResetField()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(new List<string> { SuppressedScreen.IdDocumentEducation })
                .WithSuppressedScreens(null)
                .Build();

            Assert.IsNull(sdkConfig.SuppressedScreens);
        }

        [TestMethod]
        public void ShouldBuildWithSuppressedScreenIndividually()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreen(SuppressedScreen.IdDocumentEducation)
                .WithSuppressedScreen(SuppressedScreen.ZoomLivenessEducation)
                .Build();

            Assert.AreEqual(2, sdkConfig.SuppressedScreens.Count);
            CollectionAssert.Contains(sdkConfig.SuppressedScreens, SuppressedScreen.IdDocumentEducation);
            CollectionAssert.Contains(sdkConfig.SuppressedScreens, SuppressedScreen.ZoomLivenessEducation);
        }

        [TestMethod]
        public void SuppressedScreensConstantsShouldHaveExpectedValues()
        {
            Assert.AreEqual("ID_DOCUMENT_EDUCATION", SuppressedScreen.IdDocumentEducation);
            Assert.AreEqual("ID_DOCUMENT_REQUIREMENTS", SuppressedScreen.IdDocumentRequirements);
            Assert.AreEqual("SUPPLEMENTARY_DOCUMENT_EDUCATION", SuppressedScreen.SupplementaryDocumentEducation);
            Assert.AreEqual("ZOOM_LIVENESS_EDUCATION", SuppressedScreen.ZoomLivenessEducation);
            Assert.AreEqual("STATIC_LIVENESS_EDUCATION", SuppressedScreen.StaticLivenessEducation);
            Assert.AreEqual("FACE_CAPTURE_EDUCATION", SuppressedScreen.FaceCaptureEducation);
            Assert.AreEqual("FLOW_COMPLETION", SuppressedScreen.FlowCompletion);
        }

        [TestMethod]
        public void IsScreenSuppressedShouldReturnTrueForListedScreen()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(new List<string> { SuppressedScreen.IdDocumentEducation })
                .Build();

            Assert.IsTrue(sdkConfig.IsScreenSuppressed(SuppressedScreen.IdDocumentEducation));
        }

        [TestMethod]
        public void IsScreenSuppressedShouldReturnFalseForUnlistedScreen()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(new List<string> { SuppressedScreen.IdDocumentEducation })
                .Build();

            Assert.IsFalse(sdkConfig.IsScreenSuppressed(SuppressedScreen.FlowCompletion));
        }

        [TestMethod]
        public void IsScreenSuppressedShouldReturnFalseWhenSuppressedScreensIsNull()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .Build();

            Assert.IsFalse(sdkConfig.IsScreenSuppressed(SuppressedScreen.IdDocumentEducation));
        }

        [TestMethod]
        public void IsScreenSuppressedShouldBeCaseSensitive()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(new List<string> { SuppressedScreen.IdDocumentEducation })
                .Build();

            Assert.IsFalse(sdkConfig.IsScreenSuppressed("id_document_education"));
        }

        [TestMethod]
        public void SuppressedScreensShouldSerializeToJsonArray()
        {
            var screens = new List<string>
            {
                SuppressedScreen.IdDocumentEducation,
                SuppressedScreen.FlowCompletion,
            };

            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .WithSuppressedScreens(screens)
                .Build();

            string serialized = JsonConvert.SerializeObject(sdkConfig);
            JObject parsed = JObject.Parse(serialized);

            Assert.IsTrue(parsed.ContainsKey("suppressed_screens"));
            JArray array = (JArray)parsed["suppressed_screens"];
            Assert.AreEqual(2, array.Count);
            Assert.AreEqual("ID_DOCUMENT_EDUCATION", array[0].ToString());
            Assert.AreEqual("FLOW_COMPLETION", array[1].ToString());
        }

        [TestMethod]
        public void SuppressedScreensShouldBeOmittedFromJsonWhenNull()
        {
            SdkConfig sdkConfig =
                new SdkConfigBuilder()
                .Build();

            string serialized = JsonConvert.SerializeObject(sdkConfig);
            JObject parsed = JObject.Parse(serialized);

            Assert.IsFalse(parsed.ContainsKey("suppressed_screens"));
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
