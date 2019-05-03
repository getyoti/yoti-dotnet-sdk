using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Exceptions;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ProfileParserTests
    {
        private static AsymmetricCipherKeyPair GetKey()
        {
            return CryptoEngine.LoadRsaKey(File.OpenText("test-key.pem"));
        }

        [TestMethod]
        public void UnsuccessfulResponseThrowsYotiProfileException()
        {
            var response = new Response
            {
                Success = false
            };

            Assert.ThrowsException<YotiProfileException>(() =>
            {
                ProfileParser.HandleResponse(GetKey(), response);
            });
        }

        [TestMethod]
        public void NullOrEmptyContentThrowsProfileException()
        {
            var response = new Response
            {
                Success = true,
                Content = ""
            };

            Assert.ThrowsException<YotiProfileException>(() =>
            {
                ProfileParser.HandleResponse(GetKey(), response);
            });
        }
    }
}