namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    /// <summary>
    /// Builder for <see cref="RequiredShareCode"/>
    /// </summary>
    public class RequiredShareCodeBuilder
    {
        private string _issuer;
        private string _scheme;

        /// <summary>
        /// Sets the issuer for the share code
        /// </summary>
        /// <param name="issuer">The issuer</param>
        /// <returns>The builder</returns>
        public RequiredShareCodeBuilder WithIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        /// <summary>
        /// Sets the scheme for the share code
        /// </summary>
        /// <param name="scheme">The scheme</param>
        /// <returns>The builder</returns>
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
