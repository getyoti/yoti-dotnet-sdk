using System;
using System.Collections.Generic;
using System.Text;

namespace Yoti.Auth
{
    internal static class YotiConstants
    {
        private static string DefaultYotiHost = @"https://api.yoti.com";
        public static string YotiApiUrl = "yoti.api.url";
        public static string YotiApiPathPrefix = "/api/v1";
        public static string DefaultYotiApiUrl = DefaultYotiHost + YotiApiPathPrefix;
        public static string StagingYotiApiUrl = "https://staging0.api.yoti.com" + YotiApiPathPrefix;
        public static string AuthKeyHeader = "X-Yoti-Auth-Key";
        public static string DigestHeader = "X-Yoti-Auth-Digest";
        public static string YotiSdkHeader = "X-Yoti-SDK";
        public static string ContentTypeHeader = "Content-Type";
        public static string SdkIdentifier = ".NET";
        public static string DefaultCharset = "UTF-8";
        public static string ContentTypeJson = "application/json";
    }
}