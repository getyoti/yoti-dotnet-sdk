using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Yoti.Auth.Tests
{
    internal class FakeHttpRequester : IHttpRequester
    {
        private readonly Func<Uri, Dictionary<string, string>, Task<Response>> _doRequest;
        public FakeHttpRequester(Func<Uri, Dictionary<string, string>, Task<Response>> doRequest)
        {
            _doRequest = doRequest;
        }
        public Task<Response> DoRequest(Uri uri, Dictionary<string, string> headers)
        {
            return _doRequest(uri, headers);
        }
    }
}
