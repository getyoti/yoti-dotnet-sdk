using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Tests.Common;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests.Web
{
    [TestClass]
    public class RequestBuilderTests
    {
        private readonly Uri _testBaseUri = new Uri("https://test.com");

        private readonly Uri _ageScanBaseUri = new UriBuilder(
            "https",
            "agescan.com",
            443,
            "/api/v1/age-verification")
            .Uri;

        private const string _sdkId = "28ba2ec8-4ff9-4974-a7a2-dac7ad2a172f";
        private readonly byte[] _content = Conversion.UtfToBytes("bytes");

        [TestMethod]
        public void ShouldNotBuildWithoutKeyPair()
        {
            var argumentNullException = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new RequestBuilder()
                    .WithBaseUri(_testBaseUri)
                    .WithEndpoint("/a")
                    .WithHttpMethod(HttpMethod.Get)
                    .Build();
            });

            Assert.IsTrue(argumentNullException.Message.Contains("_keyPair"));
        }

        [TestMethod]
        public void ShouldNotBuildWithoutBaseUri()
        {
            var argumentNullException = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new RequestBuilder()
                    .WithKeyPair(KeyPair.Get())
                    .WithEndpoint("/a")
                    .WithHttpMethod(HttpMethod.Post)
                    .Build();
            });

            Assert.IsTrue(argumentNullException.Message.Contains("_baseUri"));
        }

        [TestMethod]
        public void ShouldNotBuildWithoutEndpoint()
        {
            var invalidOperationException = Assert.ThrowsException<InvalidOperationException>(() =>
            {
                new RequestBuilder()
                    .WithBaseUri(_testBaseUri)
                    .WithKeyPair(KeyPair.Get())
                    .WithHttpMethod(HttpMethod.Delete)
                    .Build();
            });

            Assert.IsTrue(invalidOperationException.Message.Contains("_endpoint"));
        }

        [TestMethod]
        public void ShouldNotBuildWithoutHttpMethod()
        {
            var argumentNullException = Assert.ThrowsException<ArgumentNullException>(() =>
            {
                new RequestBuilder()
                    .WithBaseUri(_testBaseUri)
                    .WithKeyPair(KeyPair.Get())
                    .WithEndpoint("/a")
                    .Build();
            });

            Assert.IsTrue(argumentNullException.Message.Contains("_httpMethod"));
        }

        [TestMethod]
        public void ShouldAddQueryParams()
        {
            Request request = CreateRequestBuilder()
                .WithQueryParam("key", "value")
                .Build();

            Assert.IsTrue(request.RequestMessage.RequestUri.Query.Contains("key=value"));
        }

        [TestMethod]
        public void ContentShouldBeAdded()
        {
            Request request = CreateRequestBuilder()
                .WithContent(_content)
                .Build();

            Assert.IsTrue(_content.SequenceEqual(request.RequestMessage.Content.ReadAsByteArrayAsync().Result));
        }

        [TestMethod]
        public void CustomHeadersShoudlBeAdded()
        {
            Request request = CreateRequestBuilder()
                .WithHeader("key", "value")
                .Build();

            request.RequestMessage.Headers.TryGetValues("key", out IEnumerable<string> headers);
            Assert.IsTrue(headers.Contains("value"));
        }

        private RequestBuilder CreateRequestBuilder()
        {
            return new RequestBuilder()
                .WithBaseUri(_testBaseUri)
                .WithKeyPair(KeyPair.Get())
                .WithEndpoint("/a")
                .WithHttpMethod(HttpMethod.Post);
        }

        [TestMethod]
        public void ExtraSlashInUriShouldntBeIncluded()
        {
            Request request = new RequestBuilder()
                .WithBaseUri(new Uri("https://test.com/"))
                .WithKeyPair(KeyPair.Get())
                .WithEndpoint("/a")
                .WithHttpMethod(HttpMethod.Patch)
                .Build();

            Assert.IsTrue(request.RequestMessage.RequestUri.ToString().Contains("https://test.com/a"));
        }

        [TestMethod]
        public void EndpointTrailingSlashIsAdded()
        {
            Request request = new RequestBuilder()
                .WithBaseUri(_testBaseUri)
                .WithKeyPair(KeyPair.Get())
                .WithEndpoint("a")
                .WithHttpMethod(HttpMethod.Patch)
                .Build();

            Assert.IsTrue(request.RequestMessage.RequestUri.ToString().Contains("https://test.com/a"));
        }

        [TestMethod]
        public void AgeScanBackgroundRequestBuildsCorrectly()
        {
            Request backgroundImageRequest = new RequestBuilder()
                .WithKeyPair(KeyPair.Get())
                .WithBaseUri(_ageScanBaseUri)
                .WithEndpoint("/backgrounds")
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(_content)
                .WithHeader("X-Yoti-Auth-Id", "19e52ec8-4ff9-4974-a7a2-dac7ad2a710a")
                .Build();

            Assert.IsTrue(
                backgroundImageRequest.RequestMessage.RequestUri.ToString().StartsWith(
                    "https://agescan.com/api/v1/age-verification/backgrounds?"));
            Assert.AreEqual(443, backgroundImageRequest.RequestMessage.RequestUri.Port);
        }

        [TestMethod]
        public void AgeScanChecksRequestBuildsCorrectly()
        {
            StreamReader privateKeyStream = KeyPair.GetValidKeyStream();

            Request ageScanRequest = new RequestBuilder()
                .WithStreamReader(privateKeyStream)
                .WithBaseUri(_ageScanBaseUri)
                .WithEndpoint("/checks")
                .WithHttpMethod(HttpMethod.Post)
                .WithContent(_content)
                .WithHeader("X-Yoti-Auth-Id", "19e52ec8-4ff9-4974-a7a2-dac7ad2a710a")
                .Build();

            Assert.IsTrue(
                ageScanRequest.RequestMessage.RequestUri.ToString().StartsWith(
                    "https://agescan.com/api/v1/age-verification/checks?"));
            Assert.AreEqual(443, ageScanRequest.RequestMessage.RequestUri.Port);
        }

        [TestMethod]
        public void ProfileRequestBuildsCorrectly()
        {
            string token = "cabc7ad2a172f-4974-a7a2-4ff9-36ba2ec9";

            Request profileRequest = new RequestBuilder()
                .WithKeyPair(KeyPair.Get())
                .WithBaseUri(new Uri(Constants.Api.DefaultYotiApiUrl))
                .WithEndpoint($"/profile/{token}")
                .WithHttpMethod(HttpMethod.Get)
                .WithQueryParam("appId", _sdkId)
                .Build();

            Assert.IsTrue(
                profileRequest.RequestMessage.RequestUri.ToString().StartsWith(
                    $"https://api.yoti.com/api/v1/profile/{token}?appId={_sdkId}&"));
        }

        [TestMethod]
        public void AmlRequestBuildsCorrectly()
        {
            Request amlRequest = new RequestBuilder()
                .WithKeyPair(KeyPair.Get())
                .WithBaseUri(new Uri(Constants.Api.DefaultYotiApiUrl))
                .WithEndpoint("/aml-check")
                .WithHttpMethod(HttpMethod.Post)
                .WithQueryParam("appId", _sdkId)
                .WithContent(_content)
                .Build();

            Assert.IsTrue(
                amlRequest.RequestMessage.RequestUri.ToString().StartsWith(
                    $"https://api.yoti.com/api/v1/aml-check?appId={_sdkId}&"));
        }

        [TestMethod]
        public void DocScanRequestBuildsCorrectly()
        {
            StreamReader privateKeyStream = KeyPair.GetValidKeyStream();

            Uri docScanUri = new UriBuilder("https", "docscan.base", 443, "/idverify/v1").Uri;

            Request docScanRequest = new RequestBuilder()
               .WithStreamReader(privateKeyStream)
               .WithHttpMethod(HttpMethod.Post)
               .WithBaseUri(docScanUri)
               .WithEndpoint("/sessions")
               .WithQueryParam("sdkId", _sdkId)
               .WithContent(_content)
               .WithHeader("X-Yoti-Auth-Id", _sdkId)
               .Build();

            Assert.IsTrue(
                docScanRequest.RequestMessage.RequestUri.ToString().StartsWith(
                    $"https://docscan.base/idverify/v1/sessions?sdkId={_sdkId}&"));
        }
    }
}