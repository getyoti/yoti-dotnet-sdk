using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using Yoti.Auth.Sandbox.Profile.Request;
using Yoti.Auth.Tests.Common;

namespace Yoti.Auth.Sandbox
{
    public class YotiSandboxClientTests
    {
        private const string _someAppId = "someAppId";
        private readonly YotiSandboxClient _yotiSandboxClient;
        private readonly YotiTokenRequest _yotiTokenRequest;
        private static readonly Uri _someUri = new Uri("https://www.test.com");

        public YotiSandboxClientTests()
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                handler.ServerCertificateCustomValidationCallback +=
                    (sender, cert, chain, sslPolicyErrors) => true;

                using (var httpClient = new HttpClient(handler))
                {
                    _yotiSandboxClient = new YotiSandboxClient(httpClient, _someUri, _someAppId, KeyPair.Get());
                }
            }

            _yotiTokenRequest = new YotiTokenRequestBuilder().Build();
        }

        [Fact]
        public static void BuilderShouldThrowForMissingAppId()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                YotiSandboxClient.Builder()
                .WithApiUri(_someUri)
                .WithKeyPair(KeyPair.Get())
                .Build();
            });

            Assert.Contains("appId", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void BuilderShouldThrowForMissingKey()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                YotiSandboxClient.Builder()
                .WithApiUri(_someUri)
                .ForApplication(_someAppId)
                .Build();
            });

            Assert.Contains("keyPair", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void BuilderShouldThrowForMissingApiUri()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                YotiSandboxClient.Builder()
                .ForApplication(_someAppId)
                .WithKeyPair(KeyPair.Get())
                .Build();
            });

            Assert.Contains("apiUri", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void BuilderShouldCreateClient()
        {
            var sandboxClient = YotiSandboxClient.Builder()
                .ForApplication(_someAppId)
                .WithKeyPair(KeyPair.Get())
                .WithApiUri(_someUri)
                .Build();

            Assert.NotNull(sandboxClient);
        }

        [Fact]
        public void SetupSharingProfileShouldWrapIOException()
        {
            var mockJsonConvert = new Mock<Encoding>();
            mockJsonConvert.Setup(
                x => x.GetBytes(It.IsAny<string>()))
                    .Throws(new IOException());

            Assert.Throws<SandboxException>(() =>
            {
                _yotiSandboxClient.SetupSharingProfile(
                    _yotiTokenRequest);
            });
        }

        [Fact]
        public void SetupSharingProfileShouldReturnToken()
        {
            string tokenValue = "kyHPjq2+Y48cx+9yS/XzmW09jVUylSdhbP+3Q9Tc9p6bCEnyfa8vj38";

            using (var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{\"token\": \"" + tokenValue + "\"}")
            })
            {
                YotiSandboxClient yotiSandboxClient;
                Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(httpResponseMessage);

                using (var httpClient = new HttpClient(handlerMock.Object))
                {
                    yotiSandboxClient = new YotiSandboxClient(httpClient, null, _someAppId, KeyPair.Get());

                    string result = yotiSandboxClient.SetupSharingProfile(
                            _yotiTokenRequest);

                    Assert.Equal(tokenValue, result);
                }
            };
        }

        [Fact]
        public void SetupSharingProfileNonSuccessCode()
        {
            using (var httpResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent("{}")
            })
            {
                YotiSandboxClient yotiSandboxClient;
                Mock<HttpMessageHandler> handlerMock = SetupMockMessageHandler(httpResponseMessage);

                using (var httpClient = new HttpClient(handlerMock.Object))
                {
                    yotiSandboxClient = new YotiSandboxClient(httpClient, null, _someAppId, KeyPair.Get());

                    var exception = Assert.Throws<SandboxException>(() =>
                    {
                        yotiSandboxClient.SetupSharingProfile(
                            _yotiTokenRequest);
                    });

                    Assert.Contains("Error when setting up sharing profile", exception.Message, StringComparison.Ordinal);
                }
            };
        }

        private Mock<HttpMessageHandler> SetupMockMessageHandler(HttpResponseMessage httpResponseMessage)
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>()
               )
               .ReturnsAsync(httpResponseMessage)
               .Verifiable();
            return handlerMock;
        }
    }
}