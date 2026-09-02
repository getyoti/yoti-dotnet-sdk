using System.Collections.Generic;
using System.Linq;
using Yoti.Auth.Constants;

namespace Yoti.Auth.DocScan.Session.Create
{
    public class SdkConfigBuilder
    {
        private string _allowedCaptureMethods;
        private string _primaryColour;
        private string _secondaryColour;
        private string _fontColour;
        private string _locale;
        private string _presetIssuingCountry;
        private string _successUrl;
        private string _errorUrl;
        private string _privacyPolicyUrl;
        private bool? _allowHandoff;
        private bool? _enforceHandoff;
        private string _brandId;
        private Dictionary<string, int> _idDocumentTextDataExtractionAttemptsConfig;
        private List<string> _suppressedScreens;
        private string _darkMode;
        private string _primaryColourDarkMode;

        /// <summary>
        /// Sets the allowed capture method to "CAMERA"
        /// </summary>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithAllowsCamera()
        {
            return WithAllowedCaptureMethods(DocScanConstants.Camera);
        }

        /// <summary>
        /// Sets the allowed capture method to "CAMERA_AND_UPLOAD"
        /// </summary>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithAllowsCameraAndUpload()
        {
            return WithAllowedCaptureMethods(DocScanConstants.CameraAndUpload);
        }

        /// <summary>
        /// Sets the allowed capture method
        /// </summary>
        /// <param name="allowedCaptureMethods">capture method</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithAllowedCaptureMethods(string allowedCaptureMethods)
        {
            _allowedCaptureMethods = allowedCaptureMethods;
            return this;
        }

        /// <summary>
        /// Sets the primary colour to be used by the web/native client
        /// </summary>
        /// <param name="primaryColour">the primary colour, hexadecimal value e.g. #ff0000</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithPrimaryColour(string primaryColour)
        {
            _primaryColour = primaryColour;
            return this;
        }

        /// <summary>
        /// Sets the secondary colour to be used by the web/native client (used on the button)
        /// </summary>
        /// <param name="secondaryColour">the secondary colour, hexadecimal value e.g. #ff0000</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithSecondaryColour(string secondaryColour)
        {
            _secondaryColour = secondaryColour;
            return this;
        }

        /// <summary>
        /// Sets the font colour to be used by the web/native client (used on the button)
        /// </summary>
        /// <param name="fontColour">the font colour, hexadecimal value e.g. #ff0000</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithFontColour(string fontColour)
        {
            _fontColour = fontColour;
            return this;
        }

        /// <summary>
        /// Sets the language locale used by the web/native client
        /// </summary>
        /// <param name="locale">the locale, e.g. "en"</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithLocale(string locale)
        {
            _locale = locale;
            return this;
        }

        /// <summary>
        /// Sets the preset issuing country used by the web/native client
        /// </summary>
        /// <param name="presetIssuingCountry">the preset issuing country, 3 letter ISO code e.g. "GBR"</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithPresetIssuingCountry(string presetIssuingCountry)
        {
            _presetIssuingCountry = presetIssuingCountry;
            return this;
        }

        /// <summary>
        /// Sets the success URL for the redirect that follows the web/native client uploading documents successfully
        /// </summary>
        /// <param name="successUrl">The success URL</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithSuccessUrl(string successUrl)
        {
            _successUrl = successUrl;
            return this;
        }

        /// <summary>
        /// Sets the error URL for the redirect that follows the web/native client uploading documents unsuccessfully
        /// </summary>
        /// <param name="errorUrl">The error URL</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithErrorUrl(string errorUrl)
        {
            _errorUrl = errorUrl;
            return this;
        }

        /// <summary>
        /// Sets the privacy policy URL
        /// </summary>
        /// <param name="privacyPolicyUrl">The privacy policy URL</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithPrivacyPolicyUrl(string privacyPolicyUrl)
        {
            _privacyPolicyUrl = privacyPolicyUrl;
            return this;
        }

