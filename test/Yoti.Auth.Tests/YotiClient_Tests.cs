using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiClient_Tests
    {
        private const string encryptedToken = "b6H19bUCJhwh6WqQX/sEHWX9RP+A/ANr1fkApwA4Dp2nJQFAjrF9e6YCXhNBpAIhfHnN0iXubyXxXZMNwNMSQ5VOxkqiytrvPykfKQWHC6ypSbfy0ex8ihndaAXG5FUF+qcU8QaFPMy6iF3x0cxnY0Ij0kZj0Ng2t6oiNafb7AhT+VGXxbFbtZu1QF744PpWMuH0LVyBsAa5N5GJw2AyBrnOh67fWMFDKTJRziP5qCW2k4h5vJfiYr/EOiWKCB1d/zINmUm94ZffGXxcDAkq+KxhN1ZuNhGlJ2fKcFh7KxV0BqlUWPsIEiwS0r9CJ2o1VLbEs2U/hCEXaqseEV7L29EnNIinEPVbL4WR7vkF6zQCbK/cehlk2Qwda+VIATqupRO5grKZN78R9lBitvgilDaoE7JB/VFcPoljGQ48kX0wje1mviX4oJHhuO8GdFITS5LTbojGVQWT7LUNgAUe0W0j+FLHYYck3v84OhWTqads5/jmnnLkp9bdJSRuJF0e8pNdePnn2lgF+GIcyW/0kyGVqeXZrIoxnObLpF+YeUteRBKTkSGFcy7a/V/DLiJMPmH8UXDLOyv8TVt3ppzqpyUrLN2JVMbL5wZ4oriL2INEQKvw/boDJjZDGeRlu5m1y7vGDNBRDo64+uQM9fRUULPw+YkABNwC0DeShswzT00=";

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

        [TestMethod]
        public void YotiClient_GetActivityDetails()
        {
            string sdkId = "fake-sdk-id";
            var privateStreamKey = GetValidKeyStream();

            YotiClient client = new YotiClient(sdkId, privateStreamKey);

            ActivityDetails activityDetails = client.GetActivityDetails(encryptedToken);

            Assert.IsNotNull(activityDetails.Outcome);
        }

        [TestMethod]
        public async Task YotiClient_GetActivityDetailsAsync()
        {
            string sdkId = "fake-sdk-id";
            var privateStreamKey = GetValidKeyStream();

            YotiClient client = new YotiClient(sdkId, privateStreamKey);

            ActivityDetails activityDetails = await client.GetActivityDetailsAsync(encryptedToken);

            Assert.IsNotNull(activityDetails.Outcome);
        }
    }
}