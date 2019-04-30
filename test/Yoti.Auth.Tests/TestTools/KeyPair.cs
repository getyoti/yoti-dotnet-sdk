using System.IO;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Tests.TestTools
{
    internal static class KeyPair
    {
        internal static AsymmetricCipherKeyPair Get()
        {
            using (StreamReader stream = File.OpenText("test-key.pem"))
            {
                return CryptoEngine.LoadRsaKey(stream);
            }
        }
    }
}