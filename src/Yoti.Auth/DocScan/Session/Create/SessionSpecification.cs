using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create.Check;
using Yoti.Auth.DocScan.Session.Create.Filter;
using Yoti.Auth.DocScan.Session.Create.Task;

namespace Yoti.Auth.DocScan.Session.Create
{
    public class SessionSpecification
    {
        internal SessionSpecification(int? clientSessionTokenTtl, int? resourcesTtl, string userTrackingId, NotificationConfig notifications, List<BaseRequestedCheck> requestedChecks, List<BaseRequestedTask> requestedTasks, SdkConfig sdkConfig, List<RequiredDocument> requiredDocuments, bool? blockBiometricConsent, DateTimeOffset? sessionDeadline, object identityProfileRequirements, object subject)
        {
            ClientSessionTokenTtl = clientSessionTokenTtl;
            ResourcesTtl = resourcesTtl;
            UserTrackingId = userTrackingId;
            Notifications = notifications;
            RequestedChecks = requestedChecks;
            RequestedTasks = requestedTasks;
            SdkConfig = sdkConfig;
            RequiredDocuments = requiredDocuments;
            BlockBiometricConsent = blockBiometricConsent;
            SessionDeadline = sessionDeadline;
            IdentityProfileRequirements = identityProfileRequirements;
            Subject = subject;
        }

        [JsonProperty(PropertyName = "client_session_token_ttl")]
        public int? ClientSessionTokenTtl { get; }

        [JsonProperty(PropertyName = "resources_ttl")]
        public int? ResourcesTtl { get; }

        [JsonProperty(PropertyName = "user_tracking_id")]
        public string UserTrackingId { get; }

        [JsonProperty(PropertyName = "notifications")]
        public NotificationConfig Notifications { get; }

        [JsonProperty(PropertyName = "requested_checks")]
        public List<BaseRequestedCheck> RequestedChecks { get; }

        [JsonProperty(PropertyName = "requested_tasks")]
        public List<BaseRequestedTask> RequestedTasks { get; }

        [JsonProperty(PropertyName = "sdk_config")]
        public SdkConfig SdkConfig { get; }

        [JsonProperty(PropertyName = "required_documents")]
        public List<RequiredDocument> RequiredDocuments { get; }

        [JsonProperty(PropertyName = "block_biometric_consent")]
        public bool? BlockBiometricConsent { get; }

        [JsonProperty(PropertyName = "session_deadline")]
        public DateTimeOffset? SessionDeadline { get; }

        [JsonProperty(PropertyName = "identity_profile_requirements")]
        public object IdentityProfileRequirements { get; }

        [JsonProperty(PropertyName = "subject")]
        public object Subject { get; }



    }
}
