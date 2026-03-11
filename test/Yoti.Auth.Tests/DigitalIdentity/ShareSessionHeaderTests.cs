using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Yoti.Auth.DigitalIdentity;
using Yoti.Auth.DigitalIdentity.Policy;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests.DigitalIdentity
{
    [TestClass]
    public class ShareSessionHeaderTests
    {
        private const string SdkId = "test-sdk-id";

        private static Mock<HttpMessageHandler> SetupMockMessageHandler(HttpStatusCode httpStatusCode, string responseContent, Dictionary<string, string> headers = null)
        {
            var response = new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(responseContent)
            };

            // Add custom headers if provided
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    response.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }
            }

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(response)
               .Verifiable();
            return handlerMock;
        }

        [TestMethod]
        public void CreateShareSession_ShouldReturnXRequestIdHeader()
        {
            // Arrange
            string requestId = "test-request-id-12345";
            var headers = new Dictionary<string, string>
            {
                { "X-Request-ID", requestId },
                { "Content-Type", "application/json" }
            };

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"session-123\",\"status\":\"CREATED\"}",
                headers);

            var httpClient = new HttpClient(handlerMock.Object);
            
            var yotiClient = new Auth.DigitalIdentityClient(
                httpClient,
                SdkId,
                KeyPair.Get());

            var policy = new PolicyBuilder()
                .Build();
                
            var sessionRequest = new ShareSessionRequestBuilder()
                .WithPolicy(policy)
                .WithRedirectUri("https://example.com/callback")
                .Build();

            // Act
            var result = yotiClient.CreateShareSession(sessionRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Headers);
            
            // Check if X-Request-ID header exists
            var xRequestId = result.GetHeaderValue("X-Request-ID");
            Assert.IsNotNull(xRequestId);
            Assert.AreEqual(requestId, xRequestId);
            
            // Also verify through the convenience property
            Assert.AreEqual(requestId, result.RequestId);
            
            // Print all headers for debugging
            Console.WriteLine("=== Response Headers ===");
            foreach (var header in result.Headers)
            {
                Console.WriteLine($"{header.Key}: {string.Join(", ", header.Value)}");
            }
            Console.WriteLine("========================");
            Console.WriteLine($"✓ X-Request-ID verified: {result.RequestId}");
        }

        [TestMethod]
        public void CreateShareSession_ShouldReturnAllExpectedHeaders()
        {
            // Arrange
            var expectedHeaders = new Dictionary<string, string>
            {
                { "X-Request-ID", "req-123456" },
                { "Content-Type", "application/json" },
                { "X-Yoti-SDK-Version", "1.0.0" },
                { "Cache-Control", "no-cache" }
            };

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"session-456\",\"status\":\"CREATED\"}",
                expectedHeaders);

            var httpClient = new HttpClient(handlerMock.Object);
            
            var yotiClient = new Auth.DigitalIdentityClient(
                httpClient,
                SdkId,
                KeyPair.Get());

            var policy = new PolicyBuilder()
                .Build();
                
            var sessionRequest = new ShareSessionRequestBuilder()
                .WithPolicy(policy)
                .WithRedirectUri("https://example.com/callback")
                .Build();

            // Act
            var result = yotiClient.CreateShareSession(sessionRequest);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Headers);
            
            // Verify all expected headers are present
            foreach (var expectedHeader in expectedHeaders)
            {
                var headerValue = result.GetHeaderValue(expectedHeader.Key);
                Assert.IsNotNull(headerValue, $"Header '{expectedHeader.Key}' not found");
                Console.WriteLine($"✓ Header '{expectedHeader.Key}' found: {headerValue}");
            }
            
            // Specifically verify X-Request-ID
            Assert.AreEqual("req-123456", result.RequestId);
            Console.WriteLine($"\n✓ X-Request-ID verified: {result.RequestId}");
        }

        [TestMethod]
        public void CreateShareSession_HeadersShouldBeCaseInsensitive()
        {
            // Arrange
            string requestId = "case-test-id";
            var headers = new Dictionary<string, string>
            {
                { "x-request-id", requestId }, // lowercase
                { "Content-Type", "application/json" }
            };

            Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(
                HttpStatusCode.OK,
                "{\"id\":\"session-789\",\"status\":\"CREATED\"}",
                headers);

            var httpClient = new HttpClient(handlerMock.Object);
            
            var yotiClient = new Auth.DigitalIdentityClient(
                httpClient,
                SdkId,
                KeyPair.Get());

            var policy = new PolicyBuilder()
                .Build();
                
            var sessionRequest = new ShareSessionRequestBuilder()
                .WithPolicy(policy)
                .WithRedirectUri("https://example.com/callback")
                .Build();

            // Act
            var result = yotiClient.CreateShareSession(sessionRequest);

            // Assert - should find header regardless of case
            Assert.IsNotNull(result.RequestId);
            Assert.AreEqual(requestId, result.RequestId);
            
            // Test various case combinations
            Assert.AreEqual(requestId, result.GetHeaderValue("X-Request-ID"));
            Assert.AreEqual(requestId, result.GetHeaderValue("x-request-id"));
            Assert.AreEqual(requestId, result.GetHeaderValue("X-REQUEST-ID"));
            
            Console.WriteLine($"✓ Case-insensitive header lookup working: {result.RequestId}");
        }

        [Ignore("Complex test requiring multiple HTTP mocks - needs refactoring")]
        [TestMethod]
        public async Task GetShareReceipt_ShouldReturnXRequestIdHeader()
        {
            // Arrange
            string requestId = "receipt-request-id-789";
            var headers = new Dictionary<string, string>
            {
                { "X-Request-ID", requestId }
            };

            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"id\":\"receipt-123\",\"userContent\":{}}")
            };

            // Add custom headers
            foreach (var header in headers)
            {
                response.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(response)
               .Verifiable();

            var httpClient = new HttpClient(handlerMock.Object);
            
            var yotiClient = new Auth.DigitalIdentityClient(
                httpClient,
                SdkId,
                KeyPair.Get());

            // Act
            var result = await yotiClient.GetShareReceiptAsync("test-receipt-id");

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Headers);
            
            var xRequestId = result.GetHeaderValue("X-Request-ID");
            Assert.IsNotNull(xRequestId);
            Assert.AreEqual(requestId, xRequestId);
            Assert.AreEqual(requestId, result.RequestId);
            
            Console.WriteLine($"✓ GetShareReceipt X-Request-ID: {result.RequestId}");
        }
    }
}
