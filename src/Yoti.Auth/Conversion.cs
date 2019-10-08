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

        public static byte[] UtfToBytes(string utf8)
        {
            return Encoding.UTF8.GetBytes(utf8);
        }

        public static byte[] Base64ToBytes(string base64)
        {
            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// UrlSafe Base64 uses '-' instead of '+', and '_' instead of '/' so it can be passed as a
        /// URL parameter without extra encoding.
        /// </summary>
        public static byte[] UrlSafeBase64ToBytes(string urlSafeBase64)
        {
#if NETCOREAPP2_2
            string base64 = urlSafeBase64.Replace("-", "+", StringComparison.Ordinal).Replace("_", "/", StringComparison.Ordinal);
#else
            string base64 = urlSafeBase64.Replace("-", "+").Replace("_", "/");
#endif
            return Base64ToBytes(base64);
        }
    }
}