using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Container for resources to be created as part of a session.
    /// </summary>
    public class ResourceCreationContainer
    {
        internal ResourceCreationContainer(ApplicantProfile applicantProfile)
        {
            ApplicantProfile = applicantProfile;
        }

        /// <summary>
        /// The applicant profile to be used for verification against identity documents.
        /// </summary>
        [JsonProperty(PropertyName = "applicant_profile")]
        public ApplicantProfile ApplicantProfile { get; }
    }
}
