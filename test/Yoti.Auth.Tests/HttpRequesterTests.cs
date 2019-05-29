using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class HttpRequesterTests
    {
        private readonly string _apiUrl = Constants.Web.DefaultYotiApiUrl;
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private FakeHttpResponseHandler _fakeResponseHandler;
        private HttpRequester _httpRequester;

        [TestInitialize]
        public void Startup()
        {
            _fakeResponseHandler = new FakeHttpResponseHandler();
            _httpRequester = new HttpRequester();
            _headers.Add("Key", "Value");
        }

        [TestMethod]
        public void SuccessStatusCode_ReturnsSuccess()
        {
            _fakeResponseHandler.AddFakeResponse(
                new Uri(_apiUrl),
                new HttpResponseMessage(HttpStatusCode.OK));

            var httpClient = new HttpClient(_fakeResponseHandler);
            Task<Response> response = _httpRequester.DoRequest(
              httpClient,
              HttpMethod.Get,
              new Uri(_apiUrl),
              _headers,
              byteContent: null);

            Assert.IsTrue(response.Result.Success);
        }

        [TestMethod]
        public void HttpStatusBadRequest_ThrowsException()
        {
            _fakeResponseHandler.AddFakeResponse(
                new Uri(_apiUrl),
                new HttpResponseMessage(HttpStatusCode.BadRequest));

            var httpClient = new HttpClient(_fakeResponseHandler);

            Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await _httpRequester.DoRequest(
                httpClient,
                HttpMethod.Get,
                new Uri(_apiUrl),
                _headers,
                byteContent: null);
            });
        }

        [TestMethod]
        public void HttpStatusInternalServerError_ThrowsException()
        {
            _fakeResponseHandler.AddFakeResponse(
                new Uri(_apiUrl),
                new HttpResponseMessage(HttpStatusCode.InternalServerError));

            var httpClient = new HttpClient(_fakeResponseHandler);

            Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await _httpRequester.DoRequest(
                   httpClient,
                   HttpMethod.Get,
                   new Uri(_apiUrl),
                   _headers,
                   byteContent: null);
            });
        }

        [TestMethod]
        public void HttpStatusNotFound_ThrowsException()
        {
            _fakeResponseHandler.AddFakeResponse(
                new Uri(_apiUrl),
                new HttpResponseMessage(HttpStatusCode.NotFound));

            var httpClient = new HttpClient(_fakeResponseHandler);

            Assert.ThrowsExceptionAsync<HttpRequestException>(async () =>
            {
                await _httpRequester.DoRequest(
                    httpClient,
                    HttpMethod.Get,
                    new Uri(_apiUrl),
                    _headers,
                    byteContent: null);
            });
        }
    }
}