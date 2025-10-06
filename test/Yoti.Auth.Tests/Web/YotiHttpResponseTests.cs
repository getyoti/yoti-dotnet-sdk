using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.Web
{
    [TestClass]
    public class YotiHttpResponseTests
    {
        [TestMethod]
        public void ShouldCreateFromHttpResponseMessage()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("X-Request-ID", "test-request-id-123");
            httpResponse.Headers.Add("Custom-Header", "custom-value");

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual(testData, response.Data);
            Assert.AreEqual("test-request-id-123", response.RequestId);
            Assert.AreEqual("custom-value", response.GetHeaderValue("Custom-Header"));
        }

        [TestMethod]
        public void ShouldHandleXRequestIdWithDifferentCasing()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("X-Request-Id", "test-request-id-456"); // Different casing

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual("test-request-id-456", response.RequestId);
        }

        [TestMethod]
        public void ShouldReturnNullWhenRequestIdNotPresent()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("Other-Header", "other-value");

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.IsNull(response.RequestId);
        }

        [TestMethod]
        public void ShouldGetHeaderValueCaseInsensitive()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Content = new StringContent("test content");
            httpResponse.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual("application/json", response.GetHeaderValue("content-type"));
            Assert.AreEqual("application/json", response.GetHeaderValue("Content-Type"));
            Assert.AreEqual("application/json", response.GetHeaderValue("CONTENT-TYPE"));
        }

        [TestMethod]
        public void ShouldGetMultipleHeaderValues()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("Accept", new[] { "application/json", "text/html" });

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            var acceptValues = response.GetHeaderValues("Accept").ToList();
            Assert.AreEqual(2, acceptValues.Count);
            Assert.IsTrue(acceptValues.Contains("application/json"));
            Assert.IsTrue(acceptValues.Contains("text/html"));
        }

        [TestMethod]
        public void ShouldReturnEmptyEnumerableForNonExistentHeader()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            var values = response.GetHeaderValues("Non-Existent-Header");
            Assert.IsNotNull(values);
            Assert.AreEqual(0, values.Count());
        }

        [TestMethod]
        public void ShouldImplicitlyConvertToDataType()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Act
            string implicitData = response; // Implicit conversion

            // Assert
            Assert.AreEqual(testData, implicitData);
        }

        [TestMethod]
        public void ShouldIncludeBothResponseAndContentHeaders()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("Response-Header", "response-value");
            httpResponse.Content = new StringContent("content");
            httpResponse.Content.Headers.Add("Content-Header", "content-value");

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual("response-value", response.GetHeaderValue("Response-Header"));
            Assert.AreEqual("content-value", response.GetHeaderValue("Content-Header"));
        }

        [TestMethod]
        public void ShouldHandleRequestIdWithVariousCasings()
        {
            // Arrange
            var testData = "test data";
            const string expectedRequestId = "req-ABC123-def456";

            // Test different casing variations of X-Request-ID
            var testCases = new[]
            {
                "X-Request-ID",
                "X-Request-Id", 
                "x-request-id",
                "X-REQUEST-ID",
                "x-Request-Id"
            };

            foreach (var headerName in testCases)
            {
                var httpResponse = new HttpResponseMessage();
                httpResponse.Headers.Add(headerName, expectedRequestId);

                // Act
                var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

                // Assert
                Assert.AreEqual(expectedRequestId, response.RequestId, 
                    $"Failed for header name: {headerName}");
            }
        }

        [TestMethod]
        public void ShouldHandleCommonYotiHeaders()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("X-Request-ID", "req-12345");
            httpResponse.Headers.Add("X-Yoti-Session-ID", "session-abc123");
            httpResponse.Headers.Add("X-RateLimit-Limit", "1000");
            httpResponse.Headers.Add("X-RateLimit-Remaining", "999");
            httpResponse.Headers.Add("X-RateLimit-Reset", "1640995200");
            httpResponse.Headers.Add("Server", "Yoti-API-Gateway/2.1");

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual("req-12345", response.RequestId);
            Assert.AreEqual("session-abc123", response.GetHeaderValue("X-Yoti-Session-ID"));
            Assert.AreEqual("1000", response.GetHeaderValue("X-RateLimit-Limit"));
            Assert.AreEqual("999", response.GetHeaderValue("X-RateLimit-Remaining"));
            Assert.AreEqual("1640995200", response.GetHeaderValue("X-RateLimit-Reset"));
            Assert.AreEqual("Yoti-API-Gateway/2.1", response.GetHeaderValue("Server"));
        }

        [TestMethod]
        public void ShouldHandleSecurityHeaders()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("X-Request-ID", "req-security-test");
            httpResponse.Headers.Add("Strict-Transport-Security", "max-age=31536000; includeSubDomains");
            httpResponse.Headers.Add("X-Content-Type-Options", "nosniff");
            httpResponse.Headers.Add("X-Frame-Options", "DENY");
            httpResponse.Headers.Add("X-XSS-Protection", "1; mode=block");

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual("req-security-test", response.RequestId);
            Assert.AreEqual("max-age=31536000; includeSubDomains", response.GetHeaderValue("Strict-Transport-Security"));
            Assert.AreEqual("nosniff", response.GetHeaderValue("X-Content-Type-Options"));
            Assert.AreEqual("DENY", response.GetHeaderValue("X-Frame-Options"));
            Assert.AreEqual("1; mode=block", response.GetHeaderValue("X-XSS-Protection"));
        }

        [TestMethod]
        public void ShouldHandleCacheHeaders()
        {
            // Arrange
            var testData = "test data";
            var httpResponse = new HttpResponseMessage();
            httpResponse.Headers.Add("X-Request-ID", "req-cache-test");
            httpResponse.Headers.Add("Cache-Control", "no-cache, no-store, must-revalidate");
            httpResponse.Headers.Add("Pragma", "no-cache");
            httpResponse.Headers.Add("ETag", @"""session-abc123-v1""");
            // Set Expires header properly
            httpResponse.Content = new StringContent("test");
            httpResponse.Content.Headers.Expires = DateTimeOffset.MinValue;

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual("req-cache-test", response.RequestId);
            Assert.IsTrue(response.GetHeaderValue("Cache-Control").Contains("no-cache"));
            Assert.IsTrue(response.GetHeaderValue("Cache-Control").Contains("no-store"));
            Assert.IsTrue(response.GetHeaderValue("Cache-Control").Contains("must-revalidate"));
            Assert.AreEqual("no-cache", response.GetHeaderValue("Pragma"));
            Assert.AreEqual(@"""session-abc123-v1""", response.GetHeaderValue("ETag"));
            Assert.IsNotNull(response.GetHeaderValue("Expires")); // Just check it exists
        }

        [TestMethod]
        public void ShouldHandleErrorResponseHeaders()
        {
            // Arrange
            var testData = "error data";
            var httpResponse = new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest);
            httpResponse.Headers.Add("X-Request-ID", "req-error-400");
            httpResponse.Headers.Add("X-Error-Code", "INVALID_REQUEST");
            httpResponse.Headers.Add("X-Error-Message", "Missing required parameter");
            httpResponse.Headers.Add("Retry-After", "30");

            // Act
            var response = YotiHttpResponse<string>.FromHttpResponse(testData, httpResponse);

            // Assert
            Assert.AreEqual("req-error-400", response.RequestId);
            Assert.AreEqual("INVALID_REQUEST", response.GetHeaderValue("X-Error-Code"));
            Assert.AreEqual("Missing required parameter", response.GetHeaderValue("X-Error-Message"));
            Assert.AreEqual("30", response.GetHeaderValue("Retry-After"));
        }
    }
}
