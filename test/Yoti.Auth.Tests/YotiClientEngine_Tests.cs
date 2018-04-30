using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiClientEngine_Tests
    {
        private const string token = "NpdmVVGC-28356678-c236-4518-9de4-7a93009ccaf0-c5f92f2a-5539-453e-babc-9b06e1d6b7de";
        private const string encryptedToken = "b6H19bUCJhwh6WqQX/sEHWX9RP+A/ANr1fkApwA4Dp2nJQFAjrF9e6YCXhNBpAIhfHnN0iXubyXxXZMNwNMSQ5VOxkqiytrvPykfKQWHC6ypSbfy0ex8ihndaAXG5FUF+qcU8QaFPMy6iF3x0cxnY0Ij0kZj0Ng2t6oiNafb7AhT+VGXxbFbtZu1QF744PpWMuH0LVyBsAa5N5GJw2AyBrnOh67fWMFDKTJRziP5qCW2k4h5vJfiYr/EOiWKCB1d/zINmUm94ZffGXxcDAkq+KxhN1ZuNhGlJ2fKcFh7KxV0BqlUWPsIEiwS0r9CJ2o1VLbEs2U/hCEXaqseEV7L29EnNIinEPVbL4WR7vkF6zQCbK/cehlk2Qwda+VIATqupRO5grKZN78R9lBitvgilDaoE7JB/VFcPoljGQ48kX0wje1mviX4oJHhuO8GdFITS5LTbojGVQWT7LUNgAUe0W0j+FLHYYck3v84OhWTqads5/jmnnLkp9bdJSRuJF0e8pNdePnn2lgF+GIcyW/0kyGVqeXZrIoxnObLpF+YeUteRBKTkSGFcy7a/V/DLiJMPmH8UXDLOyv8TVt3ppzqpyUrLN2JVMbL5wZ4oriL2INEQKvw/boDJjZDGeRlu5m1y7vGDNBRDo64+uQM9fRUULPw+YkABNwC0DeShswzT00=";

        private static AsymmetricCipherKeyPair GetKeyPair()
        {
            using (var stream = File.OpenText("test-key.pem"))
            {
                return CryptoEngine.LoadRsaKey(stream);
            }
        }

        [TestMethod]
        public void YotiClientEngine_HttpFailure_ReturnsFailure()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = false,
                    StatusCode = 500
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);

            var activityDetails = engine.GetActivityDetails(encryptedToken, sdkId, keyPair, YotiConstants.DefaultYotiApiUrl);

            Assert.IsNotNull(activityDetails);
            Assert.AreEqual(ActivityOutcome.Failure, activityDetails.Outcome);
        }

        [TestMethod]
        public void YotiClientEngine_Http404_ReturnsProfileNotFound()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = false,
                    StatusCode = 404
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);

            var activityDetails = engine.GetActivityDetails(encryptedToken, sdkId, keyPair, YotiConstants.DefaultYotiApiUrl);

            Assert.IsNotNull(activityDetails);
            Assert.AreEqual(ActivityOutcome.ProfileNotFound, activityDetails.Outcome);
        }

        [TestMethod]
        public void YotiClientEngine_SharingFailure_ReturnsSharingFailure()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"session_data\":null,\"receipt\":{\"receipt_id\": null,\"other_party_profile_content\": null,\"policy_uri\":null,\"personal_key\":null,\"remember_me_id\":null, \"sharing_outcome\":\"FAILURE\",\"timestamp\":\"2016-09-23T13:04:11Z\"}}"
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);

            var activityDetails = engine.GetActivityDetails(encryptedToken, sdkId, keyPair, YotiConstants.DefaultYotiApiUrl);

            Assert.IsNotNull(activityDetails);
            Assert.AreEqual(ActivityOutcome.SharingFailure, activityDetails.Outcome);
        }

        [TestMethod]
        public void YotiClientEngine_NullReceipt_ReturnsFailure()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"session_data\":null,\"receipt\":null}"
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);

            var activityDetails = engine.GetActivityDetails(encryptedToken, sdkId, keyPair, YotiConstants.DefaultYotiApiUrl);

            Assert.IsNotNull(activityDetails);
            Assert.AreEqual(ActivityOutcome.Failure, activityDetails.Outcome);
        }

        [TestMethod]
        public void YotiClientEngine_TokenDecodedSuccessfully()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                Assert.AreEqual("/api/v1/profile/" + token, uri.AbsolutePath);

                return Task.FromResult(new Response
                {
                    Success = false,
                    StatusCode = 500
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);

            ActivityDetails activityDetails = engine.GetActivityDetails(encryptedToken, sdkId, keyPair, YotiConstants.DefaultYotiApiUrl);
            Assert.IsNotNull(activityDetails.Outcome);
        }

        [TestMethod]
        public void YotiClientEngine_ParseProfile_Success()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            string wrapped_receipt_key = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=";
            string other_party_profile_content = "ChCZAib1TBm9Q5GYfFrS1ep9EnAwQB5shpAPWLBgZgFgt6bCG3S5qmZHhrqUbQr3yL6yeLIDwbM7x4nuT/MYp+LDXgmFTLQNYbDTzrEzqNuO2ZPn9Kpg+xpbm9XtP7ZLw3Ep2BCmSqtnll/OdxAqLb4DTN4/wWdrjnFC+L/oQEECu646";
            string remember_me_id = "remember_me_id0123456789";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"receipt\":{\"wrapped_receipt_key\": \"" + wrapped_receipt_key + "\",\"other_party_profile_content\": \"" + other_party_profile_content + "\",\"remember_me_id\":\"" + remember_me_id + "\", \"sharing_outcome\":\"SUCCESS\"}}"
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);

            var activityDetails = engine.GetActivityDetails(encryptedToken, sdkId, keyPair, YotiConstants.DefaultYotiApiUrl);

            Assert.IsNotNull(activityDetails);
            Assert.AreEqual(ActivityOutcome.Success, activityDetails.Outcome);

            Assert.IsNotNull(activityDetails.UserProfile);
            Assert.IsNotNull(activityDetails.Profile);

            Assert.AreEqual(remember_me_id, activityDetails.UserProfile.Id);
            Assert.AreEqual(remember_me_id, activityDetails.Profile.Id);

            Assert.IsNotNull(activityDetails.UserProfile.Selfie);
            Assert.IsNotNull(activityDetails.Profile.Selfie);

            Assert.IsNotNull(activityDetails.UserProfile.Selfie);
            Assert.AreEqual(Convert.ToBase64String(Encoding.UTF8.GetBytes("selfie0123456789")), Convert.ToBase64String(activityDetails.UserProfile.Selfie.Data));

            var selfieValue = (YotiAttributeValue)activityDetails.Profile.Selfie.GetValue();
            Assert.AreEqual(Convert.ToBase64String(Encoding.UTF8.GetBytes("selfie0123456789")), Convert.ToBase64String(selfieValue.ToBytes()));

            Assert.AreEqual("phone_number0123456789", activityDetails.UserProfile.MobileNumber);
            Assert.AreEqual("phone_number0123456789", activityDetails.Profile.MobileNumber.GetStringValue());

            Assert.AreEqual(new DateTime(1980, 1, 1), activityDetails.UserProfile.DateOfBirth);
            Assert.AreEqual(new DateTime(1980, 1, 1), (DateTime)activityDetails.Profile.DateOfBirth.GetDateTimeValue());
        }

        [TestMethod]
        public void YotiClientEngine_PerformAmlCheck()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"on_fraud_list\":false,\"on_pep_list\":true,\"on_watch_list\":false}"
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.CreateStandardAmlProfile();

            AmlResult amlResult = engine.PerformAmlCheck(sdkId, keyPair, YotiConstants.DefaultYotiApiUrl, amlProfile);

            Assert.IsNotNull(amlResult);
            Assert.IsFalse(amlResult.IsOnFraudList());
            Assert.IsTrue(amlResult.IsOnPepList());
            Assert.IsFalse(amlResult.IsOnWatchList());
        }

        [TestMethod]
        public async Task YotiClientEngine_PerformAmlCheckAsync()
        {
            var keyPair = GetKeyPair();
            string sdkId = "fake-sdk-id";

            FakeHttpRequester httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"on_fraud_list\":true,\"on_pep_list\":false,\"on_watch_list\":false}"
                });
            });

            YotiClientEngine engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.CreateStandardAmlProfile();

            AmlResult amlResult = await engine.PerformAmlCheckAsync(sdkId, keyPair, YotiConstants.DefaultYotiApiUrl, amlProfile);

            Assert.IsNotNull(amlResult);
            Assert.IsTrue(amlResult.IsOnFraudList());
            Assert.IsFalse(amlResult.IsOnPepList());
            Assert.IsFalse(amlResult.IsOnWatchList());
        }
    }
}