using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Represents an applicant profile to be used for verification against identity documents.
    /// </summary>
    public class ApplicantProfile
    {
        internal ApplicantProfile(
            string fullName,
            string dateOfBirth,
            string namePrefix,
            StructuredPostalAddress structuredPostalAddress)
        {
            FullName = fullName;
            DateOfBirth = dateOfBirth;
            NamePrefix = namePrefix;
            StructuredPostalAddress = structuredPostalAddress;
        }

        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; }

        [JsonProperty(PropertyName = "date_of_birth")]
        public string DateOfBirth { get; }

        [JsonProperty(PropertyName = "name_prefix")]
        public string NamePrefix { get; }

        [JsonProperty(PropertyName = "structured_postal_address")]
        public StructuredPostalAddress StructuredPostalAddress { get; }
    }
}
