using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration
{
    /// <summary>
    /// Represents the SDK configuration returned when fetching session configuration.
    /// </summary>
    public class SdkConfigResponse
    {
        /// <summary>
        /// The allowed capture methods configured for the session.
        /// </summary>
        [JsonProperty(PropertyName = "allowed_capture_methods")]
        public string AllowedCaptureMethods { get; private set; }

        /// <summary>
        /// The primary colour configured for the web/native client.
        /// </summary>
        [JsonProperty(PropertyName = "primary_colour")]
        public string PrimaryColour { get; private set; }

        /// <summary>
        /// The secondary colour configured for the web/native client.
        /// </summary>
        [JsonProperty(PropertyName = "secondary_colour")]
        public string SecondaryColour { get; private set; }

        /// <summary>
        /// The font colour configured for the web/native client.
        /// </summary>
        [JsonProperty(PropertyName = "font_colour")]
        public string FontColour { get; private set; }

        /// <summary>
        /// The language locale configured for the web/native client.
        /// </summary>
        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; private set; }

        /// <summary>
        /// The preset issuing country configured for the session.
        /// </summary>
        [JsonProperty(PropertyName = "preset_issuing_country")]
        public string PresetIssuingCountry { get; private set; }

        /// <summary>
        /// The success URL configured for the session.
        /// </summary>
        [JsonProperty(PropertyName = "success_url")]
        public string SuccessUrl { get; private set; }

        /// <summary>
        /// The error URL configured for the session.
        /// </summary>
        [JsonProperty(PropertyName = "error_url")]
        public string ErrorUrl { get; private set; }

        /// <summary>
        /// The privacy policy URL configured for the session.
        /// </summary>
        [JsonProperty(PropertyName = "privacy_policy_url")]
        public string PrivacyPolicyUrl { get; private set; }

        /// <summary>
        /// Whether mobile handoff is allowed for the session.
        /// </summary>
        [JsonProperty(PropertyName = "allow_handoff")]
        public bool? AllowHandoff { get; private set; }

        /// <summary>
        /// The brand identifier configured for the session.
        /// </summary>
        [JsonProperty(PropertyName = "brand_id")]
        public string BrandId { get; private set; }

        /// <summary>
        /// The biometric consent flow configured for the session.
        /// Controls where the biometric consent screen is shown.
        /// Accepted values are defined in <see cref="Yoti.Auth.DocScan.Session.Create.BiometricConsentFlow"/>.
        /// </summary>
        [JsonProperty(PropertyName = "biometric_consent_flow")]
        public string BiometricConsentFlow { get; private set; }
    }
}
