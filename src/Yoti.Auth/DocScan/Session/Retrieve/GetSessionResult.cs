using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Check;
using Yoti.Auth.DocScan.Session.Retrieve.Resource;

namespace Yoti.Auth.DocScan.Session.Retrieve
{
    public class GetSessionResult
    {
        /// <summary>
        /// The time-to-live (TTL) for the client session token for the created session
        /// </summary>
        [JsonProperty(PropertyName = "client_session_token_ttl")]
        public int ClientSessionTokenTtl { get; internal set; }

        /// <summary>
        ///  The client session token for the created session.
        /// </summary>
        [JsonProperty(PropertyName = "client_session_token")]
        public string ClientSessionToken { get; internal set; }

        /// <summary>
        /// The session id for the created session.
        /// </summary>
        [JsonProperty(PropertyName = "session_id")]
        public string SessionId { get; internal set; }

        /// <summary>
        /// The user tracking ID for the created session
        /// </summary>
        [JsonProperty(PropertyName = "user_tracking_id")]
        public string UserTrackingId { get; internal set; }

        /// <summary>
        /// The state of the created session
        /// </summary>
        [JsonProperty(PropertyName = "state")]
        public string State { get; internal set; }

        [JsonProperty(PropertyName = "checks")]
        public List<CheckResponse> Checks { get; internal set; }

        [JsonProperty(PropertyName = "resources")]
        public ResourceContainer Resources { get; internal set; }

        public List<AuthenticityCheckResponse> GetAuthenticityChecks()
        {
            return Checks?.OfType<AuthenticityCheckResponse>()?.ToList();
        }

        public List<FaceMatchCheckResponse> GetFaceMatchChecks()
        {
            return Checks?.OfType<FaceMatchCheckResponse>()?.ToList();
        }

        public List<TextDataCheckResponse> GetTextDataChecks()
        {
            return Checks?.OfType<TextDataCheckResponse>()?.ToList();
        }

        public List<LivenessCheckResponse> GetLivenessChecks()
        {
            return Checks?.OfType<LivenessCheckResponse>()?.ToList();
        }
    }
}