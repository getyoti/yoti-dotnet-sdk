using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Exceptions;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class ConversionTests
    {
        [DataTestMethod]
        [DataRow("HT-sGfUaHj-rDA", "HT+sGfUaHj+rDA==")]
        [DataRow("-OyhuDs6dAg", "+OyhuDs6dAg=")]
        [DataRow("c3RyaW5n", "c3RyaW5n")]
        public void UrlSafeBase64ToBytesShouldDecodeUnpaddedCorrectly(string b64UrlUnpadded, string b64)
        {
            Assert.AreEqual(
                Conversion.BytesToBase64(Conversion.UrlSafeBase64ToBytes(b64UrlUnpadded, false)),
                b64
            );
        }

        [DataTestMethod]
        [DataRow("HT-sGfUaHj-rDA==", "HT+sGfUaHj+rDA==")]
        [DataRow("-OyhuDs6dAg=", "+OyhuDs6dAg=")]
        [DataRow("c3RyaW5n", "c3RyaW5n")]
        public void UrlSafeBase64ToBytesShouldDecodePaddedCorrectly(string b64UrlPadded, string b64)
        {
            Assert.AreEqual(
                Conversion.BytesToBase64(Conversion.UrlSafeBase64ToBytes(b64UrlPadded)),
                b64
            );
        }


        [DataTestMethod]
        [DataRow("HT-sGfUaHj-rDA", "HT+sGfUaHj+rDA==")]
        [DataRow("-OyhuDs6dAg", "+OyhuDs6dAg=")]
        [DataRow("c3RyaW5n", "c3RyaW5n")]
        public void BytesToUrlSafeBase64ShouldEncodeUnpaddedCorrectly(string b64UrlUnpadded, string b64)
        {
            Assert.AreEqual(
                Conversion.BytesToUrlSafeBase64(Conversion.Base64ToBytes(b64), false),
                b64UrlUnpadded
            );
        }

        [DataTestMethod]
        [DataRow("HT-sGfUaHj-rDA==", "HT+sGfUaHj+rDA==")]
        [DataRow("-OyhuDs6dAg=", "+OyhuDs6dAg=")]
        [DataRow("c3RyaW5n", "c3RyaW5n")]
        public void BytesToUrlSafeBase64ShouldEncodePaddedCorrectly(string b64UrlPadded, string b64)
        {
            Assert.AreEqual(
                Conversion.BytesToUrlSafeBase64(Conversion.Base64ToBytes(b64)),
                b64UrlPadded
            );
        }
    }
}