        /// <summary>
        /// Sets if the user is allowed to perform mobile handoff
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Mobile handoff allows the user to start a session on their desktop device, and then switch to using their mobile to upload resources (generally due to better camera quality on mobile devices)
        ///     </para>
        ///     <para>
        ///         Note: Passing this value will override any value set in the Yoti Connect backend (which itself takes precedence over any value in lists of configured organisations)   
        ///     </para>
        /// </remarks>
        /// <param name="allowHandoff">If mobile handoff is allowed</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithAllowHandoff(bool allowHandoff)
        {
            _allowHandoff = allowHandoff;
            return this;
        }

        /// <summary>
        /// Sets if mobile handoff is enforced for the user
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         When enabled, the user is required to perform mobile handoff to upload their resources.
        ///     </para>
        ///     <para>
        ///         Note: <c>enforce_handoff</c> cannot be set to <c>true</c> if <c>allow_handoff</c> is <c>false</c>.
        ///         Validation is enforced server-side by the IDV API (see DOCS-3523).
        ///     </para>
        /// </remarks>
        /// <param name="enforceHandoff">If mobile handoff is enforced</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithEnforceHandoff(bool enforceHandoff)
        {
            _enforceHandoff = enforceHandoff;
            return this;
        }

        /// <summary>
        /// Allows configuring the number of attempts permitted for text extraction on an ID document
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Can be used in conjunction with call(s) to <see cref="SessionSpecificationBuilder.WithRequestedTask(Task.BaseRequestedTask)"/> passing a <see cref="Task.RequestedTextExtractionTask"/>
        ///     </para>
        ///     <para>
        ///         A <see cref="Task.RequestedTextExtractionTask"/> can be created with a <see cref="Task.RequestedTextExtractionTaskBuilder"/>
        ///     </para>
        ///     <para>
        ///         Every attempt to update a Task on an ID Document Resource linked to a requirement will result in some attempts number being decremented
        ///     </para>
        /// </remarks>
        /// <param name="category">The category for the attempts number</param>
        /// <param name="attempts">The number of attempts for the category specified</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithIdDocumentTextExtractionCategoryAttempts(string category, int attempts)
        {
            if (_idDocumentTextDataExtractionAttemptsConfig == null)
                _idDocumentTextDataExtractionAttemptsConfig = new Dictionary<string, int>();

            if (_idDocumentTextDataExtractionAttemptsConfig.ContainsKey(category))
                _idDocumentTextDataExtractionAttemptsConfig[category] = attempts;
            else
                _idDocumentTextDataExtractionAttemptsConfig.Add(category, attempts);
            return this;
        }

        /// <summary>
        ///     <para>
        ///         Allows configuring the number of 'Reclassification' attempts permitted for text extraction on an ID document
        ///     </para>
        ///     <para>
        ///         The Reclassification attempts value is decremented whenever the uploaded document is reclassified to be used by another resource requirement
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Can be used in conjunction with call(s) to <see cref="SessionSpecificationBuilder.WithRequestedTask(Task.BaseRequestedTask)"/> passing a <see cref="Task.RequestedTextExtractionTask"/>
        ///     </para>
        ///     <para>
        ///         A <see cref="Task.RequestedTextExtractionTask"/> can be created with a <see cref="Task.RequestedTextExtractionTaskBuilder"/>
        ///     </para>
        ///     <para>
        ///         Every attempt to update a Task on an ID Document Resource linked to a requirement will result in some attempts number being decremented
        ///     </para>
        /// </remarks>
        /// <param name="reclassificationAttempts">The number of attempts for reclassification</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithIdDocumentTextExtractionReclassificationAttempts(int reclassificationAttempts)
        {
            WithIdDocumentTextExtractionCategoryAttempts(DocScanConstants.Reclassification, reclassificationAttempts);
            return this;
        }

        /// <summary>
        ///     <para>
        ///         Allows configuring the number of 'Generic' attempts permitted for text extraction on an ID document
        ///     </para>
        ///     <para>
        ///         The Generic attempts value is decremented whenever some event concerning the uploaded document occurs which has not otherwise been categorised (e.g. as 'Reclassification')
        ///     </para>
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Can be used in conjunction with call(s) to <see cref="SessionSpecificationBuilder.WithRequestedTask(Task.BaseRequestedTask)"/> passing a <see cref="Task.RequestedTextExtractionTask"/>
        ///     </para>
        ///     <para>
        ///         A <see cref="Task.RequestedTextExtractionTask"/> can be created with a <see cref="Task.RequestedTextExtractionTaskBuilder"/>
        ///     </para>
        ///     <para>
        ///         Every attempt to update a Task on an ID Document Resource linked to a requirement will result in some attempts number being decremented
        ///     </para>
        /// </remarks>
        /// <param name="genericAttempts">The number of generic attempts</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithIdDocumentTextExtractionGenericAttempts(int genericAttempts)
        {
            WithIdDocumentTextExtractionCategoryAttempts(DocScanConstants.Generic, genericAttempts);
            return this;
        }

