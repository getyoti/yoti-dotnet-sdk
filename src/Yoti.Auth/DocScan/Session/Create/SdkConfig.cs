using Newtonsoft.Json;

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

        public SdkConfig(string allowedCaptureMethods,
                        string primaryColour,
                        string secondaryColour,
                        string fontColour,
                        string locale,
                        string presetIssuingCountry,
                        string successUrl,
                        string errorUrl)
        {
            AllowedCaptureMethods = allowedCaptureMethods;
            PrimaryColour = primaryColour;
            SecondaryColour = secondaryColour;
            FontColour = fontColour;
            Locale = locale;
            PresetIssuingCountry = presetIssuingCountry;
            SuccessUrl = successUrl;
            ErrorUrl = errorUrl;
        }
    }
}