using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration;

namespace Yoti.Auth.Tests.DocScan.Session.Retrieve
{
    [TestClass]
    public class SessionConfigurationResponseTests
    {
        [TestMethod]
        public void ShouldDeserializeSdkConfigWithBiometricConsentFlow()
        {
            const string json = @"{
                ""session_id"": ""some-session-id"",
                ""client_session_token_ttl"": 600,
                ""sdk_config"": {
                    ""biometric_consent_flow"": ""EARLY""
                }
            }";

            var result = JsonConvert.DeserializeObject<SessionConfigurationResponse>(json);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.SdkConfig);
            Assert.AreEqual("EARLY", result.SdkConfig.BiometricConsentFlow);
        }

        [TestMethod]
        public void SdkConfigShouldBeNullWhenNotPresent()
        {
            const string json = @"{
                ""session_id"": ""some-session-id"",
                ""client_session_token_ttl"": 600
            }";

            var result = JsonConvert.DeserializeObject<SessionConfigurationResponse>(json);

            Assert.IsNotNull(result);
            Assert.IsNull(result.SdkConfig);
        }

        [TestMethod]
        public void BiometricConsentFlowShouldBeNullWhenSdkConfigDoesNotContainIt()
        {
            const string json = @"{
                ""session_id"": ""some-session-id"",
                ""client_session_token_ttl"": 600,
                ""sdk_config"": {
                    ""allowed_capture_methods"": ""CAMERA""
                }
            }";

            var result = JsonConvert.DeserializeObject<SessionConfigurationResponse>(json);

            Assert.IsNotNull(result);
            Assert.IsNotNull(result.SdkConfig);
            Assert.IsNull(result.SdkConfig.BiometricConsentFlow);
        }
    }
}
