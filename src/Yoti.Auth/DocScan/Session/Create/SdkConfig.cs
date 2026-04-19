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

        [JsonProperty(PropertyName = "attempts_configuration")]
        public AttemptsConfiguration AttemptsConfiguration { get; }

        /// <summary>
        /// The list of screens that should be omitted from the IDV flow
        /// </summary>
        [JsonProperty(PropertyName = "suppressed_screens", NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SuppressedScreens { get; }

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
                            Dictionary<string, int> idDocumentTextDataExtractionRetriesConfig = null,
                            List<string> suppressedScreens = null)
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
            SuppressedScreens = suppressedScreens;

            if (idDocumentTextDataExtractionRetriesConfig != null)
            {
                AttemptsConfiguration = new AttemptsConfiguration
                {
                    IdDocumentTextDataExtraction = idDocumentTextDataExtractionRetriesConfig
                };
            }
        }

        /// <summary>
        /// Returns the list of screens configured to be suppressed from the IDV flow.
        /// Returns an empty list when no screens are configured.
        /// </summary>
        /// <returns>The list of suppressed screen identifiers, never null.</returns>
        public List<string> GetSuppressedScreens()
        {
            return SuppressedScreens ?? new List<string>();
        }
    }
}