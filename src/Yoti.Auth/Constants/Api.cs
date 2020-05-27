using System;

namespace Yoti.Auth.Constants
{
    public static class Api
    {
        public const string DefaultYotiHost = @"https://api.yoti.com";

        public const string YotiApiPathPrefix = "api/v1";
        public readonly static string DefaultYotiApiUrl = string.Join("/", DefaultYotiHost, YotiApiPathPrefix);

        public const string YotiDocsPathPrefix = "idverify/v1";
        public readonly static Uri DefaultYotiDocsUrl = new Uri(string.Join("/", DefaultYotiHost, YotiDocsPathPrefix));

        public const string AuthKeyHeader = "X-Yoti-Auth-Key";
        public const string DigestHeader = "X-Yoti-Auth-Digest";
        public const string YotiSdkHeader = "X-Yoti-SDK";
        public const string YotiSdkVersionHeader = YotiSdkHeader + "-Version";
        public const string ContentTypeHeader = "Content-Type";
        public const string ContentTypeJson = "application/json";

        public const string SdkIdentifier = ".NET";
    }
}