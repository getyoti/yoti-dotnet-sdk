using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class OrthogonalRestrictionsFilter : DocumentFilter
    {
        public OrthogonalRestrictionsFilter(CountryRestriction countryRestriction, TypeRestriction typeRestriction, bool allowNonLatinDocuments)
        : base(Constants.DocScanConstants.OrthogonalRestrictions)
        {
            CountryRestriction = countryRestriction;
            TypeRestriction = typeRestriction;
            AllowNonLatinDocuments = allowNonLatinDocuments;
        }

        [JsonProperty(PropertyName = "country_restriction")]
        public CountryRestriction CountryRestriction { get; }

        [JsonProperty(PropertyName = "type_restriction")]
        public TypeRestriction TypeRestriction { get; }

        [JsonProperty(PropertyName = "allow_non_latin_documents")]
        public bool AllowNonLatinDocuments { get; }
    }
}