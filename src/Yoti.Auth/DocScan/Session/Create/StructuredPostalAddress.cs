using Newtonsoft.Json;

namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Represents a structured postal address for an applicant profile.
    /// </summary>
    public class StructuredPostalAddress
    {
        internal StructuredPostalAddress(
            int? addressFormat,
            string buildingNumber,
            string addressLine1,
            string townCity,
            string postalCode,
            string countryIso,
            string country,
            string formattedAddress)
        {
            AddressFormat = addressFormat;
            BuildingNumber = buildingNumber;
            AddressLine1 = addressLine1;
            TownCity = townCity;
            PostalCode = postalCode;
            CountryIso = countryIso;
            Country = country;
            FormattedAddress = formattedAddress;
        }

        [JsonProperty(PropertyName = "address_format")]
        public int? AddressFormat { get; }

        [JsonProperty(PropertyName = "building_number")]
        public string BuildingNumber { get; }

        [JsonProperty(PropertyName = "address_line1")]
        public string AddressLine1 { get; }

        [JsonProperty(PropertyName = "town_city")]
        public string TownCity { get; }

        [JsonProperty(PropertyName = "postal_code")]
        public string PostalCode { get; }

        [JsonProperty(PropertyName = "country_iso")]
        public string CountryIso { get; }

        [JsonProperty(PropertyName = "country")]
        public string Country { get; }

        [JsonProperty(PropertyName = "formatted_address")]
        public string FormattedAddress { get; }
    }
}
