using System.IO;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Tests.Common
{
    public static class KeyPair
    {
        public static AsymmetricCipherKeyPair Get()
        {
            using (StreamReader stream = File.OpenText("test-key.pem"))
            {
                return CryptoEngine.LoadRsaKey(stream);
            }
        }

        public static AsymmetricCipherKeyPair GetInvalidKeyPair()
        {
            using (StreamReader stream = File.OpenText("test-key-invalid-format.pem"))
            {
                return CryptoEngine.LoadRsaKey(stream);
            }
        }

        public static StreamReader GetValidKeyStream()
        {
            return File.OpenText("test-key.pem");
        }

        internal static StreamReader GetInvalidFormatKeyStream()
        {
            return File.OpenText("test-key-invalid-format.pem");
        }
    }
}