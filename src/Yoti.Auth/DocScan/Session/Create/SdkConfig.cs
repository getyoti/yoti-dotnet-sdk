using Newtonsoft.Json;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// SdkConfig provides configuration properties for the the web/native clients
    /// </summary>
    public class SdkConfig
    {
        [JsonProperty(PropertyName = "allowed_capture_methods")]
        public string AllowedCaptureMethods { get; }

        [JsonProperty(PropertyName = "primary_colour")]
        public string PrimaryColour { get; }

        [JsonProperty(PropertyName = "secondary_colour")]
        public string SecondaryColour { get; }

        [JsonProperty(PropertyName = "font_colour")]
        public string FontColour { get; }

        [JsonProperty(PropertyName = "locale")]
        public string Locale { get; }

        [JsonProperty(PropertyName = "preset_issuing_country")]
        public string PresetIssuingCountry { get; }

        [JsonProperty(PropertyName = "success_url")]
        public string SuccessUrl { get; }

        [JsonProperty(PropertyName = "error_url")]
        public string ErrorUrl { get; }

        [JsonProperty(PropertyName = "privacy_policy_url")]
        public string PrivacyPolicyUrl { get; }

        [JsonProperty(PropertyName = "allow_handoff")]
        public bool? AllowHandoff { get; }

        [JsonProperty(PropertyName = "enforce_handoff")]
        public bool? EnforceHandoff { get; }

        [JsonProperty(PropertyName = "attempts_configuration")]
        public AttemptsConfiguration AttemptsConfiguration { get; }

        public SdkConfig(string allowedCaptureMethods,
                            string primaryColour,
                            string secondaryColour,
                            string fontColour,
                            string locale,
                            string presetIssuingCountry,
                            string successUrl,
                            string errorUrl,
                            string privacyPolicyUrl,
                            bool? allowHandoff = null,
                            bool? enforceHandoff = null,
                            Dictionary<string, int> idDocumentTextDataExtractionRetriesConfig = null)
        {
            AllowedCaptureMethods = allowedCaptureMethods;
            PrimaryColour = primaryColour;
            SecondaryColour = secondaryColour;
            FontColour = fontColour;
            Locale = locale;
            PresetIssuingCountry = presetIssuingCountry;
            SuccessUrl = successUrl;
            ErrorUrl = errorUrl;
            PrivacyPolicyUrl = privacyPolicyUrl;
            AllowHandoff = allowHandoff;

            // Validate enforce_handoff can only be true when allow_handoff is not false
            if (enforceHandoff == true && allowHandoff == false)
            {
                throw new System.ArgumentException("enforce_handoff cannot be true when allow_handoff is false");
            }
            EnforceHandoff = enforceHandoff;

            if (idDocumentTextDataExtractionRetriesConfig != null)
            {
                AttemptsConfiguration = new AttemptsConfiguration
                {
                    IdDocumentTextDataExtraction = idDocumentTextDataExtractionRetriesConfig
                };
            }
        }
    }
}