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
        private readonly Dictionary<Uri, HttpResponseMessage> _FakeResponses = new Dictionary<Uri, HttpResponseMessage>();

        public void AddFakeResponse(Uri uri, HttpResponseMessage responseMessage)
        {
            _FakeResponses.Add(uri, responseMessage);
        }

        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (_FakeResponses.ContainsKey(request.RequestUri))
            {
                HttpResponseMessage response = _FakeResponses[request.RequestUri];
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