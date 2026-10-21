using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Wraps a <see cref="SessionOverviewPdfPayload"/> in the "options" envelope expected
    /// by the session overview PDF endpoint. Internal wire-format detail only; callers
    /// interact with <see cref="SessionOverviewPdfPayload"/> directly.
    /// </summary>
    internal class SessionOverviewPdfRequest
    {
        public SessionOverviewPdfRequest(SessionOverviewPdfPayload options)
        {
            Options = options;
        }

        [JsonProperty(PropertyName = "options")]
        public SessionOverviewPdfPayload Options { get; }
    }
}
