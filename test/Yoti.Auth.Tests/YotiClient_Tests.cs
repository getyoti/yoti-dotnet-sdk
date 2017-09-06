using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiClient_Tests
    {
        private StreamReader GetValidKeyStream()
        {
            return File.OpenText("test-key.pem");
        }

        private StreamReader GetInvalidFormatKeyStream()
        {
            return File.OpenText("test-key-invalid-format.pem");
        }

        [TestMethod]
        public void YotiClient_ValidParameters_DoesntThrowException()
        {
            StreamReader keystream = GetValidKeyStream();
            string sdkId = "fake-sdk-id";
            YotiClient client = new YotiClient(sdkId, keystream);
        }

        [TestMethod]
        public void YotiClient_NullAppId_ThrowsException()
        {
            StreamReader keystream = GetValidKeyStream();
            string sdkId = null;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                YotiClient client = new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void YotiClient_EmptyAppId_ThrowsException()
        {
            StreamReader keystream = GetValidKeyStream();
            string sdkId = string.Empty;
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                YotiClient client = new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void YotiClient_NoKeyStream_ThrowsException()
        {
            StreamReader keystream = null;
            string sdkId = "fake-sdk-id";
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                YotiClient client = new YotiClient(sdkId, keystream);
            });
        }

        [TestMethod]
        public void YotiClient_InvalidKeyStream_ThrowsException()
        {
            StreamReader keystream = GetInvalidFormatKeyStream();
            string sdkId = "fake-sdk-id";
            Assert.ThrowsException<ArgumentException>(() =>
            {
                YotiClient client = new YotiClient(sdkId, keystream);
            });
        }
    }
}