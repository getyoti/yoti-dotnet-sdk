using System.Collections.Generic;
using System.Xml.Linq;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class DocumentRestrictionsFilterBuilder
    {
        private string _inclusion;
        private List<DocumentRestriction> _documents;
        private bool _allowExpiredDocuments;

        public DocumentRestrictionsFilterBuilder ForIncludeList()
        {
            _inclusion = Constants.DocScanConstants.IncludeList;
            return this;
        }

        public DocumentRestrictionsFilterBuilder ForExcludeList()
        {
            _inclusion = Constants.DocScanConstants.ExcludeList;
            return this;
        }

        public DocumentRestrictionsFilterBuilder WithDocumentRestriction(List<string> countryCodes, List<string> documentTypes)
        {
            if (_documents == null)
                _documents = new List<DocumentRestriction>();

            _documents.Add(new DocumentRestriction(countryCodes, documentTypes));
            return this;
        }

        public DocumentRestrictionsFilterBuilder WithDocumentRestriction(DocumentRestriction documentRestriction)
        {
            if (_documents == null)
                _documents = new List<DocumentRestriction>();

            _documents.Add(documentRestriction);
            return this;
        }

        public DocumentRestrictionsFilterBuilder withAllowExpiredDocuments()
        {
            _allowExpiredDocuments = true;
            return this;
        }

        public DocumentRestrictionsFilterBuilder withDenyExpiredDocuments()
        {
            _allowExpiredDocuments = false;
            return this;
        }

        public DocumentRestrictionsFilter Build()
        {
            Validation.NotNullOrEmpty(_inclusion, nameof(_inclusion));
            return new DocumentRestrictionsFilter(_inclusion, _documents, _allowExpiredDocuments);
        }
    }
}