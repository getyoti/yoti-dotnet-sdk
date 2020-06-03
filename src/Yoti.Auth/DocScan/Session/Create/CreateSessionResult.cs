using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    public class CreateSessionResult
    {
        /// <summary>
        /// Returns the time-to-live (TTL) for the client session token for the created session.
        /// </summary>
        [JsonProperty(PropertyName = "client_session_token_ttl")]
        public int ClientSessionTokenTtl { get; internal set; }

        /// <summary>
        /// Returns the client session token for the created session.
        /// </summary>
        [JsonProperty(PropertyName = "client_session_token")]
        public string ClientSessionToken { get; internal set; }

        /// <summary>
        /// Returns the session id for the created session.
        /// </summary>
        [JsonProperty(PropertyName = "session_id")]
        public string SessionId { get; internal set; }
    }
}