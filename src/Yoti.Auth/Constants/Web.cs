namespace Yoti.Auth.Constants
{
    internal static class Web
    {
        private const string DefaultYotiHost = @"https://api.yoti.com";
        public const string YotiApiPathPrefix = "api/v1";
        public readonly static string DefaultYotiApiUrl = string.Join("/", DefaultYotiHost, YotiApiPathPrefix);
        public const string AuthKeyHeader = "X-Yoti-Auth-Key";
        public const string DigestHeader = "X-Yoti-Auth-Digest";
        public const string YotiSdkHeader = "X-Yoti-SDK";
        public const string YotiSdkVersionHeader = YotiSdkHeader + "-Version";
        public const string SdkIdentifier = ".NET";
    }
}