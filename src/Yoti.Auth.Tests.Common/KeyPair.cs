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
    }
}