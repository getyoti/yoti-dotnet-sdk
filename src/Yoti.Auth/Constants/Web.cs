using System;

namespace Yoti.Auth.Constants
{
    public static class Web
    {
        public const string DefaultYotiHost = @"https://api.yoti.com";
        public const string YotiApiPathPrefix = "api/v1";
        public readonly static string DefaultYotiApiUrl = string.Join("/", DefaultYotiHost, YotiApiPathPrefix);
        internal const string AuthKeyHeader = "X-Yoti-Auth-Key";
        internal const string DigestHeader = "X-Yoti-Auth-Digest";
        internal const string YotiSdkHeader = "X-Yoti-SDK";
        internal const string YotiSdkVersionHeader = YotiSdkHeader + "-Version";
        internal const string SdkIdentifier = ".NET";
    }
}