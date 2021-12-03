using Newtonsoft.Json;
using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Retrieve.Configuration.Capture;

namespace Yoti.Auth.DocScan.Session.Retrieve.Configuration
{
    public class SessionConfigurationResponse
    {
        /// <summary>
        /// The time-to-live (TTL): amount of time remaining in seconds until the session expires
        /// </summary>
        [JsonProperty(PropertyName = "client_session_token_ttl")]
        public int ClientSessionTokenTtl { get; private set; }

        /// <summary>
        /// The session id that the configuration belongs to
        /// </summary>
        [JsonProperty(PropertyName = "session_id")]
        public string SessionId { get; private set; }


        /// <summary>
        /// The checks that have been requested for the session
        /// </summary>
        [JsonProperty(PropertyName = "requested_checks")]
        public List<string> RequestedChecks { get; private set; }

        /// <summary>
        /// Information about what needs to be captured to fulfill the sessions requirements
        /// </summary>
        [JsonProperty(PropertyName = "capture")]
        public CaptureResponse Capture { get; private set; }
    }
}