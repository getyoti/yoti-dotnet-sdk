using System;
using System.IO;
using System.Net.Http;
using System.Text;
using Moq;
using Xunit;
using Yoti.Auth.Sandbox.Profile.Request;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

namespace Yoti.Auth.Sandbox
{
    public class YotiSandboxClientTest
    {
        private const string _someAppId = "someAppId";
        private readonly YotiSandboxClient _yotiSandboxClient;
        private readonly YotiTokenRequest _yotiTokenRequest;

        public YotiSandboxClientTest()
        {
            _yotiSandboxClient = new YotiSandboxClient(_someAppId, KeyPair.Get());
            _yotiTokenRequest = new YotiTokenRequestBuilder().Build();
        }

        [Fact]
        public static void BuilderShouldThrowForMissingAppId()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                YotiSandboxClient.Builder().Build();
            });

            Assert.Contains("appId", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void BuilderShouldThrowForMissingKey()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                YotiSandboxClient.Builder()
                .ForApplication(_someAppId)
                .Build();
            });

            Assert.Contains("keyPair", exception.Message, StringComparison.Ordinal);
        }

        [Fact]
        public static void BuilderShouldCreateClient()
        {
            var sandboxClient = YotiSandboxClient.Builder()
                .ForApplication(_someAppId)
                .WithKeyPair(KeyPair.Get())
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
                    new HttpClient(),
                    new HttpRequester(),
                    _yotiTokenRequest);
            });
        }
    }
}