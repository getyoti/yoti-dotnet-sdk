using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Yoti.Auth.Web;

namespace Yoti.Auth.Tests
{
    internal class FakeHttpRequester : IHttpRequester
    {
        private readonly Func<HttpClient, HttpMethod, Uri, Dictionary<string, string>, byte[], Task<Response>> _doRequest;

        public FakeHttpRequester(Func<HttpClient, HttpMethod, Uri, Dictionary<string, string>, byte[], Task<Response>> doRequest)
        {
            _doRequest = doRequest;
        }

        public Task<Response> DoRequest(HttpClient httpClient, HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers, byte[] byteContent)
        {
            return _doRequest(httpClient, httpMethod, uri, headers, byteContent);
        }
    }
}