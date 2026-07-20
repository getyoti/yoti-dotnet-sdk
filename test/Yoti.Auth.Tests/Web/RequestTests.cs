using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.Web
{
    [TestClass]
    public class RequestTests
    {
        [TestMethod]
        public async Task ExecuteWithHeadersShouldReturnDataAndHeaders()
        {
            var httpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("some-body")
            };
            httpResponseMessage.Headers.Add("X-Request-ID", "req-123");

            var handlerMock = Http.SetupMockMessageHandler(httpResponseMessage);
            var request = new Request(new HttpRequestMessage(HttpMethod.Get, "https://example.com"));

            YotiHttpResponse<string> result = await request.ExecuteWithHeaders(
                new HttpClient(handlerMock.Object),
                response => response.Content.ReadAsStringAsync());

            Assert.AreEqual("some-body", result.Data);
            Assert.AreEqual("req-123", result.RequestId);
        }

        [TestMethod]
        public async Task ExecuteWithHeadersShouldThrowForNullHttpClient()
        {
            var request = new Request(new HttpRequestMessage(HttpMethod.Get, "https://example.com"));

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                request.ExecuteWithHeaders<string>(null, response => response.Content.ReadAsStringAsync()));
        }

        [TestMethod]
        public async Task ExecuteWithHeadersShouldThrowForNullDataExtractor()
        {
            var request = new Request(new HttpRequestMessage(HttpMethod.Get, "https://example.com"));

            await Assert.ThrowsExceptionAsync<ArgumentNullException>(() =>
                request.ExecuteWithHeaders<string>(new HttpClient(), null));
        }
    }
}
