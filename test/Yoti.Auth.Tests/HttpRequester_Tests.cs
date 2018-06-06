using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Yoti.Auth.Tests
{
    [TestClass]
    public class HttpRequester_Tests
    {
        private readonly string _apiUrl = YotiConstants.DefaultYotiApiUrl;
        private Dictionary<string, string> _headers = new Dictionary<string, string>();
        private FakeHttpResponseHandler _fakeResponseHandler;
        private HttpRequester _httpRequester;

        [TestInitialize]
        public void TestInitialize()
        {
            _fakeResponseHandler = new FakeHttpResponseHandler();
            _httpRequester = new HttpRequester();
            _headers.Add("Key", "Value");
        }

        [TestMethod]
        public void HttpRequester_SuccessStatusCode_ReturnsSuccess()
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
        public void HttpRequester_BadRequestStatusCode_ReturnsFailure()
        {
            _fakeResponseHandler.AddFakeResponse(
                new Uri(_apiUrl),
                new HttpResponseMessage(HttpStatusCode.BadRequest));

            var httpClient = new HttpClient(_fakeResponseHandler);
            Task<Response> response = _httpRequester.DoRequest(
                httpClient,
                HttpMethod.Get,
                new Uri(_apiUrl),
                _headers,
                byteContent: null);

            Assert.IsFalse(response.Result.Success);
        }
    }
}