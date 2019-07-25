using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

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