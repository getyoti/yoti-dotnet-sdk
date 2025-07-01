using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.Exceptions;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests
{
	[TestClass]
	public class DigitalIdentityClientEngineTests
	{
		private const string EncryptedToken = "b6H19bUCJhwh6WqQX/sEHWX9RP+A/ANr1fkApwA4Dp2nJQFAjrF9e6YCXhNBpAIhfHnN0iXubyXxXZMNwNMSQ5VOxkqiytrvPykfKQWHC6ypSbfy0ex8ihndaAXG5FUF+qcU8QaFPMy6iF3x0cxnY0Ij0kZj0Ng2t6oiNafb7AhT+VGXxbFbtZu1QF744PpWMuH0LVyBsAa5N5GJw2AyBrnOh67fWMFDKTJRziP5qCW2k4h5vJfiYr/EOiWKCB1d/zINmUm94ZffGXxcDAkq+KxhN1ZuNhGlJ2fKcFh7KxV0BqlUWPsIEiwS0r9CJ2o1VLbEs2U/hCEXaqseEV7L29EnNIinEPVbL4WR7vkF6zQCbK/cehlk2Qwda+VIATqupRO5grKZN78R9lBitvgilDaoE7JB/VFcPoljGQ48kX0wje1mviX4oJHhuO8GdFITS5LTbojGVQWT7LUNgAUe0W0j+FLHYYck3v84OhWTqads5/jmnnLkp9bdJSRuJF0e8pNdePnn2lgF+GIcyW/0kyGVqeXZrIoxnObLpF+YeUteRBKTkSGFcy7a/V/DLiJMPmH8UXDLOyv8TVt3ppzqpyUrLN2JVMbL5wZ4oriL2INEQKvw/boDJjZDGeRlu5m1y7vGDNBRDo64+uQM9fRUULPw+YkABNwC0DeShswzT00=";
		private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
		private static HttpRequestMessage _httpRequestMessage;
		private const string SdkId = "fake-sdk-id";

		[TestMethod]
		public async Task CreateSessionAsyncShouldReturnCorrectValues()
		{
			string refId = "NpdmVVGC-28356678-c236-4518-9de4-7a93009ccaf0-c5f92f2a-5539-453e-babc-9b06e1d6b7de";

			Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
				HttpStatusCode.OK,
                "{\"id\":\"" + refId + "\",\"status\":\"SOME_STATUS\",\"expiry\":\"SOME_EXPIRY\",\"created\":\"SOME_CREATED\",\"updated\":\"SOME_UPDATED\",\"qrCode\":{\"id\":\"SOME_QRCODE_ID\"},\"receipt\":{\"id\":\"SOME_RECEIPT_ID\"}}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            ShareSessionRequest shareSessionRequest = TestTools.ShareSession.CreateStandardShareSessionRequest();

			ShareSessionResult shareSessionResult = await engine.CreateShareSessionAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), shareSessionRequest);

			Assert.IsNotNull(shareSessionResult);
			Assert.AreEqual(refId, shareSessionResult.Id);
		}

        [TestMethod]
        public async Task CreateQrCodeAsyncShouldReturnCorrectValues()
        {
            string qrCodeId = "test-qr-code-id";
            string qrCodeUri = "https://code.yoti.com/CAEaJDlkOGI4ZGFjLTEyMzQtNTY3OC05MDEyLWFiY2RlZjEyMzQ1Ng==";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"" + qrCodeId + "\",\"uri\":\"" + qrCodeUri + "\"}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            QrRequest qrRequest = TestTools.CreateQr.CreateQrStandard();
            string sessionId = "test-session-id";

            CreateQrResult result = await engine.CreateQrCodeAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), sessionId, qrRequest);

            Assert.IsNotNull(result);
            Assert.AreEqual(qrCodeId, result.Id);
            Assert.AreEqual(qrCodeUri, result.Uri);
        }

        [TestMethod]
        public async Task GetQrCodeAsyncShouldReturnCorrectValues()
        {
            string qrCodeId = "test-qr-code-id";
            string expiry = "2025-12-31T23:59:59Z";
            string policy = "test-policy";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"" + qrCodeId + "\",\"expiry\":\"" + expiry + "\",\"policy\":\"" + policy + "\",\"session\":{\"id\":\"session-123\",\"status\":\"ACTIVE\"},\"redirectUri\":\"https://example.com/redirect\"}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            GetQrCodeResult result = await engine.GetQrCodeAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), qrCodeId);

            Assert.IsNotNull(result);
            Assert.AreEqual(qrCodeId, result.Id);
            Assert.AreEqual(expiry, result.Expiry);
            Assert.AreEqual(policy, result.Policy);
        }

        [TestMethod]
        public async Task GetSessionShouldReturnCorrectValues()
        {
            string sessionId = "test-session-id";
            string status = "ACTIVE";
            string expiry = "2025-12-31T23:59:59Z";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"" + sessionId + "\",\"status\":\"" + status + "\",\"expiry\":\"" + expiry + "\",\"created\":\"2025-06-27T10:00:00Z\",\"updated\":\"2025-06-27T11:00:00Z\",\"qrCode\":{\"id\":\"qr-123\"},\"receipt\":{\"id\":\"receipt-123\"}}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            GetSessionResult result = await engine.GetSession(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), sessionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(sessionId, result.Id);
            Assert.AreEqual(status, result.Status);
            Assert.AreEqual(expiry, result.Expiry);
        }
        
        [DataTestMethod]
		[DataRow(HttpStatusCode.BadRequest)]
		[DataRow(HttpStatusCode.Unauthorized)]
		[DataRow(HttpStatusCode.InternalServerError)]
		[DataRow(HttpStatusCode.RequestTimeout)]
		[DataRow(HttpStatusCode.NotFound)]
		[DataRow(HttpStatusCode.Forbidden)]
		public void CreateShareSessionNonSuccessStatusCodesShouldThrowException(HttpStatusCode httpStatusCode)
		{
			Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
				httpStatusCode,
				"{\"status\":\"bad\"");

			var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            ShareSessionRequest shareSessionRequest = TestTools.ShareSession.CreateStandardShareSessionRequest();

			var aggregateException = Assert.ThrowsException<AggregateException>(() =>
			{
				engine.CreateShareSessionAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiApiUrl), shareSessionRequest).Wait();
			});

			Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DigitalIdentityException>(aggregateException));
		}

		[DataTestMethod]
		[DataRow(HttpStatusCode.BadRequest)]
		[DataRow(HttpStatusCode.Unauthorized)]
		[DataRow(HttpStatusCode.InternalServerError)]
		[DataRow(HttpStatusCode.RequestTimeout)]
		[DataRow(HttpStatusCode.NotFound)]
		[DataRow(HttpStatusCode.Forbidden)]
		public void GetShareReceiptNonSuccessStatusCodesShouldThrowException(HttpStatusCode httpStatusCode)
		{
			Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
				httpStatusCode,
				"{\"status\":\"bad\"}");

			var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            Uri apiUrl = new Uri("https://example.com/api");
            string receiptId = "some_receiptid";

			var aggregateException = Assert.ThrowsException<AggregateException>(() =>
			{
				engine.GetShareReceipt(SdkId, _keyPair, apiUrl, receiptId).Wait();
			});

			Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<Exception>(aggregateException));
		}

        [DataTestMethod]
		[DataRow(HttpStatusCode.BadRequest)]
		[DataRow(HttpStatusCode.Unauthorized)]
		[DataRow(HttpStatusCode.InternalServerError)]
		[DataRow(HttpStatusCode.RequestTimeout)]
		[DataRow(HttpStatusCode.NotFound)]
		[DataRow(HttpStatusCode.Forbidden)]
		public void CreateQrCodeAsyncNonSuccessStatusCodesShouldThrowException(HttpStatusCode httpStatusCode)
		{
			Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
				httpStatusCode,
				"{\"status\":\"bad\"}");

			var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            QrRequest qrRequest = TestTools.CreateQr.CreateQrStandard();
            string sessionId = "test-session-id";

			var aggregateException = Assert.ThrowsException<AggregateException>(() =>
			{
				engine.CreateQrCodeAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), sessionId, qrRequest).Wait();
			});

			Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DigitalIdentityException>(aggregateException));
		}

        [DataTestMethod]
		[DataRow(HttpStatusCode.BadRequest)]
		[DataRow(HttpStatusCode.Unauthorized)]
		[DataRow(HttpStatusCode.InternalServerError)]
		[DataRow(HttpStatusCode.RequestTimeout)]
		[DataRow(HttpStatusCode.NotFound)]
		[DataRow(HttpStatusCode.Forbidden)]
		public void GetQrCodeAsyncNonSuccessStatusCodesShouldThrowException(HttpStatusCode httpStatusCode)
		{
			Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
				httpStatusCode,
				"{\"status\":\"bad\"}");

			var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            string qrCodeId = "test-qr-code-id";

			var aggregateException = Assert.ThrowsException<AggregateException>(() =>
			{
				engine.GetQrCodeAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), qrCodeId).Wait();
			});

			Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DigitalIdentityException>(aggregateException));
		}

        [DataTestMethod]
		[DataRow(HttpStatusCode.BadRequest)]
		[DataRow(HttpStatusCode.Unauthorized)]
		[DataRow(HttpStatusCode.InternalServerError)]
		[DataRow(HttpStatusCode.RequestTimeout)]
		[DataRow(HttpStatusCode.NotFound)]
		[DataRow(HttpStatusCode.Forbidden)]
		public void GetSessionNonSuccessStatusCodesShouldThrowException(HttpStatusCode httpStatusCode)
		{
			Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
				httpStatusCode,
				"{\"status\":\"bad\"}");

			var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            string sessionId = "test-session-id";

			var aggregateException = Assert.ThrowsException<AggregateException>(() =>
			{
				engine.GetSession(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), sessionId).Wait();
			});

			Assert.IsTrue(TestTools.Exceptions.IsExceptionInAggregateException<DigitalIdentityException>(aggregateException));
		}

		private static Mock<HttpMessageHandler> SetupMockMessageHandler(HttpStatusCode httpStatusCode, string responseContent)
		{
			var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Loose);
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

        [TestMethod]
        public void ConstructorShouldAcceptHttpClient()
        {
            var httpClient = new HttpClient();
            
            var engine = new DigitalIdentityClientEngine(httpClient);
            
            Assert.IsNotNull(engine);
        }

        [TestMethod]
        public async Task GetShareReceiptShouldThrowWhenReceiptIdIsEmpty()
        {
            var httpClient = new HttpClient();
            var engine = new DigitalIdentityClientEngine(httpClient);
            Uri apiUrl = new Uri("https://example.com/api");

            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                engine.GetShareReceipt(SdkId, _keyPair, apiUrl, ""));
        }

        [TestMethod]
        public async Task CreateShareSessionAsyncShouldHandleEmptyResponse()
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            ShareSessionRequest shareSessionRequest = TestTools.ShareSession.CreateStandardShareSessionRequest();

            ShareSessionResult shareSessionResult = await engine.CreateShareSessionAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), shareSessionRequest);

            Assert.IsNotNull(shareSessionResult);
        }

        [TestMethod]
        public async Task CreateQrCodeAsyncShouldHandleEmptyResponse()
        {
            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            QrRequest qrRequest = TestTools.CreateQr.CreateQrStandard();
            string sessionId = "test-session-id";

            CreateQrResult result = await engine.CreateQrCodeAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), sessionId, qrRequest);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetQrCodeAsyncShouldHandleEmptyResponse()
        {
            string qrCodeId = "test-qr-code-id";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            GetQrCodeResult result = await engine.GetQrCodeAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), qrCodeId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetSessionShouldHandleEmptyResponse()
        {
            string sessionId = "test-session-id";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            GetSessionResult result = await engine.GetSession(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), sessionId);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task CreateShareSessionAsyncShouldHandleSpecialCharactersInId()
        {
            string refId = "session-with-special-chars-123456";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"" + refId + "\",\"status\":\"ACTIVE\"}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));
            ShareSessionRequest shareSessionRequest = TestTools.ShareSession.CreateStandardShareSessionRequest();

            ShareSessionResult shareSessionResult = await engine.CreateShareSessionAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), shareSessionRequest);

            Assert.IsNotNull(shareSessionResult);
            Assert.AreEqual(refId, shareSessionResult.Id);
        }

        [TestMethod]
        public async Task GetQrCodeAsyncShouldHandleNullSessionInResponse()
        {
            string qrCodeId = "test-qr-code-id";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"" + qrCodeId + "\",\"session\":null}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            GetQrCodeResult result = await engine.GetQrCodeAsync(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), qrCodeId);

            Assert.IsNotNull(result);
            Assert.AreEqual(qrCodeId, result.Id);
            Assert.IsNull(result.Session);
        }

        [TestMethod]
        public async Task GetSessionShouldHandleNullQrCodeAndReceiptInResponse()
        {
            string sessionId = "test-session-id";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"" + sessionId + "\",\"qrCode\":null,\"receipt\":null}");

            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            GetSessionResult result = await engine.GetSession(SdkId, _keyPair, new Uri(Constants.Api.DefaultYotiShareApiUrl), sessionId);

            Assert.IsNotNull(result);
            Assert.AreEqual(sessionId, result.Id);
            Assert.IsNull(result.QrCode);
            Assert.IsNull(result.Receipt);
        }
	}
}
