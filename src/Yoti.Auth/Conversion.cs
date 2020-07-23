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
        public static byte[] UrlSafeBase64ToBytes(string urlSafeBase64, bool padded = true)
        {
#if NETCOREAPP2_2 || NETCOREAPP3_1 || NETSTANDARD2_1
            string base64 = urlSafeBase64.Replace("-", "+", StringComparison.Ordinal).Replace("_", "/", StringComparison.Ordinal);
#else
            string base64 = urlSafeBase64.Replace("-", "+").Replace("_", "/");
#endif

            if (!padded)
            {
                switch (base64.Length % 4)
                {
                    case 2: base64 += "=="; break;
                    case 3: base64 += "="; break;
                }
            }
            return Base64ToBytes(base64);
        }

        public static string BytesToUrlSafeBase64(byte[] bytes, bool padded = true)
        {
            string base64 = BytesToBase64(bytes);

#if NETCOREAPP2_2 || NETCOREAPP3_1 || NETSTANDARD2_1
            string urlSafeBase64 = base64.Replace("+", "-", StringComparison.Ordinal).Replace("/", "_", StringComparison.Ordinal);
#else
            string urlSafeBase64 = base64.Replace("+", "-").Replace("/", "_");
#endif
            if (!padded) {
                urlSafeBase64 = urlSafeBase64.TrimEnd('=');
            }
            return urlSafeBase64;
        }
    }
}