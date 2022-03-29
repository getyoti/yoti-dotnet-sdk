using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Retrieve.IdentityProfile
{
    public class IdentityProfileResponse
    {
        [JsonProperty(PropertyName = "subject_id")]
        public string SubjectId { get; private set; }
        [JsonProperty(PropertyName = "result")]
        public string Result { get; private set; }
        [JsonProperty(PropertyName = "failure_reason")]
        public FailureReasonResponse FailureReason { get; private set; }
        [JsonProperty(PropertyName = "identity_profile_report")]
        public Dictionary<string, JToken> Report { get; private set; }

    }
}
