using Newtonsoft.Json;

namespace Yoti.Auth.DigitalIdentity.Policy
{
    /// <summary>
    /// Builder for creating RequiredShareCode instances
    /// </summary>
    public class RequiredShareCodeBuilder
    {
        private string _issuer;
        private string _scheme;

        /// <summary>
        /// Sets the issuer of the share code
        /// </summary>
        /// <param name="issuer">The issuer</param>
        /// <returns>The builder instance</returns>
        public RequiredShareCodeBuilder WithIssuer(string issuer)
        {
            _issuer = issuer;
            return this;
        }

        /// <summary>
        /// Sets the scheme of the share code
        /// </summary>
        /// <param name="scheme">The scheme</param>
        /// <returns>The builder instance</returns>
        public RequiredShareCodeBuilder WithScheme(string scheme)
        {
            _scheme = scheme;
            return this;
        }

        /// <summary>
        /// Builds the RequiredShareCode instance
        /// </summary>
        /// <returns>A new RequiredShareCode instance</returns>
        public RequiredShareCode Build()
        {
            return new RequiredShareCode(_issuer, _scheme);
        }
    }
}
