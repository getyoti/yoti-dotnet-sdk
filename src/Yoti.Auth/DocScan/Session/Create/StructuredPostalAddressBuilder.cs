namespace Yoti.Auth.DocScan.Session.Create
{
    /// <summary>
    /// Builder for <see cref="StructuredPostalAddress"/>.
    /// </summary>
    public class StructuredPostalAddressBuilder
    {
        private int? _addressFormat;
        private string _buildingNumber;
        private string _addressLine1;
        private string _townCity;
        private string _postalCode;
        private string _countryIso;
        private string _country;
        private string _formattedAddress;

        /// <summary>
        /// Sets the address format
        /// </summary>
        /// <param name="addressFormat">The address format</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithAddressFormat(int addressFormat)
        {
            _addressFormat = addressFormat;
            return this;
        }

        /// <summary>
        /// Sets the building number
        /// </summary>
        /// <param name="buildingNumber">The building number</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithBuildingNumber(string buildingNumber)
        {
            _buildingNumber = buildingNumber;
            return this;
        }

        /// <summary>
        /// Sets the address line 1
        /// </summary>
        /// <param name="addressLine1">The first address line</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithAddressLine1(string addressLine1)
        {
            _addressLine1 = addressLine1;
            return this;
        }

        /// <summary>
        /// Sets the town or city
        /// </summary>
        /// <param name="townCity">The town or city</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithTownCity(string townCity)
        {
            _townCity = townCity;
            return this;
        }

        /// <summary>
        /// Sets the postal code
        /// </summary>
        /// <param name="postalCode">The postal code</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithPostalCode(string postalCode)
        {
            _postalCode = postalCode;
            return this;
        }

        /// <summary>
        /// Sets the country ISO code
        /// </summary>
        /// <param name="countryIso">The country ISO code (e.g. "GBR")</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithCountryIso(string countryIso)
        {
            _countryIso = countryIso;
            return this;
        }

        /// <summary>
        /// Sets the country name
        /// </summary>
        /// <param name="country">The country name</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithCountry(string country)
        {
            _country = country;
            return this;
        }

        /// <summary>
        /// Sets the formatted address
        /// </summary>
        /// <param name="formattedAddress">The formatted address</param>
        /// <returns>the builder</returns>
        public StructuredPostalAddressBuilder WithFormattedAddress(string formattedAddress)
        {
            _formattedAddress = formattedAddress;
            return this;
        }

        /// <summary>
        /// Builds the <see cref="StructuredPostalAddress"/> based on the values supplied to the builder.
        /// </summary>
        /// <returns>The built <see cref="StructuredPostalAddress"/></returns>
        public StructuredPostalAddress Build()
        {
            return new StructuredPostalAddress(
                _addressFormat,
                _buildingNumber,
                _addressLine1,
                _townCity,
                _postalCode,
                _countryIso,
                _country,
                _formattedAddress);
        }
    }
}
