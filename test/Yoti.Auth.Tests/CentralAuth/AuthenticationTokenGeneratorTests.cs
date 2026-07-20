using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using Yoti.Auth.CentralAuth;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Tests.CentralAuth
{
    [TestClass]
    public class AuthenticationTokenGeneratorTests
    {
        private readonly AsymmetricCipherKeyPair _keyPair = KeyPair.Get();
        private const string _sdkId = "test-sdk-id";
        private const string _scope = "identity:create";

        private AuthenticationTokenGenerator BuildGenerator()
        {
            return new AuthenticationTokenGeneratorBuilder()
                .WithSdkId(_sdkId)
                .WithKey(_keyPair)
                .WithScope(_scope)
                .Build();
        }

        private static Mock<HttpMessageHandler> SetupMockHandler(HttpStatusCode statusCode, string responseBody)
        {
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(responseBody)
                })
                .Verifiable();
            return handlerMock;
        }

        [TestMethod]
        public async Task GetToken_SuccessfulResponseShouldReturnToken()
        {
            var handlerMock = SetupMockHandler(
                HttpStatusCode.OK,
                "{\"access_token\":\"abc123\",\"expires_in\":3600,\"token_type\":\"Bearer\",\"scope\":\"identity:create\"}");

            var generator = BuildGenerator();
            var response = await generator.GetToken(new HttpClient(handlerMock.Object));

            Assert.IsNotNull(response);
            Assert.AreEqual("abc123", response.AccessToken);
            Assert.AreEqual(3600, response.ExpiresIn);
            Assert.AreEqual("Bearer", response.TokenType);
        }

        [TestMethod]
        public async Task GetToken_UnauthorizedShouldThrow()
        {
            var handlerMock = SetupMockHandler(HttpStatusCode.Unauthorized, "Unauthorized");

            var generator = BuildGenerator();
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() =>
                generator.GetToken(new HttpClient(handlerMock.Object)));
        }

        [TestMethod]
        public async Task GetToken_ServerErrorShouldThrow()
        {
            var handlerMock = SetupMockHandler(HttpStatusCode.InternalServerError, "Server Error");

            var generator = BuildGenerator();
            await Assert.ThrowsExceptionAsync<HttpRequestException>(() =>
                generator.GetToken(new HttpClient(handlerMock.Object)));
        }

        [TestMethod]
        public async Task GetToken_NullBodyShouldThrow()
        {
            var handlerMock = SetupMockHandler(HttpStatusCode.OK, "null");

            var generator = BuildGenerator();
            await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                generator.GetToken(new HttpClient(handlerMock.Object)));
        }

        [TestMethod]
        public async Task GetToken_RequestShouldContainClientAssertionType()
        {
            HttpRequestMessage capturedRequest = null;
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"access_token\":\"t\",\"expires_in\":3600,\"token_type\":\"Bearer\",\"scope\":\"s\"}")
                })
                .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
                .Verifiable();

            var generator = BuildGenerator();
            await generator.GetToken(new HttpClient(handlerMock.Object));

            Assert.IsNotNull(capturedRequest);
            string body = await capturedRequest.Content.ReadAsStringAsync();
            Assert.IsTrue(body.Contains("client_assertion_type"));
            Assert.IsTrue(body.Contains("grant_type=client_credentials"));
        }

        [TestMethod]
        public async Task GetToken_JwtClaimsShouldPrefixSdkIdWithSdkColon()
        {
            HttpRequestMessage capturedRequest = null;
            var handlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("{\"access_token\":\"t\",\"expires_in\":3600,\"token_type\":\"Bearer\",\"scope\":\"s\"}")
                })
                .Callback<HttpRequestMessage, CancellationToken>((req, _) => capturedRequest = req)
                .Verifiable();

            var generator = BuildGenerator();
            await generator.GetToken(new HttpClient(handlerMock.Object));

            string body = await capturedRequest.Content.ReadAsStringAsync();
            string jwt = ExtractFormValue(body, "client_assertion");
            string payloadJson = DecodeJwtPayload(jwt);
            var payload = JObject.Parse(payloadJson);

            Assert.AreEqual("sdk:" + _sdkId, payload["iss"].Value<string>());
            Assert.AreEqual("sdk:" + _sdkId, payload["sub"].Value<string>());
        }

        private static string ExtractFormValue(string formEncodedBody, string key)
        {
            foreach (string pair in formEncodedBody.Split('&'))
            {
                string[] parts = pair.Split('=');
                if (parts[0] == key)
                    return Uri.UnescapeDataString(parts[1]);
            }

            throw new InvalidOperationException($"Key '{key}' not found in form body.");
        }

        private static string DecodeJwtPayload(string jwt)
        {
            string base64UrlPayload = jwt.Split('.')[1];
            string base64 = base64UrlPayload.Replace('-', '+').Replace('_', '/');
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }

            return Encoding.UTF8.GetString(Convert.FromBase64String(base64));
        }
    }
}
