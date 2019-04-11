using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;
using Yoti.Auth.Tests.TestTools;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiClientEngineTests
    {
        private const string Token = "NpdmVVGC-28356678-c236-4518-9de4-7a93009ccaf0-c5f92f2a-5539-453e-babc-9b06e1d6b7de";
        private const string EncryptedToken = "b6H19bUCJhwh6WqQX/sEHWX9RP+A/ANr1fkApwA4Dp2nJQFAjrF9e6YCXhNBpAIhfHnN0iXubyXxXZMNwNMSQ5VOxkqiytrvPykfKQWHC6ypSbfy0ex8ihndaAXG5FUF+qcU8QaFPMy6iF3x0cxnY0Ij0kZj0Ng2t6oiNafb7AhT+VGXxbFbtZu1QF744PpWMuH0LVyBsAa5N5GJw2AyBrnOh67fWMFDKTJRziP5qCW2k4h5vJfiYr/EOiWKCB1d/zINmUm94ZffGXxcDAkq+KxhN1ZuNhGlJ2fKcFh7KxV0BqlUWPsIEiwS0r9CJ2o1VLbEs2U/hCEXaqseEV7L29EnNIinEPVbL4WR7vkF6zQCbK/cehlk2Qwda+VIATqupRO5grKZN78R9lBitvgilDaoE7JB/VFcPoljGQ48kX0wje1mviX4oJHhuO8GdFITS5LTbojGVQWT7LUNgAUe0W0j+FLHYYck3v84OhWTqads5/jmnnLkp9bdJSRuJF0e8pNdePnn2lgF+GIcyW/0kyGVqeXZrIoxnObLpF+YeUteRBKTkSGFcy7a/V/DLiJMPmH8UXDLOyv8TVt3ppzqpyUrLN2JVMbL5wZ4oriL2INEQKvw/boDJjZDGeRlu5m1y7vGDNBRDo64+uQM9fRUULPw+YkABNwC0DeShswzT00=";
        private readonly AsymmetricCipherKeyPair _keyPair = GetKeyPair();
        private const string SdkId = "fake-sdk-id";

        private static AsymmetricCipherKeyPair GetKeyPair()
        {
            using (StreamReader stream = File.OpenText("test-key.pem"))
            {
                return CryptoEngine.LoadRsaKey(stream);
            }
        }

        [TestMethod]
        public void SharingFailure_ReturnsSharingFailure()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"session_data\":null,\"receipt\":{\"receipt_id\": null,\"other_party_profile_content\": null,\"policy_uri\":null,\"personal_key\":null,\"remember_me_id\":null, \"sharing_outcome\":\"FAILURE\",\"timestamp\":\"2016-09-23T13:04:11Z\"}}"
                }));

            var engine = new YotiClientEngine(httpRequester);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                engine.GetActivityDetails(EncryptedToken, SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl);
            });

            Assert.IsTrue(Exceptions.IsExceptionInAggregateException<YotiProfileException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.StartsWith("The share was not successful"));
        }

        [TestMethod]
        public void NullReceipt_ThrowsException()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"session_data\":null,\"receipt\":null}"
                }));

            var engine = new YotiClientEngine(httpRequester);

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                engine.GetActivityDetails(EncryptedToken, SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl);
            });

            Assert.IsTrue(Exceptions.IsExceptionInAggregateException<YotiProfileException>(aggregateException));
            Assert.IsTrue(aggregateException.InnerException.Message.StartsWith("The receipt of the parsed response is null"));
        }

        [TestMethod]
        public void ParseProfile_Success()
        {
            const string wrappedReceiptKey = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=";
            const string otherPartyProfileContent = "ChCZAib1TBm9Q5GYfFrS1ep9EnAwQB5shpAPWLBgZgFgt6bCG3S5qmZHhrqUbQr3yL6yeLIDwbM7x4nuT/MYp+LDXgmFTLQNYbDTzrEzqNuO2ZPn9Kpg+xpbm9XtP7ZLw3Ep2BCmSqtnll/OdxAqLb4DTN4/wWdrjnFC+L/oQEECu646";
            const string rememberMeId = "remember_me_id0123456789";
            const string parentRememberMeId = "parent_remember_me_id0123456789";
            const string receiptId = "receipt_id_123";

            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                Assert.AreEqual("/api/v1/profile/" + Token, uri.AbsolutePath);

                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"receipt\":{\"wrapped_receipt_key\": \"" + wrappedReceiptKey + "\",\"other_party_profile_content\": \"" + otherPartyProfileContent + "\",\"remember_me_id\":\"" + rememberMeId + "\",\"parent_remember_me_id\":\"" + parentRememberMeId + "\",\"receipt_id\":\"" + receiptId + "\", \"sharing_outcome\":\"SUCCESS\", \"timestamp\":\"2016-01-01T00:00:00Z\"}}"
                });
            });

            var engine = new YotiClientEngine(httpRequester);

            ActivityDetails activityDetails = engine.GetActivityDetails(EncryptedToken, SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl);

            Assert.IsNotNull(activityDetails);

            Assert.IsNotNull(activityDetails.Profile);

            Assert.AreEqual(receiptId, activityDetails.ReceiptId);

            Assert.AreEqual(rememberMeId, activityDetails.RememberMeId);
            Assert.AreEqual(new DateTime(2016, 1, 1, 0, 0, 0), activityDetails.Timestamp);

            Assert.IsNotNull(activityDetails.Profile.Selfie);
            Assert.AreEqual(Convert.ToBase64String(Encoding.UTF8.GetBytes("selfie0123456789")), Convert.ToBase64String(activityDetails.Profile.Selfie.GetValue().GetContent()));

            Assert.AreEqual("phone_number0123456789", activityDetails.Profile.MobileNumber.GetValue());

            Assert.AreEqual(new DateTime(1980, 1, 1), activityDetails.Profile.DateOfBirth.GetValue());
        }

        [TestMethod]
        public void EmptyStringParentRememberMeIdIsHandled()
        {
            const string wrappedReceiptKey = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=";
            const string parentRememberMeId = "";

            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"receipt\":{\"wrapped_receipt_key\": \"" + wrappedReceiptKey + "\",\"parent_remember_me_id\":\"" + parentRememberMeId + "\", \"sharing_outcome\":\"SUCCESS\", \"timestamp\":\"2016-01-01T00:00:00Z\"}}"
                });
            });

            var engine = new YotiClientEngine(httpRequester);

            ActivityDetails activityDetails = engine.GetActivityDetails(EncryptedToken, SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl);

            Assert.AreEqual(string.Empty, activityDetails.ParentRememberMeId);
        }

        [TestMethod]
        public void ShouldHandleMissingValuesInReceipt()
        {
            const string wrappedReceiptKey = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=";

            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
            {
                return Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"receipt\":{\"wrapped_receipt_key\": \"" + wrappedReceiptKey + "\", \"sharing_outcome\":\"SUCCESS\", \"timestamp\":\"2016-01-01T00:00:00Z\"}}"
                });
            });

            var engine = new YotiClientEngine(httpRequester);

            ActivityDetails activityDetails = engine.GetActivityDetails(EncryptedToken, SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl);

            Assert.IsNotNull(activityDetails.Profile);

            Assert.IsNull(activityDetails.ReceiptId);
            Assert.IsNull(activityDetails.RememberMeId);
            Assert.IsNull(activityDetails.ParentRememberMeId);
        }

        [TestMethod]
        public void PerformAmlCheck()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"on_fraud_list\":false,\"on_pep_list\":true,\"on_watch_list\":false}"
                }));

            var engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            AmlResult amlResult = engine.PerformAmlCheck(SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl, amlProfile);

            Assert.IsNotNull(amlResult);
            Assert.IsFalse(amlResult.IsOnFraudList());
            Assert.IsTrue(amlResult.IsOnPepList());
            Assert.IsFalse(amlResult.IsOnWatchList());
        }

        [TestMethod]
        public async Task PerformAmlCheckAsync()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = true,
                    StatusCode = 200,
                    Content = "{\"on_fraud_list\":true,\"on_pep_list\":false,\"on_watch_list\":false}"
                }));

            var engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            AmlResult amlResult = await engine.PerformAmlCheckAsync(SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl, amlProfile);

            Assert.IsNotNull(amlResult);
            Assert.IsTrue(amlResult.IsOnFraudList());
            Assert.IsFalse(amlResult.IsOnPepList());
            Assert.IsFalse(amlResult.IsOnWatchList());
        }

        [TestMethod]
        public void AmlBadRequest_ThrowsException()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    Content = "{Content}"
                }));

            var engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                engine.PerformAmlCheck(SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl, amlProfile);
            });

            Assert.IsTrue(Exceptions.IsExceptionInAggregateException<AmlException>(aggregateException));
        }

        [TestMethod]
        public void AmlRequest_Unauthorized_ThrowsException()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.Unauthorized,
                    Content = "{Content}"
                }));

            var engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                engine.PerformAmlCheck(SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl, amlProfile);
            });

            Assert.IsTrue(Exceptions.IsExceptionInAggregateException<AmlException>(aggregateException));
        }

        [TestMethod]
        public void AmlRequest_InternalServerError_ThrowsException()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.InternalServerError,
                    Content = "{Content}"
                }));

            var engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            AggregateException aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                engine.PerformAmlCheck(SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl, amlProfile);
            });

            Assert.IsTrue(Exceptions.IsExceptionInAggregateException<AmlException>(aggregateException));
        }

        [TestMethod]
        public void AmlRequest_Forbidden_ThrowsException()
        {
            var httpRequester = new FakeHttpRequester((httpClient, httpMethod, uri, headers, byteContent) =>
                Task.FromResult(new Response
                {
                    Success = false,
                    StatusCode = (int)HttpStatusCode.Forbidden,
                    Content = "{Content}"
                }));

            var engine = new YotiClientEngine(httpRequester);
            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            AggregateException aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                engine.PerformAmlCheck(SdkId, _keyPair, Constants.Web.DefaultYotiApiUrl, amlProfile);
            });

            Assert.IsTrue(Exceptions.IsExceptionInAggregateException<AmlException>(aggregateException));
        }
    }
}