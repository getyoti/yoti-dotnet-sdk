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
        public void TestGetShareReceipt()
        {
            
            Uri apiUrl = new Uri("https://example.com/api");
            string receiptId = "some_receiptid";
            string refId = "NpdmVVGC-28356678-c236-4518-9de4-7a93009ccaf0-c5f92f2a-5539-453e-babc-9b06e1d6b7de";

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"" + refId + "\",\"status\":\"SOME_STATUS\",\"expiry\":\"SOME_EXPIRY\",\"created\":\"SOME_CREATED\",\"updated\":\"SOME_UPDATED\",\"qrCode\":{\"id\":\"SOME_QRCODE_ID\"},\"receipt\":{\"id\":\"SOME_RECEIPT_ID\"}}");


            var engine = new DigitalIdentityClientEngine(new HttpClient(handlerMock.Object));

            Assert.ThrowsException<AggregateException>(() =>
            {
                SharedReceiptResponse response =  engine.GetShareReceipt(SdkId, _keyPair, apiUrl, receiptId).Result;
            });

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
