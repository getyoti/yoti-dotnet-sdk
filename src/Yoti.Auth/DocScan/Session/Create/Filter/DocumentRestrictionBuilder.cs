using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class DocumentRestrictionBuilder
    {
        private List<string> _countryCodes;
        private List<string> _documentTypes;

        public DocumentRestrictionBuilder WithCountries(List<string> countryCodes)
        {
            _countryCodes = countryCodes;
            return this;
        }

        public DocumentRestrictionBuilder WithDocumentTypes(List<string> documentTypes)
        {
            _documentTypes = documentTypes;
            return this;
        }

        public DocumentRestriction Build()
        {
            return new DocumentRestriction(_countryCodes, _documentTypes);
        }
    }
}