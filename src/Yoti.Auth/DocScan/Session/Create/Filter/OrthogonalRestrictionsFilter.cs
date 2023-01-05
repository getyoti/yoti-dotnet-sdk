using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class OrthogonalRestrictionsFilter : DocumentFilter
    {
        public OrthogonalRestrictionsFilter(CountryRestriction countryRestriction, TypeRestriction typeRestriction, bool allowExpiredDocuments)
        : base(Constants.DocScanConstants.OrthogonalRestrictions)
        {
            CountryRestriction = countryRestriction;
            TypeRestriction = typeRestriction;
            AllowExpiredDocuments = allowExpiredDocuments;
        }

        [JsonProperty(PropertyName = "country_restriction")]
        public CountryRestriction CountryRestriction { get; }

        [JsonProperty(PropertyName = "type_restriction")]
        public TypeRestriction TypeRestriction { get; }

        [JsonProperty(PropertyName = "allow_expired_documents")]
        public bool AllowExpiredDocuments { get; }
    }
}