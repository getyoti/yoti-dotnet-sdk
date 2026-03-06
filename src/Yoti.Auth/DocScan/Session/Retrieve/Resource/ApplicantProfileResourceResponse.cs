using System;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Retrieve.Resource
{
    /// <summary>
    /// Applicant Profile Resource Response wrapping a <see cref="MediaResponse"/>
    /// </summary>
    public class ApplicantProfileResourceResponse : ResourceResponse
    {
        [JsonProperty(PropertyName = "media")]
        public MediaResponse Media { get; private set; }

        [JsonProperty(PropertyName = "created_at")]
        public DateTime? CreatedAt { get; private set; }

        [JsonProperty(PropertyName = "last_updated")]
        public DateTime? LastUpdated { get; private set; }
    }
}
