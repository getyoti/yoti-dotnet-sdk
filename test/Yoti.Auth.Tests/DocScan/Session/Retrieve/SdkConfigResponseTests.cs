using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve
{
    [TestClass]
    public class SdkConfigResponseTests
    {
        [TestMethod]
        public void ShouldDeserializeBiometricConsentFlowEarly()
        {
            const string json = @"{
                ""biometric_consent_flow"": ""EARLY""
            }";

            var result = JsonConvert.DeserializeObject<SdkConfigResponse>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("EARLY", result.BiometricConsentFlow);
        }

        [TestMethod]
        public void ShouldDeserializeBiometricConsentFlowJustInTime()
        {
            const string json = @"{
                ""biometric_consent_flow"": ""JUST_IN_TIME""
            }";

            var result = JsonConvert.DeserializeObject<SdkConfigResponse>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("JUST_IN_TIME", result.BiometricConsentFlow);
        }

        [TestMethod]
        public void BiometricConsentFlowShouldBeNullWhenNotPresent()
        {
            const string json = @"{
                ""allowed_capture_methods"": ""CAMERA""
            }";

            var result = JsonConvert.DeserializeObject<SdkConfigResponse>(json);

            Assert.IsNotNull(result);
            Assert.IsNull(result.BiometricConsentFlow);
        }

        [TestMethod]
        public void ShouldDeserializeUnknownBiometricConsentFlowGracefully()
        {
            const string json = @"{
                ""biometric_consent_flow"": ""UNKNOWN_FUTURE_VALUE""
            }";

            var result = JsonConvert.DeserializeObject<SdkConfigResponse>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("UNKNOWN_FUTURE_VALUE", result.BiometricConsentFlow);
        }

        [TestMethod]
        public void ShouldDeserializeAllSdkConfigFields()
        {
            const string json = @"{
                ""allowed_capture_methods"": ""CAMERA_AND_UPLOAD"",
                ""primary_colour"": ""#ffffff"",
                ""secondary_colour"": ""#000000"",
                ""font_colour"": ""#ff0000"",
                ""locale"": ""en"",
                ""preset_issuing_country"": ""GBR"",
                ""success_url"": ""https://example.com/success"",
                ""error_url"": ""https://example.com/error"",
                ""privacy_policy_url"": ""https://example.com/privacy"",
                ""allow_handoff"": true,
                ""brand_id"": ""my-brand"",
                ""biometric_consent_flow"": ""EARLY""
            }";

            var result = JsonConvert.DeserializeObject<SdkConfigResponse>(json);

            Assert.IsNotNull(result);
            Assert.AreEqual("CAMERA_AND_UPLOAD", result.AllowedCaptureMethods);
            Assert.AreEqual("#ffffff", result.PrimaryColour);
            Assert.AreEqual("#000000", result.SecondaryColour);
            Assert.AreEqual("#ff0000", result.FontColour);
            Assert.AreEqual("en", result.Locale);
            Assert.AreEqual("GBR", result.PresetIssuingCountry);
            Assert.AreEqual("https://example.com/success", result.SuccessUrl);
            Assert.AreEqual("https://example.com/error", result.ErrorUrl);
            Assert.AreEqual("https://example.com/privacy", result.PrivacyPolicyUrl);
            Assert.IsTrue(result.AllowHandoff.HasValue);
            Assert.IsTrue(result.AllowHandoff.Value);
            Assert.AreEqual("my-brand", result.BrandId);
            Assert.AreEqual("EARLY", result.BiometricConsentFlow);
        }
    }
}
