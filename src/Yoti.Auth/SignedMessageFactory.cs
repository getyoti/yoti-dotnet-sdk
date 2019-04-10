using System.Net.Http;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth
{
    internal class SignedMessageFactory
    {
        public static string SignMessage(HttpMethod httpMethod, string endpoint, AsymmetricCipherKeyPair keyPair, byte[] content)
        {
            string stringToConvert = $"{httpMethod.ToString()}&{endpoint}";

            if (content != null)
                stringToConvert += "&" + Conversion.BytesToBase64(content);

            byte[] digestBytes = Conversion.UtfToBytes(stringToConvert);
            byte[] signedDigestBytes = CryptoEngine.SignDigest(digestBytes, keyPair);

            return Conversion.BytesToBase64(signedDigestBytes);
        }
    }
}