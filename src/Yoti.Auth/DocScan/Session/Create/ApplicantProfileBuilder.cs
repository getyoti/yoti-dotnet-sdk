namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Builder for <see cref="ApplicantProfile"/>.
    /// </summary>
    public class ApplicantProfileBuilder
    {
        private string _fullName;
        private string _dateOfBirth;
        private string _namePrefix;
        private StructuredPostalAddress _structuredPostalAddress;

        /// <summary>
        /// Sets the full name of the applicant
        /// </summary>
        /// <param name="fullName">The full name</param>
        /// <returns>the builder</returns>
        public ApplicantProfileBuilder WithFullName(string fullName)
        {
            _fullName = fullName;
            return this;
        }

        /// <summary>
        /// Sets the date of birth of the applicant
        /// </summary>
        /// <param name="dateOfBirth">The date of birth (e.g. "1988-11-02")</param>
        /// <returns>the builder</returns>
        public ApplicantProfileBuilder WithDateOfBirth(string dateOfBirth)
        {
            _dateOfBirth = dateOfBirth;
            return this;
        }

        /// <summary>
        /// Sets the name prefix of the applicant
        /// </summary>
        /// <param name="namePrefix">The name prefix (e.g. "Mr")</param>
        /// <returns>the builder</returns>
        public ApplicantProfileBuilder WithNamePrefix(string namePrefix)
        {
            _namePrefix = namePrefix;
            return this;
        }

        /// <summary>
        /// Sets the structured postal address of the applicant
        /// </summary>
        /// <param name="structuredPostalAddress">The <see cref="StructuredPostalAddress"/></param>
        /// <returns>the builder</returns>
        public ApplicantProfileBuilder WithStructuredPostalAddress(StructuredPostalAddress structuredPostalAddress)
        {
            _structuredPostalAddress = structuredPostalAddress;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="ApplicantProfile"/> based on the values supplied to the builder.
        /// </summary>
        /// <returns>The built <see cref="ApplicantProfile"/></returns>
        public ApplicantProfile Build()
        {
            return new ApplicantProfile(
                _fullName,
                _dateOfBirth,
                _namePrefix,
                _structuredPostalAddress);
        }
    }
}