        /// <summary>
        /// Replaces the suppressed screens list with the provided collection, filtering out any null or whitespace entries.
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         Valid screen identifier values are defined in <see cref="Constants.DocScanConstants"/>.
        ///     </para>
        ///     <para>
        ///         Passing null or omitting this call leaves <c>suppressed_screens</c> out of the
        ///         serialized config, so the flow shows all screens by default.
        ///     </para>
        ///     <para>
        ///         To append a single screen without replacing the list, use <see cref="WithSuppressedScreen"/>.
        ///     </para>
        /// </remarks>
        /// <param name="suppressedScreens">The list of screen identifiers to suppress</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithSuppressedScreens(List<string> suppressedScreens)
        {
            _suppressedScreens = suppressedScreens?.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
            return this;
        }

        /// <summary>
        /// Appends a single screen identifier to the suppressed screens list, ignoring duplicates.
        /// </summary>
        /// <remarks>Use <see cref="WithSuppressedScreens"/> to replace the entire list at once.</remarks>
        /// <param name="suppressedScreen">The screen identifier to suppress; use constants from <see cref="Constants.DocScanConstants"/></param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithSuppressedScreen(string suppressedScreen)
        {
            Validation.NotNullOrWhiteSpace(suppressedScreen, nameof(suppressedScreen));

            if (_suppressedScreens == null)
                _suppressedScreens = new List<string>();

            if (!_suppressedScreens.Contains(suppressedScreen))
                _suppressedScreens.Add(suppressedScreen);

            return this;
        }

        /// <summary>
        /// Sets the Brand Id
        /// </summary>
        /// <param name="brandId">BrandID</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithBrandId(string brandId)
        {
            _brandId = brandId;
            return this;
        }

        /// <summary>
        /// Sets the dark mode preference for the web/native client
        /// </summary>
        /// <param name="darkMode">The dark mode value (e.g. "ON", "OFF", "AUTO")</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithDarkMode(string darkMode)
        {
            _darkMode = darkMode;
            return this;
        }

        /// <summary>
        /// Sets the dark mode preference to "ON"
        /// </summary>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithDarkModeOn()
        {
            return WithDarkMode("ON");
        }

        /// <summary>
        /// Sets the dark mode preference to "OFF"
        /// </summary>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithDarkModeOff()
        {
            return WithDarkMode("OFF");
        }

        /// <summary>
        /// Sets the dark mode preference to "AUTO"
        /// </summary>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithDarkModeAuto()
        {
            return WithDarkMode("AUTO");
        }

        /// <summary>
        /// Sets the primary colour to be used by the web/native client in dark mode
        /// </summary>
        /// <param name="primaryColourDarkMode">the primary colour for dark mode, hexadecimal value e.g. #ff0000</param>
        /// <returns>The <see cref="SdkConfigBuilder"/></returns>
        public SdkConfigBuilder WithPrimaryColourDarkMode(string primaryColourDarkMode)
        {
            _primaryColourDarkMode = primaryColourDarkMode;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="SdkConfig"/> based on values supplied to the builder
        /// </summary>
        /// <returns>The built <see cref="SdkConfig"/></returns>
        public SdkConfig Build()
        {
            return new SdkConfig(
                _allowedCaptureMethods,
                _primaryColour,
                _secondaryColour,
                _fontColour,
                _locale,
                _presetIssuingCountry,
                _successUrl,
                _errorUrl,
                _privacyPolicyUrl,
                _allowHandoff,
                _idDocumentTextDataExtractionAttemptsConfig,
                _enforceHandoff,
                _suppressedScreens,
                _brandId,
                _darkMode,
                _primaryColourDarkMode);
        }
    }
}
