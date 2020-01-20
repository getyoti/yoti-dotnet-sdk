using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    }
}