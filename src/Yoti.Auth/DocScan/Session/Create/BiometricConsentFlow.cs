namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Defines the allowed values for the biometric consent flow configuration.
    /// Controls where the biometric consent screen is shown during the IDV session.
    /// </summary>
    public static class BiometricConsentFlow
    {
        /// <summary>
        /// The biometric consent screen is shown early in the flow, before the user begins capturing documents.
        /// </summary>
        public const string Early = "EARLY";

        /// <summary>
        /// The biometric consent screen is shown just in time, immediately before biometric capture.
        /// This is the default behaviour when the property is not set.
        /// </summary>
        public const string JustInTime = "JUST_IN_TIME";
    }
}
