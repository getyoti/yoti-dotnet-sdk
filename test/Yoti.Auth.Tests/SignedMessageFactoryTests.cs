using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class SignedMessageFactoryTests
    {
        [TestMethod]
        public void ShouldThrowExceptionWhenHttpMethodIsNull()
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                SignedMessageFactory.SignMessage(null, "endpoint", KeyPair.Get(), null);
            });
        }
    }
}