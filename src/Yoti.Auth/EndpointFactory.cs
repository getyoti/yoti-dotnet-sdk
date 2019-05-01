using System;
using System.Globalization;

namespace Yoti.Auth
{
    internal static class EndpointFactory
    {
        public static string CreateProfileEndpoint(string path, string token, string sdkId)
        {
            return $"/{path}/{token}?nonce={CryptoEngine.GenerateNonce()}&timestamp={GetTimestamp()}&appId={sdkId}";
        }

        public static string CreateAmlEndpoint(string appId)
        {
            return $"/aml-check?appId={appId}&timestamp={GetTimestamp()}&nonce={CryptoEngine.GenerateNonce()}";
        }

        public static string CreateDynamicSharingPath(string sdkId)
        {
            return $"/qrcodes/apps/{sdkId}?nonce={CryptoEngine.GenerateNonce()}&timestamp={GetTimestamp()}";
        }

        private static string GetTimestamp()
        {
            var milliseconds = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            return milliseconds.ToString(CultureInfo.InvariantCulture);
        }
    }
}