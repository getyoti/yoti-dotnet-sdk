using System.Net.Http;
using System.Threading.Tasks;

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