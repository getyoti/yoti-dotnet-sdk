namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Builder for <see cref="RequiredShareCode"/>
    /// </summary>
    public class RequiredShareCodeBuilder
    {
        private string _issuer;
        private string _scheme;

        /// <summary>
        /// Sets the issuer
        /// </summary>
        /// <param name="issuer">The issuer</param>
        /// <returns>the builder</returns>
        public RequiredShareCodeBuilder WithIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        /// <summary>
        /// Sets the scheme
        /// </summary>
        /// <param name="scheme">The scheme</param>
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
