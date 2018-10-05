namespace Yoti.Auth
{
    internal static class YotiConstants
    {
        private const string DefaultYotiHost = @"https://api.yoti.com";
        public static string YotiApiPathPrefix = "/api/v1";
        public static string DefaultYotiApiUrl = DefaultYotiHost + YotiApiPathPrefix;
        public static string StagingYotiApiUrl = "https://staging0.api.yoti.com:8443" + YotiApiPathPrefix;
        public static string AuthKeyHeader = "X-Yoti-Auth-Key";
        public static string DigestHeader = "X-Yoti-Auth-Digest";
        public static string YotiSdkHeader = "X-Yoti-SDK";
        public static string SdkIdentifier = ".NET";
        public const string AttributeAgeOver = "age_over:";
        public const string AttributeAgeUnder = "age_under:";
        public const string AttributeAgeVerified = "AgeVerified";
        public const string AttributeStructuredPostalAddress = "structured_postal_address";
    }
}