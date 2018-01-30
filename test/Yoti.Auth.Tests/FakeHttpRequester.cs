using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoti.Auth.Tests
{
    internal class FakeHttpRequester : IHttpRequester
    {
        private readonly Func<HttpClient, HttpMethod, Uri, Dictionary<string, string>, Task<Response>> _doRequest;

        public FakeHttpRequester(Func<HttpClient, HttpMethod, Uri, Dictionary<string, string>, Task<Response>> doRequest)
        {
            _doRequest = doRequest;
        }

        public Task<Response> DoRequest(HttpClient httpClient, HttpMethod httpMethod, Uri uri, Dictionary<string, string> headers)
        {
            return _doRequest(httpClient, httpMethod, uri, headers);
        }
    }
}