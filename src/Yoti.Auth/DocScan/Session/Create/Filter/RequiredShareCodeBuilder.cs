namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class RequiredShareCodeBuilder
    {
        private string _issuer;
        private string _scheme;

        /// <summary>
        /// Sets the issuer for the share code
        /// </summary>
        /// <param name="issuer">The issuer of the share code</param>
        /// <returns>the builder</returns>
        public RequiredShareCodeBuilder WithIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        /// <summary>
        /// Sets the scheme for the share code
        /// </summary>
        /// <param name="scheme">The scheme of the share code</param>
        /// <returns>the builder</returns>
        public RequiredShareCodeBuilder WithScheme(string scheme)
        {
            _scheme = scheme;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="RequiredShareCode"/>
        /// </summary>
        /// <returns>The built <see cref="RequiredShareCode"/></returns>
        public RequiredShareCode Build()
        {
            return new RequiredShareCode(_issuer, _scheme);
        }
    }
}
