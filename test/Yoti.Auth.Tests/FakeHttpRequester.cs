using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Yoti.Auth.Tests
{
    internal class FakeHttpRequester : IHttpRequester
    {
        private readonly Func<HttpClient, Uri, Dictionary<string, string>, Task<Response>> _doRequest;

        public FakeHttpRequester(Func<HttpClient, Uri, Dictionary<string, string>, Task<Response>> doRequest)
        {
            _doRequest = doRequest;
        }

        public Task<Response> DoRequest(HttpClient httpClient, Uri uri, Dictionary<string, string> headers)
        {
            return _doRequest(httpClient, uri, headers);
        }
    }
}