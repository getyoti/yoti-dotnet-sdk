using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Exceptions;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ValidationTests
    {
        [TestMethod]
        public void IsDefaultShouldReturnTrueForNullValue()
        {
            Assert.IsTrue(Validation.IsDefault(null));
        }

        [TestMethod]
        public void IsDefaultShouldReturnFalseForNonValueType()
        {
            Assert.IsFalse(Validation.IsDefault(new ExtraDataException()));
        }

        [TestMethod]
        public void IsDefaultShouldReturnTrueForValueType()
        {
            Assert.IsTrue(Validation.IsDefault(0));
        }

        [TestMethod]
        public void IsNotDefaultShouldAllowNonDefaultType()
        {
            int someInteger = 2;
            Validation.IsNotDefault(someInteger, nameof(someInteger));
        }

        [TestMethod]
        public void IsNotDefaultShouldThrowForDefaultType()
        {
            int defaultValue = 0;
            Assert.ThrowsException<System.InvalidOperationException>(() =>
            {
                Validation.IsNotDefault(defaultValue, nameof(defaultValue));
            });
        }
    }
}