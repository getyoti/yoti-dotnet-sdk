using System.Collections.Generic;
using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create.Filter
{
    public class CountryRestriction
    {
        public CountryRestriction(string inclusion, List<string> countryCodes)
        {
            Validation.NotNull(inclusion, nameof(inclusion));
            Validation.CollectionNotEmpty(countryCodes, nameof(countryCodes));

            Inclusion = inclusion;
            CountryCodes = countryCodes;
        }

        [JsonProperty(PropertyName = "inclusion")]
        public string Inclusion { get; }

        [JsonProperty(PropertyName = "country_codes")]
        public List<string> CountryCodes { get; }
    }
}