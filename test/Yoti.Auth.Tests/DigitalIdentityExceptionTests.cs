using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Yoti.Auth.Exceptions;
namespace Yoti.Auth.Tests
{
    [TestClass]
    public class DigitalIdentityExceptionTests
    {
        [TestMethod]
        public void DigitalIdentityException_NoParameters_ErrorMessageIsNull()
        {

            var exception = new DigitalIdentityException();
            Assert.IsNotNull(exception.Message);
        }

        [TestMethod]
        public void DigitalIdentityException_WithMessage_MessageIsSet()
        {

            var message = "Test message";
            var exception = new DigitalIdentityException(message);
            Assert.AreEqual(message, exception.Message);
        }

        [TestMethod]
        public void DigitalIdentityException_WithMessageAndInnerException_MessageAndInnerExceptionAreSet()
        {

            var message = "Test message";
            var innerException = new Exception("Inner exception message");
            var exception = new DigitalIdentityException(message, innerException);
            Assert.AreEqual(message, exception.Message);
            Assert.AreEqual(innerException, exception.InnerException);
        }
    }
}