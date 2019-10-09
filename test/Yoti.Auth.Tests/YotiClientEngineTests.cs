using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.Aml;
using Yoti.Auth.Exceptions;
using Yoti.Auth.ShareUrl;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class YotiClientEngineTests
    {
        private const string EncryptedToken = "b6H19bUCJhwh6WqQX/sEHWX9RP+A/ANr1fkApwA4Dp2nJQFAjrF9e6YCXhNBpAIhfHnN0iXubyXxXZMNwNMSQ5VOxkqiytrvPykfKQWHC6ypSbfy0ex8ihndaAXG5FUF+qcU8QaFPMy6iF3x0cxnY0Ij0kZj0Ng2t6oiNafb7AhT+VGXxbFbtZu1QF744PpWMuH0LVyBsAa5N5GJw2AyBrnOh67fWMFDKTJRziP5qCW2k4h5vJfiYr/EOiWKCB1d/zINmUm94ZffGXxcDAkq+KxhN1ZuNhGlJ2fKcFh7KxV0BqlUWPsIEiwS0r9CJ2o1VLbEs2U/hCEXaqseEV7L29EnNIinEPVbL4WR7vkF6zQCbK/cehlk2Qwda+VIATqupRO5grKZN78R9lBitvgilDaoE7JB/VFcPoljGQ48kX0wje1mviX4oJHhuO8GdFITS5LTbojGVQWT7LUNgAUe0W0j+FLHYYck3v84OhWTqads5/jmnnLkp9bdJSRuJF0e8pNdePnn2lgF+GIcyW/0kyGVqeXZrIoxnObLpF+YeUteRBKTkSGFcy7a/V/DLiJMPmH8UXDLOyv8TVt3ppzqpyUrLN2JVMbL5wZ4oriL2INEQKvw/boDJjZDGeRlu5m1y7vGDNBRDo64+uQM9fRUULPw+YkABNwC0DeShswzT00=";
        private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
        private static HttpRequestMessage _httpRequestMessage;
        private const string SdkId = "fake-sdk-id";

        [TestMethod]
        public void SharingFailure_ReturnsSharingFailure()
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"session_data\":null,\"receipt\":{\"receipt_id\": null,\"other_party_profile_content\": null,\"policy_uri\":null,\"personal_key\":null,\"remember_me_id\":null, \"sharing_outcome\":\"FAILURE\",\"timestamp\":\"2016-09-23T13:04:11Z\"}}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            var profileException = Assert.ThrowsExceptionAsync<YotiProfileException>(async () =>
            {
                await engine.GetActivityDetailsAsync(EncryptedToken, SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl));
            }).Result;

            Assert.IsTrue(profileException.Message.StartsWith("The share was not successful"));
        }

        [TestMethod]
        public void NullReceipt_ThrowsException()
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"session_data\":null,\"receipt\":null}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            var profileException = Assert.ThrowsExceptionAsync<YotiProfileException>(async () =>
            {
                await engine.GetActivityDetailsAsync(EncryptedToken, SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl));
            }).Result;

            Assert.IsTrue(profileException.Message.StartsWith("The receipt of the parsed response is null"));
        }

        [TestMethod]
        public void ParseProfile_Success()
        {
            const string wrappedReceiptKey = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=";
            const string otherPartyProfileContent = "ChCZAib1TBm9Q5GYfFrS1ep9EnAwQB5shpAPWLBgZgFgt6bCG3S5qmZHhrqUbQr3yL6yeLIDwbM7x4nuT/MYp+LDXgmFTLQNYbDTzrEzqNuO2ZPn9Kpg+xpbm9XtP7ZLw3Ep2BCmSqtnll/OdxAqLb4DTN4/wWdrjnFC+L/oQEECu646";
            const string rememberMeId = "remember_me_id0123456789";
            const string parentRememberMeId = "parent_remember_me_id0123456789";
            const string receiptId = "receipt_id_123";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"receipt\":{\"wrapped_receipt_key\": \"" + wrappedReceiptKey + "\",\"other_party_profile_content\": \"" + otherPartyProfileContent + "\",\"remember_me_id\":\"" + rememberMeId + "\",\"parent_remember_me_id\":\"" + parentRememberMeId + "\",\"receipt_id\":\"" + receiptId + "\", \"sharing_outcome\":\"SUCCESS\", \"timestamp\":\"2016-01-01T00:00:00Z\"}}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            ActivityDetails activityDetails = engine.GetActivityDetailsAsync(EncryptedToken, SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl)).Result;

            Assert.IsNotNull(activityDetails);

            Assert.IsNotNull(activityDetails.Profile);

            Assert.AreEqual(receiptId, activityDetails.ReceiptId);

            Assert.AreEqual(rememberMeId, activityDetails.RememberMeId);
            Assert.AreEqual(parentRememberMeId, activityDetails.ParentRememberMeId);

            Assert.AreEqual(new DateTime(2016, 1, 1, 0, 0, 0), activityDetails.Timestamp);

            Assert.IsNotNull(activityDetails.Profile.Selfie);
            Assert.AreEqual(Convert.ToBase64String(Encoding.UTF8.GetBytes("selfie0123456789")), Convert.ToBase64String(activityDetails.Profile.Selfie.GetValue().GetContent()));

            Assert.AreEqual("phone_number0123456789", activityDetails.Profile.MobileNumber.GetValue());

            Assert.AreEqual(new DateTime(1980, 1, 1), activityDetails.Profile.DateOfBirth.GetValue());
        }

        [TestMethod]
        public void ShouldAddAuthKeyHeaderToProfileRequest()
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"receipt\":{\"wrapped_receipt_key\": \"kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=\",\"other_party_profile_content\": \"ChCZAib1TBm9Q5GYfFrS1ep9EnAwQB5shpAPWLBgZgFgt6bCG3S5qmZHhrqUbQr3yL6yeLIDwbM7x4nuT/MYp+LDXgmFTLQNYbDTzrEzqNuO2ZPn9Kpg+xpbm9XtP7ZLw3Ep2BCmSqtnll/OdxAqLb4DTN4/wWdrjnFC+L/oQEECu646\",\"remember_me_id\":\"remember_me_id0123456789\",\"parent_remember_me_id\":\"parent_remember_me_id0123456789\",\"receipt_id\":\"receipt_id_123\", \"sharing_outcome\":\"SUCCESS\", \"timestamp\":\"2016-01-01T00:00:00Z\"}}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            ActivityDetails _ = engine.GetActivityDetailsAsync(EncryptedToken, SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl)).Result;

            Assert.IsTrue(_httpRequestMessage.Headers.Contains(Constants.Api.AuthKeyHeader));
        }

        [TestMethod]
        public void EmptyStringParentRememberMeIdIsHandled()
        {
            const string wrappedReceiptKey = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=";
            const string parentRememberMeId = "";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"receipt\":{\"wrapped_receipt_key\": \"" + wrappedReceiptKey + "\",\"parent_remember_me_id\":\"" + parentRememberMeId + "\", \"sharing_outcome\":\"SUCCESS\", \"timestamp\":\"2016-01-01T00:00:00Z\"}}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            ActivityDetails activityDetails = engine.GetActivityDetailsAsync(EncryptedToken, SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl)).Result;

            Assert.AreEqual(string.Empty, activityDetails.ParentRememberMeId);
        }

        [TestMethod]
        public void ShouldHandleMissingValuesInReceipt()
        {
            const string wrappedReceiptKey = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38AIu744RzzE+Dc4qkSF21VfzQKtJVILfOXu5xRc7MYa5k3zWhjiesg/gsrv7J4wDyyBpHIJB8TWXnubYMbSYQJjlsfwyxE9kGe0YI08pRo2Tiht0bfR5Z/YrhAk4UBvjp84D+oyug/1mtGhKphA4vgPhQ9/y2wcInYxju7Q6yzOsXGaRUXR38Tn2YmY9OBgjxiTnhoYJFP1X9YJkHeWMW0vxF1RHxgIVrpf7oRzdY1nq28qzRg5+wC7cjRpS2i/CKUAo0oVG4pbpXsaFhaTewStVC7UFtA77JHb3EnF4HcSWMnK5FM7GGkL9MMXQenh11NZHKPWXpux0nLZ6/vwffXZfsiyTIcFL/NajGN8C/hnNBljoQ+B3fzWbjcq5ueUOPwARZ1y38W83UwMynzkud/iEdHLaZIu4qUCRkfSxJg7Dc+O9/BdiffkOn2GyFmNjVeq754DCUypxzMkjYxokedN84nK13OU4afVyC7t5DDxAK/MqAc69NCBRLqMi5f8BMeOZfMcSWPGC9a2Qu8VgG125TuZT4+wIykUhGyj3Bb2/fdPsxwuKFR+E0uqs0ZKvcv1tkNRRtKYBqTacgGK9Yoehg12cyLrITLdjU1fmIDn4/vrhztN5w=";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
               HttpStatusCode.OK,
               "{\"receipt\":{\"wrapped_receipt_key\": \"" + wrappedReceiptKey + "\", \"sharing_outcome\":\"SUCCESS\", \"timestamp\":\"2016-01-01T00:00:00Z\"}}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            ActivityDetails activityDetails = engine.GetActivityDetailsAsync(EncryptedToken, SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl)).Result;

            Assert.IsNotNull(activityDetails.Profile);

            Assert.IsNull(activityDetails.ReceiptId);
            Assert.IsNull(activityDetails.RememberMeId);
            Assert.IsNull(activityDetails.ParentRememberMeId);
        }

        [TestMethod]
        public async Task PerformAmlCheckAsync()
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"on_fraud_list\":true,\"on_pep_list\":false,\"on_watch_list\":false}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            AmlResult amlResult = await engine.PerformAmlCheckAsync(
                SdkId, _keyPair,
                new Uri(Constants.Api.DefaultYotiApiUrl),
                amlProfile);

            Assert.IsNotNull(amlResult);
            Assert.IsTrue(amlResult.IsOnFraudList());
            Assert.IsFalse(amlResult.IsOnPepList());
            Assert.IsFalse(amlResult.IsOnWatchList());
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void AmlBadRequest_ThrowsException(HttpStatusCode httpStatusCode)
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                httpStatusCode,
                "{\"status\":\"bad\"");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            AmlProfile amlProfile = TestTools.Aml.CreateStandardAmlProfile();

            Assert.ThrowsExceptionAsync<AmlException>(async () =>
            {
                await engine.PerformAmlCheckAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl), amlProfile);
            });
        }

        [TestMethod]
        public async Task CreateShareURLAsync()
        {
            string shareUrl = @"https://yoti.com/shareurl";
            string refId = "NpdmVVGC-28356678-c236-4518-9de4-7a93009ccaf0-c5f92f2a-5539-453e-babc-9b06e1d6b7de";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"qrcode\":\"" + shareUrl + "\",\"ref_id\":\"" + refId + "\"}");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));
            DynamicScenario dynamicScenario = TestTools.ShareUrl.CreateStandardDynamicScenario();

            ShareUrlResult shareUrlResult = await engine.CreateShareURLAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl), dynamicScenario);

            Assert.IsNotNull(shareUrlResult);
            Assert.AreEqual(new Uri(shareUrl), shareUrlResult.Url);
            Assert.AreEqual(refId, shareUrlResult.RefId);
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.Unauthorized)]
        [DataRow(HttpStatusCode.InternalServerError)]
        [DataRow(HttpStatusCode.RequestTimeout)]
        [DataRow(HttpStatusCode.NotFound)]
        [DataRow(HttpStatusCode.Forbidden)]
        public void ShareURL_NonSuccessStatusCodes_ThrowException(HttpStatusCode httpStatusCode)
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                httpStatusCode,
                "{\"status\":\"bad\"");

            var engine = new YotiClientEngine(new HttpClient(handlerMock.Object));

            DynamicScenario dynamicScenario = TestTools.ShareUrl.CreateStandardDynamicScenario();

            var aggregateException = Assert.ThrowsException<AggregateException>(() =>
            {
                engine.CreateShareURLAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl), dynamicScenario).Wait();
            });

            Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DynamicShareException>(aggregateException));
        }

        private static Mock<HttpMessageHandler> SetupMockMessageHandler(HttpStatusCode httpStatusCode, string responseContent)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(new HttpResponseMessage()
               {
                   StatusCode = httpStatusCode,
                   Content = new StringContent(responseContent)
               })
               .Callback<HttpRequestMessage, CancellationToken>((http, token) => _httpRequestMessage = http)
               .Verifiable();
            return handlerMock;
        }
    }
}