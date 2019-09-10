using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Org.BouncyCastle.Crypto;

namespace Yoti.Auth.Web
{
    public class Request
    {
        public HttpRequestMessage RequestMessage { get; set; }

        public Request(HttpRequestMessage message)
        {
            RequestMessage = message;
        }

        public async Task<HttpResponseMessage> Execute(HttpClient httpClient)
        {
            Validation.NotNull(httpClient, nameof(httpClient));

            return await httpClient.SendAsync(RequestMessage).ConfigureAwait(false);
        }
    }
}