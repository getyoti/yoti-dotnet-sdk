using System.Collections.Generic;
using Yoti.Auth.DocScan.Session.Create.Objectives;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class RequiredSupplementaryDocumentBuilder
    {
        private List<string> _documentTypes;
        private List<string> _countryCodes;
        private Objective _objective;

        public RequiredSupplementaryDocumentBuilder WithObjective(Objective objective)
        {
            _objective = objective;
            return this;
        }

        public RequiredSupplementaryDocumentBuilder WithDocumentTypes(List<string> documentTypes)
        {
            _documentTypes = documentTypes;
            return this;
        }

        public RequiredSupplementaryDocumentBuilder WithCountries(List<string> countryCodes)
        {
            _countryCodes = countryCodes;
            return this;
        }

        public RequiredSupplementaryDocument Build()
        {
            return new RequiredSupplementaryDocument(_objective, _documentTypes, _countryCodes);
        }
    }
}