namespace Yoti.Auth.Constants
{
    internal static class Web
    {
        private const string DefaultYotiHost = @"https://api.yoti.com";
        public static string YotiApiPathPrefix = "/api/v1";
        public static string DefaultYotiApiUrl = DefaultYotiHost + YotiApiPathPrefix;
        public static string StagingYotiApiUrl = "https://staging0.api.yoti.com:8443" + YotiApiPathPrefix;
        public static string AuthKeyHeader = "X-Yoti-Auth-Key";
        public static string DigestHeader = "X-Yoti-Auth-Digest";
        public static string YotiSdkHeader = "X-Yoti-SDK";
        public static string YotiSdkVersionHeader = YotiSdkHeader + "-Version";
        public static string SdkIdentifier = ".NET";
    }
}