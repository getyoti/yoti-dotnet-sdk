namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Builder for <see cref="ResourceCreationContainer"/>.
    /// </summary>
    public class ResourceCreationContainerBuilder
    {
        private ApplicantProfile _applicantProfile;

        /// <summary>
        /// Sets the applicant profile to be used for verification against identity documents.
        /// </summary>
        /// <param name="applicantProfile">The <see cref="ApplicantProfile"/> for the session</param>
        /// <returns>the builder</returns>
        public ResourceCreationContainerBuilder WithApplicantProfile(ApplicantProfile applicantProfile)
        {
            _applicantProfile = applicantProfile;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="ResourceCreationContainer"/> based on the values supplied to the builder.
        /// </summary>
        /// <returns>The built <see cref="ResourceCreationContainer"/></returns>
        public ResourceCreationContainer Build()
        {
            return new ResourceCreationContainer(_applicantProfile);
        }
    }
}
