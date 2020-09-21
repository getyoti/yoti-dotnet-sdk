using System.Collections.Generic;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class DocumentRestrictionsFilterBuilder
    {
        private string _inclusion;
        private List<DocumentRestriction> _documents;

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
            _documents ??= new List<DocumentRestriction>();

            _documents.Add(new DocumentRestriction(countryCodes, documentTypes));
            return this;
        }

        public DocumentRestrictionsFilterBuilder WithDocumentRestriction(DocumentRestriction documentRestriction)
        {
            _documents ??= new List<DocumentRestriction>();

            _documents.Add(documentRestriction);
            return this;
        }

        public DocumentRestrictionsFilter Build()
        {
            Validation.NotNullOrEmpty(_inclusion, nameof(_inclusion));
            return new DocumentRestrictionsFilter(_inclusion, _documents);
        }
    }
}