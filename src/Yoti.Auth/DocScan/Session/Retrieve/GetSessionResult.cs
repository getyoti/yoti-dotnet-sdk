using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Retrieve.Check;
using Yoti.Auth.DocScan.Session.Retrieve.IdentityProfile;
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

        [JsonProperty(PropertyName = "biometric_consent")]
        public DateTime? BiometricConsentTimestamp { get; internal set; }

        [JsonProperty(PropertyName = "identity_profile")]
        public IdentityProfileResponse IdentityProfile { get; internal set; }

        public List<AuthenticityCheckResponse> GetAuthenticityChecks()
        {
            if (Checks == null)
                return new List<AuthenticityCheckResponse>();

            return Checks.OfType<AuthenticityCheckResponse>().ToList();
        }

        public List<FaceMatchCheckResponse> GetFaceMatchChecks()
        {
            if (Checks == null)
                return new List<FaceMatchCheckResponse>();

            return Checks.OfType<FaceMatchCheckResponse>().ToList();
        }

        [Obsolete("Use GetIdDocumentTextDataChecks() instead")]
        public List<TextDataCheckResponse> GetTextDataChecks()
        {
            return GetIdDocumentTextDataChecks();
        }

        public List<TextDataCheckResponse> GetIdDocumentTextDataChecks()
        {
            if (Checks == null)
                return new List<TextDataCheckResponse>();

            return Checks.OfType<TextDataCheckResponse>().ToList();
        }

        public List<LivenessCheckResponse> GetLivenessChecks()
        {
            if (Checks == null)
                return new List<LivenessCheckResponse>();

            return Checks.OfType<LivenessCheckResponse>().ToList();
        }

        public List<IdDocumentComparisonCheckResponse> GetIdDocumentComparisonChecks()
        {
            if (Checks == null)
                return new List<IdDocumentComparisonCheckResponse>();

            return Checks.OfType<IdDocumentComparisonCheckResponse>().ToList();
        }

        public List<SupplementaryDocTextDataCheckResponse> GetSupplementaryDocTextDataChecks()
        {
            if (Checks == null)
                return new List<SupplementaryDocTextDataCheckResponse>();

            return Checks.OfType<SupplementaryDocTextDataCheckResponse>().ToList();
        }

        public List<ThirdPartyIdentityCheckResponse> GetThirdPartyIdentityChecks()
        {
            if (Checks == null)
                return new List<ThirdPartyIdentityCheckResponse>();

            return Checks.OfType<ThirdPartyIdentityCheckResponse>().ToList();
        }

        public List<WatchlistScreeningCheckResponse> GetWatchlistScreeningChecks()
        {
            if (Checks == null)
                return new List<WatchlistScreeningCheckResponse>();

            return Checks.OfType<WatchlistScreeningCheckResponse>().ToList();
        }

        public List<WatchlistAdvancedCaCheckResponse> GetWatchlistAdvancedCaChecks()
        {
            if (Checks == null)
                return new List<WatchlistAdvancedCaCheckResponse>();

            return Checks.OfType<WatchlistAdvancedCaCheckResponse>().ToList();
        }
    }
}