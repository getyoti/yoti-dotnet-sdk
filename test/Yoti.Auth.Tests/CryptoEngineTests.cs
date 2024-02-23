using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class CryptoEngineTests
    {
        [TestMethod]
        public void NonBase64TokenThrowsError()
        {
            var exception = Assert.ThrowsException<FormatException>(() =>
            {
                CryptoEngine.DecryptToken("_aasi", KeyPair.Get());
            });

            Assert.IsTrue(exception.Message.Contains("The input is not a valid Base-64 string as it contains a non-base 64 character"));
        }

        [TestMethod]
        public void EmptyOneTimeUseTokenThrowsError()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                CryptoEngine.DecryptToken("", KeyPair.Get());
            });

            Assert.IsTrue(exception.Message.Contains("one time use token"));
        }

        [TestMethod]
        public void DecryptAesGcm_EmptySecretsThrowsError()
        {
            // Arrange
            byte[] iv = new byte[12];
            byte[] secret = new byte[16];
            byte[] cipherText = new byte[32];


            var exception = Assert.ThrowsException<Exception>(() =>
            {
                CryptoEngine.DecryptAesGcm(cipherText, iv, secret);
            });

            Assert.IsTrue(exception.Message.Contains("Failed to decrypt receipt key"));
        }

        [TestMethod]
        public void UnwrapReceiptKey_EmptySecretsThrowsError()
        {
            
                byte[] wrappedReceiptKey = new byte[32];
            byte[] encryptedItemKey = new byte[32];
            byte[] itemKeyIv = new byte[12];
            AsymmetricCipherKeyPair key = null;


            //Failed to decrypt receipt content

            var exception = Assert.ThrowsException<Exception>(() =>
            {
                byte[] unwrappedKey = CryptoEngine.UnwrapReceiptKey(wrappedReceiptKey, encryptedItemKey, itemKeyIv, key);
            });

            Assert.IsTrue(exception.Message.Contains("Failed to unwrap receipt key"));
        }

        [TestMethod]
        public void DecryptContent_EmptySecretsThrowsError()
        {
            byte[] content = new byte[] { 0x01, 0x02, 0x03 }; // Example content
            byte[] receiptContentKey = new byte[16]; // Example receipt content key

            var exception = Assert.ThrowsException<Exception>(() =>
            {
                byte[] decryptedContent = CryptoEngine.DecryptReceiptContent(content, receiptContentKey);

            });

            Assert.IsTrue(exception.Message.Contains("Failed to decrypt receipt content"));
        }

    }
}