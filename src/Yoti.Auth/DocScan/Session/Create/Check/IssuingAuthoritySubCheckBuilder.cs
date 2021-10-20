using Yoti.Auth.DocScan.Session.Create.Filter;

namespace Yoti.Auth.DocScan.Session.Create.Check
{
    public class IssuingAuthoritySubCheckBuilder
    {
        private bool _requested;
        private DocumentFilter _filter;

        /// <summary>
        /// Sets whether the sub check will be requested for the Authenticity check.
        /// </summary>
        /// <param name="requested">If the sub check is requested</param>
        /// <returns>The <see cref="IssuingAuthoritySubCheckBuilder"/></returns>
        public IssuingAuthoritySubCheckBuilder WithRequested(bool requested)
        {
            _requested = requested;
            return this;
        }

        /// <summary>
        /// Sets the <see cref="DocumentFilter"/> that will be used to determine which documents the sub check is performed on.
        /// </summary>
        /// <param name="documentFilter">The <see cref="DocumentFilter"/> used to determine which documents the sub check is performed on</param>
        /// <returns>The <see cref="IssuingAuthoritySubCheckBuilder"/></returns>
        public IssuingAuthoritySubCheckBuilder WithFilter(DocumentFilter documentFilter)
        {
            _filter = documentFilter;
            return this;
        }

        public IssuingAuthoritySubCheck Build()
        {
            return new IssuingAuthoritySubCheck(_requested, _filter);
        }
    }
}