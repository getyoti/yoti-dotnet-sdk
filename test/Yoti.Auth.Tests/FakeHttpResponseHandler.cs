using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Yoti.Auth.Tests
{
    public class FakeHttpResponseHandler : DelegatingHandler
    {
        private readonly Dictionary<Uri, HttpResponseMessage> _fakeResponses = new Dictionary<Uri, HttpResponseMessage>();

        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _fakeResponses.Add(uri, responseMessage);
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_fakeResponses.ContainsKey(request.RequestUri))
            {
                HttpResponseMessage response = _fakeResponses[request.RequestUri];
                response.Content = new StringContent("Content");
                return await Task.FromResult(response);
            }
            else
            {
                return new HttpResponseMessage(HttpStatusCode.NotFound) { RequestMessage = request };
            }
        }
    }
}