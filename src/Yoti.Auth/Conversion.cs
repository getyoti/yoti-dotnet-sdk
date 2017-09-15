using System;
using System.Text;

namespace Yoti.Auth
{
    internal static class Conversion
    {
        public static string BytesToUtf8(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        public static string BytesToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// UrlSafe Base64 uses '-' instead of '+', and '_' instead of '/' so it
        /// can be passed as a url parameter without extra encoding.
        /// </summary>
        public static string BytesToUrlsafeBase64(byte[] bytes)
        {
            var base64 = BytesToBase64(bytes);

            return base64.Replace("+", "-").Replace("/", "_");
        }

        public static byte[] UtfToBytes(string utf8)
        {
            return Encoding.UTF8.GetBytes(utf8);
        }

        public static byte[] Base64ToBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// UrlSafe Base64 uses '-' instead of '+', and '_' instead of '/' so it
        /// can be passed as a url parameter without extra encoding.
        /// </summary>
        public static byte[] UrlSafeBase64ToBytes(string urlSafeBase64)
        {
            var base64 = urlSafeBase64.Replace("-", "+").Replace("_", "/");

            return Base64ToBytes(base64);
        }
    }
}