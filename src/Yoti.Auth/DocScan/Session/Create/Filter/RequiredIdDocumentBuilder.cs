namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class RequiredIdDocumentBuilder
    {
        private DocumentFilter _filter;

        public RequiredIdDocumentBuilder WithFilter(DocumentFilter filter)
        {
            _filter = filter;
            return this;
        }

        public RequiredIdDocument Build()
        {
            return new RequiredIdDocument(_filter);
        }
    }
}