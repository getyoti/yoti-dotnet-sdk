using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class OrthogonalRestrictionsFilterBuilder
    {
        private CountryRestriction _countryRestriction;
        private TypeRestriction _typeRestriction;

        public OrthogonalRestrictionsFilterBuilder WithIncludedCountries(List<string> countryCodes)
        {
            _countryRestriction = new CountryRestriction(Constants.DocScanConstants.IncludeList, countryCodes);
            return this;
        }

        public OrthogonalRestrictionsFilterBuilder WithExcludedCountries(List<string> countryCodes)
        {
            _countryRestriction = new CountryRestriction(Constants.DocScanConstants.ExcludeList, countryCodes);
            return this;
        }

        public OrthogonalRestrictionsFilterBuilder WithIncludedDocumentTypes(List<string> documentTypes)
        {
            _typeRestriction = new TypeRestriction(Constants.DocScanConstants.IncludeList, documentTypes);
            return this;
        }

        public OrthogonalRestrictionsFilterBuilder WithExcludedDocumentTypes(List<string> documentTypes)
        {
            _typeRestriction = new TypeRestriction(Constants.DocScanConstants.ExcludeList, documentTypes);
            return this;
        }

        public OrthogonalRestrictionsFilter Build()
        {
            return new OrthogonalRestrictionsFilter(_countryRestriction, _typeRestriction);
        }
    }
}