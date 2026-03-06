using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Yoti.Auth.DocScan.Session.Create;

namespace Yoti.Auth.Tests.DocScan.Session.Create
{
    [TestClass]
    public class StructuredPostalAddressBuilderTests
    {
        [TestMethod]
        public void ShouldBuildWithAddressFormat()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithAddressFormat(1)
                .Build();

            Assert.AreEqual(1, address.AddressFormat);
        }

        [TestMethod]
        public void ShouldBuildWithBuildingNumber()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithBuildingNumber("74")
                .Build();

            Assert.AreEqual("74", address.BuildingNumber);
        }

        [TestMethod]
        public void ShouldBuildWithAddressLine1()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithAddressLine1("AddressLine1")
                .Build();

            Assert.AreEqual("AddressLine1", address.AddressLine1);
        }

        [TestMethod]
        public void ShouldBuildWithTownCity()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithTownCity("CityName")
                .Build();

            Assert.AreEqual("CityName", address.TownCity);
        }

        [TestMethod]
        public void ShouldBuildWithPostalCode()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithPostalCode("E143RN")
                .Build();

            Assert.AreEqual("E143RN", address.PostalCode);
        }

        [TestMethod]
        public void ShouldBuildWithCountryIso()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithCountryIso("GBR")
                .Build();

            Assert.AreEqual("GBR", address.CountryIso);
        }

        [TestMethod]
        public void ShouldBuildWithCountry()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithCountry("United Kingdom")
                .Build();

            Assert.AreEqual("United Kingdom", address.Country);
        }

        [TestMethod]
        public void ShouldBuildWithFormattedAddress()
        {
            var formattedAddress = "74\nAddressLine1\nCityName\nE143RN\nGBR";

            var address = new StructuredPostalAddressBuilder()
                .WithFormattedAddress(formattedAddress)
                .Build();

            Assert.AreEqual(formattedAddress, address.FormattedAddress);
        }

        [TestMethod]
        public void ShouldCorrectlySerializeAllProperties()
        {
            var address = new StructuredPostalAddressBuilder()
                .WithAddressFormat(1)
                .WithBuildingNumber("74")
                .WithAddressLine1("AddressLine1")
                .WithTownCity("CityName")
                .WithPostalCode("E143RN")
                .WithCountryIso("GBR")
                .WithCountry("United Kingdom")
                .WithFormattedAddress("74\nAddressLine1\nCityName\nE143RN\nGBR")
                .Build();

            string json = JsonConvert.SerializeObject(address);

            Assert.IsTrue(json.Contains("\"address_format\":1"));
            Assert.IsTrue(json.Contains("\"building_number\":\"74\""));
            Assert.IsTrue(json.Contains("\"address_line1\":\"AddressLine1\""));
            Assert.IsTrue(json.Contains("\"town_city\":\"CityName\""));
            Assert.IsTrue(json.Contains("\"postal_code\":\"E143RN\""));
            Assert.IsTrue(json.Contains("\"country_iso\":\"GBR\""));
            Assert.IsTrue(json.Contains("\"country\":\"United Kingdom\""));
            Assert.IsTrue(json.Contains("\"formatted_address\""));
        }
    }
}
